using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Rocket.ParticleEngine;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers;
using CRUSTEngine.ProjectEngines.PhysicsEngine;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;

namespace CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Rocket
{
    [Serializable]
    public class RocketService : BoxRigid, IUpdatableComponent
    {
        private ParticleEngineCore _particleEngine;
        public Vector3 ForceThrottle { set; get; }
        public int CloseArea { set; get; }
        public bool IsExploded { set; get; }
        
        public RocketService(Vector3 intitialForce, 
            Vector3 positionXNA,
            Vector3 halfSize,
            int closeArea)
            :base(positionXNA, Material.Ice, halfSize)
        {
            this.CloseArea = closeArea;
            this.IsExploded = false;
            this.ForceThrottle = intitialForce;
            this.TextureType = TextureType.Rocket;
            StaticData.EngineManager.RigidsManagerEngine.AddRigidBody(this);
            _particleEngine = new ParticleEngineCore(new Vector2(400, 240), StaticData.MinTTL, StaticData.MaxNextTTL, 10);
        }

        public void Explode()
        {
            this.IsExploded = true;
            this._particleEngine.IsExploding = true;
            List<RigidBody> listOfCloseRigids = CatchCloseRigids();
            ApplyRandomForces(listOfCloseRigids);
            StaticData.EngineManager.RigidsManagerEngine.ListOfBoxRigids.Remove((BoxRigid)this);
        }

        private void ApplyRandomForces(List<RigidBody> listOfCloseRigids)
        {
            Random rand = new Random();
            foreach (var rigid in listOfCloseRigids)
            {
                rigid.AddForce(new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), 0) * rigid.Mass * 10000);
            }
        }

        private List<RigidBody> CatchCloseRigids()
        {
            List<RigidBody> listToReturn = new List<RigidBody>();
            foreach (var rigidBody in StaticData.EngineManager.RigidsManagerEngine.ListOfRigids)
            {
                if ((rigidBody.PositionXNA - new Vector3(this.RectangleArea.X, this.RectangleArea.Y, 0)).Length() < CloseArea)
                {
                    listToReturn.Add(rigidBody);
                }
            }

            return listToReturn;
        }

        public void Update(GameTime gameTime)
        {
            this.AddForce(ForceThrottle);
            _particleEngine.EmitterLocation = new Vector2(this.PositionXNA.X, this.PositionXNA.Y) + new Vector2(10, 0);
            _particleEngine.Update(StaticData.BasicParticleVelocity);
        }

        private int timeToVanish;
        public void Draw(GameTime gameTime)
        {
            if (IsExploded)
            {
                timeToVanish++;
                if (timeToVanish == 150)
                {
                    StaticData.EngineManager.RocketsManagerEngine.ListOfRockets.Remove(this);
                }
            }
            // Draw particles
            _particleEngine.Draw(gameTime);
        }
    }
}
