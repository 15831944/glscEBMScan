using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeamScanDll;
namespace GlscEBMProject
{
   public enum EShortCut
    {
        ManuScan,
        AreaScan,
        CADScan
    }
    public  partial class GlobleParameter
    {
        public static EBMBeamScan _eBMBeamScan=null ;
        public static ShowMessDelegate DelShowMess;
    }
    
}