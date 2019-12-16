using System;
using System.Collections.Generic;
using System.Text;

namespace TmRobotArm
{
    public static class CommonTools
    {
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
