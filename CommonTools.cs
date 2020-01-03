using System;
using System.Collections.Generic;
using System.Text;

namespace TmRobotArm
{
    public static class CommonTools
    {
        /// <summary>
        /// 将Word中的unsigned short转换成float；
        /// </summary>
        /// <param name="data"> unsigned short </param>
        /// <returns></returns>
        public static float GetFloatValue(ushort data)
        {
            byte[] temp = new byte[4] { 0, 0, 0, 0 };
            byte[] byteData = BitConverter.GetBytes(data);

            Array.Reverse(byteData);

            for (int i = 0; i < byteData.Length; i++)
            {
                temp[i] = byteData[i];
            }

            Array.Reverse(temp);

            return BitConverter.ToSingle(temp, 0);
        }

        public static float GetFloatFromDoubleWord(ushort addrLow, ushort addrHigh)
        {
            byte[] temp = new byte[] { 0, 0, 0, 0 };
            byte[] byteData1 = BitConverter.GetBytes(addrLow);
            byte[] byteData2 = BitConverter.GetBytes(addrHigh);

            Array.Reverse(byteData1);
            Array.Reverse(byteData2);

            for(int i = 0; i< byteData1.Length; ++i)
            {
                temp[i] = byteData1[i];
            }
            for(int i = 0; i< byteData1.Length; ++i)
            {
                temp[i + 2] = byteData1[i];
            }

            Array.Reverse(temp);

            return BitConverter.ToSingle(temp, 0);
        }

        /// <summary>
        /// CRC校验码生成：计算一个字节流的BCC校验码，输出两位hex字符（00-FF)；
        /// </summary>
        /// <param name="hex"> 字节流 </param>
        /// <returns></returns>
        public static String CRC(byte[] hex)
        {
            int temp = 0;
            for(int i = 0; i<hex.Length; ++i)
            {
                temp ^= hex[i];
            }
            return temp.ToString("X2");
        }

    }
}
