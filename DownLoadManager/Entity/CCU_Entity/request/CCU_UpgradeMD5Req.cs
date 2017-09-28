using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownLoadManager.Entity.CCU_Entity.request
{
    class CCU_UpgradeMD5Req : BaseProtocolImpl<CCU_UpgradeMD5Req>
    {
        [ProtocolAttribute("FirmVersion", 0, 4)]
        public byte[] FirmVersion { get; set; }

        [ProtocolAttribute("FileLen", 4, 3)]
        public int FileLen { get; set; }

        [ProtocolAttribute("FirmUpgrade", 7, 1)]
        public int FirmUpgrade { get; set; }

        [ProtocolAttribute("MD5", 8, 16)]
        public byte[] MD5 { get; set; }

        public override int GetCommand()
        {
            return Const.CCU_UPGRADE_MD5_REQ;
        }
    }
}
