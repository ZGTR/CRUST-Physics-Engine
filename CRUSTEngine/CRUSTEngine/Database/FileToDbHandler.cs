using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CRUSTEngine.ProjectEngines.PCGEngine;

namespace CRUSTEngine.Database
{
    class FileToDbHandler
    {
        public static void FileToDB(String path, int patternNr)
        {
            StreamReader sr = new StreamReader(path);
            String level = String.Empty;
            while ((level = sr.ReadLine()) != null)
            {
                String[] parameters = level.Split('\t');
                DatabaseHandler.InsertToPlayabilityTestTable(patternNr.ToString(),
                    parameters[0],
                    parameters[1],
                    parameters[2],
                    parameters[3],
                    parameters[4],
                    parameters[5],
                    parameters[6],
                    GetNumberOfActions(parameters[7]).ToString(),
                    parameters[7]
                    );
            }
        }

        private static int GetNumberOfActions(string actions)
        {
            ActionsGenerator actionsGenerator = new ActionsGenerator(actions);
            return actionsGenerator.Actions.Count;
        }
    }
}
