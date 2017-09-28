using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownLoadManager.Entity.CCU_Entity.response
{
    class CCU_UpgradeProcessRsp : BaseProtocolImpl<CCU_UpgradeProcessRsp>
    {
        [ProtocolAttribute("Result", 0, 1)]
        public int Result { get; set; }

        [ProtocolAttribute("Cause", 1, 1)]
        public int Cause { get; set; }

        public override int GetCommand()
        {
            return Const.CCU_UPGRADE_PROCESS_RSP;
        }
    }
}
