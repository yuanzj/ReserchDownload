using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownLoadManager.Entity.PCU_Entity.request
{
    class PCU_UpgradeProcessReq : BaseProtocolImpl<PCU_UpgradeProcessReq>
    {
        [ProtocolAttribute("currentPage", 0, 1)]
        public int currentPage { get; set; }

        [ProtocolAttribute("currentSeg", 1, 1)]
        public int currentSeg { get; set; }

        [ProtocolAttribute("buff", 2, 59)]
        public byte[] buff { get; set; }

        public int length { get; set; }
        
        //编码 拼包 重写
        public override byte[] Encode()
        {
            byte[] temp = base.Encode();
            return base.SplitArray(temp, 0, length + 2);
        }

        public override int GetCommand()
        {
            return Const.PCU_UPGRADE_PROCESS_REQ;
        }
    }
}
