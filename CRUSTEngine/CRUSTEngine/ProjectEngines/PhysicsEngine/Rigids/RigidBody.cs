using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CRUSTEngine.ProjectEngines.GraphicsEngine;

namespace CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids
{
    [Serializable]
    public abstract class RigidBody : Visual2DRigid
    {
        #region --- DataMembers ---

        public VertexPositionColor[] vertices;
        //[NonSerialized]
        //protected GraphicsDevice device;
        //[NonSerialized]
        //protected BasicEffect basicEffect;
        //protected VertexDeclaration vertDeclaration;
        protected Vector3 _positionCenter;
        protected Vector3 _positionXNA;
        public bool IsCollidable = true;
        public bool IsDrawable = true;
        public bool IsFixedRigid = false;

        public virtual Vector3 PositionCenterEngine
        {
            get
            {
                return _positionCenter;
            }
            set
            {
                _positionCenter = value;
            }
        }

        public virtual Vector3 PositionXNACenter
        {
            get
            {
                return _positionXNA + this.getHalfSize();
            }
            //set
            //{
            //    _positionXNA = value - this.getHalfSize();
            //}
        }

        public virtual Vector2 PositionXNACenter2D
        {
            get
            {
                var vec3 = _positionXNA + this.getHalfSize();
                return new Vector2(vec3.X, vec3.Y);
            }
            //set
            //{
            //    _positionXNA = value - this.getHalfSize();
            //}
        }

        public virtual Vector3 PositionXNA
        {
            get
            {
                return _positionXNA;
            }
            set
            {
                _positionXNA = value;
            }
        }

        private float _volume;
        public virtual float Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                _volume = value;
            }
        }

        protected float InvMass;
        protected float gravity;
        protected Vector3 velocity;
        protected Vector3 acceleration;
        public float dAngle;
        protected float angVelocity;
        protected float angAcceleration;
        public float orientation;
        protected float baseMomentOfInertia;

        protected float torque;
        protected Vector3 LastFrameAcceleration;
        protected Matrix InverseInertiaTensorWorld;

        protected bool isAwake;
        protected bool canSleep;
        protected double motion;
        protected double sleepEpsilon = 0.3f;

        protected float torqueAccum;
        protected Vector3 forceAccum = new Vector3();
        protected Material material;
        protected Vector3 LastFrameVelocity;
        public float Mass { set;  get; }
        public abstract Vector2 PositionXNA2D { get; set; }

        //public Vector3 SpringForce { set; get; }
        public EngineManager Game2D;
        

        #endregion

        #region SetterAndGetters

        public Vector3 GetLastFrameVelocity()
        {
            return LastFrameVelocity;
        }

        public void SetObInertia()
        {
            SetInverseMass(float.Epsilon);
            InverseInertiaTensorWorld.M11 = InverseInertiaTensorWorld.M22 = InverseInertiaTensorWorld.M33 = float.Epsilon;
        }

        public void SetLastFrameAccelaration(Vector3 a)
        {
            LastFrameAcceleration = a;
        }

        public bool GetisAwake()
        {
            return isAwake;
        }

        public bool GetcanSleep()
        {
            return canSleep;
        }
       
        public Vector3 GetVelocity()
        {
            return velocity;
        }

        public void SetVelocity(Vector3 vel)
        {
            this.velocity = vel;
        }

        public float GetInverseMass()
        {
            return InvMass;
        }

        public void SetInverseMass(float m)
        {
            InvMass = m;
        }

        public Vector3 GetAcceleration()
        {
            return acceleration;
        }

        public void SetAcceleration(Vector3 acc)
        {
            acceleration = acc;
        }

        public float GetRotation()
        {
            return angVelocity;
        }

        public void SetRotation(float angR)
        {
            angVelocity = angR;
        }

        public Vector3 GetLastFrameAcceleration()
        {
            return LastFrameAcceleration;
        }

        public Matrix GetInverseInertiaTensorWorld()
        {
            return InverseInertiaTensorWorld;
        }

        public float GetOrientation()
        {
            return orientation;
        }

        public void SetOrientation(float o)
        {
            orientation = o;
        }

        public Material GetMaterial()
        {
            return material;
        }

        public void NormalizedForces()
        {
            forceAccum = new Vector3();
        }

        //public virtual void SetMass(float mass)
        //{
        //}

        //public virtual float GetMass()
        //{
        //}

        public void SetMaterial(Material m)
        {
            material = m;
        }

        public void SetCanSleep(bool canSleepe)
        {
            canSleep = canSleepe;

            if (!canSleep && !isAwake) SetAwake(true);
        }

        public void SetAwake(bool awake)
        {
            if (awake)
            {
                isAwake = true;

                // Add a bit of motion to avoid it falling asleep immediately.
                motion = sleepEpsilon * 2.0f;
            }
            else
            {
                isAwake = false;
                velocity = Vector3.Zero;
                angVelocity = 0;
            }
        }

        #endregion

        protected RigidBody(Rectangle rectangleArea, Vector3 position, TextureType texture)
            : base(rectangleArea, texture)
        {
            forceAccum = new Vector3();
            torqueAccum = 0;

            isAwake = true;
            canSleep = false;
            dAngle = 0;

            velocity = new Vector3(0, 0, 0);
            gravity = StaticData.GravityConstant;
            this.PositionCenterEngine = position;
        }


        public void AddForce(Vector3 force)
        {
            if (!IsFixedRigid)
            {
                forceAccum += force;
                isAwake = true;
                canSleep = false;
            }
        }

        public void AddTorque(Vector3 force, Vector3 begin)
        {
            if (!IsFixedRigid)
            {
                Vector3 temp = Vector3.Cross(force, begin);
                torqueAccum += temp.Z;
                isAwake = true;
                canSleep = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!IsFixedRigid)
            {
                UpdatePhysics(gameTime);
            }
            UpdateGraphics(gameTime);
        }

        protected virtual void UpdatePhysics(GameTime gameTime)
        {

        }

        protected virtual void UpdateGraphics(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            if (IsDrawable)
            {
                base.Draw(gameTime);
            }
        }

        public void SetMass(int mass)
        {
            this.Mass = mass;
            this.InvMass = 1/this.Mass;
        }

        public Vector3 getHalfSize()
        {
            if (this is SphereRigid)
            {
                float rad = ((SphereRigid) this).Radius;
                return new Vector3(rad, rad, 0);
            }
            else
            {
                return ((BoxRigid) this).HalfSize;
            }
        }
    }
}
