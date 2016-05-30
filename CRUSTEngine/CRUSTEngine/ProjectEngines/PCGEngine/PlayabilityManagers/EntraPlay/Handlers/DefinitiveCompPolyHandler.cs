using System;
using System.Collections.Generic;
using System.Linq;
using ClipperLib;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Bubble;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Handlers
{
    [Serializable]
    public class DefinitiveCompPolyHandler
    {
        public EngineManager _engineState;
        public DefinitiveCompPolyHandler(EngineManager engineState)
        {
            _engineState = engineState;
        }

        public List<List<IntPoint>> GetDefRocketsPolys()
        {
            List<List<IntPoint>> allPolys = new List<List<IntPoint>>();
            foreach (var service in _engineState.RocketsCarrierManagerEngine.GetListOfServices())
            {
                allPolys.Add(DefinitiveCompPolyHandler.GetDefComponentIntersection(service.PositionXNACenter2D,
                                                                                     service.Width));
            }
            return allPolys;
        }

        public List<IntPoint> GetDefRocketPoly(RocketCarrierService service)
        {
            return DefinitiveCompPolyHandler.GetDefComponentIntersection(service.PositionXNACenter2D,
                                                                           service.Width);
        }

        public List<List<IntPoint>> GetDefBubblesPolys()
        {
            List<List<IntPoint>> allPolys = new List<List<IntPoint>>();
            foreach (var service in _engineState.BubbleManagerEngine.ListOfServices)
            {
                allPolys.Add(DefinitiveCompPolyHandler.GetDefComponentIntersection(service.PositionXNACenter,
                                                                                     service.Width));
            }
            return allPolys;
        }

        public List<IntPoint> GetDefBubblePoly(BubbleService service)
        {
            return DefinitiveCompPolyHandler.GetDefComponentIntersection(service.PositionXNACenter,
                                                                           service.Width);

        }

        public List<List<IntPoint>> GetDefBumpersPolys()
        {
            List<List<IntPoint>> allPolys = new List<List<IntPoint>>();
            List<BumpRigid> bumpers = _engineState.RigidsManagerEngine.
                ListOfBoxRigids.Where(item => item is BumpRigid).Cast<BumpRigid>()
                .ToList();
            foreach (var service in bumpers)
            {
                allPolys.Add(DefinitiveCompPolyHandler.GetDefComponentIntersection(service.PositionXNACenter2D,
                                                                                     service.Width));
            }
            return allPolys;
        }

        public List<IntPoint> GetDefBumperPoly(BumpRigid bump)
        {
            return DefinitiveCompPolyHandler.GetDefComponentIntersection(bump.PositionXNACenter2D,
                                                                                     bump.Width);
        }

        public static List<IntPoint> GetDefComponentIntersection(Vector2 posCenter, int width)
        {
            return PolysHelper.GetShapeSquarePoly(posCenter, width / 2);
        }

        public static bool IsDefComponentIntersection(Vector2 posCenter, int width, List<List<IntPoint>> polys)
        {
            List<IntPoint> poly = PolysHelper.GetShapeSquarePoly(posCenter, width / 2);
            if (EntraSolver.IsPolyOperation(new List<List<IntPoint>>() { poly }, polys, ClipType.ctIntersection))
            {
                return true;
            }
            return false;
        }
    }
}
