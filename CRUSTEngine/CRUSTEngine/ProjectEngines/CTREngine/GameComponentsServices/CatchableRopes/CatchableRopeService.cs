using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs;

namespace CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.CatchableRopes
{
    [Serializable]
    public class CatchableRopeService : SpringService
    {
        //public int Radius;
        //private Visual2D pin;
        private Visual2D ring;
        //public Visual2D pin;
        public bool IsVisible;
        public bool IsActivated = false;
        private int _nrOfMasses;
        private bool _isCollide;
        private RigidType _rigidType;
        private float _normalLength;
        private Vector3 _rigidSize;
        private Vector3 _initialPos;

        public new int Length
        {
            get { return this._nrOfMasses * 15; }
        }

        public CatchableRopeService(int nrOfMasses,
            Vector3 initialPos,
            float springConstant, 
            float springLength, 
            float springFrictionConstant, 
            bool isCollide, 
            Vector3 rigidSize,
            SpringType springType,
            RigidType rigidType)
            : base(springConstant, springLength, springFrictionConstant, null, springType)
    {
            this._nrOfMasses = nrOfMasses;
            this._initialPos = initialPos;
            this._isCollide = isCollide;
            this._normalLength = springLength;
            this._rigidSize = rigidSize;
            this._rigidType = rigidType;

            this.IsVisible = false;
            this.Masses = DefaultAdder.GetMassesRope(2, _initialPos,
                                                     null, _rigidSize,
                                                     this._normalLength, this._rigidType, this._isCollide);
            this.Masses[0].TextureType = TextureType.Pin;
            this.Masses[0].IsDrawable = true;
            this.Masses[0].IsFixedRigid = true;
            BuildSprings(springConstant, springLength, springFrictionConstant);
            //this.Radius = radius;
            SetPinRingFirstTime();
        }

        private void SetPinRingFirstTime()
        {
            //pin = new Visual2D(this.Masses[0].PositionXNA, 15, 15, TextureType.Pin);
            Vector3 posPin = this.Masses[0].PositionXNA3D;
            Vector3 posRing3D = posPin - new Vector3(Length, Length, 0) + this._rigidSize;
            //Vector3 posRing3D = new Vector3(posRing2D.X, posRing2D.Y, 0);
            //pin = new Visual2D(posPin, (int) (_rigidSize.X * 2), (int) (_rigidSize.Y * 2), TextureType.Pin);
            ring = new Visual2D(posRing3D, 2 * Length, 2 * Length, TextureType.PinRing);
        }

        private void SetPinRing()
        {
            //this._initialPos = pin.PositionXNA3D;
            Vector3 posRing3D = Masses[0].PositionXNA3D - new Vector3(Length, Length, 0) + this._rigidSize;
            //Vector3 posRing3D = new Vector3(posRing2D.X, posRing2D.Y, 0);
            //pin = new Visual2D(pin.PositionXNA3D, (int)(_rigidSize.X * 2), (int)(_rigidSize.Y * 2), TextureType.Pin);
            ring = new Visual2D(posRing3D, 2 * Length, 2 * Length, TextureType.PinRing);
        }

        public override void Update(GameTime gameTime)
        {
            if (StaticData.GameSessionMode == SessionMode.PlayingMode)
            {
                if(!IsActivated)
                    CatchCookieIfNear();
            }
            SetPinRing();
            base.Update(gameTime);
         }

        private void CatchCookieIfNear()
        {
            if (RigidsHelperModule.IsCloseEnough(StaticData.EngineManager.CookieRB, ring, Length))
            {
                List<RigidBody> lastMasses = new List<RigidBody>(this.Masses);
                this.Springs.Clear();

                this.Masses= DefaultAdder.GetMassesRope(this._nrOfMasses, lastMasses[0].PositionXNA3D,
                    StaticData.EngineManager.CookieRB.PositionXNACenter, _rigidSize, this._normalLength, this._rigidType, this._isCollide);
                this.Masses[0].TextureType = TextureType.Pin;
                this.Masses[0].IsDrawable = true;
                this.Masses[0].IsFixedRigid = true;
                BuildSprings(this.SpringConstant, this._normalLength, this.SpringFrictionConstant);

                lastMasses.ForEach(m => StaticData.EngineManager.RigidsManagerEngine.DeleteRigid(m));                

                this.ApplyServiceOnRigid(StaticData.EngineManager.CookieRB);
                IsVisible = true;
                IsActivated = true;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (IsVisible)
                base.Draw(gameTime);
            DrawRing(gameTime);
        }

        private void DrawRing(GameTime gameTime)
        {
            //SetPinRing();
            ring.Draw(gameTime);
            //pin.Draw(gameTime);
            //Visual2D vis = new Visual2D(this.Masses[0].PositionXNACenter, 5, 5, TextureType.White);
            //vis.Draw(new GameTime());
        }

        //private void DrawPin(GameTime gameTime)
        //{
        //    pin.Draw(gameTime);
        //}
    }
}
