﻿using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace CPULogMonitor
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new CPULogMonitorService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
