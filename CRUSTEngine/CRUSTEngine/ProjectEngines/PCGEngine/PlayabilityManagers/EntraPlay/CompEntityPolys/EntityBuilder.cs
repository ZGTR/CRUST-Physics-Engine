using System;
using System.Collections.Generic;
using System.Linq;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices;

using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.CompEntityPolys.CompsEntities;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.CompEntityPolys
{
    [Serializable]
    public class EntityBuilder
	{
        private readonly CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.EntraAgentSimple _entraAgentSimple;
        private EngineManager _engineState;

	    public EntityBuilder(CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.EntraAgentSimple entraAgentSimple)
	    {
	        _entraAgentSimple = entraAgentSimple;
            this._engineState = entraAgentSimple.EngineState;
	    }

        public List<CompEntityPoly> GetRocketsEntities()
        {
            List<CompEntityPoly> entities = new List<CompEntityPoly>();
            foreach (var service in _engineState.RocketsCarrierManagerEngine.GetListOfServices())
            {
                entities.Add(new RocketEntityPoly(_entraAgentSimple, service));
            }
            return entities;
        }

        public List<CompEntityPoly> GetBubblesEntities()
        {
            List<CompEntityPoly> entities = new List<CompEntityPoly>();
            foreach (var service in _engineState.BubbleManagerEngine.ListOfServices)
            {
                entities.Add(new BubbleEntityPoly(_entraAgentSimple, service));
            }
            return entities;
        }

        public List<CompEntityPoly> GetBumpersEntities()
        {
            List<CompEntityPoly> entities = new List<CompEntityPoly>();
            List<BumpRigid> bumpers = _engineState.RigidsManagerEngine.
                                                   ListOfBoxRigids.Where(item => item is BumpRigid).Cast<BumpRigid>()
                                                  .ToList();
            foreach (var service in bumpers)
            {
                entities.Add(new BumperEntityPoly(_entraAgentSimple, service));
            }
            return entities;
        }

        public List<CompEntityPoly> GetBlowersEntities()
        {
            List<CompEntityPoly> entities = new List<CompEntityPoly>();
            foreach (var service in _engineState.BlowerManagerEngine.ListOfServices)
            {
                entities.Add(new BlowerEntityPoly(_entraAgentSimple, service));
            }
            return entities;
        }

        public List<CompEntityPoly> GetRopesEntities()
        {
            List<CompEntityPoly> entities = new List<CompEntityPoly>();
            foreach (var service in _engineState.SpringsManagerEngine.ListOfServices)
            {
                entities.Add(new RopeEntityPoly(_entraAgentSimple, service));
            }
            return entities;
        }
	}
}
