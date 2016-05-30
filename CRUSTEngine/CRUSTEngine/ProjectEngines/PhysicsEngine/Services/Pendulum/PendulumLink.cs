//using System;
//using Microsoft.Xna.Framework;
//using CRUSTEngine.ProjectEngines.GraphicsEngine;
//using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;

//namespace CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Pendulum
//{
//    public class PendulumLink											
//        //An object to represent a Link with inner friction binding two masses. The Link 
//        //has a normal length (the length that the Link does not exert any force)
//    {
//        public RigidBody Mass1 { get; set; }										//The first mass at one tip of the Link
//        public RigidBody Mass2 { get; set; }										//The second mass at the other tip of the Link
//        public float LinkLength { get; private set; }									//The length that the Link does not exert any force
        
//        public PendulumLink(RigidBody mass1,
//            RigidBody mass2,
//            float linkLength)
//        {
//            this.LinkLength = linkLength;										
//            this.Mass1 = mass1;													
//            this.Mass2 = mass2;													
//        }

//        public void Update(GameTime gameTime)
//        {
//            Vector3 linkVector = Mass1.PositionCenterEngine - Mass2.PositionCenterEngine;
//            float DistancesList = linkVector.Length();
//            float mukabelX1 = Math.Abs(Mass1.PositionCenterEngine.X - Mass2.PositionCenterEngine.X);
//            float watarX2 = DistancesList;
//            float angle = MathHelperModule.GetAngleBetweenTwoVectors(Mass1.PositionCenterEngine, linkVector);
//            Vector3 force = new Vector3();

//            if (DistancesList != 0)																	//to avoid a division by zero check if r is zero
//                //if (mukabelX1 != 0)
//                //    force = -Mass2.GetAcceleration() * Mass2.Mass * (watarX2 / mukabelX1);
//                //else
//                    force = // - Mass2.GetAcceleration() * Mass2.Mass;
//                     (linkVector / DistancesList) *  (DistancesList - LinkLength) * (-Mass2.Mass);	    

//            //force += -(Mass1.GetVelocity() - Mass2.GetVelocity());// *FrictionConstant;	//the friction force is added to the force
//                                                                                        //with this addition we obtain the net force of the Link

//            Mass1.AddForce(force);													//force is applied to mass1
//            Mass2.AddForce(-force);													//the opposite of force is applied to mass2
//        }
//    };
//}
