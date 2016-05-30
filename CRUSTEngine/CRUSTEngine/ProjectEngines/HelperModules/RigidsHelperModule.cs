using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;

namespace CRUSTEngine.ProjectEngines.HelperModules
{
    public class RigidsHelperModule
    {
        public static RigidBody CatchNearestRigidBody(Visual2D refVisual2D, int areaRadius, bool isFirstCatch = false)
        {
            RigidBody rigidToReturn = null;
            foreach (var rigidBox in StaticData.EngineManager.RigidsManagerEngine.ListOfBoxRigids)
            {
                if (IsCloseEnough(rigidBox, refVisual2D, areaRadius))
                {
                    rigidToReturn = rigidBox;
                    if (isFirstCatch)
                    {
                        break;
                    }
                }
            }
            if (rigidToReturn == null)
            {
                foreach (var rigidCircle in StaticData.EngineManager.RigidsManagerEngine.ListOfSphereRigids)
                {
                    if (IsCloseEnough(rigidCircle, refVisual2D, areaRadius))
                    {
                        rigidToReturn = rigidCircle;
                        break;
                    }
                }
            }
            return rigidToReturn;
        }

        public static Visual2D CatchNearestVisual2D(Visual2D refVisual2D, List<Visual2D> listCompare,
            int areaRadius, bool isFirstCatch = false)
        {
            Visual2D visual2DToReturn = null;
            float minDist = float.MaxValue;
            foreach (var visual2D in listCompare)
            {
                if (IsCloseEnough(visual2D, refVisual2D, areaRadius))
                {
                    float dis = Distance(visual2D, refVisual2D);
                    if (dis < minDist)
                    {
                        minDist = dis;
                        visual2DToReturn = visual2D;
                    }
                    if (isFirstCatch)
                    {
                        break;
                    }
                }
            }
            return visual2DToReturn;
        }

        public static float Distance(Visual2D c1, Visual2D c2)
        {
            float length = (c1.Center - c2.Center).Length();
            return length;
            //return false;
        }

        public static bool IsCloseEnough(Vector2 c1, Vector2 c2, int areaRadius)
        {
            float length = (c1 - c2).Length();
            if (length < areaRadius)
            {
                return true;
            }
            return false;
        }

        public static bool IsCloseEnough(RigidBody rigidBody, Visual2D component, int areaRadius)
        {
            float length = (rigidBody.Center - component.Center).Length
                ();
            if (length < areaRadius)
            {
                return true;
            }
            return false;
        }

        public static bool IsCloseEnough(Visual2D vis1, Visual2D vis2, int areaRadius)
        {
            float length = (new Vector3(vis1.RectangleArea.Center.X, vis1.RectangleArea.Center.Y, 0) -
                            new Vector3(vis2.RectangleArea.Center.X, vis2.RectangleArea.Center.Y, 0)).Length();
            if (length  < areaRadius)
            {
                return true;
            }
            return false;
        }

        public static bool IsCloseEnough(Visual2D vis, Vector3 posCenter, int areaRadius)
        {
            if ((new Vector3(vis.RectangleArea.Center.X, vis.RectangleArea.Center.Y, 0) -
                new Vector3(posCenter.X, posCenter.Y, 0)).Length() < areaRadius)
            {
                return true;
            }
            return false;
        }

        public static bool IsCloseEnough(RigidBody rigidBody, Vector3 posCenter, int areaRadius)
        {
            if ((rigidBody.PositionXNA + rigidBody.getHalfSize() - posCenter).Length() < areaRadius)
            {
                return true;
            }
            return false;
        }

        public static VertexPositionColor[] MakeNewVPCMatrix(VertexPositionColor[] vertices)
        {
            VertexPositionColor[] mat = new VertexPositionColor[vertices.Count()];
            for (int i = 0; i < vertices.Count(); i++)
            {
                mat[i] = new VertexPositionColor(vertices[i].Position, vertices[i].Color);
            }
            return mat;
        }

        public static double GetDistance(Visual2D v1, Visual2D v2)
        {
            try
            {
                return (new Vector3(v1.RectangleArea.Center.X, v1.RectangleArea.Center.Y, 0) -
                 new Vector3(v2.RectangleArea.Center.X, v2.RectangleArea.Center.Y, 0)).Length();
            }
            catch (Exception)
            {
            }
            return Double.MaxValue;
        }
    }
}
