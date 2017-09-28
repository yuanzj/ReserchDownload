using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownLoadManager.Entity.CCU_Entity.request
{
    class CCU_UpgradeStartReq : BaseProtocolImpl<CCU_UpgradeStartReq>
    {
        [ProtocolAttribute("PackageID", 0, 2)]
        public int PackageID { get; set; }

        [ProtocolAttribute("PackageLen", 2, 4)]
        public int PackageLen { get; set; }

        [ProtocolAttribute("AlreadyDownLoadLen", 6, 4)]
        public int AlreadyDownLoadLen { get; set; }

        public override int GetCommand()
        {
            return Const.CCU_UPGRADE_START_REQ;
        }
    }
}
