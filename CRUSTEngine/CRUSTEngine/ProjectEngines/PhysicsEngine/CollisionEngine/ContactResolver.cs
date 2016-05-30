using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CRUSTEngine.ProjectEngines.PhysicsEngine.CollisionEngine
{
    [Serializable]
    public class ContactResolver
    {
        public void ResolveContacts(List<Contact> contacts, int numContacts, float duration)
        {
            // Make sure we have something to do.
            if (numContacts == 0) return;

            // Prepare the contacts for processing
            PrepareContacts(contacts, numContacts, duration);

            // Resolve the interpenetration problems with the contacts.
            AdjustPositions(contacts, numContacts, duration);

            // Resolve the velocity problems with the contacts.
            AdjustVelocities(contacts, numContacts, duration);
        }

        public void PrepareContacts(List<Contact> contacts, float numContacts, float duration)
        {
            // Generate contact velocity and axis information.

            for (int i = 0; i < numContacts; i++)
            {
                // Calculate the internal contact data (inertia, basis, etc).
                contacts[i].CalculateInternals(duration);
            }
        }

        public void AdjustPositions(List<Contact> c, int numContacts, float duration)
        {
            Vector3[] linearChange = new Vector3[2];
            Vector3[] angularChange = new Vector3[2];

            // iteratively resolve interpenetrations in order of severity.
            int positionIterationsUsed = 0, positionIterations = numContacts * 2;
            while (positionIterationsUsed < positionIterations)
            {
                // Find biggest penetration
                float max = 0.1f;
                int index = numContacts;
                for (int i = 0; i < numContacts; i++)
                {
                    if (c[i].Penetration > max)
                    {
                        max = c[i].Penetration;
                        index = i;
                    }
                }
                if (index == numContacts) break;

                // Match the awake state at the contact
                c[index].MatchAwakeState();

                // Resolve the penetration.
                c[index].ApplyPositionChange(linearChange, angularChange, max);

                // Again this action may have changed the penetration of other
                // bodies, so we update contacts.
                for (int i = 0; i < numContacts; i++)
                {
                    // Check each body in the contact
                    for (int b = 0; b < 2; b++) if (c[i].Particle[b] != null)
                        {
                            // Check for a match with each body in the newly
                            // resolved contact
                            for (int d = 0; d < 2; d++)
                            {
                                if (c[i].Particle[b] == c[index].Particle[d])
                                {
                                    Vector3 deltaPosition = linearChange[d] +
                                                            Vector3.Cross(angularChange[d],
                                                                          c[i].RelativeContactPosition[b]);

                                    // The sign of the change is positive if we're
                                    // dealing with the second body in a contact
                                    // and negative otherwise (because we're
                                    // subtracting the resolution)..
                                    c[i].Penetration +=
                                        Vector3.Dot(deltaPosition, c[i].ContactNormal)
                                        *(b == 1 ? 1 : -1);
                                }
                            }
                        }
                }
                positionIterationsUsed++;
            }
        }


        public void AdjustVelocities(List<Contact> c, int numContacts, float duration)
        {
            Vector3[] velocityChange = new Vector3[2];
            Vector3[] rotationChange = new Vector3[2];

            // iteratively handle impacts in order of severity.
            int velocityIterationsUsed = 0, velocityIterations = numContacts * 2;
            while (velocityIterationsUsed < velocityIterations)
            {
                // Find contact with maximum magnitude of probable velocity change.
                float max = 0.1f;//velocityEpsilon;
                int index = numContacts;
                for (int i = 0; i < numContacts; i++)
                {
                    if (c[i].DesiredDeltaVelocity > max)
                    {
                        max = c[i].DesiredDeltaVelocity;
                        index = i;
                    }
                }
                if (index == numContacts) break;

                // Match the awake state at the contact
                c[index].MatchAwakeState();

                // Do the resolution on the contact that came out top.
                c[index].ApplyVelocityChange(velocityChange, rotationChange);

                // With the change in velocity of the two bodies, the update of
                // contact velocities means that some of the relative closing
                // velocities need recomputing.
                for (int i = 0; i < numContacts; i++)
                {
                    // Check each body in the contact
                    for (int b = 0; b < 2; b++) if (c[i].Particle[b] != null)
                        {
                            // Check for a match with each body in the newly
                            // resolved contact
                            for (int d = 0; d < 2; d++)
                            {
                                if (c[i].Particle[b] == c[index].Particle[d])
                                {
                                    Vector3 deltaVel = velocityChange[d] +
                                                       Vector3.Cross(rotationChange[d], c[i].RelativeContactPosition[b]);

                                    // The sign of the change is negative if we're dealing
                                    // with the second body in a contact.
                                    c[i].ContactVelocity +=
                                        Matrix2.transformTranspose(c[i].ContactToWorld, deltaVel)
                                        *(b == 1 ? -1 : 1);
                                    c[i].CalculateDesiredDeltaVelocity(duration);
                                }
                            }
                        }
                }
                velocityIterationsUsed++;
            }
        }
    }
}
