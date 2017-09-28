using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownLoadManager.Entity.PCU_Entity.request
{
    class PCU_UpgradeDoneReq : BaseProtocolImpl<PCU_UpgradeDoneReq>
    {
        [ProtocolAttribute("currentPage", 0, 1)]
        public int currentPage { get; set; }

        [ProtocolAttribute("pageChecksum", 1, 2)]
        public int pageChecksum { get; set; }

        public override int GetCommand()
        {
            return Const.PCU_UPGRADE_DONE_REQ;
        }
    }
}
