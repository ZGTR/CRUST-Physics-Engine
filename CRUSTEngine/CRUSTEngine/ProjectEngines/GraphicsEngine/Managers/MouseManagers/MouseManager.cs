using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using CRUSTEngine.ProjectEngines.CTREngine;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Bubble;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Rocket;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Blower;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.CatchableRopes;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers.GameModes;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs;

namespace CRUSTEngine.ProjectEngines.GraphicsEngine.Managers.MouseManagers
{
    [Serializable]
    public class MouseManager : IUpdatableComponent
    {
        private Vector2 posPressed;
        private Vector2 posReleased;
        private SpringService _currentSpringService;

        private bool _isCaptured;
        
        public MouseManager()
        {
            _isCaptured = false;
        }

        public void NormalizeRigidsDragger()
        {
            _isCaptured = false;
        }

        private Vector3 mousePositionStart;
        bool staticCompsClick = false;
        public void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            //ManipulateScrolling(mouseState);
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
            if (mousePosition.X <= 0 || mousePosition.X >= 900 || mousePosition.Y >= 550 || mousePosition.Y <= 0)
            {
                posPressed = new Vector2(0, 0);
                return;
            } 
            //if (!IsMouseClickedBefore)
            //{
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (posPressed == new Vector2(0, 0))
                        posPressed = mousePosition;
                    if (_isCaptured)
                    {
                        //if (StaticData.CurrentVisual2D is RocketCarrierService)
                        //{
                        //    StaticData.EngineManager.RocketsCarrierManagerEngine.RemoveService(StaticData.CurrentVisual2D as RocketCarrierService);
                        //}

                        if (StaticData.CurrentVisual2D is RigidBody)
                        {
                            //_currentSpringService =
                            //    SpringService.CatchSpringServiceForRigidBody(StaticData.CurrentVisual2D as RigidBody);
                            RigidBody rigidBody = StaticData.CurrentVisual2D as RigidBody;
                            switch (StaticData.ManipulationGameMode)
                            {
                                case ManipulationGameMode.TorqueManipulationMode:
                                    {
                                        Vector3 forceOfTorque = new Vector3(mousePosition.X, mousePosition.Y, 0) -
                                                                new Vector3(mousePositionStart.X,
                                                                            mousePositionStart.Y,
                                                                            0);
                                        forceOfTorque *= rigidBody.Mass;
                                        rigidBody.AddTorque(forceOfTorque, mousePositionStart);

                                    }
                                    break;
                                case ManipulationGameMode.ForceManipulationMode:
                                    {
                                        Vector3 force = new Vector3(mousePosition.X, mousePosition.Y, 0) -
                                                        new Vector3(mousePositionStart.X,
                                                                    mousePositionStart.Y,
                                                                    0);
                                        force *= rigidBody.Mass*10;
                                        rigidBody.AddForce(force);
                                    }
                                    break;
                                case ManipulationGameMode.DeleteRigidMode:
                                    StaticData.EngineManager.RigidsManagerEngine.DeleteRigid(rigidBody);
                                    break;

                                case ManipulationGameMode.DragRigidMode:
                                    ((RigidBody)
                                     StaticData.CurrentVisual2D).PositionXNA = new Vector3(mousePosition.X,
                                                                                           mousePosition.Y,
                                                                                           0);
                                    break;
                                    //case ManipulationGameMode.DeleteRopesMode:
                                    //    TryDeleteRope();
                                    //    break;
                                    //case ManipulationGameMode.DeleteBubblesMode:
                                    //    TryDeleteBubbles();
                                    //    break;
                            }
                        }
                        else
                        {
                            switch (StaticData.ManipulationGameMode)
                            {
                                case ManipulationGameMode.DragRigidMode:
                                    StaticData.CurrentVisual2D.SetPositionXNA(new Vector3(mousePosition.X,
                                                                                          mousePosition.Y,
                                                                                          0));
                                    break;
                                case ManipulationGameMode.DeleteRigidMode:
                                    DeleteNonRigidBodies();
                                    break;
                            }
                        }
                        if (StaticData.ManipulationGameMode == ManipulationGameMode.SetStaticComps && staticCompsClick == false)
                        {
                            staticCompsClick = true;
                            StaticData.EngineManager.PrefCompsManager.ToggleComponentSetterState(
                                    StaticData.CurrentVisual2D);
                        }
                    }
                    else
                    {
                        var tryVisual = CatchVisual2D();
                        if (tryVisual != null)
                        {
                            StaticData.CurrentVisual2D = tryVisual;
                            mousePositionStart = new Vector3(mousePosition.X, mousePosition.Y, 0);
                            _isCaptured = true;
                        }
                    }
                }
            //}
            //if (StaticData.ManipulationGameMode == ManipulationGameMode.DeleteBubblesMode)
                //    TryDeleteBubbles();
            if (mouseState.LeftButton == ButtonState.Released)
            {
                if (posPressed != new Vector2(0, 0) && StaticData.GameSessionMode == SessionMode.PlayingMode)
                {
                    posReleased = mousePosition;
                    TryRopeCut(posPressed, posReleased);
                    posPressed = new Vector2(0, 0);
                }
                _isCaptured = false;
                staticCompsClick = false;
            }

        }

        //private void ManipulateScrolling(MouseState mouseState)
        //{
        //    MouseState s = Mouse.GetState();
        //    if (mouseState.ScrollWheelValue > 0)
        //    {
        //        if (_currentSpringService != null)
        //            _currentSpringService.AddNewMass();
        //        //mouseState.ScrollWheelValue = 0;
        //    }
        //}

        private void DeleteNonRigidBodies()
        {
            if (StaticData.CurrentVisual2D is BlowerService)
            {
                StaticData.EngineManager.BlowerManagerEngine.RemoveService(StaticData.CurrentVisual2D as BlowerService);
            }
            if (StaticData.CurrentVisual2D is BubbleService)
            {
                StaticData.EngineManager.BubbleManagerEngine.RemoveService(StaticData.CurrentVisual2D as BubbleService);
            }
        }

        private void TryRopeCut(Vector2 posPressed, Vector2 posReleased)
        {
            try
            {
                for (int i = 0; i < StaticData.EngineManager.SpringsManagerEngine.ListOfServices.Count; i++)
                {
                    SpringService cService = StaticData.EngineManager.SpringsManagerEngine.ListOfServices[i];
                    Vector2 posRopeStart = new Vector2(cService.Masses[0].PositionXNA.X, cService.Masses[0].PositionXNA.Y);
                    Vector2 posRopeEnd = new Vector2(cService.Masses[cService.Masses.Count - 1].PositionXNA.X,
                                                   cService.Masses[cService.Masses.Count - 1].PositionXNA.Y);
                    if (MathHelperModule.IsIntersecting(posPressed, posReleased, posRopeStart, posRopeEnd))
                    {
                        if (!(cService is CatchableRopeService))
                        {
                            StaticData.EngineManager.SpringsManagerEngine.RemoveService(cService);
                            i--;
                        }
                        else
                        {
                            if ((cService as CatchableRopeService).IsActivated)
                            {
                                StaticData.EngineManager.SpringsManagerEngine.RemoveService(cService);
                                i--;
                            }
                        }

                    }
                    //if (!((posPressed.Y < posRopeStart.Y && posReleased.Y < posRopeStart.Y)
                    //    || (posPressed.Y > posRopeEnd.Y && posReleased.Y > posRopeEnd.Y)))
                    //{
                    //    StaticData.EngineManager.SpringsManagerEngine.RemoveService(cService);
                    //}
                }
            }
            catch (Exception)
            {
            }
        }

        private bool IsPointInY(Vector2 pos, Vector2 posRopeStart, Vector2 posRopeEnd)
        {
            if (pos.Y > posRopeStart.Y && pos.Y < posRopeEnd.Y)
                return true;
            return false;
        }

        private bool IsPointBeforeX(Vector2 pos, Vector2 posRopeStart, Vector2 posRopeEnd)
        {
            if (posPressed.Y > posRopeStart.Y && posPressed.Y < posRopeEnd.Y)
                return true;
            return false;
        }

        //private void TryDeleteBubbles()
        //{
        //    var serviceToDelete = CatchBubble();
        //    if (serviceToDelete != null)
        //        StaticData.EngineManager.BubbleManagerEngine.RemoveService(serviceToDelete);
        //}

        //private void TryDeleteRope()
        //{
        //    var serviceToDelete = CatchRope(StaticData.CurrentVisual2D);
        //    if (serviceToDelete != null)
        //        StaticData.EngineManager.SpringsManagerEngine.RemoveService(serviceToDelete);
        //}

        public void Draw(GameTime gameTime)
        {
            
        }

        public static Visual2D CatchVisual2D()
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
            Visual2D visual2D = CatchVisual2D(mousePosition);

            //if (rectRigid != null)
            //{
            //    return rectRigid;
            //}
            //else
            //{
            //    SphereRigid SphereRigid = CatchSphereRigid(mousePosition);
            //    if (SphereRigid != null)
            //    {
            //        return SphereRigid;
            //    }
            //}
            return visual2D;
        }

        private static Visual2D CatchVisual2D(Vector2 mousePosition)
        {
            Visual2D visual2D = null;
            for (int i = 0; i < StaticData.EngineManager.RigidsManagerEngine.ListOfRigids.Count; i++)
            {
                Visual2D current = StaticData.EngineManager.RigidsManagerEngine.ListOfRigids[i];
                if (IsMouseOverRectArea(mousePosition, current.RectangleArea))
                {
                    return current;
                }
            }

            //for (int i = 0; i < StaticData.EngineManager.SpringsManagerEngine.ListOfServices.Count; i++)
            //{
            //    var current = StaticData.EngineManager.SpringsManagerEngine.ListOfServices[i];
            //    if (current is CatchableRopeService)
            //    {
            //        CatchableRopeService cRope = (CatchableRopeService) current;

            //        if (IsMouseOverRectArea(mousePosition, cRope.pin.RectangleArea))
            //        {
            //            return cRope.pin;
            //        }
            //    }
            //}

            for (int i = 0; i < StaticData.EngineManager.BubbleManagerEngine.ListOfServices.Count; i++)
            {
                Visual2D current = StaticData.EngineManager.BubbleManagerEngine.ListOfServices[i];
                if (IsMouseOverRectArea(mousePosition, current.RectangleArea))
                {
                    return current;
                }
            }

            for (int i = 0; i < StaticData.EngineManager.BlowerManagerEngine.ListOfServices.Count; i++)
            {
                Visual2D current = StaticData.EngineManager.BlowerManagerEngine.ListOfServices[i];
                if (IsMouseOverRectArea(mousePosition, current.RectangleArea))
                {
                    return current;
                }
            }

            return visual2D;
        }

        public static BubbleService CatchBubble()
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
            BubbleService bubble = CatchBubbleServiceForRigidBody(mousePosition);
            if (bubble != null)
            {
                return bubble;
            }
            return null;
        }

        public static BubbleService CatchBubbleServiceForRigidBody(Vector2 mousePosition)
        {
            foreach (BubbleService bubble in StaticData.EngineManager.BubbleManagerEngine.ListOfServices)
            {
                if (IsMouseOverRectArea(mousePosition, bubble.RectangleArea))
                {
                    return bubble;
                }
            }
            return null;
        }

        public static BoxRigid CatchBoxRigid(Vector2 mousePosition)
        {
            foreach (BoxRigid boxRigid in StaticData.EngineManager.RigidsManagerEngine.ListOfBoxRigids)
            {
                if (IsMouseOverRectArea(mousePosition, boxRigid.RectangleArea))
                {
                    return boxRigid;
                }
            }
            return null;
        }

        public static Visual2D CatchDrawableComponent(Vector2 mousePosition, List<Visual2D> listOfItems)
        {
            foreach (Visual2D component in listOfItems)
            {
                if (IsMouseOverDrawableComponent(mousePosition, component))
                {
                    return component;
                }
            }
            return null;
        }

        public static bool IsMouseOverDrawableComponent(Vector2 mousePosition, Visual2D comopnent)
        {
            return (
                       (mousePosition.X >= comopnent.RectangleArea.X) &&
                       (mousePosition.X <= comopnent.RectangleArea.X + comopnent.RectangleArea.Width)
                       &&
                       (mousePosition.Y >= comopnent.RectangleArea.Y) &&
                       (mousePosition.Y <= comopnent.RectangleArea.Y + comopnent.RectangleArea.Height)
                   );
        }

        public static bool IsMouseOverRectArea(Vector2 mousePosition, Rectangle rectangle)
        {
            return (
                       (mousePosition.X >= rectangle.X) &&
                       (mousePosition.X <= rectangle.X + rectangle.Width)
                       &&
                       (mousePosition.Y >= rectangle.Y) &&
                       (mousePosition.Y <= rectangle.Y + rectangle.Height)
                   );
        }

        public static SphereRigid CatchSphereRigid(Vector2 mousePosition)
        {
            Vector2 normMousePosition = mousePosition;
            normMousePosition.Normalize();

            foreach (SphereRigid SphereRigid in StaticData.EngineManager.RigidsManagerEngine.ListOfSphereRigids)
            {
                if (IsMouseOverRectArea(mousePosition, SphereRigid.RectangleArea))
                {
                    Vector2 normCircleCenter = SphereRigid.Center;
                    normCircleCenter.Normalize();
                    if ((normMousePosition - normCircleCenter).Length() < SphereRigid.Radius)
                    {
                        return SphereRigid;
                    }
                }
            }
            return null;
        }
    }
}
