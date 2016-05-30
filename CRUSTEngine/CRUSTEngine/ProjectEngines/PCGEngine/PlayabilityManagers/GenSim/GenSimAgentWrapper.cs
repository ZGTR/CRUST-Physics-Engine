using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CRUSTEngine.ProjectEngines.PCGEngine.Generators.GenManagers;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.GenSim
{
    public class GenSimAgentWrapper
    {
        private readonly List<CATimePair> _catPairs = null;
        private bool _isDirRandomized;
        private bool _isRopesRandomized;
        private List<ActionTimePair> _atPairs = null;
        private List<Thread> _cThreads;
        public GenSimAgent WAgent;
        private int MAXTRY;
        private readonly bool _isTestingOn;
        private float bestFitSoFar = float.MaxValue;

        public GenSimAgentWrapper(List<ActionTimePair> atPairs, int maxtry, bool isTestingOn)
        {
            MAXTRY = maxtry;
            _atPairs = atPairs;
            _isTestingOn = isTestingOn;
            _cThreads = new List<Thread>();
        }

        public GenSimAgentWrapper(List<CATimePair> catPairs, bool isDirRandomized, bool isRopesRandomized, int maxtry
            , bool isTestingOn)
        {
            _catPairs = catPairs;
            _isDirRandomized = isDirRandomized;
            _isRopesRandomized = isRopesRandomized;
            MAXTRY = maxtry;
            _isTestingOn = isTestingOn;
            _cThreads = new List<Thread>();
        }

        public void ScatterComps()
        {
            bool succeeded = false;
            int attempts = 0;
            if (_atPairs == null)
            {
                GenSimAgent agent = new GenSimAgent(_catPairs, _isDirRandomized, _isRopesRandomized);
                agent.IsTesting = _isTestingOn;
                agent.Shotter = _isTestingOn ? new EngineShotsManager() : null;
                bool succeed = agent.ExecuteScatter();
                WAgent = agent;
            }
            else
            {
                while (attempts < MAXTRY && !succeeded)
                {
                    //for (int i = 0; i < 3; i++)
                    //{
                        //Thread t = new Thread(new ThreadStart(() =>
                            //{
                                GenSimAgent agent = _atPairs != null
                                            ? new GenSimAgent(_atPairs)
                                            : new GenSimAgent(_catPairs, _isDirRandomized, _isRopesRandomized);
                                agent.IsTesting = _isTestingOn;
                                agent.Shotter = _isTestingOn ? new EngineShotsManager() : null;

                                agent.MAXTRY = MAXTRY;
                                if (agent.ExecuteScatter())
                                {
                                    WAgent = agent;
                                    WAgent.BestCTPPairs = WAgent.CATPairs;
                                    succeeded = true;
                                }
                                else
                                {
                                    var fit = GenSimManager.GetFitness(agent);
                                    if (fit < bestFitSoFar)
                                    {
                                        WAgent = agent;
                                        bestFitSoFar = fit;
                                    }
                                }
                            //}
                        //));
                        //_cThreads.Add(t);
                        //t.Start();
                    //}
                    //while (true)
                    //{
                    //    foreach (Thread thread in _cThreads)
                    //    {
                    //        if (!thread.IsAlive)
                    //        {
                    //            _cThreads.Remove(thread);
                    //        }
                    //    }
                    //    if (_cThreads.Count != 0)
                    //    {
                    //        // wait
                    //        Thread.Sleep(100);
                    //    }
                    //    else
                    //    {
                    //        break;
                    //    }
                    //}
                    //attempts += 3;
                    attempts ++;
                }
            }
        }


    }
}
