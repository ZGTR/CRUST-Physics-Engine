using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.CatchableRopes;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs;

namespace CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Bubble
{
    [Serializable]
    public class BubbleService : Visual2D, IUpdatableComponent
    {
        public RigidBody RigidInService { get; private set; }
        private float _maxDim;//= StaticData.BubbleMaxDim;
        private float _minDim;//= StaticData.BubbleMinDim;
        private bool _isMaximizing = true;
        public int Id;
        public bool ShouldDie = false;
        public bool IsCookieCatched = false;
        private float _heightDif = StaticData.BubbleDimBubble;
        private float _widthDif = StaticData.BubbleDimBubble;
        private float _amountDif = 0.3f;
        //public bool IsPinch = false;
        public Vector2 PositionXNAInitial;
        private Vector3 lastStaticServicePos;
        private bool firstTime = true;

        public BubbleService(Vector3 positionXNA)
            : base(MathHelperModule.Get2DRectangleForNonRigids(positionXNA, 
            StaticData.BubbleDimBubble, StaticData.BubbleDimBubble),
            TextureType.BubbleWithoutCandy)
        {
            this._maxDim = StaticData.BubbleDimBubble;
            this._minDim = _maxDim - (10* this._maxDim / 100);
            this.Id = StaticData.EngineManager.BubbleManagerEngine.GetNextServiceId();
            this.PositionXNAInitial = this.PositionXNA;
        }

        public override void Update(GameTime gameTime)
        {
            if (StaticData.GameSessionMode == SessionMode.PlayingMode)
            {
                if (!IsCookieCatched)
                {
                    if (RigidsHelperModule.IsCloseEnough(StaticData.EngineManager.CookieRB,
                                                         this, StaticData.BubbleCloseArea))
                    {
                        RigidInService = StaticData.EngineManager.CookieRB;
                        //SetAccIntoRigid(new Vector3(0, 5, 0));
                        //DisableRopesAcc();
                        IsCookieCatched = true;
                    }
                }
                else
                {
                    MoveServiceUpward();
                    RepositionMeAccordingToService();

                    if (
                        //(IsPinch && StaticData.GameSessionMode != SessionMode.DesignMode) ||
                        (IsClicked && StaticData.GameSessionMode != SessionMode.DesignMode))
                    {
                        //if (RigidInService != null)
                        //{
                        //    SetAccIntoRigid(new Vector3(0, -9.8f, 0));
                        //    EnableRopesAcc();
                        //}
                        RigidInService.SetAcceleration(new Vector3(0, -StaticData.GravityConstant, 0));
                        ShouldDie = true;
                    }
                }
            }
            base.Update(gameTime);
        }

        
        private void MoveServiceUpward()
        {
            //RigidInService.PositionXNA = new Vector3(RigidInService.PositionXNA.X, RigidInService.PositionXNA.Y - 1f,0);
            //Vector3 me = new Vector3(this.PositionXNACenter.X, this.PositionXNACenter.Y , 0);
            if (firstTime)
            {
                lastStaticServicePos = RigidInService.PositionXNA;
                firstTime = false;
            }   

            var cookie = RigidInService;// StaticData.EngineManager.CookieRB;
            //cookie.SetAcceleration(Vector3.Zero);
            //cookie.PositionXNA = lastStaticServicePos;
            //lastStaticServicePos.Y-= 0.7f;

            var ropes = StaticData.EngineManager.SpringsManagerEngine.ListOfServices;
            int totalMasses = 0;
            if(ropes != null)
                ropes.ForEach(r => totalMasses += r.Masses.Count);

            float YAcc = 5 + 0.7f * totalMasses;

            if (ropes.Count == 0)
            {
                YAcc = 6;
            }

            Vector3 accForce = new Vector3(0, YAcc, 0);
            cookie.SetAcceleration(accForce);
        }

        private void RepositionMeAccordingToService()
        {
            int widthHalfDiff = (int) Math.Abs(base.Width - RigidInService.Width)/2;
            int heightHalfDiff = (int) Math.Abs(base.Height - RigidInService.Height)/2;
            int newX = (int) RigidInService.PositionXNA.X - (widthHalfDiff);// -RigidInService.RectangleArea.Width / 2;
            int newY = (int) RigidInService.PositionXNA.Y - (heightHalfDiff);// +RigidInService.RectangleArea.Height / 2;

            this.ChangePosition(new Vector2(newX, newY));

            //base.ChangePosition(new Vector2(this.RigidInService.PositionXNA.X - widthHalfDiff,
            //    this.RigidInService.PositionXNA.Y - heightHalfDiff));
        }

        private void DisableRopesAcc()
        {
            var ropes = StaticData.EngineManager.SpringsManagerEngine.ListOfServices;
            foreach (var rope in ropes)
            {
                if (rope is CatchableRopeService)
                {
                    if (((CatchableRopeService)rope).IsActivated)
                    {
                        rope.Masses.ForEach(m => m.SetAcceleration(Vector3.Zero));
                    }
                }
                else
                {
                    if(rope is SpringService)
                    {
                        rope.Masses.ForEach(m => m.SetAcceleration(Vector3.Zero));
                    }
                }
            }
        }

        private void EnableRopesAcc()
        {
            var ropes = StaticData.EngineManager.SpringsManagerEngine.ListOfServices;
            foreach (var rope in ropes)
            {
                if (rope is CatchableRopeService)
                {
                    if (((CatchableRopeService)rope).IsActivated)
                    {
                        rope.Masses.ForEach(m => m.SetAcceleration(new Vector3(0, -4, 0)));
                    }
                }
                else
                {
                    //if (rope is SpringService)
                    {
                        rope.Masses.ForEach(m => m.SetAcceleration(new Vector3(0, -4, 0)));
                    }
                }
            }
        }

        private void SetAccIntoRigid( Vector3 newAcc)
        {
            RigidInService.SetAcceleration(newAcc);
            //RigidInService.AddForce(-RigidInService.GetAcceleration() * RigidInService.Mass +
            //                        StaticData.BubbleForceUp * RigidInService.Mass);
        }

        //private void AnimateBubble(GameTime gameTime)
        //{
        //    // Width is the same as Height in this component
        //    if (gameTime.TotalGameTime.Milliseconds % 10 == 0)
        //    {
        //        if (_isMaximizing)
        //        {
        //            if (this.Width < this._maxDim)
        //            {
        //                this.Width++;
        //                this.Height++;
        //            }
        //            else
        //            {
        //                _isMaximizing = false;
        //            }
        //        }
        //        else
        //        {
        //            if (this.Width > this._minDim)
        //            {
        //                this.Width--;
        //                this.Height--;
        //            }
        //            else
        //            {
        //                _isMaximizing = true;
        //            }
        //        }
        //    }
        //}

        private void Animate(GameTime gameTime)
        {
            if (_isMaximizing)
            {
                if (_heightDif < this._maxDim)
                {
                    this._heightDif += _amountDif;
                    this._widthDif += _amountDif;
                }
                else
                {
                    _isMaximizing = false;
                }
            }
            else
            {
                if (_heightDif > this._minDim)
                {
                    this._heightDif -= _amountDif;
                    this._widthDif -= _amountDif;
                }
                else
                {
                    _isMaximizing = true;
                }
            }
            //this.Height = (int)_heightDif;
        }

        public void Draw(GameTime gameTime)
        {
            //Animate(gameTime);
            this.RectangleArea = new Rectangle((int)(this.PositionXNA.X),
                                               (int)(this.PositionXNA.Y),
                                               this.RectangleArea.Width,
                                               this.RectangleArea.Height);
            base.Draw(gameTime);
            //Rectangle RectangleArea = new Rectangle((int)(this.PositionXNA.X),
            //                                   (int)(this.PositionXNA.Y + this._maxDim - this._heightDif),
            //                                   this.RectangleArea.Width,
            //                                   this.RectangleArea.Height);
            ////base.Draw(gameTime);
            //Visual2D vis = new Visual2D(RectangleArea, TextureType.BubbleWithoutCandy);
            //vis.Draw(gameTime);
        }
    }
}
