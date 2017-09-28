using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownLoadManager.Entity.PCU_Entity.request
{
    class PCU_UpgradeRunningReq : BaseProtocolImpl<PCU_UpgradeRunningReq>
    {
        [ProtocolAttribute("devType", 0, 1)]
        public int devType { get; set; }

        public override int GetCommand()
        {
            return Const.PCU_UPGRADE_RUNNING_REQ;
        }
    }
}
