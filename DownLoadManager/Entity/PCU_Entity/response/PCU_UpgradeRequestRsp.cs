using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownLoadManager.Entity.PCU_Entity.response
{
    class PCU_UpgradeRequestRsp : BaseProtocolImpl<PCU_UpgradeRequestRsp>
    {
        [ProtocolAttribute("status", 0, 1)]
        public int status { get; set; }

        public override int GetCommand()
        {
            return Const.PCU_UPGRADE_REQUEST_RSP;
        }
    }
}
