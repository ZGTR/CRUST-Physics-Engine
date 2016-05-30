//using System;
//using System.Collections.Generic;
//using System.IO;
//using Prolog;
//using Prolog.Code;

//namespace CRUSTEngine.ProjectEngines.PCGEngine
//{
//    class RYSEManager
//    {
//        private Prolog.Program _program;
//        private PrologMachine _machine;
//        private CodeSentence _codeSentence;

//        public RYSEManager(string fileName)
//        {
//            _program = Prolog.Program.Load(fileName);
//        }

//        public RYSEManager(string fileName, bool b)
//        {
//            List<string> programS = Readfile(fileName);
//            AddAllRules(programS);
//        }

//        private List<string> Readfile(string fileName)
//        {
//            List<string> lines = new List<string>();
//            using (var sr = new StreamReader(fileName))
//            {
//                String line = String.Empty;
//                while ((line = sr.ReadLine()) != null)
//                {
//                    lines.Add(line);
//                }
//            }
//            return lines;
//        }

//        private void AddAllRules(List<string> prog)
//        {
//            _program = new Prolog.Program();
//            foreach (string r in prog)
//            {
//                _codeSentence = Parser.Parse(r)[0];
//                _program.Add(_codeSentence);
//            }
//        }

//        public void DeleteRule(string rule)
//        {
//            foreach (Procedure procedure in _program.Procedures)
//            {
//                foreach (Clause clause in procedure.Clauses)
//                {
//                    if (clause.CodeSentence.ToString() == rule)
//                    {
//                        procedure.Clauses.Remove(clause);
//                        break;
//                    }
//                }
//            }
//        }

//        public void AddRule(string rule)
//        {
//            _codeSentence = Parser.Parse(rule)[0];
//            _program.Add(_codeSentence);
//        }

//        public void SetGoal(string goal)
//        {
//            _codeSentence = Parser.Parse(goal)[0];
//            Query query = new Query(_codeSentence);
//            _machine = PrologMachine.Create(_program, query);
//        }

//        public string GetResults()
//        {
//            ExecutionResults results = _machine.RunToSuccess();
//            string ans = results.ToString();
//            if (ans == "Success")
//            {
//                if (_machine.QueryResults.Variables.Count != 0)
//                {
//                    ans = "";
//                    foreach (PrologVariable variable in _machine.QueryResults.Variables)
//                    {
//                        ans += variable.Text + ' ';
//                    }
//                    ans = ans.Remove(ans.Length - 1, 1);

//                    while (_machine.CanRunToBacktrack)
//                    {
//                        _machine.RunToSuccess();
//                        foreach (PrologVariable variable in _machine.QueryResults.Variables)
//                        {
//                            ans += ' ' + variable.Text;
//                        }
//                    }
//                }
//                ans = FixAnswer(ans, _machine.QueryResults.Variables.Count);
//                return ans;
//            }
//            else
//            {
//                return ans;
//            }
//        }

//        private string FixAnswer(string strAns, int count)
//        {
//            string ans = null;
//            if (strAns == "Success")
//                return strAns;
//            else
//            {
//                string[] answerWords = strAns.Split(' ');
//                ans = answerWords[0];
//                int i = 1;
//                while (i != answerWords.Length - count)
//                {
//                    ans += ' ' + answerWords[i];
//                    i++;
//                }
//                return ans;
//            }
//        }
//    }
//}
