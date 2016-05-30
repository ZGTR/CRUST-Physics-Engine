using System;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;

namespace CRUSTEngine.ProjectEngines.PhysicsEngine.CollisionEngine
{
    [Serializable]
    public class Contact
    {
        public RigidBody[] Particle = new RigidBody[2];
        public float Restitution;
        public Vector3 ContactNormal;
        public float Penetration;
        public Vector3 ContactPoint;
        public float Friction;
        /**
        * A transform matrix that converts coordinates in the contact’s
        * frame of reference to world coordinates. The columns of this
        * matrix form an orthonormal set of vectors.
        */
        public Matrix ContactToWorld;
        /**
        * Holds the closing velocity at the point of contact. This is
        * set when the calculateInternals function is run.
        */
        public Vector3 ContactVelocity;
        /**
        * Holds the required change in velocity for this contact to be
        * resolved.
        */
        public float DesiredDeltaVelocity;
        /**
        * Holds the world space position of the contact point
        * relative to the center of each body. This is set when
        * the calculateInternals function is run.
        */
        public Vector3[] RelativeContactPosition;

        public Contact()
        {
            Particle[0] = null;
            Particle[1] = null;
            Restitution = 0.5f;
            ContactNormal = new Vector3();
            Penetration = 0;
            RelativeContactPosition = new Vector3[2];
        }

        public Contact(RigidBody x)
        {
            Particle[0] = x;
            Particle[1] = null;
            Restitution = 0.5f;
            ContactNormal = new Vector3(0, 1, 0);
            Penetration = 0;
            RelativeContactPosition = new Vector3[2];
        }

        public Contact(RigidBody x1, RigidBody x2)
        {
            Particle[0] = x1;
            Particle[1] = x2;
            Restitution = 0.5f;
            //contactNormal = new Vector3(0, 1, 0);
            Penetration = 0;
            RelativeContactPosition = new Vector3[2];
        }

        public void MatchAwakeState()
        {
            // Collisions with the world never cause a body to wake up.
            if (Particle[1] == null) return;

            bool body0Awake = Particle[0].GetisAwake();
            bool body1Awake = Particle[1].GetisAwake();

            // Wake up only the sleeping one
            if (!(body0Awake && body1Awake))
            {
                if (body0Awake) Particle[1].SetAwake(true);
                else Particle[0].SetAwake(true);
            }
        }

        public void SetBodyData(RigidBody one, RigidBody two, float friction, float restitution)
        {
            Particle[0] = one;
            Particle[1] = two;
            this.Friction = friction;
            this.Restitution = restitution;
        }

        /*
         * Swaps the bodies in the current contact, so body 0 is at body 1 and
         * vice versa. This also changes the direction of the contact normal,
         * but doesn't update any calculated internal data. If you are calling
         * this method manually, then call calculateInternals afterwards to
         * make sure the internal data is up to date.
         */
        public void SwapBodies()
        {
            ContactNormal *= -1;

            RigidBody temp = Particle[0];
            Particle[0] = Particle[1];
            Particle[1] = temp;
        }

        public Vector3 CalculateLocalVelocity(int bodyIndex, float duration)
        {
            RigidBody thisBody = Particle[bodyIndex];

            // Work out the velocity of the contact point.
            Vector3 velocity =
                Vector3.Cross(new Vector3(0, 0, thisBody.GetRotation()), RelativeContactPosition[bodyIndex]);
            velocity += thisBody.GetVelocity();

            // Turn the velocity into contact-coordinates.
            Vector3 contactVelocity = Matrix2.transformTranspose(ContactToWorld, velocity);

            // Calculate the ammount of velocity that is due to forces without
            // reactions.
            Vector3 accVelocity = thisBody.GetLastFrameAcceleration() * duration;

            // Calculate the velocity in contact-coordinates.
            accVelocity = Matrix2.transformTranspose(ContactToWorld, accVelocity);

            // We ignore any component of acceleration in the contact normal
            // direction, we are only interested in planar acceleration
            accVelocity.X = 0;

            // Add the planar velocities - if there's enough friction they will
            // be removed during velocity resolution
            contactVelocity += accVelocity;

            // And return it
            return contactVelocity;
        }

        public void CalculateDesiredDeltaVelocity(float duration)
        {
            const /*static*/ float velocityLimit = 0.25f;

            // Calculate the acceleration induced velocity accumulated this frame
            float velocityFromAcc = 0;

            if (Particle[0].GetisAwake())
            {
                velocityFromAcc = Vector3.Dot(Particle[0].GetLastFrameAcceleration() * duration, ContactNormal);
            }

            if (Particle[1] != null && Particle[1].GetisAwake())
            {
                velocityFromAcc -= Vector3.Dot(Particle[1].GetLastFrameAcceleration()*duration, ContactNormal);
            }

            // If the velocity is very slow, limit the restitution
            float thisRestitution = Restitution;
            if (ContactVelocity.Length() < velocityLimit)
            {
                thisRestitution = 0.0f;
            }

            // Combine the bounce velocity with the removed
            // acceleration velocity.
            DesiredDeltaVelocity = -ContactVelocity.X - thisRestitution*(ContactVelocity.X - velocityFromAcc);
        }

        public void CalculateInternals(float duration)
        {
            // Check if the first object is NULL, and swap if it is.
            if (Particle[0] == null) SwapBodies();

            // Store the relative position of the contact relative to each body
            RelativeContactPosition[0] = ContactPoint - Particle[0].PositionCenterEngine;
            if (Particle[1] != null)
            {
                RelativeContactPosition[1] = ContactPoint - Particle[1].PositionCenterEngine;
            }

            // Find the relative velocity of the bodies at the contact point.
            ContactVelocity = CalculateLocalVelocity(0, duration);
            if (Particle[1] != null)
            {
                ContactVelocity -= CalculateLocalVelocity(1, duration);
            }

            // Calculate the desired change in velocity for resolution
            CalculateDesiredDeltaVelocity(duration);
        }


        Vector3 CalculateFrictionlessImpulse(Matrix[] inverseInertiaTensor)
        {
            Vector3 impulseContact;

            // Build a vector that shows the change in velocity in
            // world space for a unit impulse in the direction of the contact
            // normal.
            Vector3 deltaVelWorld = Vector3.Cross(RelativeContactPosition[0], ContactNormal);
            deltaVelWorld = Matrix2.transform(inverseInertiaTensor[0], deltaVelWorld);
            deltaVelWorld = Vector3.Cross(deltaVelWorld, RelativeContactPosition[0]);

            // Work out the change in velocity in contact coordiantes.
            float deltaVelocity = Vector3.Dot(deltaVelWorld, ContactNormal);

            // Add the linear component of velocity change
            deltaVelocity += Particle[0].GetInverseMass();

            // Check if we need to the second body's data
            if (Particle[1] != null)
            {
                // Go through the same transformation sequence again
                /*vector3*/
                deltaVelWorld = Vector3.Cross(RelativeContactPosition[1], ContactNormal);
                deltaVelWorld = Matrix2.transform(inverseInertiaTensor[1], deltaVelWorld);
                deltaVelWorld = Vector3.Cross(deltaVelWorld, RelativeContactPosition[1]);

                // Add the change in velocity due to rotation
                deltaVelocity += Vector3.Dot(deltaVelWorld, ContactNormal);

                // Add the change in velocity due to linear motion
                deltaVelocity += Particle[1].GetInverseMass();
            }

            // Calculate the required size of the impulse
            impulseContact.X = DesiredDeltaVelocity / deltaVelocity;
            impulseContact.Y = 0;
            impulseContact.Z = 0;
            return impulseContact;
        }

        Vector3 CalculateFrictionImpulse(Matrix[] inverseInertiaTensor)
        {
            float inverseMass = Particle[0].GetInverseMass();

            // The equivalent of a cross product in matrices is multiplication
            // by a skew symmetric matrix - we build the matrix for converting
            // between linear and angular quantities.
            Matrix impulseToTorque = Matrix2.setSkewSymmetric(RelativeContactPosition[0]);
            //impulseToTorque.setSkewSymmetric(relativeContactPosition[0]);

            // Build the matrix to convert contact impulse to change in velocity
            // in world coordinates.
            Matrix deltaVelWorld = impulseToTorque;
            deltaVelWorld *= inverseInertiaTensor[0];
            deltaVelWorld *= impulseToTorque;
            deltaVelWorld *= -1;

            // Check if we need to add body two's data
            if (Particle[1] != null)
            {
                // Set the cross product matrix
                impulseToTorque = Matrix2.setSkewSymmetric(RelativeContactPosition[1]);

                // Calculate the velocity change matrix
                Matrix deltaVelWorld2 = impulseToTorque;
                deltaVelWorld2 *= inverseInertiaTensor[1];
                deltaVelWorld2 *= impulseToTorque;
                deltaVelWorld2 *= -1;

                deltaVelWorld2.M44 = 1;

                // Add to the total delta velocity.
                deltaVelWorld += deltaVelWorld2;

                // Add to the inverse mass
                inverseMass += Particle[1].GetInverseMass();
            }

            // Do a change of basis to convert into contact coordinates.
            Matrix deltaVelocity = Matrix.Transpose(ContactToWorld);
            deltaVelocity.M44 = 1;
            deltaVelocity *= deltaVelWorld;
            deltaVelocity *= ContactToWorld;

            // Add in the linear velocity change
            deltaVelocity.M11 += inverseMass;
            deltaVelocity.M22 += inverseMass;
            deltaVelocity.M33 += inverseMass;
            deltaVelocity.M44 = 1;

            // Invert to get the impulse needed per unit velocity
            Matrix impulseMatrix = Matrix.Invert(deltaVelocity);

            // Find the target velocities to kill
            Vector3 velKill = new Vector3(DesiredDeltaVelocity,
                -ContactVelocity.Y,
                -ContactVelocity.Z);

            // Find the impulse to kill target velocities
            Vector3 impulseContact = Matrix2.transform(impulseMatrix, velKill);

            // Check for exceeding friction
            float planarImpulse = (float)Math.Sqrt(
                impulseContact.Y * impulseContact.Y +
                impulseContact.Z * impulseContact.Z
                );
            if (planarImpulse > impulseContact.X * Friction)
            {
                // We need to use dynamic friction
                impulseContact.Y /= planarImpulse;
                impulseContact.Z /= planarImpulse;

                impulseContact.X = deltaVelocity.M11 +
                    deltaVelocity.M12 * Friction * impulseContact.Y +
                    deltaVelocity.M13 * Friction * impulseContact.Z;
                impulseContact.X = DesiredDeltaVelocity / impulseContact.X;
                impulseContact.Y *= Friction * impulseContact.X;
                impulseContact.Z *= Friction * impulseContact.X;
            }
            return impulseContact;
        }

        public void ApplyVelocityChange(Vector3[] velocityChange, Vector3[] rotationChange)
        {
            // Get hold of the inverse mass and inverse inertia tensor, both in
            // world coordinates.
            Matrix[] inverseInertiaTensor = new Matrix[2];
            inverseInertiaTensor[0] = Particle[0].GetInverseInertiaTensorWorld();
            if (Particle[1] != null)
                inverseInertiaTensor[1] = Particle[1].GetInverseInertiaTensorWorld();

            // We will calculate the impulse for each contact axis
            Vector3 impulseContact;

            if (Friction == (float)0.0)
            {
                // Use the short format for frictionless contacts
                impulseContact = CalculateFrictionlessImpulse(inverseInertiaTensor);
            }
            else
            {
                // Otherwise we may have impulses that aren't in the direction of the
                // contact, so we need the more complex version.
                impulseContact = CalculateFrictionImpulse(inverseInertiaTensor);
            }

            // Convert impulse to world coordinates
            Vector3 impulse = Matrix2.transform(ContactToWorld, impulseContact);

            // Split in the impulse into linear and rotational components
            Vector3 impulsiveTorque = Vector3.Cross(RelativeContactPosition[0], impulse);
            rotationChange[0] = Matrix2.transform(inverseInertiaTensor[0], impulsiveTorque); //inverseInertiaTensor[0].transform(impulsiveTorque);
            velocityChange[0].X = 0;
            velocityChange[0].Y = 0;
            velocityChange[0].Z = 0;

            velocityChange[0] += (impulse * Particle[0].GetInverseMass());

            // Apply the changes
            if (!Particle[0].IsFixedRigid)
            {
                Particle[0].SetVelocity(Particle[0].GetVelocity() + velocityChange[0]);
                Particle[0].SetRotation(Particle[0].GetRotation() + rotationChange[0].Z);
            }


            if (Particle[1] != null)
            {
                if (!Particle[1].IsFixedRigid)
                {
                    // Work out body one's linear and angular changes
                    impulsiveTorque = Vector3.Cross(impulse, RelativeContactPosition[1]);
                    rotationChange[1] = Matrix2.transform(inverseInertiaTensor[1], impulsiveTorque);
                    velocityChange[1].X = 0;
                    velocityChange[1].Y = 0;
                    velocityChange[1].Z = 0;
                    velocityChange[1] += (impulse*-Particle[1].GetInverseMass());

                    // And apply them.
                    Particle[1].SetVelocity(Particle[1].GetVelocity() + velocityChange[1]);
                    Particle[1].SetRotation(Particle[1].GetRotation() + rotationChange[1].Z);
                }
            }
        }

        public void ApplyPositionChange(Vector3[] linearChange, Vector3[] angularChange, float penetration)
        {
            const float angularLimit = 0.2f;
            float[] angularMove = new float[2];
            float[] linearMove = new float[2];

            float totalInertia = 0;
            float[] linearInertia = new float[2];
            float[] angularInertia = new float[2];

            // We need to work out the inertia of each object in the direction
            // of the contact normal, due to angular inertia only.
            for (int i = 0; i < 2; i++)
            {
                if (Particle[i] != null)
                {
                    if (!Particle[i].IsFixedRigid)
                    {
                        Matrix inverseInertiaTensor = Particle[i].GetInverseInertiaTensorWorld();

                        // Use the same procedure as for calculating frictionless
                        // velocity change to work out the angular inertia.
                        Vector3 angularInertiaWorld =
                            Vector3.Cross(RelativeContactPosition[i], ContactNormal);
                        angularInertiaWorld =
                            Matrix2.transform(inverseInertiaTensor, angularInertiaWorld);
                        //inverseInertiaTensor.transform(angularInertiaWorld);
                        angularInertiaWorld =
                            Vector3.Cross(angularInertiaWorld, RelativeContactPosition[i]);
                        angularInertia[i] =
                            Vector3.Dot(angularInertiaWorld, ContactNormal);

                        // The linear component is simply the inverse mass
                        linearInertia[i] = Particle[i].GetInverseMass();

                        // Keep track of the total inertia from all components
                        totalInertia += linearInertia[i] + angularInertia[i];

                        // We break the loop here so that the totalInertia value is
                        // completely calculated (by both iterations) before
                        // continuing.
                    }
                }
            }

            // Loop through again calculating and applying the changes
            for (int i = 0; i < 2; i++)
            {
                if (Particle[i] != null)
                {
                    if (!Particle[i].IsFixedRigid)
                    {
                        // The linear and angular movements required are in proportion to
                        // the two inverse inertias.
                        float sign = (i == 0) ? 1 : -1;
                        angularMove[i] =
                            sign*penetration*(angularInertia[i]/totalInertia);
                        linearMove[i] =
                            sign*penetration*(linearInertia[i]/totalInertia);

                        // To avoid angular projections that are too great (when mass is large
                        // but inertia tensor is small) limit the angular move.
                        Vector3 projection = RelativeContactPosition[i];
                        projection += (
                                          ContactNormal*
                                          -Vector3.Dot(RelativeContactPosition[i], ContactNormal)
                                      );

                        // Use the small angle approximation for the sine of the angle (i.e.
                        // the magnitude would be sine(angularLimit) * projection.magnitude
                        // but we approximate sine(angularLimit) to angularLimit).
                        float maxMagnitude = angularLimit*projection.Length();

                        if (angularMove[i] < -maxMagnitude)
                        {
                            float totalMove = angularMove[i] + linearMove[i];
                            angularMove[i] = -maxMagnitude;
                            linearMove[i] = totalMove - angularMove[i];
                        }
                        else if (angularMove[i] > maxMagnitude)
                        {
                            float totalMove = angularMove[i] + linearMove[i];
                            angularMove[i] = maxMagnitude;
                            linearMove[i] = totalMove - angularMove[i];
                        }

                        // We have the linear amount of movement required by turning
                        // the rigid body (in angularMove[i]). We now need to
                        // calculate the desired rotation to achieve that.
                        if (angularMove[i] == 0)
                        {

                        }
                        else
                        {
                            // Work out the direction we'd like to rotate in.
                            Vector3 targetAngularDirection = Vector3.Cross(RelativeContactPosition[i], ContactNormal);

                            Matrix inverseInertiaTensor = Particle[i].GetInverseInertiaTensorWorld();

                            // Work out the direction we'd need to rotate to achieve that
                            angularChange[i] =
                                Matrix2.transform(inverseInertiaTensor, targetAngularDirection)*
                                // inverseInertiaTensor.transform(targetAngularDirection) *
                                (angularMove[i]/angularInertia[i]);
                        }

                        // Velocity change is easier - it is just the linear movement
                        // along the contact normal.
                        linearChange[i] = ContactNormal*linearMove[i];

                        // Now we can start to apply the values we've calculated.
                        // Apply the linear movement
                        Vector3 pos = Particle[i].PositionCenterEngine;
                        //     particle[i].getPosition(&pos);
                        pos += (ContactNormal*linearMove[i]);
                        Particle[i].PositionCenterEngine = pos;

                        // And the change in orientation
                        float orient = Particle[i].GetOrientation();
                        orient += angularChange[i].Z;
                        Particle[i].SetOrientation(orient);
                    }
                }
            }
        }

    }
}
