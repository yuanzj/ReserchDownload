﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownLoadManager.Entity.CCU_Entity.request
{
    class pcTakeoverReq : BaseProtocolImpl<pcTakeoverReq>
    {
        /*
        [ProtocolAttribute("deviceType", 0, 1)]
        public int deviceType { get; set; }

        [ProtocolAttribute("firmVer", 1, 4)]
        public int firmVer { get; set; }

        [ProtocolAttribute("softVer", 5, 4)]
        public int softVer { get; set; }
        */

        [ProtocolAttribute("Status", 0, 1)]
        public int Status { get; set; }


        [ProtocolAttribute("hardwareID", 1, 4)]
        public int hardwareID { get; set; }

        [ProtocolAttribute("softwareID", 5, 4)]
        public int softwareID { get; set; }

        /*
        [ProtocolAttribute("Param1", 1, 1)]
        public int Param1 { get; set; }

        [ProtocolAttribute("Param2", 2, 1)]
        public int Param2 { get; set; }

        [ProtocolAttribute("Param3", 3, 1)]
        public int Param3 { get; set; }

        [ProtocolAttribute("Param4", 4, 2)]
        public int Param4 { get; set; }

        [ProtocolAttribute("Param5", 6, 1)]
        public int Param5 { get; set; }

        [ProtocolAttribute("Param6", 7, 3)]
        public int Param6 { get; set; }

        [ProtocolAttribute("Param7", 10, 1)]
        public int Param7 { get; set; }
         */
        public override int GetCommand()
        {
            return Const.PC_TAKEOVER_REQ;
        }
    }
}
