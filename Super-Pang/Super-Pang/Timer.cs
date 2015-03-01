namespace SuperPang
{
    using System.Threading;
    using System.Threading.Tasks;

    public static class Timer
    {
        private static int duration = 100;

        public static void Start()
        {
            while (duration > 0)
            {
                duration--;
                Thread.Sleep(1000);
            }
        }

        internal static int GetRemainingTime()
        {
            return duration;
        }

        internal static void Restart()
        {
            duration = 100;
        }
    }
}
