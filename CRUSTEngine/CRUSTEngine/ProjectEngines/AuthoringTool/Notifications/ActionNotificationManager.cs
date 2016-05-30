using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.PCGEngine.Actions;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Actions;
using Action  = CRUSTEngine.ProjectEngines.PCGEngine.Actions.Action;

namespace CRUSTEngine.ProjectEngines.AuthoringTool
{
    [Serializable]
    public class ActionsNotificationManager : IUpdatableComponent
    {
        public List<Visual2D> Notifications { private set; get; }
        private Visual2D background;
        private Visual2D marker;
        //private float oneNotificationPeriod = 15;
        //private float lastNotificationStartTimeStamp;
        private int scaleX = 70;
        private int scaleY = 25;
        private int notifNr = 0;


        public ActionsNotificationManager(List<Action> actions)
        {
            background = new Visual2D(new Rectangle(0, 0, 900, 40
                ), TextureType.ActionsBG);
            Notifications = new List<Visual2D>();
            if (actions != null)
            {
                actions = actions.Where(action => !(action is VoidAction)).ToList();

                int xCurrent = - ((actions.Count - 1)*scaleX - 400);
                actions.Reverse();
                foreach (var action in actions)
                {
                    if (!(action is VoidAction))
                    {
                        Notifications.Add(new Visual2D(new Rectangle(xCurrent, 10, scaleX, scaleY),
                                                        GetNotifAction(action)));
                        xCurrent += scaleX;
                    }
                }
            }
            //marker = new Visual2D(new Rectangle(_notifications[notifNr].RectangleArea.X,
            //                                        _notifications[notifNr].RectangleArea.Y - 8,
            //                                        _notifications[notifNr].RectangleArea.Width,
            //                                        6), TextureType.Red);
        }

        private TextureType GetNotifAction(Action action)
        {
            switch (action.AType)
            {
                case ActionType.BlowerPress:
                    return TextureType.BlowerPressNotif;
                    break;
                case ActionType.RopeCut:
                    return TextureType.RopeCutNotif;
                    break;
                //case ActionType.VoidAction:
                //    return TextureType.VoidActionNotif;
                //    break;
                case ActionType.BubblePinch:
                    return TextureType.BubblePinchNotif;
                    break;
                case ActionType.TerminateBranch:
                    break;
                case ActionType.RocketPress:
                    return TextureType.RocketPressNotif;
                    break;
                default:
                    return TextureType.VoidActionNotif;
            }
            return TextureType.VoidActionNotif;
        }

        public void PushNextNotification()
        {
            shouldShift = true;
            notifNr++;
        }
        
        int counter = 0;
        private bool shouldShift = false;
        double shiftMargin = 0;
        public void Update(GameTime gameTime)
        {
            shiftMargin += 0.8f;
            //if (shouldShift)
            {
                if (shiftMargin > 1)
                {
                    foreach (var notification in Notifications)
                    {
                        notification.RectangleArea = new Rectangle(notification.RectangleArea.X + 1,
                                                                   notification.RectangleArea.Y,
                                                                   notification.RectangleArea.Width,
                                                                   notification.RectangleArea.Height);
                        int index = Notifications.Count - notifNr - 1;
                        if (index >= 0)
                        {
                            marker = new Visual2D(new Rectangle(Notifications[index].RectangleArea.X,
                                                                Notifications[index].RectangleArea.Y - 10,
                                                                Notifications[index].RectangleArea.Width,
                                                                10), TextureType.Red);
                        }
                        else
                        {
                            marker = new Visual2D(new Rectangle(0,
                                                               -100,
                                                               1,
                                                               10), TextureType.Red);
                        }
                    }
                    shiftMargin = 0;
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            background.Draw(gameTime);
            if (marker != null)
                marker.Draw(gameTime);
            foreach (var notification in Notifications)
            {
                notification.Draw(gameTime);
            }
        }
    }
}
