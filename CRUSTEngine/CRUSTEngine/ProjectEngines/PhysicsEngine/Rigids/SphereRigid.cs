using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.HelperModules;

namespace CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids
{
    [Serializable]
    public class SphereRigid : RigidBody
    {
        #region DataMembers
        private float InitTorque;
        private float InitRotation;
        public float Radius { set; get; }
        private bool wired = true;
        protected int divisions = 64;
        
        public override float Volume
        {
            get
            {
                return (4/3)* MathHelper.Pi * (this.Radius * this.Radius * 1);
            }
            set
            {
                base.Volume = value;
            }
        }

        public override Vector3 PositionCenterEngine
        {
            get
            {
                return _positionCenter;
            }
            set
            {
                _positionCenter = value;
                _positionXNA = MathHelperModule.GetPositionXNA(this._positionCenter, this.Radius);
                this.RectangleArea = new Rectangle((int)this.PositionXNA.X, (int)this.PositionXNA.Y,
                                                   (int)(Radius * 2), (int)(Radius * 2));
            }
        }

        public override Vector3 PositionXNA
        {
            get
            {
                return _positionXNA;
            }
            set
            {
                _positionXNA = value;
                _positionCenter = MathHelperModule.GetPositionCenter(this._positionXNA, this.Radius);
                this.RectangleArea = new Rectangle((int) this.PositionXNA.X, (int) this.PositionXNA.Y,
                                                   (int) (Radius*2), (int) (Radius*2));
            }
        }

        public override Vector2 PositionXNA2D
        {
            get
            {
                return new Vector2(_positionXNA.X, _positionXNA.Y);
            }
            set
            {
                _positionXNA = new Vector3(value.X, value.Y, 0);
                _positionCenter = MathHelperModule.GetPositionCenter(this._positionXNA, this.Radius);
                this.RectangleArea = new Rectangle((int)this.PositionXNA.X, (int)this.PositionXNA.Y,
                                                   (int)(Radius * 2), (int)(Radius * 2));
            }
        }

        public Rectangle PositionRec2D
        {
            get
            {
                return MathHelperModule.GetSphereRigid2DCoordinatesPositionCenter(this.PositionCenterEngine, (int) this.Radius);
            }
        }
        #endregion

        #region SetterAndGetters

        public void SetInitTorque(float t)
        {
            InitTorque = t;
        }

        public void SetInitRotation(float r)
        {
            angVelocity = InitRotation = r;
        }

        public bool Intersect(SphereRigid C)
        {
            bool b = true;

            if (Vector3.Distance(this.PositionCenterEngine, C.PositionCenterEngine) < (this.Radius + C.Radius))
            {
                b = false;
            }
            return b;
        }

        public void SetRadius(float radius)
        {
            this.Radius = radius;
        }

        #endregion

        public SphereRigid(Vector3 positionXNA, Material mat, float radius)
            : base(MathHelperModule.GetSphereRigid2DCoordinatesPositionXNA(positionXNA, radius),
            MathHelperModule.GetPositionCenter(positionXNA, radius), TextureType.DefaultCircle)
        {
            //Texture = StaticData.EngineManager.Content.Load<Texture2D>(@"RigidsTextures/DefaultCircle");
            ReInitializeData(positionXNA, mat, radius);
        }

        public SphereRigid(SphereRigid anotherRB)
            : base(MathHelperModule.GetSphereRigid2DCoordinatesPositionXNA(anotherRB.PositionXNA, anotherRB.Radius),
                MathHelperModule.GetPositionCenter(anotherRB.PositionXNA, anotherRB.Radius), TextureType.DefaultCircle)
        {
            this._positionCenter = anotherRB.PositionXNA;
            this._positionCenter = anotherRB.PositionCenterEngine;

            this.Width = anotherRB.Width;
            this.Height = anotherRB.Height;

            this.vertices = RigidsHelperModule.MakeNewVPCMatrix(anotherRB.vertices);
            this.InverseInertiaTensorWorld = anotherRB.InverseInertiaTensorWorld;


            this.baseMomentOfInertia = anotherRB.baseMomentOfInertia;
            //this.basicEffect = anotherRB.basicEffect;
            this.acceleration = anotherRB.acceleration;
            this.canSleep = anotherRB.canSleep;
            this.Center = anotherRB.Center;
            this.dAngle = anotherRB.dAngle;
            this.torqueAccum = anotherRB.torqueAccum;
            this.torque = anotherRB.torque;
            this.EnlargingFactor = anotherRB.EnlargingFactor;
            this.gravity = anotherRB.gravity;
            this.Radius = anotherRB.Radius;
            this.isAwake = anotherRB.isAwake;
            this.IsCollidable = anotherRB.IsCollidable;
            this.IsClicked = anotherRB.IsClicked;
            this.LastFrameAcceleration = anotherRB.LastFrameAcceleration;
            this.LastFrameVelocity = anotherRB.LastFrameVelocity;
            this.Mass = anotherRB.Mass;
            this.InvMass = anotherRB.InvMass;
            this.material = anotherRB.material;
            this.motion = anotherRB.motion;
            this.orientation = anotherRB.orientation;
            this.RectangleArea = anotherRB.RectangleArea;
            this.TextureType = anotherRB.TextureType;
            this.sleepEpsilon = anotherRB.sleepEpsilon;
            this.angVelocity = anotherRB.angVelocity;
            this.angAcceleration = anotherRB.angAcceleration;
            this.divisions = anotherRB.divisions;
            this.wired = anotherRB.wired;
            this.InitRotation = anotherRB.InitRotation;
            this.InitTorque = anotherRB.InitTorque;

        }

        public void ReInitializeData(Vector3 positionXNA, Material mat, float radius)
        {
            this.Radius = radius;
            this.PositionCenterEngine = MathHelperModule.GetPositionCenter(positionXNA, radius);
            this.RectangleArea = MathHelperModule.GetSphereRigid2DCoordinatesPositionCenter(this.PositionCenterEngine, (int)radius);

            InitTorque = 0;
            InitRotation = 0;
            this.Radius = radius;
            //this.color = StaticData.colors[(int)mat];

            SetMaterial(mat);
            SetMass(radius);

            baseMomentOfInertia = 0.5f * Mass * radius * radius;
            dAngle = 0;

            velocity = new Vector3(0, 0, 0);

            InverseInertiaTensorWorld = new Matrix();
            InverseInertiaTensorWorld.M11 = InverseInertiaTensorWorld.M22 = InverseInertiaTensorWorld.M33 = (float)1 / baseMomentOfInertia;
            InverseInertiaTensorWorld.M44 = 1;

            vertices = new VertexPositionColor[divisions + 1];
            float angleIncrement = (float)(Math.PI * 2 / (divisions - 50));
            for (int i = 0; i < vertices.Length; i++)
            {
                float angle = angleIncrement * i;

                Vector3 position1 = this.PositionCenterEngine + new Vector3((float)Math.Sin(angle + 3) * Radius, (float)Math.Cos(angle + 3) * Radius, 0);
                vertices[i].Position = position1;
            }
        }

        private void SetMass(float radius)
        {
            this.Mass = 3.14f * 4 / 3 * radius * radius * radius * StaticData.DensityTable[(int)this.material];
            this.Mass /= StaticData.MassDivConst;
            this.InvMass = 1 / Mass;
        }

        public int FindJointVertex(Vector3 pos)
        {
            int result = -1;
            float min = Vector3.Distance(pos, PositionCenterEngine);

            for (int i = 0; i < vertices.Length; i++)
            {
                if (Vector3.Distance(pos, vertices[i].Position) < min)
                {
                    min = Vector3.Distance(pos, vertices[i].Position);
                    result = i;
                }
            }
            return result;
        }

        protected override void UpdatePhysics(GameTime gameTime)
        {
            if (!isAwake) return;

            LastFrameAcceleration = acceleration;
            LastFrameAcceleration += forceAccum * InvMass;

            angAcceleration = torqueAccum / baseMomentOfInertia;

            LastFrameVelocity = velocity;
            velocity += LastFrameAcceleration * StaticData.Dtime;
            angVelocity += angAcceleration * StaticData.Dtime;

            velocity *= (float)Math.Pow(.95, StaticData.Dtime);
            angVelocity *= (float)Math.Pow(0.80, StaticData.Dtime);

            PositionCenterEngine += velocity * StaticData.Dtime;
            orientation += angVelocity * StaticData.Dtime;


            velocity *= (float)Math.Pow(.95, StaticData.Dtime);
            angVelocity *= (float)Math.Pow(0.80, StaticData.Dtime);
            dAngle = orientation;

            float angleIncrement = (float)(Math.PI * 2 / (divisions - 50));
            for (int i = 0; i < vertices.Length; i++)
            {
                float angle = angleIncrement * i;
                vertices[i].Position = PositionCenterEngine + Matrix2.M_V(new Vector3((float)Math.Sin(angle + 3) * Radius,
                    (float)Math.Cos(angle + 3) * Radius, 0), dAngle);
            }

            if (canSleep)
            {
                float currentMotion = Vector3.Dot(velocity, velocity) + angVelocity * angVelocity;

                double bias = Math.Pow((double)0.5, (double)StaticData.Dtime);
                motion = bias * motion + (1 - bias) * currentMotion;

                //if (motion < sleepEpsilon) SetAwake(false);
                //else
                if (motion > 10 * sleepEpsilon) motion = 10 * sleepEpsilon;
            }

            canSleep = true;
            forceAccum = new Vector3(0, 0, 0);
            if ((Math.Abs((double)angVelocity - InitRotation) > 1))
                torqueAccum = InitTorque;
            else
                torqueAccum = 0;
        }

        protected override void UpdateGraphics(GameTime gametime)
        {
            this.RectangleArea = MathHelperModule.GetSphereRigid2DCoordinatesPositionCenter(PositionCenterEngine, Radius);
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (IsDrawable)
            {
                base.AngleOfRotation = -1*this.GetOrientation();
                base.Draw(gameTime);
            }
        }
    }
}