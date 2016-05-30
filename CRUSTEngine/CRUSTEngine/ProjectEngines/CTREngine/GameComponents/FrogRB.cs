using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PCGEngine;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.Ryse;
using CRUSTEngine.ProjectEngines.PhysicsEngine;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;

namespace CRUSTEngine.ProjectEngines.CTREngine.GameComponents
{
    [Serializable]
    public class FrogRB : BoxRigid
    {
        private int _maxDim;
        private int _minDim;
        private static bool _isMaximizing = true;
        //private Vector3 OrigianlPositionLeftBottomXNA;
        private float _heightDif = 55;
        private float _amountDif = 0.3f;
        public bool IsWon = false;

        public FrogRB(Vector3 positionXNA, Material mat, Vector3 halfSize) : base(positionXNA, mat, halfSize)
        {
            _maxDim = (int)this.HalfSize.Y + 30;
            _minDim = (int)this.HalfSize.Y + 25;
            //this.OrigianlPositionLeftBottomXNA = positionXNA + new Vector3(0, halfSize.Y * 2, 0);
            this.acceleration = new Vector3(0, 0, 0);
            this.TextureType = TextureType.FrogMouthOpen;
            this.IsCollidable = false;
            this.IsFixedRigid = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsWon)
            {
                Animate(gameTime);
            }
            else
            {
                AnimateWhenWon();
            }

            CheckRemoveCookie();
            
            base.Update(gameTime);
        }

        private float _dis = 1;
        private void AnimateWhenWon()
        {
            if (IsWon)
            {
                //_dis = _dis < -20 ? 0 : _dis;
                if (_dis > 20) _dis = 20;
                //_dis = _dis > 20 ? 20;
                _dis += 0.3f;
                //float _disLoc = _dis;// *0.8f;
                //this.PositionXNA = new Vector3(this.PositionXNA.X - _disLoc, this.PositionXNA.Y - _disLoc, 0);
                this.Width = (int)(_dis + this.Width);
                this.Height = (int)(_dis + this.Height);
            }
        }

        private void CheckRemoveCookie()
        {
            if (StaticData.GameSessionMode == SessionMode.PlayingMode)
            {
                if (RigidsHelperModule.GetDistance(this, StaticData.EngineManager.CookieRB)
                    <= PlayabilitySimulatorEngineProlog.NarrativeDist)
                {
                    //this.TextureType = TextureType.FrogWithCookie;
                    StaticData.EngineManager.RigidsManagerEngine.ListOfSphereRigids.Remove(
                        StaticData.EngineManager.CookieRB);
                    IsWon = true;
                }
            }
        }

        private void Animate(GameTime gameTime)
        {
            if (_isMaximizing)
            {
                if (_heightDif < this._maxDim)
                {
                    this._heightDif += _amountDif;
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
                }
                else
                {
                    _isMaximizing = true;
                }
            }
            this.Height = (int)_heightDif;
        }

        public override void Draw(GameTime gameTime)
        {
            this.RectangleArea = new Rectangle((int)(this.RectangleArea.X),
                                               (int)(this.RectangleArea.Y),//- this.Height),
                                               this.RectangleArea.Width,
                                               this.RectangleArea.Height);
            base.Draw(gameTime);
        }
    }
}
