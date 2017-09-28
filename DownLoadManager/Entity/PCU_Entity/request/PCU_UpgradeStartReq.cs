using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownLoadManager.Entity.PCU_Entity.request
{
    class PCU_UpgradeStartReq : BaseProtocolImpl<PCU_UpgradeStartReq>
    {
        [ProtocolAttribute("deviceType", 0, 1)]
        public int deviceType { get; set; }

        [ProtocolAttribute("Param1", 1, 1)]
        public int Param1 { get; set; }

        [ProtocolAttribute("Param2", 2, 1)]
        public int Param2 { get; set; }

        [ProtocolAttribute("Param3", 3, 1)]
        public int Param3 { get; set; }

        [ProtocolAttribute("Param4", 4, 1)]
        public int Param4 { get; set; }
        public override int GetCommand()
        {
            return Const.PCU_UPGRADE_START_REQ;
        }


    }
}
