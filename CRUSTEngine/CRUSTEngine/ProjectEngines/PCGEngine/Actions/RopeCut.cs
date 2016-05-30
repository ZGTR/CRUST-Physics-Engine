using System;
using System.Collections.Generic;
using System.Linq;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs;

namespace CRUSTEngine.ProjectEngines.PCGEngine.Actions
{
    [Serializable]
    public class RopeCut : CRUSTEngine.ProjectEngines.PCGEngine.Actions.Action
    {
        [NonSerialized]
        public SpringService RopeBeingCut;
        public int RopeId;
        public RopeCut(String[] pars)
        {
            AType = ActionType.RopeCut;
            RopeId = Int32.Parse(pars[0]);
        }

        public RopeCut(int ropeId)
        {
            AType = ActionType.RopeCut;
            RopeId = ropeId;
        }

        public override void ExcecuteAction()
        {
            try
            {
                List<int> listOfIds = StaticData.EngineManager.SpringsManagerEngine.ListOfServices.Select(rope => rope.Id).ToList();
                if (listOfIds.Contains(RopeId))
                {
                    RopeBeingCut = StaticData.EngineManager.SpringsManagerEngine.RemoveService(RopeId);
                }
            }
            catch(Exception)
            {}
        }

        public override string ToString()
        {
            return "rope_cut(" + this.RopeId + ")";
        }
    }
}
