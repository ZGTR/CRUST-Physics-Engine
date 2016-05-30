using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.CatchableRopes;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.PCGEngine;
using CRUSTEngine.ProjectEngines.PCGEngine.Generators;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;

namespace CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs
{
    [Serializable]
    public class SpringService : IUpdatableComponent
    {
        public List<RigidBody> Masses { protected set; get; }
        public List<Spring> Springs { private set; get; }
        //public Vector3 RopeConnectionPos;
        //public Vector3 RopeConnectionVel;
        public float SpringConstant{ private set; get; }
        public float SpringLength{ private set; get; }
        public float SpringFrictionConstant{ private set; get; }
        public SpringType SpringType { set; get; }
        public int Id = 0;
        public int Length
        {
            get { return this.Masses.Count*15; }
        }

        public bool IsPinDrawable = true;

        public RigidBody GetMass(int index)
        {
            return Masses[index];						// get the mass at the index
        }

        public SpringService(float springConstant, float springLength, float springFrictionConstant, 
            List<RigidBody> masses, SpringType type // how stiff the springs are
                             // the length that a spring does not exert any force
                             // inner friction constant of spring
            )				//The super class creates masses with weights m of each
        {
            this.SpringType = type;
            this.Masses = masses;
            this.SpringConstant = springConstant;
            this.SpringLength = springLength;
            this.SpringFrictionConstant = springFrictionConstant;
            //this.RopeConnectionPos = Masses[0].PositionXNA;
            this.Id = StaticData.EngineManager.SpringsManagerEngine.GetNextRopeId();
            if (!(this is CatchableRopeService))
            {
                this.Masses[0].TextureType = TextureType.Pin;
                if (SpringType == SpringType.StrictRope)
                {
                    this.Masses[0].IsFixedRigid = true;
                }
                BuildSprings(springConstant, springLength, springFrictionConstant);
            }
        }

        protected void BuildSprings(float springConstant,							
                                float springLength,
                                float springFrictionConstant)
        {
            Springs = new List<Spring>();			
            for (var i = 0; i < Masses.Count - 1; i++)			
            {
                Springs.Add(new Spring(Masses[i], Masses[i + 1], springConstant, springLength, springFrictionConstant));
            }
            Springs[Springs.Count - 1].SpringConstant -= (30 * springConstant) / 100;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (StaticData.GameSessionMode == SessionMode.DesignMode)
            {
                if (!(this is CatchableRopeService))
                    UpdateDoubleClickFirstMass(gameTime);
            }
            for (int i = 0; i < Springs.Count; i++)
            {
                Springs[i].Update(gameTime);
            }		
            //if (SpringType == SpringType.StrictRope)
            //{
            //    UpdateFixedFirstPoint(gameTime);
            //}								
        }

        private void UpdateDoubleClickFirstMass(GameTime gameTime)
        {
            if (IsDoubleClicked(gameTime))
            {
                AddNewMass();
            }
        }

        private double timeStamp1 = 0;
        private double timeStamp2 = 0;
        private bool IsDoubleClicked(GameTime gameTime)
        {
            if (this.Masses[0].IsClicked)
            {
                if (timeStamp1 == 0)
                {
                    timeStamp1 = gameTime.TotalGameTime.TotalMilliseconds;
                }
                else
                {
                    timeStamp2 = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
            //else
            { 
                //timeStamp2 = gameTime.TotalGameTime.TotalMilliseconds;
                if (timeStamp1 != 0)
                {
                    if (timeStamp2 != 0)
                    {
                        if (Math.Abs(timeStamp1 - timeStamp2) > 100 && Math.Abs(timeStamp1 - timeStamp2) < 1000)
                        {
                            timeStamp1 = 0;
                            timeStamp2 = 0;
                            return true;
                        }
                        timeStamp1 = 0;
                        timeStamp2 = 0;
                    }
                }
            }
            return false;
        }

        public virtual void Draw(GameTime gameTime)
        {
            for (int i = 0; i < this.Springs.Count; i++)
            {
                this.Springs[i].Draw(gameTime);
            }
            if (!(this is CatchableRopeService))
            {
                // Draw pin
                if (IsPinDrawable)
                {
                    this.Masses[0].IsDrawable = true;
                }
                else
                {
                    this.Masses[0].IsDrawable = false;
                }
            }
        }

        public void SetMassesNewState(bool isCollide)
        {
            for (int i = 0; i < this.Masses.Count; i++)
            {
                if (Masses[i] != StaticData.EngineManager.CookieRB)
                {
                    if (Masses[i] is SphereRigid)
                    {
                        StaticData.EngineManager.RigidsManagerEngine.ListOfSphereRigids.Remove(Masses[i] as SphereRigid);
                    }
                    else
                    {
                        StaticData.EngineManager.RigidsManagerEngine.ListOfBoxRigids.Remove(Masses[i] as BoxRigid);
                    }
                }
            }
        }

        //public void UpdateFixedFirstPoint(GameTime gameTime)
        //{
        //    RopeConnectionPos += new Vector3((float)(RopeConnectionVel.X * StaticData.Dtime),
        //        (float)(RopeConnectionVel.Y * StaticData.Dtime),
        //        (float)(RopeConnectionVel.Z * StaticData.Dtime));
        //    try
        //    {
        //        Masses[0].PositionXNA = RopeConnectionPos;	//mass with index "0" shall position at ropeConnectionPos
        //        Masses[0].SetVelocity(RopeConnectionVel);	
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        //public void SetConnectionVel(Vector3 ropeConnectionVel)	//the method to set ropeConnectionVel
        //{
        //    this.RopeConnectionVel = ropeConnectionVel;
        //}

        public void ApplyServiceOnRigid(RigidBody rigidBody)
        {
            //rigidBody.PositionXNA = this.Masses[this.Masses.Count - 1].PositionXNA + new Vector3(0, 20, 0);
            this.Masses.Add(rigidBody);
            this.Springs.Add(new Spring(Masses[Masses.Count-2], Masses[Masses.Count-1],
                    SpringConstant, SpringLength, SpringFrictionConstant));
        }

        //private bool TryDeleteRope(RigidBody ropeHead)
        //{
        //    SpringService springCatched = CatchSpringServiceForRigidBody(ropeHead);
        //    if (springCatched != null)
        //    {
        //        StaticData.EngineManager.SpringsManagerEngine.RemoveService(springCatched);
        //        return true;
        //    }
        //    return false;
        //}

        public static SpringService CatchSpringServiceForRigidBody(RigidBody rigid)
        {
            foreach (var rope in StaticData.EngineManager.SpringsManagerEngine.ListOfServices)
            {
                if (rigid == rope.Masses[0])
                {
                    return rope;
                }
            }
            return null;
        }

        public void AddNewMass()
        {
            if (this.Length < StaticData.MaxRopeLength)
            {
                RigidBody newRigid = ObjectSerializer.DeepCopy(this.Masses[1]);
                newRigid.PositionXNA = this.Masses[0].PositionXNA + new Vector3(0, 5, 0);
                StaticData.EngineManager.RigidsManagerEngine.AddRigidBody(newRigid);
                this.Masses.Insert(1, newRigid);
                var oldMass2 = this.Springs[0].Mass2;
                this.Springs[0].Mass2 = newRigid;
                this.Springs.Insert(1, new Spring(newRigid, oldMass2, 
                    this.Springs[0].SpringConstant,
                                                  this.Springs[0].SpringLength,
                                                  this.Springs[0].FrictionConstant));
            }
        }
    }
}