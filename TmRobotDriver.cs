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

        #region Addr Definations:

        // Stick Address:
        private const ushort ADDR_STICK_BASE = 7100;
        private const ushort ADDR_STICK_OVERRIDE = ADDR_STICK_BASE + 1;
        private const ushort ADDR_STICK_MANUAL_AUTO_MODE = ADDR_STICK_BASE + 2;
        private const ushort ADDR_STICK_PLAYPAUSE = ADDR_STICK_BASE + 4;
        private const ushort ADDR_STICK_STOP = ADDR_STICK_BASE + 5;
        private const ushort ADDR_STICK_STICKPLUS = ADDR_STICK_BASE + 6;
        private const ushort ADDR_STICK_STICKMINUS = ADDR_STICK_BASE + 7;

        // Robot Coordinates Reg:
        private const ushort ADDR_REG_BASE = 7000;
        private const ushort ADDR_REG_X = ADDR_REG_BASE + 1;
        private const ushort ADDR_REG_Y = ADDR_REG_BASE + 3;
        private const ushort ADDR_REG_Z = ADDR_REG_BASE + 5;
        private const ushort ADDR_REG_RX = ADDR_REG_BASE + 7;
        private const ushort ADDR_REG_RY = ADDR_REG_BASE + 9;
        private const ushort ADDR_REG_RZ = ADDR_REG_BASE + 11;
        private const ushort ADDR_REG_J1 = ADDR_REG_BASE + 13;
        private const ushort ADDR_REG_J2 = ADDR_REG_BASE + 15;
        private const ushort ADDR_REG_J3 = ADDR_REG_BASE + 17;
        private const ushort ADDR_REG_J4 = ADDR_REG_BASE + 19;
        private const ushort ADDR_REG_J5 = ADDR_REG_BASE + 21;
        private const ushort ADDR_REG_J6 = ADDR_REG_BASE + 23;
        private const ushort ADDR_REG_TOOL_X = ADDR_REG_BASE + 25;
        private const ushort ADDR_REG_TOOL_Y = ADDR_REG_BASE + 27;
        private const ushort ADDR_REG_TOOL_Z = ADDR_REG_BASE + 29;
        private const ushort ADDR_REG_TOOL_RX = ADDR_REG_BASE + 31;
        private const ushort ADDR_REG_TOOL_RY = ADDR_REG_BASE + 33;
        private const ushort ADDR_REG_TOOL_RZ = ADDR_REG_BASE + 35;

        // Robot Status:
        private const ushort ADDR_STATUS_BASE = 7200;
        private const ushort ADDR_STATUS_PROJ_ERROR = ADDR_REG_BASE + 1;
        private const ushort ADDR_STATUS_PROJ_RUNNING = ADDR_REG_BASE + 2;
        private const ushort ADDR_STATUS_PROJ_EDITING = ADDR_REG_BASE + 3;
        private const ushort ADDR_STATUS_PROJ_PAUSE = ADDR_REG_BASE + 4;
        private const ushort ADDR_STATUS_GET_PERMISSION = ADDR_REG_BASE + 5;

        // Robot Arm IO:
        private const ushort ADDR_ARM_IO_BASE = 800;
        private const ushort ADDR_ARM_DI0 = ADDR_ARM_IO_BASE + 0;
        private const ushort ADDR_ARM_DI1 = ADDR_ARM_IO_BASE + 1;
        private const ushort ADDR_ARM_DI2 = ADDR_ARM_IO_BASE + 2;
        private const ushort ADDR_ARM_DQ0 = ADDR_ARM_IO_BASE + 0;
        private const ushort ADDR_ARM_DQ1 = ADDR_ARM_IO_BASE + 1;
        private const ushort ADDR_ARM_DQ2 = ADDR_ARM_IO_BASE + 2;
        private const ushort ADDR_ARM_DQ3 = ADDR_ARM_IO_BASE + 3;
        private const ushort ADDR_ARM_AI0 = ADDR_ARM_IO_BASE + 0;

        // Robot Cabin IO:
        private const ushort ADDR_CABIN_IO_BASE = 0;
        private const ushort ADDR_CABIN_DI0 = ADDR_CABIN_IO_BASE + 0;
        private const ushort ADDR_CABIN_DI1 = ADDR_CABIN_IO_BASE + 1;
        private const ushort ADDR_CABIN_DI2 = ADDR_CABIN_IO_BASE + 2;
        private const ushort ADDR_CABIN_DI3 = ADDR_CABIN_IO_BASE + 3;
        private const ushort ADDR_CABIN_DI4 = ADDR_CABIN_IO_BASE + 4;
        private const ushort ADDR_CABIN_DI5 = ADDR_CABIN_IO_BASE + 5;
        private const ushort ADDR_CABIN_DI6 = ADDR_CABIN_IO_BASE + 6;
        private const ushort ADDR_CABIN_DI7 = ADDR_CABIN_IO_BASE + 7;
        private const ushort ADDR_CABIN_DI8 = ADDR_CABIN_IO_BASE + 8;
        private const ushort ADDR_CABIN_DI9 = ADDR_CABIN_IO_BASE + 9;
        private const ushort ADDR_CABIN_DI10 = ADDR_CABIN_IO_BASE + 10;
        private const ushort ADDR_CABIN_DI11 = ADDR_CABIN_IO_BASE + 11;
        private const ushort ADDR_CABIN_DI12 = ADDR_CABIN_IO_BASE + 12;
        private const ushort ADDR_CABIN_DI13 = ADDR_CABIN_IO_BASE + 13;
        private const ushort ADDR_CABIN_DI14 = ADDR_CABIN_IO_BASE + 14;
        private const ushort ADDR_CABIN_DI15 = ADDR_CABIN_IO_BASE + 15;

        private const ushort ADDR_CABIN_DQ0 = ADDR_CABIN_IO_BASE + 0;
        private const ushort ADDR_CABIN_DQ1 = ADDR_CABIN_IO_BASE + 1;
        private const ushort ADDR_CABIN_DQ2 = ADDR_CABIN_IO_BASE + 2;
        private const ushort ADDR_CABIN_DQ3 = ADDR_CABIN_IO_BASE + 3;
        private const ushort ADDR_CABIN_DQ4 = ADDR_CABIN_IO_BASE + 4;
        private const ushort ADDR_CABIN_DQ5 = ADDR_CABIN_IO_BASE + 5;
        private const ushort ADDR_CABIN_DQ6 = ADDR_CABIN_IO_BASE + 6;
        private const ushort ADDR_CABIN_DQ7 = ADDR_CABIN_IO_BASE + 7;
        private const ushort ADDR_CABIN_DQ8 = ADDR_CABIN_IO_BASE + 8;
        private const ushort ADDR_CABIN_DQ9 = ADDR_CABIN_IO_BASE + 9;
        private const ushort ADDR_CABIN_DQ10 = ADDR_CABIN_IO_BASE + 10;
        private const ushort ADDR_CABIN_DQ11 = ADDR_CABIN_IO_BASE + 11;
        private const ushort ADDR_CABIN_DQ12 = ADDR_CABIN_IO_BASE + 12;
        private const ushort ADDR_CABIN_DQ13 = ADDR_CABIN_IO_BASE + 13;
        private const ushort ADDR_CABIN_DQ14 = ADDR_CABIN_IO_BASE + 14;
        private const ushort ADDR_CABIN_DQ15 = ADDR_CABIN_IO_BASE + 15;

        private const ushort ADDR_CABIN_AI0 = ADDR_CABIN_IO_BASE + 0;
        private const ushort ADDR_CABIN_AI1 = ADDR_CABIN_IO_BASE + 2;
        private const ushort ADDR_CABIN_AQ0 = ADDR_CABIN_IO_BASE + 0;

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
            catch (SocketException e)
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }
        }
        #endregion

        #region[Modbus Bit functions: Bits operations]
        public int GetArmDiValue(ushort bit)
        {
            const ushort num = 3;

            if (bit > num)
            {
                Console.WriteLine("Wrong bit!");
                return -1;
            }

            // ADDR = 800
            bool[] b = master.ReadInputs(0, ADDR_ARM_IO_BASE, num);

            return b[bit] ? 1 : 0;
        }

        public int GetArmDqValue(ushort bit)
        {
            const ushort num = 3;

            if (bit > num)
            {
                Console.WriteLine("Wrong bit!");
                return -1;
            }

            // ADDR = 800
            bool[] b = master.ReadCoils(0, ADDR_ARM_IO_BASE, num);

            return b[bit] ? 1 : 0;

        }

        public int GetCabinDiValue(ushort bit)
        {
            //const ushort addr = 0;
            const ushort num = 16;

            if (bit > num)
            {
                Console.WriteLine("Wrong bit!");
                return -1;
            }

            // ADDR = 0
            bool[] b = master.ReadInputs(0, ADDR_CABIN_IO_BASE, num);

            return b[bit] ? 1 : 0;
        }
        public int GetCabinDqValue(ushort bit)
        {
            //const ushort addr = 0;
            const ushort num = 16;

            if (bit > num)
            {
                Console.WriteLine("Wrong bit!");
                return -1;
            }

            // ADDR = 0
            bool[] b = master.ReadCoils(0, ADDR_CABIN_IO_BASE, num);

            return b[bit] ? 1 : 0;
        }


        /// <summary>
        /// 置位TM机器人手臂上的DQ；
        /// </summary>
        /// <param name="bit"> 位地址，0-3；</param>
        /// <param name="value"> Set = 1, Reset = 0; </param>
        /// <returns></returns>
        public int SetArmDqValue(ushort bit, ushort value)
        {
            //ushort baseAddr = 800;
            ushort num = 3;

            if (bit > num || !(value == 1 || value == 0))
            {
                Console.WriteLine("Wrong bit or value!");
                return -1;
            }

            // ADDR = 800
            ushort addr = (ushort)(ADDR_ARM_IO_BASE + bit);

            master.WriteSingleCoil(0, addr, value == 1 ? true : false);

            return 0;
        }

        // public int SetArmDqValue(ushort bit, ushort value)
        // {
        //     //const ushort addr = 800;
        //     const ushort num = 3;

        //     if(bit > num || !(value == 1 || value == 0))
        //     {
        //         Console.WriteLine("Wrong bit or value!");
        //         return -1;
        //     }
        // ADDR = 800
        //     bool[] b = master.ReadCoils(0, ADDR_ARM_IO_BASE, num+1);
        //     b[bit] = value == 1 ? true: false;

        //     master.WriteMultipleCoils(0, bit, b);

        //     return b[bit]? 1:0;
        // }


        public int SetCabinDqValue(ushort bit, ushort value)
        {
            //ushort baseAddr = 0x0;
            ushort num = 3;

            if (bit > num || !(value == 1 || value == 0))
            {
                Console.WriteLine("Wrong bit or value!");
                return -1;
            }

            // ADDR = 0;
            ushort addr = (ushort)(ADDR_CABIN_IO_BASE + bit);

            master.WriteSingleCoil(0, addr, value == 1 ? true : false);

            return 0;
        }

        #endregion

        #region Control Stick Functions:

        public ushort GetOverride()
        {
            //ushort addr = 7101;
            // ADDR = 7101
            return master.ReadInputRegisters(0, ADDR_STICK_OVERRIDE, 1)[0];
        }

        public ushort GetCurrentMode()
        {
            //ushort addr = 7102;
            // ADDR = 7102
            return master.ReadInputRegisters(0, ADDR_STICK_MANUAL_AUTO_MODE, 1)[0];
        }

        private void PressButton(ushort addr)
        {
            master.WriteSingleCoil(0, addr, true);
            Thread.Sleep(500);
            master.WriteSingleCoil(0, addr, false);
        }

        public void PressPlayPauseButton()
        {
            //ushort addr = 7104;
            // ADDR = 7104
            PressButton(ADDR_STICK_PLAYPAUSE);
        }

        public void PressStopButton()
        {
            //ushort addr = 7105;
            // ADDR = 7105
            PressButton(ADDR_STICK_STOP);
        }

        public void PressStickPlusButton()
        {
            //ushort addr = 7106;
            // ADDR = 7106
            PressButton(ADDR_STICK_STICKPLUS);
        }

        public void PressStickMinusButton()
        {
            //ushort addr = 7107;
            // ADDR = 7107
            PressButton(ADDR_STICK_STICKMINUS);
        }

        public bool IsError()
        {
            // ADDR = 7201
            ushort[] temp = master.ReadInputRegisters(0, ADDR_STATUS_PROJ_ERROR, 1);
            return temp[0] == 1 ? true : false;
        }

        #endregion

        #region[Modbus Reg Functions: Reg-read operations]

        /// <summary>
        /// 从地址获得寄存器的值
        /// </summary>
        /// <param name="addr"> 地址 </param>
        /// <returns></returns>
        private float GetValue(ushort addr)
        {
            ushort[] Data = master.ReadInputRegisters(0, addr, 2);
            return CommonTools.GetFloatFromDoubleWord(Data[0], Data[1]);
        }

        /// <summary>
        /// 获得World坐标系下X的值；
        /// </summary>
        /// <returns></returns>
        public float GetX()
        {
            //ushort addr = 7001;
            // ADDR = 7001
            return GetValue(ADDR_REG_X);
        }

        /// <summary>
        /// 获得World坐标系下Y的值；
        /// </summary>
        /// <returns></returns>
        public float GetY()
        {
            //ushort addr = 7003;
            // ADDR = 7003
            return GetValue(ADDR_REG_Y);
        }

        /// <summary>
        /// 获得World坐标系下Z的值；
        /// </summary>
        /// <returns></returns>
        public float GetZ()
        {
            //ushort addr = 7005;
            // ADDR = 7005
            return GetValue(ADDR_REG_Z);
        }

        /// <summary>
        /// 获得World坐标系下Rx的值；
        /// </summary>
        /// <returns></returns>
        public float GetRx()
        {
            //ushort addr = 7007;
            // ADDR = 7007
            return GetValue(ADDR_REG_RX);
        }

        /// <summary>
        /// 获得World坐标系下Ry的值；
        /// </summary>
        /// <returns></returns>
        public float GetRy()
        {
            //ushort addr = 7009;
            // ADDR = 7009
            return GetValue(ADDR_REG_RY);
        }

        /// <summary>
        /// 获得World坐标系下Rz的值；
        /// </summary>
        /// <returns></returns>
        public float GetRz()
        {
            //ushort addr = 7011;
            // ADDR = 7011
            return GetValue(ADDR_REG_RZ);
        }

        /// <summary>
        /// 获得J1；
        /// </summary>
        /// <returns></returns>
        public float GetJ1()
        {
            //ushort addr = 7013;
            // ADDR = 7013
            return GetValue(ADDR_REG_J1);
        }

        /// <summary>
        /// 获得J2;
        /// </summary>
        /// <returns></returns>
        public float GetJ2()
        {
            //ushort addr = 7015;
            // ADDR = 7015
            return GetValue(ADDR_REG_J2);
        }

        /// <summary>
        /// 获得J3;
        /// </summary>
        /// <returns></returns>
        public float GetJ3()
        {
            //ushort addr = 7017;
            // ADDR = 7017
            return GetValue(ADDR_REG_J3);
        }

        /// <summary>
        /// 获得J4；
        /// </summary>
        /// <returns></returns>
        public float GetJ4()
        {
            //ushort addr = 7019;
            // ADDR = 7019
            return GetValue(ADDR_REG_J4);
        }

        /// <summary>
        /// 获得J5；
        /// </summary>
        /// <returns></returns>
        public float GetJ5()
        {
            //ushort addr = 7021;
            // ADDR = 7021
            return GetValue(ADDR_REG_J5);
        }

        /// <summary>
        /// 获得J6;
        /// </summary>
        /// <returns></returns>
        public float GetJ6()
        {
            //ushort addr = 7023;
            // ADDR = 7023
            return GetValue(ADDR_REG_J6);
        }

        /// <summary>
        /// 获得当前的J1-J6坐标，生数据，不进行任何转化；
        /// </summary>
        /// <returns></returns>
        private ushort[] GetRawJointValues()
        {
            //const ushort addr = 7013;
            const ushort num = 12;

            // ADDR = 7013
            ushort[] temp = master.ReadInputRegisters(0, ADDR_REG_BASE + 13, num);
            return temp;
        }

        /// <summary>
        /// 将寄存器中读回的坐标值转换成浮点数；
        /// </summary>
        /// <returns></returns>
        public float[] GetCoordinationValue()
        {
            //const ushort addr = 7001;

            const ushort num = 6;

            float[] coordinates = { 0, 0, 0, 0, 0, 0 };

            // ADDR = 7001
            ushort[] temp = master.ReadInputRegisters(0, ADDR_REG_BASE + 1, num);
            for (int i = 0; i < num; i++)
            {
                coordinates[i] = CommonTools.GetFloatValue(temp[i]);
            }

            return coordinates;
        }

        /// <summary>
        /// 判断机械臂是否静止
        /// </summary>
        /// <param name="tol"> 精度 </param>
        /// <returns> true = 静止 false = 未静止 </returns>
        public bool StandStill(ushort tol = 0)
        {
            ushort[] joints = GetRawJointValues();
            Thread.Sleep(100);
            ushort[] jointsOld = new ushort[12];

            for (int i = 0; i < jointsOld.Length; i++)
            {
                jointsOld[i] = joints[i];
            }

            joints = GetRawJointValues();

            for (int i = 0; i < joints.Length; i++)
            {
                if (Math.Abs(joints[i] - jointsOld[i]) <= tol)
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
        public void SendToListenNode(byte[] data, int tick = 1)
        {

            try
            {
                sock.Send(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                try
                {
                    sock.Close();
                }
                catch { }
                InitListenNodeSocketConnection();
                if (tick > 0)// 进行重试
                {
                    SendToListenNode(data, --tick);
                }
                else {
                    throw e;
                }
            }
        }

        public string RecvFromListenNode(int tick = 1)
        {
            byte[] data = new byte[1024];
            try
            {
                //return sock.Receive(data).ToString();
                int length = sock.Receive(data);
                string msg = System.Text.Encoding.ASCII.GetString(data);
                return msg.Substring(0, length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                try
                {
                    sock.Close();
                }
                catch { }
                InitListenNodeSocketConnection();
                if (tick > 0)// 进行重试
                {
                    return RecvFromListenNode(--tick);
                }
                else
                {
                    throw e;
                }
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
        /// 组织运动命令字符串
        /// </summary>
        /// <param name="type"> PTP or Line</param>
        /// <param name="motion"> CAP, CPP or JPP</param>
        /// <param name="speed"> 速度 </param>
        /// <param name="coordinates"> 目标坐标 </param>
        /// <param name="sRate"> 不精确率 </param>
        /// <param name="imprecise"> 不精确方式到达 </param>
        /// <returns> 成功返回0， 输入有误返回-1， 设备反馈错误信息返回-10</returns>
        private int MakeCommand(string type, string motion, ushort speed, float[] coordinates, ushort sRate = 0, bool imprecise = false)
        {
            const String head = "TMSCT";
            String data = null;
            String crc = null;
            String temp_imprecise = imprecise ? "true" : "false";

            if (!(type.Equals("PTP") || type.Equals("Line") || type.Equals("Move_PTP") || type.Equals("Move_Line") || motion.Equals("CPP") || motion.Equals("CAP") || motion.Equals("JPP")))
            {
                return -1;
            }
            else
            {
                data = "1," + type + "(\"" + motion + "\","
                    + coordinates[0].ToString() + ","
                    + coordinates[1].ToString() + ","
                    + coordinates[2].ToString() + ","
                    + coordinates[3].ToString() + ","
                    + coordinates[4].ToString() + ","
                    + coordinates[5].ToString() + ","
                    + speed.ToString() + ","
                    + "1" + ","
                    + sRate.ToString() + ","
                    + temp_imprecise
                    + ")";
            }

            String crcBody = head + "," + data.Length.ToString() + "," + data + ",";
            crc = CommonTools.CRC(Encoding.Default.GetBytes(crcBody));

            String cmd = "$" + crcBody + "*" + crc + "\r\n";
            this.SendToListenNode(Encoding.Default.GetBytes(cmd));

            String buffer = this.RecvFromListenNode();
            Console.WriteLine(buffer);

            if (buffer.StartsWith("$TMSCT,4,1,OK,*5C"))
            {
                return 0;
            }
            else
            {
                return -10;
            }

        }
        /// <summary>
        /// 向 Listen Node 发送命令;
        /// </summary>
        /// <param name="type">PTP 或 Line</param>
        /// <param name="speed">输入一个整形量表示最大速度的百分比， 例如"20"代表 20%速度</param>
        /// <param name="coordinates">一个6维向量表示坐标，格式为{x, y, z, Rx, Ry, Rz }</param>
        /// <param name="sRate"> 不精确到达时，sRate值越大，过渡曲率越大，离中间过渡点越远</param>
        /// <param name="imprecise"> 不精确到达 </param>
        /// <returns> -1 表示输入有误， 0 表示成功</returns>

        public int SendCmd(String type, ushort speed, float[] coordinates, ushort sRate = 0, bool imprecise = false)
        {

            if (type.Equals("PTP"))
            {
                return MakeCommand(type, "CPP", speed, coordinates, sRate, imprecise);
            }
            else if (type.Equals("Line"))
            {
                return MakeCommand(type, "CAP", speed, coordinates, sRate, imprecise);
            }
            else
            {
                return -20;
            }

            #region Old function, to be delete later!
            /*
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
            */
            #endregion
        }

        /// <summary>
        /// 绝对坐标移轴
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="coordinates"></param>
        /// <param name="sRate"> 不精确到达时，sRate值越大，过渡曲率越大，离中间过渡点越远</param>
        /// <param name="imprecise"> 不精确到达 </param>
        /// <returns></returns>

        public int MoveJointAbso(ushort speed, float[] coordinates, ushort sRate = 0, bool imprecise = false)
        {
            return MakeCommand("PTP", "JPP", speed, coordinates, sRate, imprecise);

            #region Old function, to be deleted
            /*
            const String head = "TMSCT";
            String data = null;
            String crc = null;

            data = "1,PTP(\"JPP\","
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

            if (msg.StartsWith("$TMSCT,4,1,OK,*5C"))
            {
                return 0;
            }
            else
            {
                return -10;
            }
            */
            #endregion
        }
        /// <summary>
        /// 在TM机器人的Word坐标下以相对运动方式移动轴；
        /// 注意：是TM机械臂的Word坐标，不是楼板放线机器人的坐标！！！
        /// </summary>
        /// <param name="speed">速度</param>
        /// <param name="coordinates">轴移动的角度</param>
        /// <param name="sRate"> 不精确到达时，sRate值越大，过渡曲率越大，离中间过渡点越远</param>
        /// <param name="imprecise"> 不精确到达 </param>
        /// <returns>无效指令返回-1， 成功返回0；</returns>
        public int MoveJointRelated(ushort speed, float[] coordinates, ushort sRate = 0, bool imprecise = false)
        {
            return MakeCommand("Move_PTP", "JPP", speed, coordinates, sRate, imprecise);
            #region Old Function, to be deleted
            /*
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
            */
            #endregion
        }


        /// 在TM机器人的Word坐标下以相对运动方式进行直线移动；
        /// 注意：是TM机械臂的Word坐标，不是楼板放线机器人的坐标！！！
        /// </summary>
        /// <param name="speed"> 速度</param>
        /// <param name="coordinates"> 点对当前位置的笛卡尔偏移向量</param>
        /// <param name="sRate"> 不精确到达时，sRate值越大，过渡曲率越大，离中间过渡点越远</param>
        /// <param name="imprecise"> 不精确到达 </param>
        /// <returns>无效指令返回-1， 成功返回0；</returns>

        public int MoveLineRelated(ushort speed, float[] coordinates, ushort sRate = 0, bool imprecise = false)
        {
            return MakeCommand("Move_Line", "CPP", speed, coordinates, sRate, imprecise);
            #region Old function, to be deleted
            /*
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
            */
            #endregion
        }
        #endregion
    }
}
