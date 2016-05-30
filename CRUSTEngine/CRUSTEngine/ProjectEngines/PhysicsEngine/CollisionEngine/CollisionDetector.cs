using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;

namespace CRUSTEngine.ProjectEngines.PhysicsEngine.CollisionEngine
{
    [Serializable]
    public struct CollisionData
    {
        public List<Contact> contacts;
        public int contactsLeft;
        /** Holds the friction value to write into any collisions. */
        public float friction;

        /** Holds the restitution value to write into any collisions. */
        public float restitution;

        public int contactCount;
        public int index;
        public void addContacts(int count)
        {
            // Reduce the number of contacts remaining, add number used
            contactsLeft -= count;
            contactCount += count;

            // Move the array forward
            index++;
        }
    }

    [Serializable]
    static class CollisionDetector
    {
        public static bool sphereAndHalfSpace(
            SphereRigid sphere,
            CollisionPlane plane,
            ref CollisionData data
            )
        {
            // Make sure we have contacts
            if (data.contactsLeft <= 0) return false;

            // Cache the sphere position
            Vector3 position = sphere.PositionCenterEngine;

            // Find the distance from the plane
            float ballDistance =
                Vector3.Dot(plane.Direction, position) -
                sphere.Radius - plane.Offset;

            if (ballDistance >= 0) return false;

            Contact c = new Contact();
            c.ContactNormal = plane.Direction;
            c.Penetration = -ballDistance;
            c.ContactPoint =
                position - plane.Direction * (ballDistance + sphere.Radius);
            c.SetBodyData(sphere, null,
                data.friction, data.restitution);


            c.ContactToWorld.M11 = plane.Direction.X;
            c.ContactToWorld.M12 = -plane.Direction.Y;
            c.ContactToWorld.M21 = plane.Direction.Y;
            c.ContactToWorld.M22 = plane.Direction.X;

            data.contacts.Add(c);

            sphere.SetCanSleep(true);

            data.addContacts(1);

            return true;
        }



        public static bool sphereAndSphere(
            SphereRigid one,
            SphereRigid two,
            ref CollisionData data
            )
        {
            // Cache the sphere positions
            Vector3 positionOne = one.PositionCenterEngine;
            Vector3 positionTwo = two.PositionCenterEngine;

            // Find the vector between the objects
            Vector3 midline = positionOne - positionTwo;
            float size = midline.Length();

            // See if it is large enough.
            if (size <= 0.0f || size >= one.Radius + two.Radius)
            {
                return false;
            }

            // We manually create the normal, because we have the
            // size to hand.
            Vector3 normal = midline * (((float)1.0) / size);
            
            Contact c = new Contact();
            c.ContactNormal = normal;                              
            c.ContactPoint = positionTwo + midline * (float)0.5;   
            c.Penetration = (one.Radius + two.Radius - size);
            c.SetBodyData(one, two,
                data.friction, data.restitution);

          

            c.ContactToWorld.M11 = normal.X;
            c.ContactToWorld.M12 = -normal.Y;
            c.ContactToWorld.M21 = normal.Y;
            c.ContactToWorld.M22 = normal.X;

         
            data.contacts.Add(c);

            one.SetCanSleep(true);
            two.SetCanSleep(true);
            data.addContacts(1);
            return true;
        }



        static float transformToAxis(
            BoxRigid box,
            Vector3 axis
            )
        {
            return
                box.HalfSize.X * (float)Math.Abs(Vector3.Dot(axis, ((BoxRigid)box).XAxis)) +
                box.HalfSize.Y * (float)Math.Abs(Vector3.Dot(axis, ((BoxRigid)box).YAxis));
        }

        static bool IntersectionTestboxAndHalfSpace(
            BoxRigid box,
            CollisionPlane plane
            )
        {
            // Work out the projected radius of the box onto the plane direction
            float projectedRadius = transformToAxis(box, plane.Direction);

            // Work out how far the box is from the origin
            float boxDistance =
                Vector3.Dot(plane.Direction,
                box.PositionCenterEngine) -
                projectedRadius;

            // Check for the intersection
            return boxDistance <= plane.Offset;
        }


        public static bool boxAndHalfSpace(
            BoxRigid box,
            CollisionPlane plane,
            ref CollisionData data
            )
        {
            // Make sure we have contacts
            if (data.contactsLeft <= 0) return false;

            // Check for intersection
            if (!IntersectionTestboxAndHalfSpace(box, plane))
            {
                return false;
            }

            bool b = false;
            int contactsUsed = 0;
            for (int i = 0; i < 4; i++)
            {
                  Vector3 vertexPos = ((BoxRigid)box).vertices[i].Position;

                // Calculate the distance from the plane
                float vertexDistance = Vector3.Dot(vertexPos, plane.Direction);

                // Compare this to the plane's distance
                if (vertexDistance <= plane.Offset)
                {
                    b = true;
                    // Create the contact data.

                    // The contact point is halfway between the vertex and the
                    // plane - we multiply the direction by half the separation
                    // distance and add the vertex location.

                    Contact c = new Contact();
                    c.ContactPoint = plane.Direction;
                    c.ContactPoint *= (0.5f * (plane.Offset - vertexDistance));
                    c.ContactPoint = vertexPos;
                    c.ContactNormal = plane.Direction;
                    c.Penetration = plane.Offset - vertexDistance;

                    c.Particle[0] = box;
                    c.Particle[1] = null;
                    c.Friction = data.friction;
                    c.Restitution = data.restitution;

                    c.Friction = StaticData.FrictionTable[(int)plane.Material][(int)box.GetMaterial()];
                    c.Restitution = StaticData.RestitutionTable[(int)plane.Material][(int)box.GetMaterial()];


                    c.ContactToWorld.M11 = plane.Direction.X;
                    c.ContactToWorld.M12 = -plane.Direction.Y;
                    c.ContactToWorld.M21 = plane.Direction.Y;
                    c.ContactToWorld.M22 = plane.Direction.X;

                    data.contacts.Add(c);

                    box.SetCanSleep(true);
                   
                    data.index++;
                    data.contactsLeft--;
                    data.contactCount++;

                   
                    contactsUsed++;
                    
                }
            }
            return b;

       
        }

        public static bool overlapOnAxis(
            BoxRigid one,
            BoxRigid two,
            Vector3 axis,
            Vector3 toCentre
            )
        {
            // Project the half-size of one onto axis
            float oneProject = transformToAxis(one, axis);
            float twoProject = transformToAxis(two, axis);

            // Project this onto the axis
            float distance = Math.Abs(Vector3.Dot(toCentre, axis));

            // Check for overlap
            return (distance < oneProject + twoProject);
        }


        public static bool IntersectionTestsboxAndBox(
        BoxRigid one,
        BoxRigid two
            )
        {
            // Find the vector between the two centres
            Vector3 toCentre = two.PositionCenterEngine - one.PositionCenterEngine;


            return (
                // Check on box one's axes first
                overlapOnAxis(one, two, ((BoxRigid)one).XAxis, toCentre) &&
                overlapOnAxis(one, two, ((BoxRigid)one).YAxis, toCentre) &&

                // And on two's
                overlapOnAxis(one, two, ((BoxRigid)two).XAxis, toCentre) &&
                overlapOnAxis(one, two, ((BoxRigid)two).YAxis, toCentre) //&& 
            );
        }


        /*
         * This function checks if the two boxes overlap
         * along the given axis, returning the ammount of overlap.
         * The final parameter toCentre
         * is used to pass in the vector between the boxes centre
         * points, to avoid having to recalculate it each time.
         */
        static float penetrationOnAxis(
             BoxRigid one,
             BoxRigid two,
             Vector3 axis,
             Vector3 toCentre
            )
        {
            // Project the half-size of one onto axis
            float oneProject = transformToAxis(one, axis);
            float twoProject = transformToAxis(two, axis);

            // Project this onto the axis
            float distance = Math.Abs(Vector3.Dot(toCentre, axis));

            // Return the overlap (i.e. positive indicates
            // overlap, negative indicates separation).
            return oneProject + twoProject - distance;
        }

        static bool tryAxis(
             BoxRigid one,
             BoxRigid two,
             Vector3 axis,
             Vector3 toCentre,
             int index,

            // These values may be updated
            ref float smallestPenetration,
            ref int smallestCase
            )
        {
            // Make sure we have a normalized axis, and don't check almost parallel axes
            if (axis.Length() * axis.Length() < 0.00001) return true;
            axis.Normalize();

            float penetration = penetrationOnAxis(one, two, axis, toCentre);

            if (penetration < 0) return false;
            if (penetration < smallestPenetration)
            {
                smallestPenetration = penetration;
                smallestCase = index;
            }
            return true;
        }

        static void fillPointFaceBoxBox(
            BoxRigid one,
            BoxRigid two,
            Vector3 toCentre,
            ref CollisionData data,
            int best,
            float pen
            )
        {
            // This method is called when we know that a vertex from
            // box two is in contact with box one.

            // Contact contact = data.contacts;

            // We know which axis the collision is on (i.e. best),
            // but we need to work out which of the two faces on
            // this axis.
           
            Vector3 normal;
            if (best == 0)
            {
                normal = ((BoxRigid)one).XAxis;
            }
            else
            {
                normal = ((BoxRigid)one).YAxis;
            }


            if (Vector3.Dot(normal, toCentre) > 0)   /*one.getAxis(best) * toCentre*/
            {
                normal = normal * -1.0f;
            }

            // Work out which vertex of box two we're colliding with.
            // Using toCentre doesn't work!
            Vector3 vertex = two.HalfSize;

            if (Vector3.Dot(((BoxRigid)two).XAxis, normal) < 0) vertex.X = -vertex.X;
            if (Vector3.Dot(((BoxRigid)two).YAxis, normal) < 0) vertex.Y = -vertex.Y;

            vertex = Matrix2.M_V(vertex, two.GetOrientation());
            vertex += two.PositionCenterEngine;

            Contact c = new Contact();
            c.ContactToWorld.M11 = normal.X;
            c.ContactToWorld.M12 = -normal.Y;
            c.ContactToWorld.M21 = normal.Y;
            c.ContactToWorld.M22 = normal.X;

            // Create the contact data
            c.ContactNormal = normal;
            c.Penetration = pen;
            c.ContactPoint = vertex; //Matrix2.transform(data.contacts[data.index].contactToWorld, vertex);     /////////////////// مشكوكة
            c.SetBodyData(one, two,
                        data.friction, data.restitution);


            data.contacts.Add(c);
        }


        public static bool boxAndBox(
             BoxRigid one,
             BoxRigid two,
             ref CollisionData data
             )
        {
            if (!IntersectionTestsboxAndBox(one, two)) return false;

            // Find the vector between the two centres
            Vector3 toCentre = two.PositionCenterEngine - one.PositionCenterEngine;

            // We start assuming there is no contact
            float pen = float.MaxValue;
            int best = 0xffffff;

            // Now we check each axes, returning if it gives us
            // a separating axis, and keeping track of the axis with
            // the smallest penetration otherwise.

            if (!tryAxis(one, two, (one).XAxis, toCentre, 0, ref pen, ref best)) return false;
            if (!tryAxis(one, two, (one).YAxis, toCentre, 1, ref pen, ref best)) return false;

            if (!tryAxis(one, two, (two).XAxis, toCentre, 2, ref pen, ref best)) return false;
            if (!tryAxis(one, two, (two).YAxis, toCentre, 3, ref pen, ref best)) return false;


            // Store the best axis-major, in case we run into almost
            // parallel edge collisions later
            int bestSingleAxis = best;

            // Make sure we've got a result.

            if (best != 0xffffff)
            {
                // We now know there's a collision, and we know which
                // of the axes gave the smallest penetration. We now
                // can deal with it in different ways depending on
                // the case.
                if (best < 2)
                {
                    // We've got a vertex of box two on a face of box one.
                    fillPointFaceBoxBox(one, two, toCentre, ref data, best, pen);
                    one.SetCanSleep(true);
                    two.SetCanSleep(true);
                    data.addContacts(1);
                }
                else if (best < 4)
                {
                    fillPointFaceBoxBox(two, one, toCentre * -1.0f, ref data, best - 2, pen);
                    one.SetCanSleep(true);
                    two.SetCanSleep(true);
                    data.addContacts(1);
                }
                return true;
            }
            return false;
        }

        public static bool boxAndSphere(
        BoxRigid box,
        SphereRigid sphere,
        ref CollisionData data
        )
        {
            // Transform the centre of the sphere into box coordinates
            Vector3 centre = sphere.PositionCenterEngine;
            Vector3 relCentre = centre - box.PositionCenterEngine; //box.transform.transformInverse(centre);

            relCentre = Matrix2.M_V(relCentre, -box.GetOrientation());

            // Early out check to see if we can exclude the contact
            if (Math.Abs(relCentre.X) - sphere.Radius > box.HalfSize.X ||
                Math.Abs(relCentre.Y) - sphere.Radius > box.HalfSize.Y
                )
            {
                return false;
            }

            Vector3 closestPt = new Vector3(0, 0, 0);
            float dist;

            // Clamp each coordinate to the box.
            dist = relCentre.X;
            if (dist > box.HalfSize.X) dist = box.HalfSize.X;
            if (dist < -box.HalfSize.X) dist = -box.HalfSize.X;
            closestPt.X = dist;

            dist = relCentre.Y;
            if (dist > box.HalfSize.Y) dist = box.HalfSize.Y;
            if (dist < -box.HalfSize.Y) dist = -box.HalfSize.Y;
            closestPt.Y = dist;

            // Check we're in contact
            Vector3 temp1 = closestPt - relCentre;
            if (temp1 == Vector3.Zero)
                return true;
            float temp2 = temp1.Length();
            double temp3 = temp2;
            dist = (float)Math.Pow(temp3, 2);
            if (dist > sphere.Radius * sphere.Radius) return false;

            // Compile the contact
            Vector3 closestPtWorld = closestPt;//box.transform.transform(closestPt);

            closestPtWorld = Matrix2.M_V(closestPtWorld, box.GetOrientation());
            closestPtWorld += box.PositionCenterEngine;
            //Contact* contact = data->contacts;


            // Create the contact data

            Vector3 temp = closestPtWorld - centre;
            temp.Normalize();


            Contact c = new Contact();

            c.ContactNormal = temp;
            c.Penetration = (float)(sphere.Radius - Math.Sqrt(dist));
            c.ContactPoint = closestPtWorld;    
            c.SetBodyData(box, sphere,
            data.friction, data.restitution);

            c.Friction = StaticData.FrictionTable[(int)sphere.GetMaterial()][(int)box.GetMaterial()];
            // between bump and cookie
            c.Restitution = 1f; // StaticData.RestitutionTable[(int)sphere.GetMaterial()][(int)box.GetMaterial()];

            c.ContactToWorld.M11 = c.ContactNormal.X;
            c.ContactToWorld.M12 = -c.ContactNormal.Y;
            c.ContactToWorld.M21 = c.ContactNormal.Y;
            c.ContactToWorld.M22 = c.ContactNormal.X;


            data.addContacts(1);

            data.contacts.Add(c);

            return true;
        }
    }
}
