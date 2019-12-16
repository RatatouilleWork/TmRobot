using System;

namespace TmRobotArm
{
    public class TmRobot
    {
        TmRobotDriver driver;

        #region[Constructors]
        public TmRobot()
        {
            Console.WriteLine("TM_Robot is constructed:");
            Console.WriteLine("Connections to Control Babin are 2 links:");
            Console.WriteLine("1. Modbus: ServerIP = 192.168.1.120, Port = 502");
            Console.WriteLine("2. Tcp/Ip: ServerIP = 192.168.1.120, Port = 5980");
            Console.WriteLine("Achtung: ports are fixed, please do not change!");

            driver = new TmRobotDriver();

            driver.CreateMaster();
            driver.InitListenNodeSocketConnection();
        }
        #endregion

        #region Public MoveTo
        /// <summary>
        /// 以PTP方式将TM机器人移动至coordinate位置；
        /// </summary>
        /// <param name="coordinates"> 坐标 </param>
        /// <param name="speed"> 速度 </param>
        /// <returns> 格式错误返回-1， 正确执行返回0 </returns>
        public int PtpMoveTo(float[] coordinates, ushort speed = 50)
        {
            if (coordinates.Length != 3)
            {
                Console.WriteLine("Wrong Coordinates!");
                return -1;

            }
            float[] temp = new float[6] { 0.0F, 0.0F, 0.0F, -90.0F, -90.0F, 0.0F };

            for (int i = 0; i < coordinates.Length; i++)
            {
                temp[i] = coordinates[i];
            }

            return driver.SendCmd("PTP", speed, temp);
        }
        /// <summary>
        /// 以Line方式将TM机器人移动至coordinate；
        /// </summary>
        /// <param name="coordinates"> 坐标 </param>
        /// <param name="speed"> 速度 </param>
        /// <returns> 格式错误返回-1， 正确执行返回0 </returns>
        public int LineMoveTo(float[] coordinates, ushort speed = 50)
        {
            if (coordinates.Length != 3)
            {
                Console.WriteLine("Wrong Coordinates!");
                return -1;
            }
            float[] temp = new float[6] { 0.0F, 0.0F, 0.0F, -90.0F, -90.0F, 0.0F };
            for (int i = 0; i < coordinates.Length; i++)
            {
                temp[i] = coordinates[i];
            }

            return driver.SendCmd("Line", speed, temp);
        }
        /// <summary>
        /// TM机器人回原点；
        /// </summary>
        /// <param name="speed"> 速度</param>
        public int GoHome(ushort speed = 50)
        {
            return driver.SendCmd("PTP", speed, new float[] { 150.0F, 0.0F, 450.0F, -90.0F, 0.0F, 0.0F });
        }
        #endregion

        /// <summary>
        /// 就位
        /// </summary>
        /// <param name="coordinates"> 坐标</param>
        /// <returns></returns>
        public bool InPos(float[] coordinates)
        {
            return driver.StandStill();
        }

        #region Single Public Joint Movement
        /// <summary>
        /// 旋转第x轴
        /// </summary>
        /// <param name="jointNumb">轴编号1~6</param>
        /// <param name="angle">角度</param>
        /// <param name="speed">速度(百分比)</param>
        public void RotateJX(int jointNumb, float angle, ushort speed = 50)
        {
            if (jointNumb < 1 || jointNumb > 6)
            {
                Console.WriteLine("There is no joint-" + jointNumb);
                return;
            }
            float[] temp = new float[6] { 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F };
            temp[jointNumb - 1] = angle;
            driver.MoveJointRelated(speed, temp);
        }
        public void RotateJ1(float angle, ushort speed = 50)
        {
            RotateJX(1, angle, speed);
        }
        public void RotateJ2(float angle, ushort speed = 50)
        {
            RotateJX(2, angle, speed);
        }
        public void RotateJ3(float angle, ushort speed = 50)
        {
            RotateJX(3, angle, speed);
        }
        public void RotateJ4(float angle, ushort speed = 50)
        {
            RotateJX(4, angle, speed);
        }
        public void RotateJ5(float angle, ushort speed = 50)
        {
            RotateJX(5, angle, speed);
        }
        public void RotateJ6(float angle, ushort speed = 50)
        {
            RotateJX(6, angle, speed);
        }

        #endregion

        #region Public 放线设备
        public void TurnOnLayout()
        {
            driver.SetArmDqValue(0, 1);
        }
        public void TurnOffLayout()
        {
            driver.SetArmDqValue(0, 0);
        }
        #endregion

        public bool IsError()
        {
            return driver.IsError();
        }
    }

}
