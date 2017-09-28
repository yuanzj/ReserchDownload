/**
 * 1、Input: 报文实体类（泛型）
 * 2、Output:执行结果
 * 3、Dependency class: SimpleSerialPortTask.cs
 * 4、注册事件回调: 
 *          1)None
 *          2)None
 * 5、包含关系：
 *          1)CheckOK     —— 校验和
 *          2)DataLength  —— 报文长度
 *          3)Decode      —— 转码，拼整条报文
 *          4)Encode      —— 解码， 解析报文
 *          5)StartInProtocol  —— 解析报文头
 *          
 */
using CommonUtils;
using Roky.SerialPortHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownLoadManager
{
    //此串口对象 封装了一些 算法
    public class SerialPortProtocoImpl<T> : ISerialProtocol where T : class, IEntityProtocol, new()
    {

        public T Entity { get; set; }

        public bool CheckOK(List<byte> buf)
        {
            //如果buf里的数值小于3
            if (buf.Count < 3)
                return false;
            int _dataLength = DataLength(buf);
            if (_dataLength < 3)
                return false;
            //和校验算法
            int sumValue = 0;
            for (int i = 0; i < (_dataLength - 1); i++)
            {
                sumValue += buf[i];
            }
            if (ByteProcess.intToByteArray(sumValue)[3] == buf[_dataLength - 1])
            {
                return true;
            }
            return false;
        }

        public int DataLength(List<byte> buf)
        {
            return buf[1];
        }

        public IEntityProtocol Decode(byte[] args)
        {
            byte Command = args[0];
            byte Length = args[1];

            byte[] Args = null;

            if ((args.Length - 3) > 0)
            {
                var bytesNew = new byte[args.Length - 3];
                Array.Copy(args, 2, bytesNew, 0, bytesNew.Length);

                Args = bytesNew;
            }

            byte CheckValue = args[args.Length - 1];

            if (null != Args)
            {
                T mT = new T();
                this.Entity = mT.Decode(Args) as T;
                return Entity;
            }
            else
            {
                return default(T);
            }

        }

        public byte[] Encode()
        {
            byte[] Args = this.Entity.Encode();
            int sum = 0;
            byte[] bytesNew = new byte[Args.Length + MinLength()];
            bytesNew[0] = (byte)Entity.GetCommand();
            bytesNew[1] = (byte)bytesNew.Length;

            sum += bytesNew[0];
            sum += bytesNew[1];
            for (int i = 0; i < Args.Length; i++)
            {
                bytesNew[2 + i] = Args[i];
                sum += Args[i];
            }
            bytesNew[bytesNew.Length - 1] = ByteProcess.intToByteArray(sum)[3];

            return bytesNew;
        }

        public int MinLength()
        {
            return 3;
        }

        public bool StartInProtocol(List<byte> buf)
        {
            T mT = new T();
            if (buf[0] == mT.GetCommand())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int StartLength()
        {
            return 1;
        }
        public int GetCommand()
        {
            if (this.Entity == null)
            {
                this.Entity = new T();
            }
            return this.Entity.GetCommand();
        }
    }
}
