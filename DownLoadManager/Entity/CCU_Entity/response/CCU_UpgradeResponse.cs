using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownLoadManager.Entity.CCU_Entity.response
{
    class CCU_UpgradeResponse : BaseProtocolImpl<CCU_UpgradeResponse>
    {
        [ProtocolAttribute("UpgradeResult", 0, 1)]
        public int UpgradeResult { get; set; }

        [ProtocolAttribute("ContiueUpgradePacketID", 1, 2)]
        public int ContiueUpgradePacketID { get; set; }

        [ProtocolAttribute("AlreadyDownLoadLen", 3, 4)]
        public int AlreadyDownLoadLen { get; set; }

        public override int GetCommand()
        {
            return Const.CCU_UPGRADE_RESPONSE;
        }
    }
}
