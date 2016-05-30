using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRUSTEngine.Database
{
    class DatabaseHandler
    {
        public static DatabaseLinqDataContext STYX_DB = new DatabaseLinqDataContext(@"Data Source=ZGTR-PC\SQLEXPRESS;Initial Catalog=STYX-PCG;Integrated Security=True;Pooling=False");


        public static void InsertToPlayabilityTestTable(int patternNr, bool playability, int prologTime, int totalTime, string distance, int maxDepthArr, int nodesExplored, 
            string strLevel, int nrOfActions, string actionsString)
        {
            // Add the new object to the Orders collection.
            STYX_DB.PlayabilityTests.InsertOnSubmit(new PlayabilityTest()
                                                        {
                                                            Pattern = patternNr,
                                                            Playability = playability.ToString(),
                                                            PrologTime =  prologTime,
                                                            TotalTime = totalTime,
                                                            ClosestFCDistance = Convert.ToDouble(distance),
                                                            MaxDepthReached = maxDepthArr,
                                                            NodesExplored = nodesExplored,
                                                            LevelString = strLevel,
                                                            BestActions = actionsString,
                                                            NrOfActions = nrOfActions
                                                        });

            // Submit the change to the database.
            try
            {
                STYX_DB.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Make some adjustments.
                // ...
                // Try again.
                STYX_DB.SubmitChanges();
            }
        }

        public static void InsertToPlayabilityTestTable(string patternNr,
            string playability, string prologTime, string totalTime, string distance, string maxDepthArr, string nodesExplored, string strLevel, string nrOfActions, string actionsString)
        {
            // Add the new object to the Orders collection.
            STYX_DB.PlayabilityTests.InsertOnSubmit(new PlayabilityTest()
            {
                Pattern = Int32.Parse(patternNr),
                Playability = playability,
                PrologTime = Int32.Parse(prologTime),
                TotalTime = Int32.Parse(totalTime),
                ClosestFCDistance = Convert.ToDouble(distance),
                MaxDepthReached = Int32.Parse(maxDepthArr),
                NodesExplored = Int32.Parse(nodesExplored),
                LevelString = strLevel,
                BestActions = actionsString,
                NrOfActions = Int32.Parse(nrOfActions)
            });

            // Submit the change to the database.
            try
            {
                STYX_DB.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Make some adjustments.
                // ...
                // Try again.
                STYX_DB.SubmitChanges();
            }
        }
    }
}
