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
        /// <param name="sRate"> 不精确到达时，sRate值越大，过渡曲率越大，离中间过渡点越远</param>
        /// <param name="imprecise"> 不精确到达 </param>
        /// <returns> 格式错误返回-1， 正确执行返回0 </returns>
        public int PtpMoveTo(float[] coordinates, ushort speed = 100, ushort sRate = 0, bool imprecise = false)
        {
            if (coordinates.Length != 3 && coordinates.Length != 6)
            {
                Console.WriteLine("Wrong Coordinates!");
                return -1;

            }
            float[] temp = new float[6] { 0.0F, 0.0F, 0.0F, -90.0F, -90.0F, 0.0F };

            for (int i = 0; i < coordinates.Length; i++)
            {
                temp[i] = coordinates[i];
            }

            return driver.SendCmd("PTP", speed, temp, sRate, imprecise);
        }
        /// <summary>
        /// 以Line方式将TM机器人移动至coordinate；
        /// </summary>
        /// <param name="coordinates"> 坐标 </param>
        /// <param name="speed"> 速度 </param>
        /// <param name="sRate"> 不精确到达时，sRate值越大，过渡曲率越大，离中间过渡点越远</param>
        /// <param name="imprecise"> 不精确到达 </param>
        /// <returns> 格式错误返回-1， 正确执行返回0 </returns>
        public int LineMoveTo(float[] coordinates, ushort speed = 500, ushort sRate = 0, bool imprecise = false)
        {
            if (coordinates.Length != 3 && coordinates.Length != 6)
            {
                Console.WriteLine("Wrong Coordinates!");
                return -1;
            }
            float[] temp = new float[6] { 0.0F, 0.0F, 0.0F, -90.0F, -90.0F, 0.0F };
            for (int i = 0; i < coordinates.Length; i++)
            {
                temp[i] = coordinates[i];
            }

            return driver.SendCmd("Line", speed, temp, sRate, imprecise);
        }
        /// <summary>
        /// TM机器人回原点；
        /// </summary>
        /// <param name="speed"> 速度</param>
        public int GoHome(ushort speed = 100)
        {
            float[] temp = new float[6] { 86.49F, -88.28F, 161.65F, -72.45F, 93.04F, 179.85F};
            return driver.MoveJointAbso(speed, temp);
        }
        #endregion

        #region Get Coordinates

        /// <summary>
        /// X坐标
        /// </summary>
        /// <returns></returns>
        public float X() => driver.GetX();

        /// <summary>
        /// Y坐标
        /// </summary>
        /// <returns></returns>
        public float Y() => driver.GetY();

        /// <summary>
        /// Z坐标
        /// </summary>
        /// <returns></returns>
        public float Z() => driver.GetZ();

        /// <summary>
        /// Rx
        /// </summary>
        /// <returns></returns>
        public float Rx() => driver.GetRx();

        /// <summary>
        /// Ry
        /// </summary>
        /// <returns></returns>
        public float Ry() => driver.GetRy();

        /// <summary>
        /// Rz
        /// </summary>
        /// <returns></returns>
        public float Rz() => driver.GetRz();

        /// <summary>
        /// J1
        /// </summary>
        /// <returns></returns>
        public float J1() => driver.GetJ1();

        /// <summary>
        /// J2
        /// </summary>
        /// <returns></returns>
        public float J2() => driver.GetJ2();

        /// <summary>
        /// J3
        /// </summary>
        /// <returns></returns>
        public float J3() => driver.GetJ3();

        /// <summary>
        /// J4
        /// </summary>
        /// <returns></returns>
        public float J4() => driver.GetJ4();
        
        /// <summary>
        /// J5
        /// </summary>
        /// <returns></returns>
        public float J5() => driver.GetJ5();

        /// <summary>
        /// J6
        /// </summary>
        /// <returns></returns>
        public float J6() => driver.GetJ6();


        #endregion

        #region Single Public Joint Movement
        /// <summary>
        /// 旋转第x轴
        /// </summary>
        /// <param name="jointNumb">轴编号1~6</param>
        /// <param name="angle">角度</param>
        /// <param name="speed">速度(百分比)</param>
        /// <param name="sRate"> 不精确到达时，sRate值越大，过渡曲率越大，离中间过渡点越远</param>
        /// <param name="imprecise"> 不精确到达 </param>
        public void RotateJxRelated(int jointNumb, float angle, ushort speed = 100, ushort sRate = 0, bool imprecise = false)
        {
            if (jointNumb < 1 || jointNumb > 6)
            {
                Console.WriteLine("There is no joint-" + jointNumb);
                return;
            }
            float[] temp = new float[6] { 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F };
            temp[jointNumb - 1] = angle;
            driver.MoveJointRelated(speed, temp, sRate, imprecise);
        }
        public void RotateJ1Related(float angle, ushort speed = 100)
        {
            RotateJxRelated(1, angle, speed);
        }
        public void RotateJ2Related(float angle, ushort speed = 100)
        {
            RotateJxRelated(2, angle, speed);
        }
        public void RotateJ3Related(float angle, ushort speed = 100)
        {
            RotateJxRelated(3, angle, speed);
        }
        public void RotateJ4Related(float angle, ushort speed = 100)
        {
            RotateJxRelated(4, angle, speed);
        }
        public void RotateJ5Related(float angle, ushort speed = 100)
        {
            RotateJxRelated(5, angle, speed);
        }
        public void RotateJ6Related(float angle, ushort speed = 100)
        {
            RotateJxRelated(6, angle, speed);
        }

        /// <summary>
        /// 绝对方式移轴
        /// </summary>
        /// <param name="angles"> 各轴目标角度 </param>
        /// <param name="speed"> 移动速度 </param>
        /// <param name="sRate"> 不精确到达时，sRate值越大，过渡曲率越大，离中间过渡点越远</param>
        /// <param name="imprecise"> 不精确到达 </param>
        public void RotateJsAbso(float[] angles, ushort speed = 100, ushort sRate = 0, bool imprecise = false)
        {
            driver.MoveJointAbso(speed, angles, sRate, imprecise);
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

        #region Control Info


        /// <summary>
        /// 报错！
        /// </summary>
        /// <returns></returns>
        public bool IsError()
        {
            return driver.IsError();
        }

        /// <summary>
        /// 就位
        /// </summary>
        /// <returns></returns>
        public bool InPos(ushort tol = 0)
        {
            return driver.StandStill(tol)&&(!IsError());
        }

        #endregion

        #region Public Control Stick Functions
        public void StickPlayPause() => driver.PressPlayPauseButton();
        public void StickStop() => driver.PressStopButton();
        public void StickPlus() => driver.PressStickPlusButton();
        public void StickMinus() => driver.PressStickMinusButton();


        #endregion
    }

}
