using System;

namespace TmRobotFunctionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TmRobot robi = new TmRobot();
            float[] coordinates = new float[] { 1.1F, 2.1F, 3.1F};
            robi.PtpMoveTo(coordinates);
        }
    }
   

}
