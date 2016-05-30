//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;

//namespace CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Pendulum
//{
//    public class PendulumService
//    {
//        public List<RigidBody> Masses { private set; get; }
//        public List<PendulumLink> PendulumLinks { private set; get; }
//        public Vector3 ConnectionPos;
//        public Vector3 ConnectionVel;
//        public float LinkLength{ private set; get; }

//        public RigidBody GetMass(int index)
//        {
//            return Masses[index];						// get the mass at the index
//        }

//        public PendulumService(										
//            float LinkLength,								    // the length that a Link does not exert any force
//            List<RigidBody> masses)				//The super class creates masses with weights m of each
//        {
//            this.Masses = masses;
//            this.LinkLength = LinkLength;
//            this.ConnectionPos = Masses[0].PositionXNA;

//            BuildLinks(LinkLength);
//        }

//        private void BuildLinks(float LinkLength)
//        {
//            PendulumLinks = new List<PendulumLink>();			
//            for (var a = 0; a < Masses.Count - 1; ++a)			
//            {
//                PendulumLinks.Add(new PendulumLink(Masses[a], Masses[a + 1], LinkLength));
//            }
//        }

//        public virtual void Update(GameTime gameTime)					    // The complete procedure of simulation
//        {
//            UpdateStrings(gameTime);									    // Step 2: apply forces
//            UpdateFixedFirstPoint(gameTime);								// Step 3: iterate the masses by the change in time
//        }

//        public void UpdateStrings(GameTime gameTime)										//solve() is overriden because we have forces to be applied
//        {
//            for (int a = 0; a < Masses.Count - 1; ++a)		//apply force of all Links
//            {
//                PendulumLinks[a].Update(gameTime);						//Link with index "a" should apply its force
//            }
//        }

//        public void UpdateFixedFirstPoint(GameTime gameTime)
//        {
//            ConnectionPos += new Vector3((float)(ConnectionVel.X * StaticData.Dtime),
//                (float)(ConnectionVel.Y * StaticData.Dtime),
//                (float)(ConnectionVel.Z * StaticData.Dtime));
//            try
//            {
//                Masses[0].PositionXNA = ConnectionPos;				//mass with index "0" shall position at ropeConnectionPos
//                Masses[0].SetVelocity(ConnectionVel);	
//            }
//            catch (Exception)
//            {

//            }
//        }

//        public void SetConnectionVel(Vector3 ropeConnectionVel)	//the method to set ropeConnectionVel
//        {
//            this.ConnectionVel = ropeConnectionVel;
//        }

//        public void ApplyServiceOnRigid(RigidBody CurrentVisual2D)
//        {
//            this.Masses.Add(CurrentVisual2D);
//            this.PendulumLinks.Add(new PendulumLink(Masses[Masses.Count - 2], Masses[Masses.Count - 1], LinkLength));
//        }
//    };
//}