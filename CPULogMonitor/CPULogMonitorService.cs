using CPULogMonitor.Models;
using System;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;

namespace CPULogMonitor
{
    public partial class CPULogMonitorService : ServiceBase
    {
        private readonly Logger _logger;
        private readonly TcpClientManager _tcpManager;
        private readonly DataCollector _dataCollector;

        private Timer _reconectTimer;
        private Timer _sensorTimer;
        public double reconnectInterval = 1000;
        public double sensorInterval = 6000;
        public CPULogMonitorService()
        {
            _logger = new Logger($"Logs/CPUMonitor_{DateTime.Today}");
            _tcpManager = new TcpClientManager(_logger);
            _dataCollector = new DataCollector(_logger);
        }

        protected override void OnStart(string[] args)
        {
            _logger.WriteToFile($"{DateTime.Now}: Service started!");

            _reconectTimer = new Timer();
            _reconectTimer.Interval = reconnectInterval;
            _reconectTimer.Elapsed += OnReconectTimerElapsed;
            _reconectTimer.Start();

            _sensorTimer = new Timer();
            _sensorTimer.Interval = sensorInterval;
            _sensorTimer.Elapsed += OnSensorTimerElapsed;
            _sensorTimer.Start();

            _tcpManager.ConnectToServer();
        }

        private void OnSensorTimerElapsed(object sender, ElapsedEventArgs e)
        {
            CPUDataModel data = _dataCollector.CollectData();
            _logger.WriteToFile($"{DateTime.Now}: Sensor Timer Elapsed Interval: {_sensorTimer.Interval}!");
        }

        private void OnReconectTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!_tcpManager.Connected)
            {
                _logger.WriteToFile($"{DateTime.Now}: Reconnecting...");
                _tcpManager.ConnectToServer();
            }
            if (_tcpManager.SensorInterval != _sensorTimer.Interval)
            {
                _sensorTimer.Interval = _tcpManager.SensorInterval;
            }
        }

        protected override void OnStop()
        {
            _sensorTimer.Stop();
            _reconectTimer.Stop();
            _tcpManager.Dispose();
            _logger.WriteToFile($"{DateTime.Now}: Service stopped!");
        }
    }
}
