using System;
using log4net;
using log4net.Config;

namespace StructuredLogging.Client.Log4Net.Sample
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        static void Main()
        {
            XmlConfigurator.Configure();

            Logger.InfoFormat("Hello {0}, from log4net!", Environment.UserName);

            try
            {
                throw new DivideByZeroException();
            }
            catch (Exception ex)
            {
                Logger.Error("Something went wrong!", ex);
            }

            Console.WriteLine("Press ANY key to quit.");
            Console.ReadKey();
        }
    }
}