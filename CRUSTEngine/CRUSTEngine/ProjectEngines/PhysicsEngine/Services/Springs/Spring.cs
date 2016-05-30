using System;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;

namespace CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs
{
    [Serializable]
    public class Spring											//An object to represent a spring with inner friction binding two masses. The spring 
                                                                //has a normal length (the length that the spring does not exert any force)
    {
        public RigidBody Mass1 { get; set; }										//The first mass at one tip of the spring
        public RigidBody Mass2 { get; set; }										//The second mass at the other tip of the spring

        public float SpringConstant { get; set; }								//A constant to represent the stiffness of the spring
        public float SpringLength { get; private set; }									//The length that the spring does not exert any force
        public float FrictionConstant { get; private set; }								//A constant to be used for the inner friction of the spring

        public Spring(RigidBody mass1, RigidBody mass2,
            float springConstant, 
            float springLength, 
            float frictionConstant)		//Constructor
        {
            this.SpringConstant = springConstant;									//set the springConstant
            this.SpringLength = springLength;										//set the springLength
            this.FrictionConstant = frictionConstant;								//set the frictionConstant

            this.Mass1 = mass1;													//set mass1
            this.Mass2 = mass2;													//set mass2
        }

        public void Update(GameTime gameTime)																	
        {
            Vector3 springVector = Mass1.PositionCenterEngine - Mass2.PositionCenterEngine;			//vector between the two masses
            float r = springVector.Length();											//distance between the two masses
            //if (r < SpringLength)
            {

                Vector3 force = new Vector3(); //force initially has a zero value

                if (r != 0) //to avoid a division by zero check if r is zero
                    force += (springVector/r)*(r - SpringLength)*(-SpringConstant);
                        //the spring force is added to the force

                force += -(Mass1.GetVelocity() - Mass2.GetVelocity())*FrictionConstant;
                    //the friction force is added to the force
                //with this addition we obtain the net force of the spring

                Mass1.AddForce(force); //force is applied to mass1
                Mass2.AddForce(-force); //the opposite of force is applied to mass2
            }
        }

        public void Draw(GameTime gameTime)
        {
            Vector3 posXna1 = this.Mass1.PositionXNACenter;
            Vector3 posXna2 = this.Mass2.PositionXNACenter;
            Vector3 posPointer = posXna2 - posXna1;
            Visual2DRotatable visual2DRotatable = new Visual2DRotatable(posXna1 - (posPointer / 10),//- (posXna1 / 4), 
                                                                        posXna2 + (posPointer / 10),//+ (posXna2 / 4), 
                                                                        3,
                                                                        TextureType.RealRope);
            visual2DRotatable.Draw(gameTime);
            //visual2DRotatable.Draw(gameTime, Color.);
        }
    };
}
