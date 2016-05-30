using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;

namespace CRUSTEngine.ProjectEngines.HelperModules
{
    public class MathHelperModule
    {
        public static float Normalize(float value, float max, float min)
        {
            if (min == max)
            {
                return 0;
            }

            float normalized = (value - min) / (float)(max - min);
            if (normalized > 1)
            {
                normalized = 1;
            }
            if (normalized < 0)
            {
                normalized = 0;
            }
            return normalized;
        }

        public static float CalcStd(List<int> distances)
        {
            double avg = CalcAvg(distances);

            double diffs = 0;
            distances.ForEach(i => diffs += Math.Pow((i - avg), 2));

            double div = diffs / avg;

            float res = (float)Math.Sqrt(div);

            return res;
        }

        public static float CalcAvg(List<int> distances)
        {
            double total = CalcTotal(distances);
            double avg = total / (float)distances.Count;
            return (float)avg;
        }

        public static float CalcTotal(List<int> distances)
        {
            double total = 0;
            distances.ForEach(i => total += i);

            return (float)total;
        }

        public static bool IsIntersecting(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            float firstLineSlopeX, firstLineSlopeY, secondLineSlopeX, secondLineSlopeY;

            firstLineSlopeX = b.X - a.X;
            firstLineSlopeY = b.Y - a.Y;

            secondLineSlopeX = d.X - c.X;
            secondLineSlopeY = d.Y - c.Y;

            float s, t;
            s = (-firstLineSlopeY * (a.X - c.X) + firstLineSlopeX * (a.Y - c.Y)) /
                (-secondLineSlopeX * firstLineSlopeY + firstLineSlopeX * secondLineSlopeY);
            t = (secondLineSlopeX * (a.Y - c.Y) - secondLineSlopeY * (a.X - c.X)) /
                (-secondLineSlopeX * firstLineSlopeY + firstLineSlopeX * secondLineSlopeY);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                float intersectionPointX = a.X + (t * firstLineSlopeX);
                float intersectionPointY = a.Y + (t * firstLineSlopeY);
                return true;
            }

            return false; // No collision
        }

        public static bool FindIntersection(Vector2 a, Vector2 b, Vector2 c, Vector2 d,
            out Vector2 pInter)
        {
            float firstLineSlopeX, firstLineSlopeY, secondLineSlopeX, secondLineSlopeY;

            firstLineSlopeX = b.X - a.X;
            firstLineSlopeY = b.Y - a.Y;

            secondLineSlopeX = d.X - c.X;
            secondLineSlopeY = d.Y - c.Y;

            float s, t;
            s = (-firstLineSlopeY * (a.X - c.X) + firstLineSlopeX * (a.Y - c.Y)) /
                (-secondLineSlopeX * firstLineSlopeY + firstLineSlopeX * secondLineSlopeY);
            t = (secondLineSlopeX * (a.Y - c.Y) - secondLineSlopeY * (a.X - c.X)) /
                (-secondLineSlopeX * firstLineSlopeY + firstLineSlopeX * secondLineSlopeY);

            //if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                pInter.X = (a.X + (t * firstLineSlopeX));
                pInter.Y = (a.Y + (t * firstLineSlopeY));
                return true;
            }
            pInter.X = -100;
            pInter.Y = -100;
            return false; // No collision
        }

        public static Rectangle GetInverseYRectangle(Rectangle rectIn)
        {
            Rectangle rectToReturn = new Rectangle()
            {
                X = rectIn.X,
                Y = -rectIn.Y,
                Width = rectIn.Width,
                Height = rectIn.Height
            };
            return rectToReturn;
        }

        public static Vector3 GetInverseYPosition(Vector3 positionIn)
        {
            Vector3 posToReturn = new Vector3
            {
                X = positionIn.X,
                Y = -positionIn.Y,
                Z = positionIn.Z
            };
            return posToReturn;
        }

        public static Rectangle GetSphereRigid2DCoordinatesPositionCenter(Vector3 positionCenter, float radius)
        {
            return GetInverseYRectangle(new Rectangle((int)(positionCenter.X - radius),
                                                      (int)(positionCenter.Y + radius),
                                                      (int)(radius * 2),
                                                      (int)(radius * 2)));
        }

        public static Rectangle GetSphereRigid2DCoordinatesPositionXNA(Vector3 positionXNA, float radius)
        {
            return GetInverseYRectangle(new Rectangle((int)positionXNA.X,
                                                      (int)positionXNA.Y,
                                                      (int)(radius * 2),
                                                      (int)(radius * 2)));
        }

        public static Rectangle GetBoxRigid2DCoordinatesPositionCenter(Vector3 positionCenter, float halfWidth, float halfHeight)
        {
            return GetInverseYRectangle(new Rectangle(
                                            (int)(positionCenter.X - halfWidth),
                                            (int)(positionCenter.Y + halfHeight),
                                            (int)(halfWidth * 2),
                                            (int)(halfHeight * 2)
                                            ));
        }


        public static Rectangle GetBoxRigid2DCoordinatesPositionXNA(Vector3 positionXNA, float halfWidth, float halfHeight)
        {
            return GetInverseYRectangle(new Rectangle(
                                            (int)(positionXNA.X),
                                            (int)(positionXNA.Y),
                                            (int)(halfWidth * 2),
                                            (int)(halfHeight * 2)
                                            ));
        }

        public static Rectangle Get2DRectangleForNonRigids(Vector3 positionXNA, float width, float height)
        {
            return new Rectangle((int)(positionXNA.X),
                                (int)(positionXNA.Y),
                                (int)(width),
                                (int)(height));
        }

        public static Vector3 GetPositionCenter(Vector3 positionXNA, float halfWidth, float halfHeight)
        {
            return GetInverseYPosition(new Vector3(
                                           (positionXNA.X + (halfWidth)),
                                           (positionXNA.Y + (halfHeight)),
                                           0));
        }

        public static Vector3 GetPositionCenter(Vector3 positionXNA, float radius)
        {
            return GetInverseYPosition(new Vector3(
                                           (positionXNA.X + radius),
                                           (positionXNA.Y + radius),
                                           0));
        }


        public static Vector3 GetPositionXNA(Vector3 positionCenter, float halfWidth, float halfHeight)
        {
            return GetInverseYPosition(new Vector3(
                                   (positionCenter.X - (halfWidth)),
                                   (positionCenter.Y + (halfHeight)),
                                   0));
        }


        public static Vector3 GetPositionXNA(Vector3 positionCenter, float radius)
        {
            return GetInverseYPosition(new Vector3(
                                           (positionCenter.X - radius),
                                           (positionCenter.Y + radius),
                                           0));
        }

        // NOT USED, AND IT WON'T BE USED!
        //public static Rectangle EnlargeRectangleArea(Rectangle rectangleArea, int enlargingFactor)
        //{
        //    var rectToReturn = new Rectangle
        //    {
        //        X = rectangleArea.X,
        //        Y = rectangleArea.Y,
        //        Width = rectangleArea.Width * enlargingFactor,
        //        Height = rectangleArea.Height * enlargingFactor
        //    };
        //    return rectToReturn;
        //}

        public static Vector2 ScaleRectByTexture(Texture2D texture, Rectangle rectangleArea)
        {
            Vector2 scaledVector = Vector2.One;
            if (texture.Width < rectangleArea.Width)
            {
                scaledVector.X = (float)rectangleArea.Width / texture.Width;
            }
            else
            {
                scaledVector.X = 1 / ((float)texture.Width / rectangleArea.Width);
            }
            if (texture.Height < rectangleArea.Height)
            {
                scaledVector.Y = (float)rectangleArea.Height / texture.Height;
            }
            else
            {
                scaledVector.Y = 1 / ((float)texture.Height / rectangleArea.Height);
            }
            return scaledVector;
        }

        public static float GetAngleBetweenTwoVectors(Vector2 vectorStart, Vector2 vectorEnd)
        {
            float dot = Vector2.Dot(vectorStart, vectorEnd);
            float angle = ((float)Math.Acos(dot));
            return angle;
        }

        public static float GetAngleBetweenTwoVectors(Vector3 vectorStart, Vector3 vectorEnd)
        {
            Vector2 vecStart = new Vector2(vectorStart.X, vectorStart.Y);
            Vector2 vecEnd = new Vector2(vectorEnd.X, vectorEnd.Y);
            float dot = Vector2.Dot(vecStart, vecEnd);
            float angle = ((float)Math.Acos(dot));
            return angle;
        }

        public static Vector3 GetPositionCenterXNA(RigidBody rigidBody)
        {
            if (rigidBody is BoxRigid)
            {
                return GetPositionCenterXNA(rigidBody.PositionXNA, ((BoxRigid) rigidBody).HalfSize);
            }
            else
            {
                if (rigidBody is SphereRigid)
                {
                    return GetPositionCenterXNA(rigidBody.PositionXNA, ((SphereRigid)rigidBody).Radius);
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        public static Vector3 GetPositionCenterXNA(Vector3 positionXNA, Vector3 halfSize)
        {
            return new Vector3(positionXNA.X + halfSize.X /2, positionXNA.Y + halfSize.Y/2, 0);
        }

        public static Vector3 GetPositionCenterXNA(Vector3 positionXNA, float radius)
        {
            return new Vector3(positionXNA.X + radius, positionXNA.Y + radius, 0);
        }

        public static Vector3 GetVector3(Vector2 vec2)
        {
            return new Vector3(vec2.X, vec2.Y, 0);
        }

        public static Vector2 Get2DVector(Vector3 pos)
        {
            return new Vector2(pos.X, pos.Y);
        }
    }
}
