using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownLoadManager.Entity.PCU_Entity.response
{
    class PCU_UpgradeStartRsp : BaseProtocolImpl<PCU_UpgradeStartRsp>
    {
        [ProtocolAttribute("deviceType", 0, 1)]
        public int deviceType { get; set; }

        public override int GetCommand()
        {
            return Const.PCU_UPGRADE_START_RSP;
        }
    }
}
