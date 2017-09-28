using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownLoadManager.Entity.PCU_Entity.request
{
    class PCU_UpgradeRequestReq : BaseProtocolImpl<PCU_UpgradeRequestReq>
    {
        [ProtocolAttribute("currentPage", 0, 1)]
        public int currentPage { get; set; }

        [ProtocolAttribute("year", 1, 1)]
        public int year { get; set; }

        [ProtocolAttribute("month", 2, 1)]
        public int month { get; set; }

        [ProtocolAttribute("version", 3, 1)]
        public int version { get; set; }

        [ProtocolAttribute("fileLength", 4, 4)]
        public int fileLength { get; set; }

        [ProtocolAttribute("singlepagesize", 8, 2)]
        public int singlepagesize { get; set; }

        public override int GetCommand()
        {
            return Const.PCU_UPGRADE_REQUEST_REQ;
        }
    }
}
