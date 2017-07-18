namespace WebApi.Extensions
{
    using System;

    public class Number
    {
        private static int seed = DateTime.Now.Millisecond;
        static Random rand = new Random(DateTime.Now.Millisecond);

        public static double Rnd()
        {
            return rand.NextDouble();
        }
    }
}