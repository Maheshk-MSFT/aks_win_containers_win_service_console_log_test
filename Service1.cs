using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WindowsService3Mikky
{
    public partial class Service1 : ServiceBase
    {
        private Timer _timer;
        private Random _random;

        private string filePath = string.Empty;
        private string content = string.Empty;

        public Service1()
        {
            InitializeComponent();
            _random = new Random();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                filePath = @"C:\app\log.txt";
                content = "My super app started....OnStart";

                File.AppendAllText(filePath, content);
                
                Console.WriteLine(content);

                _timer = new Timer();
                _timer.Interval = 10000;
                _timer.Elapsed += OnTimerElapsed;
                _timer.AutoReset = true;
                _timer.Enabled = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in OnStart: {ex.Message}");
                File.AppendAllText(filePath, $"Exception in OnStart: {ex.Message}");
            }
            File.AppendAllText(filePath, "done");

        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                Console.WriteLine("Timer elapsed: " + DateTime.Now);
                File.AppendAllText(filePath, "Timer elapsed: " + DateTime.Now);

                ThrowRandomException();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in OnTimerElapsed: {ex.Message}");
            }
        }

        protected override void OnStop()
        {
            try
            {
                Console.WriteLine("bye-bye!  OnStop");
                File.AppendAllText(filePath, "bye-bye!  OnStop");

                if (_timer != null)
                {
                    _timer.Stop();
                    _timer.Dispose();
                }

                // Intentionally throw a random exception to test exception handling
                ThrowRandomException();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in OnStop: {ex.Message}");
                File.AppendAllText(filePath, $"Exception in OnStop: {ex.Message}");
                // Handle exception (e.g., log it)
            }
        }

        private void ThrowRandomException()
        {
            int exceptionType = _random.Next(8);
            switch (exceptionType)
            {
                case 0:
                    throw new InvalidOperationException("Random InvalidOperationException");
                case 1:
                    throw new ArgumentNullException("Random ArgumentNullException");
                case 2:
                    throw new NotImplementedException("Random NotImplementedException");
                case 3:
                    throw new Exception("Random General exception");
                default:
                    Console.WriteLine($"You are lucky this time!");
                    File.AppendAllText(filePath, $"You are lucky this time!");
                    break;
            }

        }
    }
}
