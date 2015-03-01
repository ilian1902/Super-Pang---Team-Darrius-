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
            //while (this.Duration > 0)
            //{
            //    this.Duration--;
            //    Thread.Sleep(1000);
            //    if (this.Duration <= 0)
            //    {
            //        if (lives <= 0)
            //        {
            //            timeLeft = 100;
            //            music.Abort();
            //            RestartGame();
            //        }
            //        else
            //        {
            //            lives--;
            //        }
            //    }
            //}
        }

        internal static int GetRemainingTime()
        {
            return duration;
        }

        internal static void Restart()
        {
            duration = 100;
            Task.Run(() =>
                {
                    Start();
                });
        }
    }
}
