using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownLoadManager.Entity.CCU_Entity.request
{
    class CCU_UpgradeRequest : BaseProtocolImpl<CCU_UpgradeRequest>
    {
        [ProtocolAttribute("firmVersion", 0, 4)]
        public byte[] firmVersion { get; set; }

        [ProtocolAttribute("fileLen", 4, 3)]
        public int fileLen { get; set; }

        [ProtocolAttribute("singlePackageLen", 7, 1)]
        public int singlePackageLen { get; set; }

        [ProtocolAttribute("singleFrameLen", 8, 2)]
        public int singleFrameLen { get; set; }

        [ProtocolAttribute("ForceUpgrade", 10, 1)]
        public int ForceUpgrade { get; set; }

        public override int GetCommand()
        {
            return Const.CCU_UPGRADE_REQUEST;
        }
    }
}
