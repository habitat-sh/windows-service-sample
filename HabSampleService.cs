using System.Configuration;
using System.IO;
using System.Reflection;
using System.ServiceProcess;

namespace HabitatSample
{
    public partial class HabSampleService : ServiceBase
    {
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new HabSampleService()
            };
            ServiceBase.Run(ServicesToRun);
        }

        public HabSampleService()
        {
            ServiceName = "HabSampleService";
            CanStop = true;
            AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            WriteStatus("HabSampleService is starting");
            System.Timers.Timer timer = new System.Timers.Timer
            {
                Interval = 60000 // 60 seconds  
            };
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
            WriteStatus("HabSampleService is started");
        }

        protected override void OnStop()
        {
            WriteStatus("HabSampleService is stopped");
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            WriteStatus("HabSampleService is up");
        }

        private static void WriteStatus(string message)
        {
            var statusFileDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (ConfigurationManager.AppSettings["statusFileDir"] != null)
            {
                statusFileDir = ConfigurationManager.AppSettings["statusFileDir"];
            }
            using (FileStream fs = new FileStream(Path.Combine(statusFileDir, "status.txt"), FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8))
                {
                    sw.WriteLine(message);
                }
            }
        }

    }
}
