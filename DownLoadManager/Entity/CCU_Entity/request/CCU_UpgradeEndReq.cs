﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownLoadManager.Entity.CCU_Entity.request
{
    class CCU_UpgradeEndReq : BaseProtocolImpl<CCU_UpgradeEndReq>
    {
        [ProtocolAttribute("PackageID", 0, 2)]
        public int PackageID { get; set; }

        [ProtocolAttribute("CRC", 2, 2)]
        public int CRC { get; set; }

        public override int GetCommand()
        {
            return Const.CCU_UPGRADE_END_REQ;
        }
    }
}
