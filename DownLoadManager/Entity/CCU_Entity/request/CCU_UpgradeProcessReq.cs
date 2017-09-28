using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownLoadManager.Entity.CCU_Entity.request
{
    public class CCU_UpgradeProcessReq : BaseProtocolImpl<CCU_UpgradeProcessReq>
    {
        [ProtocolAttribute("FrameData", 0, 60)]
        public byte[] FrameData { get; set; }

        public int length { get; set; }

        //编码 拼包 重写
        public override byte[] Encode()
        {            
            byte[] temp = base.Encode();
            return base.SplitArray(temp, 0, length);
        }

        public override int GetCommand()
        {
            return Const.CCU_UPGRADE_PROCESS_REQ;
        }
    }
}
