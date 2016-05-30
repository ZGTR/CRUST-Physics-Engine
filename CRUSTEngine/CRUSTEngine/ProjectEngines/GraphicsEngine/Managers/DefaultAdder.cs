using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Bubble;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.CatchableRopes;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components;
using CRUSTEngine.ProjectEngines.PhysicsEngine;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs;

namespace CRUSTEngine.ProjectEngines.GraphicsEngine.Managers
{
    [Serializable]
    public class DefaultAdder
    {
        public static BoxRigid GetDefaultBox(
            Vector3 positionXNA,
            Material material,
            Vector3 halfSize,
            Vector3? acc,
            Vector3? initialForce,
            Vector3? initialTorque,
            float orientationValue = 0,
            bool obInertia = false)
        {
            BoxRigid rectToReturn = new BoxRigid(positionXNA,
                                                            material,
                                                            new Vector3(halfSize.X, halfSize.Y, 0));
            // Inertia
            if (obInertia)
            {
                rectToReturn.SetObInertia();
                // Awaking
                rectToReturn.SetAwake(true);
                return rectToReturn;
            }

            // Acc
            Vector3 accVector = acc ?? new Vector3(0, -StaticData.GravityConstant, 0);
            rectToReturn.SetAcceleration(accVector);

            // Force
            Vector3 forceIn = initialForce ?? new Vector3(0, 0, 0);
            rectToReturn.AddForce(forceIn);

            // Torque
            Vector3 torqueIn = initialTorque ?? new Vector3(0, 0, 0);
            rectToReturn.AddTorque(torqueIn, MathHelperModule.GetInverseYPosition(rectToReturn.PositionCenterEngine));

            // Orientation
            rectToReturn.SetOrientation(orientationValue);

            // Awaking
            rectToReturn.SetAwake(true);

            return rectToReturn;
        }

        public static BoxRigidHardConstraint GetDefaultBoxRigidHardConstraint(
                              Vector3 positionXNA,
                              Material material,
                              Vector3 halfSize,
                              Vector3? acc,
                              Vector3? initialForce,
                              Vector3? initialTorque,
                              float orientationValue = 0,
                              bool obInertia = false)
        {
            BoxRigidHardConstraint rectToReturn = new BoxRigidHardConstraint(positionXNA,
                                                            material,
                                                            new Vector3(halfSize.X, halfSize.Y, 0));
            // Inertia
            if (obInertia)
            {
                rectToReturn.SetObInertia();
                // Awaking
                rectToReturn.SetAwake(true);
                return rectToReturn;
            }

            // Acc
            Vector3 accVector = acc ?? new Vector3(0, -StaticData.GravityConstant, 0);
            rectToReturn.SetAcceleration(accVector);

            // Force
            Vector3 forceIn = initialForce ?? new Vector3(0, 0, 0);
            rectToReturn.AddForce(forceIn);

            // Torque
            Vector3 torqueIn = initialTorque ?? new Vector3(0, 0, 0);
            rectToReturn.AddTorque(torqueIn, MathHelperModule.GetInverseYPosition(rectToReturn.PositionCenterEngine));

            // Orientation
            rectToReturn.SetOrientation(orientationValue);

            // Awaking
            rectToReturn.SetAwake(true);

            return rectToReturn;
        }

        public static SphereRigid GetDefaultSphere(
            Vector3 positionXNA,
            Material material,
            float radius,
            Vector3? acc,
            Vector3? initialForce,
            Vector3? initialTorque,
            float orientationValue = 0 ,
            bool obInertia = false)
        {
            SphereRigid rectToReturn = new SphereRigid(positionXNA,
                                                            material,
                                                            radius);
            // Inertia
            if (obInertia)
            {
                rectToReturn.SetObInertia();
                // Awaking
                rectToReturn.SetAwake(true);
                return rectToReturn;
            }

            // Acc
            Vector3 accVector = acc ?? new Vector3(0, 0, 0);
            rectToReturn.SetAcceleration(accVector);

            // Forece
            Vector3 forceIn = initialForce ?? new Vector3(0, 0, 0);
            rectToReturn.AddForce(forceIn);

            // Torque
            Vector3 torqueIn = initialTorque ?? new Vector3(0, 0, 0);
            rectToReturn.AddTorque(torqueIn, MathHelperModule.GetInverseYPosition(rectToReturn.PositionCenterEngine));

            // Orientation
            rectToReturn.SetOrientation(orientationValue);

            // Awaking
            rectToReturn.SetAwake(true);
            return rectToReturn;
        }


        public static SpringService GetDefaultSpringRope(Vector3 initialPosition, Vector3? targetPos, int nrOfMasses, 
            float springConstant, float normalLength, float springInnerFriction, 
            RigidType type, Vector3 rigidSize, bool isCollidable, SpringType springType)
        {
            List<RigidBody> masses = GetMassesRope(nrOfMasses, initialPosition, targetPos, rigidSize, normalLength, type,
                                                   isCollidable);
            SpringService springService = new SpringService(springConstant, // springConstant In The Rope
                                                  normalLength, // Normal Length Of Springs In The Rope
                                                  springInnerFriction, // Spring Inner Friction Constant
                                                  masses,
                                                  springType);
            return springService;
        }

        public static CatchableRopeService GetDefaultCatchableRope(Vector3 initialPosition,
            Vector3? targetPos,
            int nrOfMasses,
            float springConstant,
            float normalLength, 
            float springInnerFriction,
            RigidType type,
            Vector3 rigidSize,
            bool isCollidable,
            SpringType springType)
        {
            //List<RigidBody> masses = GetMassesRope(nrOfMasses, initialPosition, targetPos, rigidSize, normalLength, type, isCollidable);
            CatchableRopeService springService = new CatchableRopeService(nrOfMasses, initialPosition, springConstant, // springConstant In The Rope
                                                  normalLength, // Normal Length Of Springs In The Rope
                                                  springInnerFriction, // Spring Inner Friction Constant
                                                  isCollidable,
                                                  rigidSize,
                                                  springType,
                                                  type);
            return springService;
        }

        public static List<RigidBody> GetMassesRope(int nrOfMasses, Vector3 initialPosition, Vector3? targetPosition,
            Vector3 rigidSize, float normalLength, RigidType type, bool isCollidable)
        {
            double diffX = 0, diffY = 0;
            if (targetPosition != null)
            {
                diffX = Math.Abs(targetPosition.Value.X - initialPosition.X) / nrOfMasses;
                diffY = Math.Abs(targetPosition.Value.Y - initialPosition.Y) / nrOfMasses;
            }

            Vector3 massPos = initialPosition;
            var masses = new List<RigidBody>();
            for (int i = 0; i < nrOfMasses; ++i)
            {
                if (type == RigidType.BoxRigid)
                {
                    masses.Add(DefaultAdder.GetDefaultBox(massPos,
                                                          Material.Rubber, rigidSize,
                                                          new Vector3(0, 0f, 0), null,
                                                          null, 0,
                                                          false));
                    masses[masses.Count - 1].IsCollidable = isCollidable;
                    masses[masses.Count - 1].TextureType = TextureType.Transparent;
                }
                else
                {
                    masses.Add(DefaultAdder.GetDefaultSphere(massPos,
                                                             Material.Rubber, rigidSize.X,
                                                             new Vector3(0, -4f, 0),
                                                             null,
                                                             null, 0,
                                                             false));
                    masses[masses.Count - 1].IsCollidable = isCollidable;
                    masses[masses.Count - 1].TextureType = TextureType.Transparent;
                }
                StaticData.EngineManager.RigidsManagerEngine.AddRigidBody(masses[i]);
                if (targetPosition == null)
                {
                    massPos += new Vector3(0, normalLength, 0);
                }
                else
                {
                    if (initialPosition.X < targetPosition.Value.X)
                    {
                        massPos.X = (float) (initialPosition.X + i*diffX);
                    }
                    else
                    {
                        massPos.X = (float)(initialPosition.X - i * diffX );
                    }

                    if (initialPosition.Y < targetPosition.Value.Y)
                    {
                        massPos.Y = (float)(initialPosition.Y + i * diffY);
                    }
                    else
                    {
                        massPos.Y = (float)(initialPosition.Y - i * diffY);
                    }
                }
            }
            return masses;
        }

        public static BubbleService GetDefaultBubble(Vector2 positionXNA)
        {
            return new BubbleService(new Vector3(positionXNA.X, positionXNA.Y, 0));
        }
    }
}
