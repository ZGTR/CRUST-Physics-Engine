using System;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Water;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components;

namespace CRUSTEngine.ProjectEngines.PCGEngine.Components.Comps
{
    [Serializable]
    public class Water : Component
    {
        private bool _isWater;
        private int _waterLevel;

        public Water(String []pars)
        {
            _isWater = Int32.Parse(pars[0]) == 0 ? false : true;
            _waterLevel = Int32.Parse(pars[1]);
            this.CType = ComponentType.Water;
        }

        public override void AddSelfToEngine()
        {
            StaticData.IsWater = _isWater;
            LiquidService.LiquidLevel = _waterLevel;
        }
    }
}
