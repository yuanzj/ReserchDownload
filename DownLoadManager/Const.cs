using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownLoadManager
{
    public class Const
    {
        //串口
        public static string COM_PORT = "COM1";

        public static int BAUD_RATE = 115200;

        public const int SERIAL_SUCCESS = 0;

        public const int MAX_TRY_COUNT = 150;

        public const int PC_TAKEOVER_REQ = 0x06;

        public const int PC_TAKEOVER_RSP = 0X86;

        public const int RESET_ECU_REQ = 0X0A;

        public const int CCU_UPGRADE_REQUEST = 0X68;

        public const int CCU_UPGRADE_RESPONSE = 0XE8;

        public const int CCU_UPGRADE_START_REQ = 0X61;

        public const int CCU_UPGRADE_START_RSP = 0XE1;

        public const int CCU_UPGRADE_PROCESS_REQ = 0X62;

        public const int CCU_UPGRADE_PROCESS_RSP = 0XE2;

        public const int CCU_UPGRADE_END_REQ = 0X63;

        public const int CCU_UPGRADE_END_RSP = 0XE3;

        public const int CCU_UPGRADE_MD5_REQ = 0X65;

        public const int CCU_UPGRADE_MD5_RSP = 0XE5;

        public const int PCU_UPGRADE_START_REQ = 0X50;

        public const int PCU_UPGRADE_START_RSP = 0XD0;

        public const int PCU_UPGRADE_REQUEST_REQ = 0X5A;

        public const int PCU_UPGRADE_REQUEST_RSP = 0XDA;

        public const int PCU_UPGRADE_PROCESS_REQ = 0X5B;

        public const int PCU_UPGRADE_PROCESS_RSP = 0XDB;

        public const int PCU_UPGRADE_DONE_REQ = 0X5C;

        public const int PCU_UPGRADE_DONE_RSP = 0XDC;

        public const int PCU_UPGRADE_RUNNING_REQ = 0X5D;

        public const int PCU_UPGRADE_RUNNING_RSP = 0XDD;

        public const int DO_NOT_UPGRADE = 0X0;

        public const int DO_UPGRADE = 0X1;

        public const int CONTINUE_UPGRADE = 0X2;

        public const int RECV_DATA_FAIL = 0X0;

        public const int RECV_DATA_OK = 0X1;

        public const int NONE_ERROR = 0X0;

        public const int CRC_ERROR = 0X1;

        public const int WRITE_FLASH_ERROR = 0X2;

        public const int DATA_ERROR = 0X3;

        public const int PACKAGE_ID_ERROR = 0X04;

        public const int DISARGEE_UPGRADE = 0X0;

        public const int ARGEE_UPGRADE = 0X1;

        public const int DEVICE_NAME = 0X0;

        public const int STEP_STATUS = 0X1;

        public const int PROGRESS_BAR = 0X2;

        public const int DISPLAY_TIMER = 0X3;

        public const int UPGRADE_RESULT = 0X4;

        public const int VER_0 = 7172;

        public const int VER_1 = 7173;

        public const int VER_2 = 7174;

        public const int VER_3 = 7175;

        public const int Dev_0 = 7168;

        public const int Dev_1 = 7169;

        public const int Dev_2 = 7170;

        public const int Dev_3 = 7171;

    }
}
