﻿using Roky;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownLoadManager.Entity.CCU_Entity.request
{
    public class NullEntity : BaseProtocolImpl<NullEntity>
    {
        public override int GetCommand()
        {
            return SerialPortConst.COMMAND_EMPTY_VIRTUAL;
        }
    }
}
