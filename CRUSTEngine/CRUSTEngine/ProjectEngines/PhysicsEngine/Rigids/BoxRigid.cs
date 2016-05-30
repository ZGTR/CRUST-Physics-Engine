using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.HelperModules;

namespace CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids
{
    [Serializable]
    public class BoxRigid : RigidBody
    {
        #region DataMembers
        public Vector3 XAxis;
        public Vector3 YAxis;
        public Vector3 HalfSize;

        public override float Volume
        {
            get
            {
                return this.Width * this.Height * 1;
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
                _positionXNA = MathHelperModule.GetPositionXNA(this._positionCenter, this.GetHalfWidth(),
                                                           this.GetHalfHeight());
                this.RectangleArea = new Rectangle((int)this.PositionXNA.X, (int)this.PositionXNA.Y,
                    this.RectangleArea.Width, this.RectangleArea.Height);
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
                _positionCenter = MathHelperModule.GetPositionCenter(this._positionXNA, this.GetHalfWidth(),
                                                                 this.GetHalfHeight());
                this.RectangleArea = new Rectangle((int)this.PositionXNA.X, (int)this.PositionXNA.Y,
                    this.RectangleArea.Width, this.RectangleArea.Height);
            }
        }

        public override Vector2 PositionXNA2D
        {
            get
            {
                return MathHelperModule.Get2DVector(PositionXNA);
            }
            set
            {
                _positionXNA = new Vector3(value.X, value.Y, 0);
                _positionCenter = MathHelperModule.GetPositionCenter(_positionXNA, this.HalfSize.X, this.HalfSize.Y);
                this.RectangleArea = new Rectangle((int)this.PositionXNA.X, (int)this.PositionXNA.Y,
                                                   (int)(this.HalfSize.X * 2), (int)(this.HalfSize.Y * 2));
            }
        }

        #endregion

        #region SettersAndGetters

        public Rectangle PositionRec2D
        {
            get { return MathHelperModule.GetBoxRigid2DCoordinatesPositionCenter(this.PositionCenterEngine, this.HalfSize.X * 2, this.HalfSize.Y * 2); }
        }

        

        public float GetHalfWidth()
        {
            return HalfSize.X;
        }

        public float GetHalfHeight()
        {
            return HalfSize.Y;
        }

        public void SetHalfWidth(float w)
        {
            HalfSize.X = w;
        }

        public void SetHalfHeight(float h)
        {
            HalfSize.Y = h;
        }

        public virtual void SetMass(Vector3 halfSize)
        {
            // *3 : to match the spheres masses
            Mass = halfSize.X * 2 * halfSize.Y * 2 * 4 * StaticData.DensityTable[(int)this.material] * 3;
            Mass /= StaticData.MassDivConst;
            InvMass = 1 / Mass;
        }

        public float GetMass()
        {
            return Mass;
        }
        
        #endregion

        public BoxRigid(Vector3 positionXNA, Material mat, Vector3 halfSize)
            : base(
            MathHelperModule.GetBoxRigid2DCoordinatesPositionXNA(positionXNA, halfSize.X, halfSize.Y),
            MathHelperModule.GetPositionCenter(positionXNA, halfSize.X, halfSize.Y), TextureType.DefaultBox)
        {
            ReInitializeData(positionXNA, mat, halfSize);
        }

        public BoxRigid(BoxRigid anotherRB)
            : base(
            MathHelperModule.GetBoxRigid2DCoordinatesPositionXNA(anotherRB.PositionXNA, anotherRB.HalfSize.X, anotherRB.HalfSize.Y),
            MathHelperModule.GetPositionCenter(anotherRB.PositionXNA, anotherRB.HalfSize.X, anotherRB.HalfSize.Y), TextureType.DefaultBox)
        {

            //this.HalfSize = anotherRB.HalfSize;
            //this.PositionCenterEngine = MathHelperModule.GetPositionCenter(anotherRB.PositionXNA, anotherRB.HalfSize.X,
            //                                                         anotherRB.HalfSize.Y);
            //this.RectangleArea = MathHelperModule.GetBoxRigid2DCoordinatesPositionCenter(anotherRB.PositionCenterEngine,
            //                                                                             anotherRB.HalfSize.X,
            //                                                                             anotherRB.HalfSize.Y);
            //SetMaterial(anotherRB.material);
            //SetMass(HalfSize);

            //baseMomentOfInertia = Mass * Math.Max(HalfSize.X, HalfSize.Y) * Math.Max(HalfSize.X, HalfSize.Y) * 4 / 12f;
            //dAngle = 0;
            //vertices = new VertexPositionColor[5];
            //velocity = new Vector3(0, 0, 0);

            //InverseInertiaTensorWorld = new Matrix();
            //InverseInertiaTensorWorld.M11 = InverseInertiaTensorWorld.M22 = InverseInertiaTensorWorld.M33 = (float)1 / baseMomentOfInertia;
            //InverseInertiaTensorWorld.M44 = 1;

            //vertices[0].Position = this.PositionCenterEngine + new Vector3(HalfSize.X, HalfSize.Y, 0);
            //vertices[1].Position = this.PositionCenterEngine + new Vector3(-HalfSize.X, HalfSize.Y, 0);
            //vertices[2].Position = this.PositionCenterEngine + new Vector3(-HalfSize.X, -HalfSize.Y, 0);
            //vertices[3].Position = this.PositionCenterEngine + new Vector3(HalfSize.X, -HalfSize.Y, 0);
            //vertices[4].Position = this.PositionCenterEngine + new Vector3(HalfSize.X, HalfSize.Y, 0);

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
            this.HalfSize = anotherRB.HalfSize;
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
            this.angAcceleration = anotherRB.angVelocity;
            this.XAxis = anotherRB.XAxis;
            this.YAxis = anotherRB.YAxis;
        }

        public void ReInitializeData(Vector3 positionXNA, Material mat, Vector3 halfSize)
        {
            this.HalfSize = halfSize;
            this.PositionCenterEngine = MathHelperModule.GetPositionCenter(positionXNA, halfSize.X, halfSize.Y);
            this.RectangleArea = MathHelperModule.GetBoxRigid2DCoordinatesPositionCenter(this.PositionCenterEngine, halfSize.X,
                                                                                     halfSize.Y);

            XAxis = new Vector3(1, 0, 0);
            YAxis = new Vector3(0, 1, 0);
            XAxis.Normalize();
            YAxis.Normalize();

            SetMaterial(mat);
            SetMass(HalfSize);

            this.InvMass = 1 / Mass;

            baseMomentOfInertia = Mass * Math.Max(HalfSize.X, HalfSize.Y) * Math.Max(HalfSize.X, HalfSize.Y) * 4 / 12f;
            dAngle = 0;
            vertices = new VertexPositionColor[5];
            velocity = new Vector3(0, 0, 0);

            InverseInertiaTensorWorld = new Matrix();
            InverseInertiaTensorWorld.M11 =
                InverseInertiaTensorWorld.M22 = InverseInertiaTensorWorld.M33 = (float) 1/baseMomentOfInertia;
            InverseInertiaTensorWorld.M44 = 1;

            vertices[0].Position = this.PositionCenterEngine + new Vector3(HalfSize.X, HalfSize.Y, 0);
            vertices[1].Position = this.PositionCenterEngine + new Vector3(-HalfSize.X, HalfSize.Y, 0);
            vertices[2].Position = this.PositionCenterEngine + new Vector3(-HalfSize.X, -HalfSize.Y, 0);
            vertices[3].Position = this.PositionCenterEngine + new Vector3(HalfSize.X, -HalfSize.Y, 0);
            vertices[4].Position = this.PositionCenterEngine + new Vector3(HalfSize.X, HalfSize.Y, 0);
        }

        public int FindJointIndex(Vector3 pos)
        {
            int result = 0;
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

            vertices[0].Position = PositionCenterEngine + Matrix2.M_V(new Vector3(HalfSize.X, HalfSize.Y, 0), dAngle);
            vertices[1].Position = PositionCenterEngine + Matrix2.M_V(new Vector3(-HalfSize.X, HalfSize.Y, 0), dAngle);
            vertices[2].Position = PositionCenterEngine + Matrix2.M_V(new Vector3(-HalfSize.X, -HalfSize.Y, 0), dAngle);
            vertices[3].Position = PositionCenterEngine + Matrix2.M_V(new Vector3(HalfSize.X, -HalfSize.Y, 0), dAngle);
            vertices[4].Position = PositionCenterEngine + Matrix2.M_V(new Vector3(HalfSize.X, HalfSize.Y, 0), dAngle);

            XAxis = Matrix2.M_V(new Vector3(1, 0, 0), dAngle);
            YAxis = Matrix2.M_V(new Vector3(0, 1, 0), dAngle);
            XAxis.Normalize();
            YAxis.Normalize();

            if (canSleep)
            {
                float currentMotion = Vector3.Dot(velocity, velocity) + angVelocity * angVelocity;
              

                double bias = Math.Pow((double)0.5, (double)StaticData.Dtime);
                motion = bias * motion + (1 - bias) * currentMotion;

                //if (motion < sleepEpsilon) SetAwake(false);
                //else 
                if (motion > 10 * sleepEpsilon) motion = 10 * sleepEpsilon;
            }

           
            forceAccum = new Vector3(0, 0, 0);
            torqueAccum = 0f;
        }

        protected override void UpdateGraphics(GameTime gametime)
        {
            //Vector3 centerPosition = this.PositionCenterEngine;
            //this.RectangleArea = MathHelperModule.GetBoxRigid2DCoordinatesPositionCenter(centerPosition,
            //                                                                             this.GetHalfWidth(),
            //                                                                             this.GetHalfHeight());
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
