using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownLoadManager.Entity.CCU_Entity.response
{
    class pcTakeOverRsp : BaseProtocolImpl<pcTakeOverRsp>
    {
        [ProtocolAttribute("DeviceType", 0, 1)]
        public int DeviceType { get; set; }

        [ProtocolAttribute("HardWareID", 1, 1)]
        public int HardWareID { get; set; }

        [ProtocolAttribute("FirmID", 2, 1)]
        public int FirmID { get; set; }

        [ProtocolAttribute("FirmID1", 3, 1)]
        public int FirmID1 { get; set; }

        [ProtocolAttribute("FirmID2", 4, 1)]
        public int FirmID2 { get; set; }

        public override int GetCommand()
        {
            return Const.PC_TAKEOVER_RSP;
        }


    }
}
