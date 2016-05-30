using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.PhysicsEngine.CollisionEngine;

namespace CRUSTEngine.ProjectEngines.PhysicsEngine.Managers
{
    [Serializable]
    public class PlanesManager : IUpdatableComponent
    {
        public List<CollisionPlane> ListOfPlanes { set; get; }

        public PlanesManager()
        {
            ListOfPlanes = new List<CollisionPlane>();

            //// Bottom
            //int offset1 = StaticData.PlaneOffsetBottom;
            //Vector3 direction1 = new Vector3(0, 1, 0);
            //direction1.Normalize();
            //Rectangle rect1 = new Rectangle(0,
            //                                offset1,
            //                                StaticData.LevelFarWidth,
            //                                10);
            //CollisionPlane plane1 = new CollisionPlane(-(offset1), direction1, Material.Wood,
            //                                           TextureType.DefaultBox,
            //                                           rect1);
            //// Right
            //int offset2 = StaticData.PlaneOffsetRight;
            //Vector3 direction2 = new Vector3(-1, 0, 0);
            //direction2.Normalize();
            //Rectangle rect2 = new Rectangle(offset2,
            //                                0,
            //                                10,
            //                                StaticData.LevelFarWidth);
            //CollisionPlane plane2 = new CollisionPlane(-offset2, direction2, Material.Steel,
            //                                           TextureType.DefaultBox,
            //                                           rect2);

            ////Left
            //int offset3 = StaticData.PlaneOffsetLeft;
            //Vector3 direction3 = new Vector3(1, 0, 0);
            //direction3.Normalize();
            //Rectangle rect3 = new Rectangle(offset3 - 10,
            //                                0,
            //                                10,
            //                                StaticData.LevelFarWidth);
            //CollisionPlane plane3 = new CollisionPlane(-offset3, direction3, Material.Steel,
            //                                           TextureType.DefaultBox,
            //                                           rect3);

            // Up
            int offset4 = StaticData.PlaneOffsetUp;
            Vector3 direction4 = new Vector3(0, -1, 0);
            direction4.Normalize();
            Rectangle rect4 = new Rectangle(0,
                                            offset4 - 10,
                                            StaticData.LevelFarWidth, 10);
            CollisionPlane plane4 = new CollisionPlane(-offset4, direction4, Material.Steel,
                                                       TextureType.DefaultBox,
                                                       rect4);
           
            //this.ListOfPlanes.Add(plane1);
            //this.ListOfPlanes.Add(plane2);
            //this.ListOfPlanes.Add(plane3);
            this.ListOfPlanes.Add(plane4);
            
        }

        public void AddPlane(CollisionPlane plane)
        {
            ListOfPlanes.Add(plane);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var plane in ListOfPlanes)
            {
                plane.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var plane in ListOfPlanes)
            {
                plane.Draw(gameTime);
            }
        }
    }
}