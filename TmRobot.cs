using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using NModbus;

namespace TmRobotFunctionTest
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

            driver= new TmRobotDriver();

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
            if(coordinates.Length != 3)
            {
                Console.WriteLine("Wrong Coordinates!");
                return -1;

            }
            float[] temp = new float[6] {0.0F, 0.0F, 0.0F, -90.0F, -90.0F, 0.0F};

            for (int i = 0; i< coordinates.Length; i++)
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
            if(coordinates.Length != 3)
            {
                Console.WriteLine("Wrong Coordinates!");
                return -1;
            }
            float[] temp = new float[6] {0.0F, 0.0F, 0.0F, -90.0F, -90.0F, 0.0F};
            for (int i = 0; i< coordinates.Length; i++)
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
        public void RotateJ1(float angle, ushort speed = 50)
        {
            float[] temp= new float[6] { angle, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F };
            driver.MoveJointRelated(speed, temp);
        }
        public void RotateJ2(float angle, ushort speed = 50)
        {
            float[] temp= new float[6] { 0.0F, angle, 0.0F, 0.0F, 0.0F, 0.0F };
            driver.MoveJointRelated(speed, temp);
        }
        public void RotateJ3(float angle, ushort speed = 50)
        {
            float[] temp= new float[6] { 0.0F, 0.0F, angle, 0.0F, 0.0F, 0.0F };
            driver.MoveJointRelated(speed, temp);
        }
        public void RotateJ4(float angle, ushort speed = 50)
        {
            float[] temp= new float[6] { 0.0F, 0.0F, 0.0F, angle, 0.0F, 0.0F };
            driver.MoveJointRelated(speed, temp);
        }
        public void RotateJ5(float angle, ushort speed = 50)
        {
            float[] temp= new float[6] { 0.0F, 0.0F, 0.0F, 0.0F, angle, 0.0F };
            driver.MoveJointRelated(speed, temp);
        }
        public void RotateJ6(float angle, ushort speed = 50)
        {
            float[] temp= new float[6] { 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, angle };
            driver.MoveJointRelated(speed, temp);
        }

        #endregion

        #region Public Bit Function
        public void TurnOnLayout()
        {
            driver.SetArmDqValue(0, 1);
        }
        public void TurnOffLayout()
        {
            driver.SetArmDqValue(0, 0);
        }

        #endregion


    }

}
