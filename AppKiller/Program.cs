using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using System.Timers;
using Timer = System.Threading.Timer;

namespace AppKiller
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Monitoring CRUST Engine for low CPU usage - Stackoverflow by Clipper");
            Timer t = new Timer(TimerCallback, null, 0, 1000);
            
            Thread.Sleep(TimeSpan.FromHours(5));
        }

        private static int _counter = 0;
        private static void TimerCallback(object sender)
        {
            string procName = "CRUSTEngine";

            Process[] runningNow = Process.GetProcesses();
            foreach (Process process in runningNow)
            {
                if (process.ProcessName == procName)
                {
                    using (
                        PerformanceCounter pcProcess = new PerformanceCounter("Process", "% Processor Time",
                                                                              process.ProcessName))
                    {
                        try
                        {
                            pcProcess.NextValue();
                            System.Threading.Thread.Sleep(200);
                            //Console.WriteLine("Process:{0} CPU% {1}", process.ProcessName, pcProcess.NextValue());
                            //float val = pcProcess.NextValue();
                            if (pcProcess.NextValue() < float.Parse("2"))
                            {
                                _counter++;
                            }
                            else
                            {
                                _counter = 0;
                            }
                            if (_counter > 7)
                            {
                                StreamWriter sw = new StreamWriter(@"C:\CTREngine\PlayabilityVal_ZGTREngine.txt");
                                sw.WriteLine("1000");
                                sw.Close();

                                Console.WriteLine(string.Format("Killing {0} at {1}", procName,
                                                                DateTime.Now.ToString()));
                                process.Kill();
                                _counter = 0;
                            }
                        }
                        catch (Exception)
                        {
                            _counter = 0;
                        }
                    }
                }
            }
            GC.Collect();
        }


    }
}