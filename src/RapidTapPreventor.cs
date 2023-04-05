using System;
using System.Threading.Tasks;

namespace XamarinSnippets
{
    public static class RapidTapPreventor
    {
        private static readonly object syncLock = new object();


        private static bool isExecutingFunction = false;



        public static async Task TryExecuteAsync(
            Func<Task> function)
        {
            lock (syncLock)
            {
                if (isExecutingFunction)
                {
                    return;
                }


                isExecutingFunction = true;
            }

            try
            {
                await function();
            }
            finally
            {
                lock (syncLock)
                {
                    isExecutingFunction = false;
                }
            }
        }

        public static void TryExecute(
            Action function)
        {
            lock (syncLock)
            {
                if (isExecutingFunction)
                {
                    return;
                }


                isExecutingFunction = true;
            }

            try
            {
                function();
            }
            finally
            {
                lock (syncLock)
                {
                    isExecutingFunction = false;
                }
            }
        }
    }
}