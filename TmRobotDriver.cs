using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using NModbus;

namespace TmRobotArm
{
    /// <summary>
    /// Drivers of TM Robot
    /// IO Operations Encapsuled
    /// </summary>
    public class TmRobotDriver
    {
        #region[Properties Declarations and Definations]
        private const string hostIpAddr = "192.168.1.120";
        private const ushort modbusPort = 502;
        private const ushort tcpPort = 5890;
        private Socket sock;
        private IModbusMaster master;
        #endregion

        #region[Private functions: Connect to host]
        /// <summary>
        /// 初始化ListenNode的socket连接
        /// </summary>
        public void InitListenNodeSocketConnection()
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(hostIpAddr), tcpPort);

            try
            {
                sock.Connect(ep);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                Console.WriteLine("Connection failed");
            }
        }
        /// <summary>
        /// 初始化modbus客户端
        /// </summary>
        public void CreateMaster()
        {
            var factory = new ModbusFactory();
            try
            {
                this.master = factory.CreateMaster(new TcpClient(hostIpAddr, modbusPort));
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }
        }
        #endregion

        #region[Modbus Bit functions: Bits operations]
        public int GetArmDiValue(ushort bit)
        {
            const ushort addr = 800;
            const ushort num = 3;

            if(bit > num) 
            {
                Console.WriteLine("Wrong bit!");
                return -1;
            }

            bool[] b = master.ReadInputs(0, addr, num);

            return b[bit]? 1:0;
        }

        public int GetArmDqValue(ushort bit)
        {
            const ushort addr = 800;
            const ushort num = 3;

            if(bit > num )
            {
                Console.WriteLine("Wrong bit!");
                return -1;
            }

            bool[] b = master.ReadCoils(0, addr, num);

            return b[bit]? 1:0;

        }

        public int GetCabinDiValue(ushort bit)
        {
            const ushort addr = 0;
            const ushort num = 16;

            if(bit > num)
            {
                Console.WriteLine("Wrong bit!");
                return -1;
            }

            bool[] b = master.ReadInputs(0, addr, num);

            return b[bit]? 1:0;
        }
        public int GetCabinDqValue(ushort bit)
        {
            const ushort addr = 0;
            const ushort num = 16;

            if(bit > num)
            {
                Console.WriteLine("Wrong bit!");
                return -1;
            }

            bool[] b = master.ReadCoils(0, addr, num);

            return b[bit]? 1:0;
        }

        public int SetArmDqValue(ushort bit, ushort value)
        {
            const ushort addr = 0;
            const ushort num = 16;

            if(bit > num || !(value == 1 || value == 0))
            {
                Console.WriteLine("Wrong bit or value!");
                return -1;
            }
            bool[] b = master.ReadCoils(0, addr, num);
            b[bit] = value == 1 ? true: false;

            master.WriteMultipleCoils(0, bit, b);

            return b[bit]? 1:0;
        }
        public int SetCabinDqValue(ushort bit, ushort value)
        {
            const ushort addr = 0;
            const ushort num = 16;

            if(bit > num || !(value == 1 || value == 0))
            {
                Console.WriteLine("Wrong bit or value!");
                return -1;
            }

            bool[] b = master.ReadCoils(0, addr, num);
            b[bit] = value == 1 ? true: false;

            master.WriteMultipleCoils(0, bit, b);

            return b[bit]? 1:0;
        }

        public bool IsError()
        {
            ushort[] temp = master.ReadInputRegisters(0, 7201, 1);
            return temp[0] == 1 ? true : false;
        }

        #endregion

        #region[Modbus Reg Functions: Reg-read operations]
        /// <summary>
        /// 获得当前的J1-J6坐标，生数据，不进行任何转化；
        /// </summary>
        /// <returns></returns>
        private ushort[] GetRawJointValues()
        {
            const ushort addr = 7013;
            const ushort num = 6;

            ushort[] temp = master.ReadInputRegisters(0, addr, num);
            return temp;
        }

        /// <summary>
        /// 将寄存器中读回的坐标值转换成浮点数；
        /// </summary>
        /// <returns></returns>
        public float[] GetCoordinationValue()
        {
            const ushort addr = 7001;
            const ushort num = 6;

            float[] coordinates = { 0, 0, 0, 0, 0, 0 };

            ushort[] temp = master.ReadInputRegisters(0, addr, num);
            for(int i = 0; i< num; i++)
            {
                coordinates[i] = CommonTools.GetFloatValue(temp[i]);
            }

            return coordinates;
        }
        public bool StandStill()
        {
            ushort[] joints = GetRawJointValues();
            Thread.Sleep(500);
            ushort[] jointsOld = new ushort[6];

            for(int i = 0; i< jointsOld.Length; i++)
            {
                jointsOld[i] = joints[i];
            }
            
            joints = GetRawJointValues();

            for(int i = 0; i<joints.Length; i++)
            {
                if(joints[i] == jointsOld[i])
                {
                    /* do nothing*/
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region Public: Sock Send and Recv
        public void SendToListenNode(byte[] data)
        {

            try
            {
                sock.Send(data);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }
        }
        public string RecvFromListenNode()
        {
            byte[] data = new byte[1024];
            try
            {
                //return sock.Receive(data).ToString();
                int length = sock.Receive(data);
                string msg = System.Text.Encoding.ASCII.GetString(data);
                return msg.Substring(0, length);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                return (-1).ToString();
            }
        }
        #endregion

        #region[ListenNode functions]
        /// <summary>
        /// 跳出 Listen Node;
        /// </summary>
        public void JumpOut()
        {
            String cmd = "$TMSCT,14,1,ScriptExit(),*67\r\n";
            byte[] b = Encoding.Default.GetBytes(cmd);
            this.SendToListenNode(b);
        }

        /// <summary>
        /// 向 Listen Node 发送命令;
        /// </summary>
        /// <param name="type">PTP 或 Line</param>
        /// <param name="speed">输入一个整形量表示最大速度的百分比， 例如"20"代表 20%速度</param>
        /// <param name="coordinates">一个6维向量表示坐标，格式为{x, y, z, Rx, Ry, Rz }</param>
        /// <returns> -1 表示输入有误， 0 表示成功</returns>
        public int SendCmd(String type, ushort speed, float[] coordinates)
        {
            const String head = "TMSCT";
            String data= null;
            String crc = null;

            if(!(type.Equals("PTP") || type.Equals("Line")))
            {
                return -1;
            }
            else if("PTP" == type)
            {
                data = "1,PTP(\"CPP\","
                    + coordinates[0].ToString() + ","
                    + coordinates[1].ToString() + ","
                    + coordinates[2].ToString() + ","
                    + coordinates[3].ToString() + ","
                    + coordinates[4].ToString() + ","
                    + coordinates[5].ToString() + ","
                    + speed.ToString() + ","
                    + "1,0,false)";
            }
            else if("Line" == type)
            {
                data = "1,Line(\"CAP\","
                    + coordinates[0].ToString() + "," 
                    + coordinates[1].ToString() + ","
                    + coordinates[2].ToString() + ","
                    + coordinates[3].ToString() + ","
                    + coordinates[4].ToString() + ","
                    + coordinates[5].ToString() + ","
                    + speed.ToString() + ","
                    + "1,0,false)";
            }

            String crcBody = head + "," + data.Length.ToString() + "," + data + ",";
            crc = CommonTools.CRC(Encoding.Default.GetBytes(crcBody));

            String cmd = "$" + crcBody + "*" + crc + "\r\n";
            this.SendToListenNode(Encoding.Default.GetBytes(cmd));

            String buffer = this.RecvFromListenNode();
            Console.WriteLine(buffer);

            if(buffer.StartsWith("$TMSCT,4,1,OK,*5C"))
            {
                return 0;
            }
            else
            {
                return -10;
            }
        }
        /// <summary>
        /// 在TM机器人的Word坐标下以相对运动方式移动轴；
        /// 注意：是TM机械臂的Word坐标，不是楼板放线机器人的坐标！！！
        /// </summary>
        /// <param name="speed">速度</param>
        /// <param name="coordinates">轴移动的角度</param>
        /// <returns>无效指令返回-1， 成功返回0；</returns>
        public int MoveJointRelated(ushort speed, float[] coordinates)
        {
            const String head = "TMSCT";
            String data= null;
            String crc = null;

            data = "1,Move_PTP(\"JPP\","
                + coordinates[0].ToString() + ","
                + coordinates[1].ToString() + ","
                + coordinates[2].ToString() + ","
                + coordinates[3].ToString() + ","
                + coordinates[4].ToString() + ","
                + coordinates[5].ToString() + ","
                + speed.ToString() + ","
                + "1,0,false)";

            String crcBody = head + "," + data.Length.ToString() + "," + data + ",";
            crc = CommonTools.CRC(Encoding.Default.GetBytes(crcBody));

            String cmd = "$" + crcBody + "*" + crc + "\r\n";
            this.SendToListenNode(Encoding.Default.GetBytes(cmd));

            String msg = this.RecvFromListenNode();
            Console.WriteLine(msg);

            if(msg.StartsWith("$TMSCT,4,1,OK,*5C"))
            {
                return 0;
            }
            else
            {
                return -10;
            }
        }
        /// 在TM机器人的Word坐标下以相对运动方式进行直线移动；
        /// 注意：是TM机械臂的Word坐标，不是楼板放线机器人的坐标！！！
        /// </summary>
        /// <param name="speed"> 速度</param>
        /// <param name="coordinates"> 点对当前位置的笛卡尔偏移向量</param>
        /// <returns>无效指令返回-1， 成功返回0；</returns>

        public int MoveLineRelated(ushort speed, float[] coordinates)
        {
            const String head = "TMSCT";
            String data= null;
            String crc = null;

            data = ",1,Move_Line(\"CPP\","
                + coordinates[0].ToString() + ","
                + coordinates[1].ToString() + ","
                + coordinates[2].ToString() + ","
                + coordinates[3].ToString() + ","
                + coordinates[4].ToString() + ","
                + coordinates[5].ToString() + ","
                + speed.ToString() + ","
                + "1,0,false)";

            String crcBody = head + "," + data.Length.ToString() + data + ",";
            crc = CommonTools.CRC(Encoding.Default.GetBytes(crcBody));

            String cmd = "$" + crcBody + "*" + crc + "\r\n";
            this.SendToListenNode(Encoding.Default.GetBytes(cmd));

            String msg = this.RecvFromListenNode();
            Console.WriteLine(msg);

            if(msg.StartsWith("$TMSCT,4,1,OK,*5C"))
            {
                return 0;
            }
            else
            {
                return -10;
            }
        }
        #endregion
    }
}
