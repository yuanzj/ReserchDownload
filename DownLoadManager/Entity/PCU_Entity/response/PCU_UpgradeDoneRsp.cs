using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownLoadManager.Entity.PCU_Entity.response
{
    class PCU_UpgradeDoneRsp : BaseProtocolImpl<PCU_UpgradeDoneRsp>
    {
        [ProtocolAttribute("command", 0, 1)]
        public int command { get; set; }

        [ProtocolAttribute("checksumResult", 1, 2)]
        public int checksumResult { get; set; }

        public override int GetCommand()
        {
            return Const.PCU_UPGRADE_DONE_RSP;
        }
    }
}
