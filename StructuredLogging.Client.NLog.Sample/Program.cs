using System;
using NLog;

namespace StructuredLogging.Client.NLog.Sample
{
    class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static void Main()
        {
            Logger.Info("Hello {0}, from NLog!", Environment.UserName);

            try
            {
                throw new DivideByZeroException();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Something went wrong!");
            }

            Console.WriteLine("Press ANY key to quit.");
            Console.ReadKey();
        }
    }
}