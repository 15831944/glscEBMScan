namespace EBMCtrl2._0.ebmScan.Package
{
  //  using Arcam.EBMControl.Framework.BeamControl;
    //using Arcam.EBMControl.Framework.Core.IO;
   // using Arcam.Utilities.Threading;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal class PackageManager
    {
        //private BlockingQueue<PackageEnvelope> m_inQueue = new BlockingQueue<PackageEnvelope>(1);
        private Queue<PackageEnvelope> m_inQueue = new Queue<PackageEnvelope>(1);//容量为1
        private volatile PackageEnvelope m_ActiveOutPackage;//可以由多个线程使用，而不用加锁
        private PackageEnvelope m_PreviousOutPackage;
        private Queue<PackageEnvelope> m_inputQueue = new Queue<PackageEnvelope>();
        private object ioSyncObject = new object();
        private ILog m_log = LogManager.GetLogger("BeamControlLog");
        private bool m_ThrowAwayNewPackages;
        private bool m_UsePreviousPackage;
      
        public event PackageOfficeEvent OnPackageCompleted;

        public void Add(PackageEnvelope package)
        {
            package.Package.ResetCursors();
            this.m_inQueue.Enqueue(package);
            package.EnqueueingTime.Stop();
        }

        private void CompletedPackage(IBeamControlPackage package)
        {
            if (this.OnPackageCompleted != null)
            {
                this.OnPackageCompleted.BeginInvoke(this, package, null, null);
            }
        }

        internal PackageEnvelope GetActiveOutPackage()
        {
            PackageEnvelope activeOutPackage;
            try
            {
                lock (this.ioSyncObject)
                {
                    if (this.m_UsePreviousPackage && (this.m_PreviousOutPackage != null))
                    {
                        return this.m_PreviousOutPackage;
                    }
                    if ((this.m_ActiveOutPackage == null) && (this.m_inQueue.Count > 0))
                    {
                        this.m_ActiveOutPackage = this.m_inQueue.Dequeue();
                        this.m_ActiveOutPackage.OutputTime.Start();
                        if (!this.m_ActiveOutPackage.IsReadOnly)
                        {
                            this.m_inputQueue.Enqueue(this.m_ActiveOutPackage);
                        }
                    }
                    if (this.m_UsePreviousPackage && (this.m_PreviousOutPackage == null))
                    {
                        this.m_PreviousOutPackage = this.m_ActiveOutPackage;
                        this.m_ActiveOutPackage = null;
                    }
                }
                activeOutPackage = this.m_ActiveOutPackage;
            }
            catch (Exception exception)
            {
                this.m_log.Error(exception.ToString());
                throw;
            }
            return activeOutPackage;
        }

        public void OnDataChanged(string[] ids, IOValue[] values)
        {
            PackageEnvelope activeOutPackage = this.m_ActiveOutPackage;
            if (activeOutPackage != null)
            {
                activeOutPackage.OnDataChanged(ids, values);
            }
        }

        public void Reset()
        {
            lock (this.ioSyncObject)
            {
                this.m_inQueue.Clear();
                this.m_inputQueue.Clear();
                this.m_ActiveOutPackage = null;
                this.m_PreviousOutPackage = null;
                this.m_log.Info("PackageManager Reset");
            }
        }

        internal void SetInPackageDone()
        {
            try
            {
                lock (this.ioSyncObject)
                {
                    PackageEnvelope envelope = this.m_inputQueue.Dequeue();
                    envelope.OutputTime.Stop();
                    //this.m_log.DebugFormat("Package {0} enqueue time {1}, output time {2}", envelope.ID, envelope.EnqueueingTime.Duration.ToString("F3"), envelope.OutputTime.Duration.ToString("F3"));
                    this.CompletedPackage(envelope.Package);
                }
            }
            catch (Exception exception)
            {
                this.m_log.Error(exception.ToString());
                throw;
            }
        }

        internal void SetOutPackageDone()
        {
            lock (this.ioSyncObject)
            {
                PackageEnvelope activeOutPackage = this.GetActiveOutPackage();
                if (activeOutPackage.Package.IsReadOnly)
                {
                    activeOutPackage.OutputTime.Stop();
                  //  this.m_log.DebugFormat("Package {0} enqueue time {1}, output time {2}", activeOutPackage.ID, activeOutPackage.EnqueueingTime.Duration.ToString("F3"), activeOutPackage.OutputTime.Duration.ToString("F3"));
                    this.CompletedPackage(activeOutPackage.Package);
                    this.m_PreviousOutPackage = activeOutPackage;
                    this.m_PreviousOutPackage.ResetCursors();
                    this.m_PreviousOutPackage.EnqueueingTime.Start();
                }
                if (!this.m_UsePreviousPackage)
                {
                    this.m_ActiveOutPackage = null;
                }
                this.m_UsePreviousPackage = false;
              
            }
        }

        internal void UsePreviousPackage()
        {
            this.m_UsePreviousPackage = true;
        }

        public bool ThrowAwayNewPackages
        {
            get => 
                this.m_ThrowAwayNewPackages;
            set
            {
                this.m_ThrowAwayNewPackages = value;
                if (this.m_ThrowAwayNewPackages)
                {
                    //this.m_inQueue.Lock();
                }
                else
                {
                   // this.m_inQueue.Unlock();
                }
            }
        }

        public int MaxQueueLength =>
            (this.m_inQueue.Count/*MaxQueueLength */+ 2);
    }
}

