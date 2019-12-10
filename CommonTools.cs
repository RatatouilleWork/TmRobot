using System;
using System.Collections.Generic;
using System.Text;

namespace TmRobotFunctionTest
{
    public static class CommonTools
    {
        public static float ToFloat(byte[] data)
        {
            unsafe
            {
                float f = 0.0F;
                byte i;
                byte[] x = data;
                void* pf;
                fixed (byte* px = x)
                {
                    pf = &f;
                    for (i = 0; i < data.Length; i++)
                    {
                        *((byte*)pf + i) = *(px + i);
                    }
                }
                return f;
            }
        }
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

            return ToFloat(temp);
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
