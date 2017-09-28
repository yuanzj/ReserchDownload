using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownLoadManager.Entity.CCU_Entity.response
{
    class CCU_UpgradeEndRsp : BaseProtocolImpl<CCU_UpgradeEndRsp>
    {
        [ProtocolAttribute("Result", 0, 1)]
        public int Result { get; set; }

        [ProtocolAttribute("Cause", 1, 1)]
        public int Cause { get; set; }
    
        public override int GetCommand()
        {
            return Const.CCU_UPGRADE_END_RSP;
        }
    }
}
