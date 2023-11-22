using CPULogMonitor.Models;
using System;
using System.ServiceProcess;
using System.Timers;

namespace CPULogMonitor
{
    public partial class CPULogMonitorService : ServiceBase
    {
        private Timer _timer;
        private readonly Logger _logger;
        private readonly DataCollector _dataCollector;
        public double sensorInterval = 6000; // default: 6000 = 1 minute

        public CPULogMonitorService()
        {
            _logger = new Logger($"Logs/CPUMonitor_{DateTime.Today}");
            _dataCollector = new DataCollector(_logger);
        }

        protected override void OnStart(string[] args)
        {
            _logger.WriteToFile($"{DateTime.Now}: Service started!");

            _timer = new Timer();
            _timer.Interval = sensorInterval;
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            CPUDataModel data = _dataCollector.CollectData();
        }

        protected override void OnStop()
        {
            _timer.Stop();
            _logger.WriteToFile($"{DateTime.Now}: Service stopped!");
        }
    }
}
