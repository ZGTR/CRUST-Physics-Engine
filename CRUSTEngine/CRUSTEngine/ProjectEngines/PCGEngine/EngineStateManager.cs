using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Bubble;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Blower;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.CatchableRopes;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs;

namespace CRUSTEngine.ProjectEngines.PCGEngine
{
    public static class EngineStateManager
    {
        public static string GetEngineStateFactStringWithEnterDelimiterToProlog()
        {
            String cookieString = GetCookieProlog();
            String velocity = GetCookieVelocity();
            String frogString = GetFrogProlog();
            String allRopesString = GetAllRopesProlog();
            String allBlowersString = GetAllBlowersProlog();
            String allBubbles = GetAllBubblesCatchedAndVoidProlog();
            String allRockets = GetAllRocketsProlog();
            String allBump = GetAllBumpsProlog();
            String stateString = cookieString + velocity + frogString + allRopesString + allBlowersString + allBubbles + allRockets +
                   allBump;
            return stateString;
        }

        public static string GetEngineStateFactStringWithEnterDelimiterToProlog(List<Point> listOfInterest)
        {
            //String cookieString = GetCookieProlog();
            String frogString = GetFrogProlog(listOfInterest);
            String allRopesString = GetAllRopesProlog(listOfInterest);
            String allBlowersString = GetAllBlowersProlog(listOfInterest);
            String allBubbles = GetAllBubblesCatchedAndVoidProlog(listOfInterest);
            String allRockets = GetAllRocketsProlog(listOfInterest);
            String allBump = GetAllBumpsProlog(listOfInterest);
            String stateString = frogString + allRopesString + allBlowersString + allBubbles + allRockets +
                   allBump;
            stateString = stateString.Replace("\r\n", " ");
            stateString = stateString.Replace(".", " ");
            return stateString;
        }

        public static string GetEngineStateFactStringWithSpaceDelimiterToStateFile()
        {
            String str = GetEngineStateFactStringWithEnterDelimiterToProlog();
            str = str.Replace("\r\n", " ");
            str = str.Replace(".", " ");
            return str;
        }

        //public static string GetEngineStateFactStringWithSpaceDelimiterToFile(bool withVelocity)
        //{
        //    String cookieString = GetCookieFile();
        //    String velocity = String.Empty;
        //    if (withVelocity)
        //        velocity = GetCookieVelocity();
        //    String frogString = GetFrogFile();
        //    String allRopesString = GetAllRopesFile();
        //    String allBlowersString = GetAllBlowersFile();
        //    String allBubbles = GetAllBubblesFile();
        //    String allRockets = GetAllRocketsFile();
        //    String allBump = GetAllBumpsFile();
        //    String stateString = cookieString + velocity + frogString + allRopesString + allBlowersString + allBubbles + allRockets +
        //           allBump;
        //    stateString = stateString.Replace("\r\n", " ");
        //    stateString = stateString.Replace(".", " ");
        //    return stateString;
        //}

        public static string GetEngineStateFactStringWithSpaceDelimiterGEVAStyle()
        {
            String cookieString = GetCookieGEVA();
            String frogString = GetFrogGEVA();
            String allRopesString = GetAllRopesGEVA();
            String allBlowersString = GetAllBlowersGEVA();
            String allBubbles = GetAllBubblesGEVA();
            String allRockets = GetAllRocketsGEVA();
            String allBump = GetAllBumpsGEVA();
            String stateString = cookieString + frogString + allRopesString + allBlowersString + allBubbles + allRockets +
                   allBump;
            stateString = stateString.Replace("\r\n", " ");
            stateString = stateString.Replace(".", " ");
            return stateString;
        }

        public static string GetEngineStateFactStringWithSpaceDelimiterGEVAStyle(List<Visual2D> comps)
        {
            //String cookieString = GetCookieGEVA();
            String frogString = String.Empty, allRopesString = String.Empty, 
                allBlowersString = String.Empty, allBubbles = String.Empty, allRockets = String.Empty, allBump = String.Empty;

            frogString = GetFrogGEVA(comps);
            allRopesString = GetAllRopesGEVA(comps);
            allBlowersString = GetAllBlowersGEVA(comps);
            allBubbles = GetAllBubblesGEVA(comps);
            allRockets = GetAllRocketsGEVA(comps);
            allBump = GetAllBumpsGEVA(comps);
            String stateString = frogString + allRopesString + allBlowersString + allBubbles + allRockets +
                   allBump;
            stateString = stateString.Replace("\r\n", " ");
            stateString = stateString.Replace(".", " ");
            return stateString;
        }

        public static string GetEngineStatePositionsOnlyFactString()
        {
            String cookieString = GetCookieGEVA();
            String frogString = GetFrogGEVA();
            String allRopesString = GetAllRopesPositionsOnly();
            String allBlowersString = GetAllBlowersPositionsOnly();
            String allBubbles = GetAllBubblesPositionsOnly();
            String allRockets = GetAllRocketsPositionsOnly();
            String allBump = GetAllBumpsPositionsOnly();
            String stateString = cookieString + frogString + allRopesString + allBlowersString + allBubbles + allRockets +
                   allBump;
            stateString = stateString.Replace("\r\n", " ");
            stateString = stateString.Replace(".", " ");
            return stateString;
        }

        private static string GetCookieVelocity()
        {
            String velocity = "velocity(" + (int)StaticData.EngineManager.CookieRB.GetVelocity().X + ", " +
                              -(int)StaticData.EngineManager.CookieRB.GetVelocity().Y + ")." + Environment.NewLine;
            return velocity;
        }

        public static string GetFrogGEVA()
        {
            Vector3 posFrog = StaticData.EngineManager.FrogRB.PositionXNA;
            return "frog" + "(" + posFrog.X + ", " + posFrog.Y + ")." + Environment.NewLine;
        }


        private static string GetFrogGEVA(List<Visual2D> comps)
        {
            if (comps.Contains(StaticData.EngineManager.FrogRB))
            {
                Vector3 posFrog = StaticData.EngineManager.FrogRB.PositionXNA;
                return "frog" + "(" + posFrog.X + ", " + posFrog.Y + ")." + Environment.NewLine;
            }
            return String.Empty;
        }

        public static string GetFrogProlog()
        {
            Vector3 posFrog = StaticData.EngineManager.FrogRB.PositionXNACenter;
            return "frog" + "(" + posFrog.X + ", " + posFrog.Y + ")." + Environment.NewLine;
        }

        public static string GetFrogProlog(List<Point> listOfInterest)
        {
            Vector3 posFrog = StaticData.EngineManager.FrogRB.PositionXNACenter;
            if (IsInList(listOfInterest, posFrog.X, posFrog.Y))
            {
                return "frog" + "(" + posFrog.X + ", " + posFrog.Y + ")." + Environment.NewLine;
            }
            return String.Empty;
        }

        public static string GetCookieGEVA()
        {
            Vector3 posCookie = StaticData.EngineManager.CookieRB.PositionXNA;
            return "cookie(" + (int)posCookie.X + ", " + (int)posCookie.Y + ")." + Environment.NewLine;
        }

        public static string GetCookieProlog()
        {
            Vector3 posCookie = StaticData.EngineManager.CookieRB.PositionXNACenter;
            return "cookie(" + (int)posCookie.X + ", " + (int)posCookie.Y + ")." + Environment.NewLine;
        }

        public static string GetAllRopesFile()
        {
            String allRopesString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.SpringsManagerEngine.ListOfServices.Count; i++)
            {
                String currentRopeString = String.Empty;
                SpringService cRope = StaticData.EngineManager.SpringsManagerEngine.ListOfServices[i];
                if (cRope is CatchableRopeService)
                {
                    currentRopeString = "catchable_rope(" + cRope.Id + ", " + cRope.Masses[0].PositionXNA.X + ", " +
                                        cRope.Masses[0].PositionXNA.Y + ", " + cRope.Length + ").";
                    allRopesString += currentRopeString + Environment.NewLine;
                }
                else
                {
                    currentRopeString = "rope(" + cRope.Id + ", " + cRope.Masses[0].PositionXNA.X + ", " +
                    cRope.Masses[0].PositionXNA.Y + ", " + cRope.Length + ").";
                    allRopesString += currentRopeString + Environment.NewLine;
                }
            }
            return allRopesString;
        }

        public static string GetAllRopesProlog()
        {
            String allRopesString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.SpringsManagerEngine.ListOfServices.Count; i++)
            {
                String currentRopeString = String.Empty;
                SpringService cRope = StaticData.EngineManager.SpringsManagerEngine.ListOfServices[i];
                if (cRope is CatchableRopeService)
                {
                    if (((CatchableRopeService)cRope).IsActivated == true)
                    {
                        currentRopeString = "rope(" + cRope.Id + ", " + cRope.Masses[0].PositionXNA.X + ", " +
                                            cRope.Masses[0].PositionXNA.Y + ", " + cRope.Length + ").";
                        allRopesString += currentRopeString + Environment.NewLine;
                    }
                    else
                    {
                        currentRopeString = "catchable_rope(" + cRope.Id + ", " + cRope.Masses[0].PositionXNA.X + ", " +
                                            cRope.Masses[0].PositionXNA.Y + ", " + cRope.Length + ").";
                        allRopesString += currentRopeString + Environment.NewLine;
                    }
                }
                else
                {
                    currentRopeString = "rope(" + cRope.Id + ", " + cRope.Masses[0].PositionXNA.X + ", " +
                    cRope.Masses[0].PositionXNA.Y + ", " + cRope.Length + ").";
                    allRopesString += currentRopeString + Environment.NewLine;
                }
            }
            return allRopesString;
        }

        public static string GetAllRopesProlog(List<Point> listOfInterest)
        {
            String allRopesString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.SpringsManagerEngine.ListOfServices.Count; i++)
            {
                String currentRopeString = String.Empty;
                SpringService cRope = StaticData.EngineManager.SpringsManagerEngine.ListOfServices[i];
                if (cRope is CatchableRopeService)
                {
                    if (((CatchableRopeService)cRope).IsActivated == true)
                    {
                        if (IsInList(listOfInterest, cRope.Masses[0].PositionXNA.X, cRope.Masses[0].PositionXNA.Y))
                        {
                            currentRopeString = "rope(" + cRope.Id + ", " + cRope.Masses[0].PositionXNA.X + ", " +
                                                cRope.Masses[0].PositionXNA.Y + ", " + cRope.Length + ").";
                            allRopesString += currentRopeString + Environment.NewLine;
                        }
                    }
                    else
                    {
                        if (IsInList(listOfInterest, cRope.Masses[0].PositionXNA.X, cRope.Masses[0].PositionXNA.Y))
                        {
                            currentRopeString = "catchable_rope(" + cRope.Id + ", " + cRope.Masses[0].PositionXNA.X +
                                                ", " +
                                                cRope.Masses[0].PositionXNA.Y + ", " + cRope.Length + ").";
                            allRopesString += currentRopeString + Environment.NewLine;
                        }
                    }
                }
                else
                {
                    if (IsInList(listOfInterest, cRope.Masses[0].PositionXNA.X, cRope.Masses[0].PositionXNA.Y))
                    {
                        currentRopeString = "rope(" + cRope.Id + ", " + cRope.Masses[0].PositionXNA.X + ", " +
                        cRope.Masses[0].PositionXNA.Y + ", " + cRope.Length + ").";
                        allRopesString += currentRopeString + Environment.NewLine;
                    }
                }
            }
            return allRopesString;
        }

        private static bool IsInList(List<Point> listOfInterest, float x, float y)
        {
            for (int i = 0; i < listOfInterest.Count; i++)
            {
                if (listOfInterest[i].X == x && listOfInterest[i].Y == y)
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetAllRopesPositionsOnly()
        {
            String allRopesString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.SpringsManagerEngine.ListOfServices.Count; i++)
            {
                String currentRopeString = String.Empty;
                SpringService cRope = StaticData.EngineManager.SpringsManagerEngine.ListOfServices[i];
                if (cRope is CatchableRopeService)
                {
                    currentRopeString = "catchable_rope(" + cRope.Masses[0].PositionXNA.X + ", " +
                                        cRope.Masses[0].PositionXNA.Y + ").";
                    allRopesString += currentRopeString + Environment.NewLine;
                }
                else
                {
                    currentRopeString = "rope(" + cRope.Masses[0].PositionXNA.X + ", " +
                    cRope.Masses[0].PositionXNA.Y + ").";
                    allRopesString += currentRopeString + Environment.NewLine;
                }
            }
            return allRopesString;
        }

        public static string GetAllRopesGEVA()
        {
            String allRopesString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.SpringsManagerEngine.ListOfServices.Count; i++)
            {
                String currentRopeString = String.Empty;
                SpringService cRope = StaticData.EngineManager.SpringsManagerEngine.ListOfServices[i];
                if (cRope is CatchableRopeService)
                {
                    currentRopeString = "catchable_rope(" + cRope.Masses[0].PositionXNA.X + ", " +
                                        cRope.Masses[0].PositionXNA.Y +  "," + cRope.Length +  ").";
                    allRopesString += currentRopeString + Environment.NewLine;
                }
                else
                {
                    currentRopeString = "rope(" + cRope.Masses[0].PositionXNA.X + ", " +
                    cRope.Masses[0].PositionXNA.Y +  "," + cRope.Length + ").";
                    allRopesString += currentRopeString + Environment.NewLine;
                }
            }
            return allRopesString;
        }

        public static string GetAllRopesGEVA(List<Visual2D> comps)
        {
            String allRopesString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.SpringsManagerEngine.ListOfServices.Count; i++)
            {
                String currentRopeString = String.Empty;
                SpringService cRope = StaticData.EngineManager.SpringsManagerEngine.ListOfServices[i];
                if (comps.Contains(cRope.Masses[0]))
                {
                    if (cRope is CatchableRopeService)
                    {
                        currentRopeString = "catchable_rope(" + cRope.Masses[0].PositionXNA.X + ", " +
                                            cRope.Masses[0].PositionXNA.Y + "," + cRope.Length + ").";
                        allRopesString += currentRopeString + Environment.NewLine;
                    }
                    else
                    {
                        currentRopeString = "rope(" + cRope.Masses[0].PositionXNA.X + ", " +
                                            cRope.Masses[0].PositionXNA.Y + "," + cRope.Length + ").";
                        allRopesString += currentRopeString + Environment.NewLine;
                    }
                }
            }
            return allRopesString;
        }

        public static string GetAllBlowersGEVA()
        {
            String allBlowersString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.BlowerManagerEngine.ListOfServices.Count; i++)
            {
                String currentBlowerString = String.Empty;
                BlowerService cBlower = StaticData.EngineManager.BlowerManagerEngine.ListOfServices[i];
                currentBlowerString = "blower(" + cBlower.PositionXNA.X + ", " + cBlower.PositionXNA.Y + ", " +
                                      (int)cBlower.Dir + ").";
                allBlowersString += currentBlowerString + Environment.NewLine;
            }
            return allBlowersString;
        }

        public static string GetAllBlowersGEVA(List<Visual2D> comps)
        {
            String allBlowersString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.BlowerManagerEngine.ListOfServices.Count; i++)
            {
                
                String currentBlowerString = String.Empty;
                BlowerService cBlower = StaticData.EngineManager.BlowerManagerEngine.ListOfServices[i];
                if (comps.Contains(cBlower))
                {
                    currentBlowerString = "blower(" + cBlower.PositionXNA.X + ", " + cBlower.PositionXNA.Y + ", " +
                                          (int) cBlower.Dir + ").";
                    allBlowersString += currentBlowerString + Environment.NewLine;
                }
            }
            return allBlowersString;
        }

        public static string GetAllBlowersProlog()
        {
            String allBlowersString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.BlowerManagerEngine.ListOfServices.Count; i++)
            {
                String currentBlowerString = String.Empty;
                BlowerService cBlower = StaticData.EngineManager.BlowerManagerEngine.ListOfServices[i];
                currentBlowerString = "blower(" + cBlower.PositionXNACenter.X + ", " + cBlower.PositionXNACenter.Y + ", " +
                                      (int)cBlower.Dir + ").";
                allBlowersString += currentBlowerString + Environment.NewLine;
            }
            return allBlowersString;
        }

        public static string GetAllBlowersProlog(List<Point> listOfInterest)
        {
            String allBlowersString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.BlowerManagerEngine.ListOfServices.Count; i++)
            {
                String currentBlowerString = String.Empty;
                BlowerService cBlower = StaticData.EngineManager.BlowerManagerEngine.ListOfServices[i];
                if (IsInList(listOfInterest, cBlower.PositionXNACenter.X, cBlower.PositionXNACenter.Y))
                {
                    currentBlowerString = "blower(" + cBlower.PositionXNACenter.X + ", " + cBlower.PositionXNACenter.Y +
                                          ", " +
                                          (int) cBlower.Dir + ").";
                    allBlowersString += currentBlowerString + Environment.NewLine;
                }
            }
            return allBlowersString;
        }



        public static string GetAllBlowersPositionsOnly()
        {
            String allBlowersString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.BlowerManagerEngine.ListOfServices.Count; i++)
            {
                String currentBlowerString = String.Empty;
                BlowerService cBlower = StaticData.EngineManager.BlowerManagerEngine.ListOfServices[i];
                currentBlowerString = "blower(" + cBlower.PositionXNA.X + ", " + cBlower.PositionXNA.Y + ").";
                allBlowersString += currentBlowerString + Environment.NewLine;
            }
            return allBlowersString;
        }

        public static string GetAllBubblesGEVA()
        {
            String allServicesString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.BubbleManagerEngine.ListOfServices.Count; i++)
            {
                
                String currentString = String.Empty;
                BubbleService cBubble = StaticData.EngineManager.BubbleManagerEngine.ListOfServices[i];
                currentString = "bubble(" + cBubble.Id + ", " + cBubble.PositionXNA.X +
                                ", " + cBubble.PositionXNA.Y + ").";
                allServicesString += currentString + Environment.NewLine;
            }
            return allServicesString;
        }

        public static string GetAllBubblesGEVA(List<Visual2D> comps)
        {
            String allServicesString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.BubbleManagerEngine.ListOfServices.Count; i++)
            {

                String currentString = String.Empty;
                BubbleService cBubble = StaticData.EngineManager.BubbleManagerEngine.ListOfServices[i];
                if (comps.Contains(cBubble))
                {
                    currentString = "bubble(" + cBubble.Id + ", " + cBubble.PositionXNA.X +
                                    ", " + cBubble.PositionXNA.Y + ").";
                    allServicesString += currentString + Environment.NewLine;
                }
            }
            return allServicesString;
        }

        public static string GetAllBubblesCatchedAndVoidProlog()
        {
            String allServicesString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.BubbleManagerEngine.ListOfServices.Count; i++)
            {

                String currentString = String.Empty;
                BubbleService cBubble = StaticData.EngineManager.BubbleManagerEngine.ListOfServices[i];
                if (cBubble.IsCookieCatched)
                {
                    currentString = "bubble_catched(" + cBubble.Id + ", " + cBubble.PositionXNACenter.X +
                                    ", " + cBubble.PositionXNACenter.Y + ").";
                    allServicesString += currentString + Environment.NewLine;
                }
                else
                {
                    currentString = "bubble_void(" + cBubble.Id + ", " + cBubble.PositionXNACenter.X +
                 ", " + cBubble.PositionXNACenter.Y + ").";
                    allServicesString += currentString + Environment.NewLine; 
                }
            }
            return allServicesString;
        }

        public static string GetAllBubblesCatchedAndVoidProlog(List<Point> listOfInterest)
        {
            String allServicesString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.BubbleManagerEngine.ListOfServices.Count; i++)
            {

                String currentString = String.Empty;
                BubbleService cBubble = StaticData.EngineManager.BubbleManagerEngine.ListOfServices[i];
                if (IsInList(listOfInterest, cBubble.PositionXNACenter.X, cBubble.PositionXNACenter.Y))
                {
                    if (cBubble.IsCookieCatched)
                    {

                        currentString = "bubble_catched(" + cBubble.Id + ", " + cBubble.PositionXNACenter.X +
                                        ", " + cBubble.PositionXNACenter.Y + ").";
                        allServicesString += currentString + Environment.NewLine;
                    }
                    else
                    {
                        currentString = "bubble_void(" + cBubble.Id + ", " + cBubble.PositionXNACenter.X +
                                        ", " + cBubble.PositionXNACenter.Y + ").";
                        allServicesString += currentString + Environment.NewLine;
                    }
                }
            }
            return allServicesString;
        }

        public static string GetAllBubblesPositionsOnly()
        {
            String allServicesString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.BubbleManagerEngine.ListOfServices.Count; i++)
            {
                String currentString = String.Empty;
                BubbleService cBubble = StaticData.EngineManager.BubbleManagerEngine.ListOfServices[i];
                currentString = "bubble(" + cBubble.PositionXNA.X + ", " + cBubble.PositionXNA.Y + ").";
                allServicesString += currentString + Environment.NewLine;
            }
            return allServicesString;
        }

        public static string GetAllRocketsGEVA()
        {
            String allServicesString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.RocketsCarrierManagerEngine.GetListOfServices().Count; i++)
            {
                String currentString = String.Empty;
                RocketCarrierService service = StaticData.EngineManager.RocketsCarrierManagerEngine.GetListOfServices()[i];
                if (service.CanCatchCookie)
                {
                    if (!service.IsActivated)
                    {
                        currentString = "rocket(" + (int)service.PositionXNA.X + ", " + (int)service.PositionXNA.Y +
                                        ", " + (int)service.Dir + ").";
                        allServicesString += currentString + Environment.NewLine;
                    }
                }
            }
            return allServicesString;
        }

        public static string GetAllRocketsGEVA(List<Visual2D> comps)
        {
            String allServicesString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.RocketsCarrierManagerEngine.GetListOfServices().Count; i++)
            {
                String currentString = String.Empty;
                RocketCarrierService service = StaticData.EngineManager.RocketsCarrierManagerEngine.GetListOfServices()[i];
                if (service.CanCatchCookie)
                {
                    if (!service.IsActivated)
                    {
                        if (comps.Contains(service))
                        {
                            currentString = "rocket(" + (int) service.PositionXNA.X + ", " + (int) service.PositionXNA.Y +
                                            ", " + (int) service.Dir + ").";
                            allServicesString += currentString + Environment.NewLine;
                        }
                    }
                }
            }
            return allServicesString;
        }

        public static string GetAllRocketsProlog()
        {
            String allServicesString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.RocketsCarrierManagerEngine.GetListOfServices().Count; i++)
            {
                String currentString = String.Empty;
                RocketCarrierService service = StaticData.EngineManager.RocketsCarrierManagerEngine.GetListOfServices()[i];
                if (service.CanCatchCookie)
                {
                    if (!service.IsActivated)
                    {

                            currentString = "rocket_void(" + (int)service.PositionXNACenter.X + ", " +
                                            (int)service.PositionXNACenter.Y +
                                            ", " + (int)service.Dir + ").";
                            allServicesString += currentString + Environment.NewLine;
                    }
                    else
                    {
                            currentString = "rocket_catched(" + (int)service.PositionXNACenter.X + ", " +
                                            (int)service.PositionXNACenter.Y +
                                            ", " + (int)service.Dir + ").";
                            allServicesString += currentString + Environment.NewLine;
                    }
                }
            }
            return allServicesString;
        }

        public static string GetAllRocketsProlog(List<Point> listOfInterest)
        {
            String allServicesString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.RocketsCarrierManagerEngine.GetListOfServices().Count; i++)
            {
                String currentString = String.Empty;
                RocketCarrierService service = StaticData.EngineManager.RocketsCarrierManagerEngine.GetListOfServices()[i];
                if (service.CanCatchCookie)
                {
                    if (IsInList(listOfInterest, service.PositionXNACenter.X, service.PositionXNACenter.Y))
                    {
                        if (!service.IsActivated)
                        {
                            currentString = "rocket_void(" + (int) service.PositionXNACenter.X + ", " +
                                            (int) service.PositionXNACenter.Y +
                                            ", " + (int) service.Dir + ").";
                            allServicesString += currentString + Environment.NewLine;
                        }
                        else
                        {
                            currentString = "rocket_catched(" + (int) service.PositionXNACenter.X + ", " +
                                            (int) service.PositionXNACenter.Y +
                                            ", " + (int) service.Dir + ").";
                            allServicesString += currentString + Environment.NewLine;
                        }
                    }
                }
            }
            return allServicesString;
        }


        public static string GetAllRocketsPositionsOnly()
        {
            String allServicesString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.RocketsCarrierManagerEngine.GetListOfServices().Count; i++)
            {
                String currentString = String.Empty;
                RocketCarrierService service = StaticData.EngineManager.RocketsCarrierManagerEngine.GetListOfServices()[i];
                currentString = "rocket(" + service.PositionXNA.X + ", " + service.PositionXNA.Y + ").";
                allServicesString += currentString + Environment.NewLine;
            }
            return allServicesString;
        }

        public static string GetAllBumpsGEVA()
        {
            String allServicesString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.RigidsManagerEngine.ListOfBoxRigids.Count; i++)
            {
                String currentString = String.Empty;
                BoxRigid boxRigid = StaticData.EngineManager.RigidsManagerEngine.ListOfBoxRigids[i];
                if (boxRigid is BumpRigid)
                {
                    currentString = "bump(" + boxRigid.PositionXNA.X + ", " + boxRigid.PositionXNA.Y +
                                    ", " + (int)((BumpRigid)boxRigid).Dir + ").";
                    allServicesString += currentString + Environment.NewLine;
                }
            }
            return allServicesString;
        }

        public static string GetAllBumpsGEVA(List<Visual2D> comps)
        {
            String allServicesString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.RigidsManagerEngine.ListOfBoxRigids.Count; i++)
            {
                String currentString = String.Empty;
                BoxRigid boxRigid = StaticData.EngineManager.RigidsManagerEngine.ListOfBoxRigids[i];
                if (boxRigid is BumpRigid)
                {
                    if (comps.Contains(boxRigid))
                    {
                        currentString = "bump(" + boxRigid.PositionXNA.X + ", " + boxRigid.PositionXNA.Y +
                                        ", " + (int) ((BumpRigid) boxRigid).Dir + ").";
                        allServicesString += currentString + Environment.NewLine;
                    }
                }
            }
            return allServicesString;
        }

        public static string GetAllBumpsProlog()
        {
            String allServicesString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.RigidsManagerEngine.ListOfBoxRigids.Count; i++)
            {
                String currentString = String.Empty;
                BoxRigid boxRigid = StaticData.EngineManager.RigidsManagerEngine.ListOfBoxRigids[i];
                if (boxRigid is BumpRigid)
                {
                    currentString = "bump(" + boxRigid.PositionXNACenter.X + ", " + boxRigid.PositionXNACenter.Y +
                                    ", " + (int)((BumpRigid)boxRigid).Dir + ").";
                    allServicesString += currentString + Environment.NewLine;
                }
            }
            return allServicesString;
        }

        public static string GetAllBumpsProlog(List<Point> listOfInterest)
        {
            String allServicesString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.RigidsManagerEngine.ListOfBoxRigids.Count; i++)
            {
                String currentString = String.Empty;
                BoxRigid boxRigid = StaticData.EngineManager.RigidsManagerEngine.ListOfBoxRigids[i];
                if (boxRigid is BumpRigid)
                {
                    if (IsInList(listOfInterest, boxRigid.PositionXNACenter.X, boxRigid.PositionXNACenter.Y))
                    {
                        currentString = "bump(" + boxRigid.PositionXNACenter.X + ", " + boxRigid.PositionXNACenter.Y +
                                        ", " + (int) ((BumpRigid) boxRigid).Dir + ").";
                        allServicesString += currentString + Environment.NewLine;
                    }
                }
            }
            return allServicesString;
        }


        public static string GetAllBumpsPositionsOnly()
        {
            String allServicesString = String.Empty;
            for (int i = 0; i < StaticData.EngineManager.RigidsManagerEngine.ListOfBoxRigids.Count; i++)
            {
                String currentString = String.Empty;
                BoxRigid boxRigid = StaticData.EngineManager.RigidsManagerEngine.ListOfBoxRigids[i];
                if (boxRigid is BumpRigid)
                {
                    currentString = "bump(" + boxRigid.PositionXNA.X + ", " + boxRigid.PositionXNA.Y + ").";
                    allServicesString += currentString + Environment.NewLine;
                }
            }
            return allServicesString;
        }
    }
}
