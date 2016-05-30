using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;

namespace CRUSTEngine.ProjectEngines.GraphicsEngine.Managers.MouseManagers
{
    [Serializable]
    public class MouseResizeManager : IUpdatableComponent
    {
        private List<Visual2DRotatable> _listOfTags;
        private bool _canSelectNewBox;
        private bool _firstTimePressed;

        private Visual2DRotatable _smartTagContainer;
        private Visual2DRotatable _currentSelectedDrawable;
        private const int minPermittedHeight = 10;
        private const int minPermittedWidth = 10;
        private const int _boxSmartTagDimSize = 20;
        private const int _boxSmartTagDimSizeHalf = _boxSmartTagDimSize / 2;
        private const int _centerSmartTag = 10;
        private const int _centerSmartTagHalf = _centerSmartTag / 2;
        private RigidBody _CurrentVisual2D;
        public RigidBody CurrentVisual2DResize
        {
            get { return _CurrentVisual2D; }
            set
            {
                _CurrentVisual2D = value;
                if (CurrentVisual2DResize != null)
                {
                    _smartTagContainer = new Visual2DRotatable(CurrentVisual2DResize.RectangleArea,
                                                               TextureType.Transparent);
                }
            }
        }

        public MouseResizeManager()
        {
            _canSelectNewBox = true;
            _firstTimePressed = false;

            _listOfTags = new List<Visual2DRotatable>();
            
            // Center
            _listOfTags.Add(new Visual2DRotatable(new Rectangle(), TextureType.CenterSmartTag));
            _listOfTags.Add(new Visual2DRotatable(new Rectangle(), TextureType.BoxSmartTag));
            _listOfTags.Add(new Visual2DRotatable(new Rectangle(), TextureType.BoxSmartTag));
            _listOfTags.Add(new Visual2DRotatable(new Rectangle(), TextureType.BoxSmartTag));
            _listOfTags.Add(new Visual2DRotatable(new Rectangle(), TextureType.BoxSmartTag));
        }

        private bool _isCaptured = false;
        public void Update(GameTime gameTime)
        {
            if (CurrentVisual2DResize == null)
                return;
            UpdateSmartTagsPosition(gameTime);
            UpdateMouseInput(gameTime);
        }

        private void UpdateMouseInput(GameTime gameTime)
        {
            bool manipulated = false;
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector3 positionCenter = CurrentVisual2DResize.PositionCenterEngine;
                float distanceX = Math.Abs(CurrentVisual2DResize.RectangleArea.X - mouseState.X);
                float distanceY = Math.Abs(CurrentVisual2DResize.RectangleArea.Y - mouseState.Y);
                _currentSelectedDrawable = GetMatchedSelectedBox();
                if (_currentSelectedDrawable != null)
                {
                    manipulated = ManipulateResizing(positionCenter, distanceY, distanceX);
                }
                _canSelectNewBox = false;
            }
            if (!manipulated)
            {
                ManipulateRotating();
            }
            if (mouseState.LeftButton == ButtonState.Released)
            {
                _canSelectNewBox = true;
            }
        }


        private Vector2 _vectorStart;
        private void ManipulateRotating()
        {
            MouseState mouseState = Mouse.GetState();
          
            {
                if (!_firstTimePressed)
                {
                    if (mouseState.RightButton == ButtonState.Pressed)
                    {
                        _firstTimePressed = true;
                        _vectorStart = new Vector2(mouseState.X, mouseState.Y);
                        _vectorStart.Normalize();
                    }
                }
                if (_firstTimePressed)
                {
                    if (mouseState.RightButton == ButtonState.Pressed)
                    {
                       
                        Vector2 vectorEnd = new Vector2(mouseState.X, mouseState.Y);
                        vectorEnd.Normalize();
                        float angle =  MathHelperModule.GetAngleBetweenTwoVectors(_vectorStart, vectorEnd);
                        CurrentVisual2DResize.SetOrientation(angle);
                    }
                }
            }
        }

        private bool ManipulateResizing(Vector3 positionCenter, float distanceY, float distanceX)
        {
            if (_currentSelectedDrawable == _listOfTags[3] || _currentSelectedDrawable == _listOfTags[1])
            {
                ManipulateTag3(distanceY, positionCenter);
                return true;
            }
            else
            {
                if (_currentSelectedDrawable == _listOfTags[2] || _currentSelectedDrawable == _listOfTags[4])
                {
                    ManipulateTag4(distanceX, positionCenter);
                    return true;
                }
            }
            return false;
        }

        private void ReIntitializeTag3Rectangle(Vector3 positionCenter, float distanceY, BoxRigid rectIn)
        {
            (rectIn).ReInitializeData
             (
                MathHelperModule.GetPositionXNA(positionCenter, rectIn.GetHalfWidth(), distanceY / 2),
                rectIn.GetMaterial(),
                new Vector3(rectIn.GetHalfWidth(), distanceY / 2, 0)
             );
        }

        private void ReIntitializeTag3Circle(Vector3 positionCenter, float distanceY, SphereRigid circleIn)
        {
            (circleIn).ReInitializeData
             (
                MathHelperModule.GetPositionXNA(positionCenter, (int)circleIn.Radius, distanceY / 2),
                circleIn.GetMaterial(),
                (distanceY / 2)
             );
        }

        private void ReIntitializeTag4Circle(Vector3 positionCenter, float distanceX, SphereRigid circleIn)
        {
            (circleIn).ReInitializeData
            (
                MathHelperModule.GetPositionXNA(positionCenter, distanceX / 2, circleIn.Radius),
                circleIn.GetMaterial(),
                (distanceX / 2)
            );
        }

        private void ReIntitializeTag4Rectangle(Vector3 positionCenter, float distanceX, BoxRigid rectIn)
        {
            (rectIn).ReInitializeData
            (
                MathHelperModule.GetPositionXNA(positionCenter, distanceX / 2, rectIn.GetHalfHeight()),
                rectIn.GetMaterial(),
                new Vector3(distanceX / 2, rectIn.GetHalfHeight(), 0)
            );
        }

        private void ManipulateTag3(float distanceY, Vector3 positionCenter)
        {
            if (distanceY > minPermittedHeight)
            {
                if (CurrentVisual2DResize is BoxRigid)
                {
                    ReIntitializeTag3Rectangle(positionCenter, distanceY, ((BoxRigid)CurrentVisual2DResize));
                }
                else
                {
                    if (CurrentVisual2DResize is SphereRigid)
                    {
                        ReIntitializeTag3Circle(positionCenter, distanceY, ((SphereRigid)CurrentVisual2DResize));
                    }
                }
            }
        }

        private void ManipulateTag4(float distanceX, Vector3 positionCenter)
        {
            if (distanceX > minPermittedWidth)
            {
                if (CurrentVisual2DResize is BoxRigid)
                {
                    ReIntitializeTag4Rectangle(positionCenter, distanceX, ((BoxRigid)CurrentVisual2DResize));
                }
                else
                {
                    if (CurrentVisual2DResize is SphereRigid)
                    {
                        ReIntitializeTag4Circle(positionCenter, distanceX, ((SphereRigid)CurrentVisual2DResize));
                    }
                }
            }
        }

        private Visual2DRotatable GetMatchedSelectedBox()
        {
            Visual2DRotatable catchedRotatable = null;
            MouseState mouseState = Mouse.GetState();
            if (_canSelectNewBox)
            {
                Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
                List<Visual2D> listOfVisual2DTags = new List<Visual2D>();
                listOfVisual2DTags.AddRange(_listOfTags);
                Visual2D catechedDrawable = MouseManager.CatchDrawableComponent(mousePosition, listOfVisual2DTags);
                if (catechedDrawable is Visual2DRotatable)
                {
                    catchedRotatable = catechedDrawable as Visual2DRotatable;
                }
            }
            else
            {
                catchedRotatable = _currentSelectedDrawable;
            }
            return catchedRotatable;
        }

        private void UpdateSmartTagsPosition(GameTime gameTime)
        {
            Rectangle RectangleArea = CurrentVisual2DResize.RectangleArea;
            // Center Smart tag
            _listOfTags[0].RectangleArea = new Rectangle(RectangleArea.X + (int)(RectangleArea.Width / 2) - _centerSmartTagHalf,
                                                         RectangleArea.Y + (int)(RectangleArea.Height / 2) - _centerSmartTagHalf,
                                                         _centerSmartTag,
                                                         _centerSmartTag);
            // 4 MidWays
            _listOfTags[1].RectangleArea = new Rectangle(RectangleArea.X + (int)(RectangleArea.Width / 2) - _boxSmartTagDimSizeHalf,
                                                         RectangleArea.Y - _boxSmartTagDimSizeHalf,
                                                         _boxSmartTagDimSize,
                                                         _boxSmartTagDimSize);
            _listOfTags[2].RectangleArea = new Rectangle(RectangleArea.X + (RectangleArea.Width) - _boxSmartTagDimSizeHalf,
                                                         RectangleArea.Y + (int)(RectangleArea.Height / 2) - _boxSmartTagDimSizeHalf,
                                                         _boxSmartTagDimSize,
                                                         _boxSmartTagDimSize);
            _listOfTags[3].RectangleArea = new Rectangle(RectangleArea.X + (int)(RectangleArea.Width / 2) - _boxSmartTagDimSizeHalf,
                                                         RectangleArea.Y + (int)(RectangleArea.Height) - _boxSmartTagDimSizeHalf,
                                                         _boxSmartTagDimSize,
                                                         _boxSmartTagDimSize);
            _listOfTags[4].RectangleArea = new Rectangle(RectangleArea.X - _boxSmartTagDimSizeHalf,
                                                         RectangleArea.Y + (int)(RectangleArea.Height / 2) - _boxSmartTagDimSizeHalf,
                                                         _boxSmartTagDimSize,
                                                         _boxSmartTagDimSize);
        }

        private ColorsProvider _colorProv = new ColorsProvider(Color.Red, Color.Black);
        public void Draw(GameTime gameTime)
        {
            if (CurrentVisual2DResize != null)
            {
                _smartTagContainer.AngleOfRotation = -CurrentVisual2DResize.GetOrientation();
                _smartTagContainer.Draw(gameTime);
                foreach (var component in _listOfTags)
                {
                    component.AngleOfRotation = -CurrentVisual2DResize.GetOrientation();
                    component.Draw(gameTime, _colorProv.ColorifyDrawing(gameTime));
                }
            }
        }
    }
}
