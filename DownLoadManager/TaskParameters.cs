using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownLoadManager
{
    public class TaskParameters
    {
        public bool bCCU { get; set; }
        public bool bPCU { get; set; }
        public string mCcuFilePath { get; set; }
        public string mPcuFilePath { get; set; }
    }

    public class ProgressArgs : EventArgs
    {
        public int percentage { get; set; }
    }

    public enum Task_Level
    {
        TASK_INIT = 0,
        TASK_WAITING = 1,
        TASK_PROCESS = 2,
        TASK_PASS = 3,
        TASK_FAIL = 4,
        TASK_PROGRESS = 5,
        TASK_DONE = 6,
        TASK_TIMER = 7,
        TASK_RETRY = 8,       
    }

    public enum Devices
    {
        NONE = 0,
        PCU = 1,
        CCU = 2
    }

    public class TaskArgs : EventArgs
    {
        public int value { get; set; }
        public string msg { get; set; }
        public Task_Level level { get; set; }
    }

}
