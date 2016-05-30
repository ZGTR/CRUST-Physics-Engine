using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRUSTEngine.ProjectEngines.PCGEngine
{
    class StringHelper
    {
        public static string[] GetPropperPhenoArrWithTerminals(string[] pStringArr)
        {
            List<String> tempStrArr = new List<string>(pStringArr);
            for (int i = 0; i < pStringArr.Count(); i++)
            {
                String currentStr = pStringArr[i];
                int indParan = currentStr.IndexOf('(');
                if (indParan > 0)
                {
                    int indLastSpace = currentStr.Substring(0, indParan - 1).LastIndexOf(' ');
                    if (indParan > indLastSpace && indLastSpace > 0)
                    {
                        // Save index
                        int index = tempStrArr.IndexOf(currentStr);
                        // Remove the wrongly added string
                        tempStrArr.Remove(currentStr);
                        // there are a dilimeters - add them
                        String[] dilimetersArr = currentStr.Substring(0, indLastSpace).Trim().Split(' ');
                        tempStrArr.InsertRange(index, dilimetersArr);
                        // Add the non-terminal part
                        tempStrArr.Insert(index + dilimetersArr.Count(),
                                          currentStr.Substring(indLastSpace));
                    }
                }
                else
                {
                    // Save index
                    int index = tempStrArr.IndexOf(currentStr);
                    // Remove the wrongly added string
                    tempStrArr.Remove(currentStr);
                    // there are a dilimeters - add them
                    String[] dilimetersArr = currentStr.Substring(0).Trim().Split(' ');
                    tempStrArr.InsertRange(index, dilimetersArr);
                }
            }
            tempStrArr.RemoveAll(EmptyOrSpace);
            return tempStrArr.ToArray();
        }

        private static bool EmptyOrSpace(String s)
        {
            return s == "" || s == " ";
        }
    }
}
