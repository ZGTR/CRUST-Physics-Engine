using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;

namespace CRUSTEngine.ProjectEngines.AuthoringTool
{
    [Serializable]
    public class NotificationManager : IUpdatableComponent
    {
        private List<TextureType> _notifications;
        private float oneNotificationPeriod = 15;
        private float lastNotificationStartTimeStamp;
        

        public NotificationManager()
        {
            _notifications = new List<TextureType>();
        }

        public void PushNotification(NotificationType notificationType)
        {
            _notifications.Add(GetTextureOfNotification(notificationType));
        }

        private TextureType GetTextureOfNotification(NotificationType notificationType)
        {
            switch (notificationType)
            {
                case NotificationType.DirectionOfBumpsBlowersRockets:
                    return TextureType.Notification01;
                    break;
                case NotificationType.HeightOfRope:
                    return TextureType.Notification02;
                    break;
            }
            return TextureType.Notification01;
        }

        //private string GetNotificationString(NotificationType notificationType)
        //{
        //    switch (notificationType)
        //    {
        //        case NotificationType.DirectionOfBumpsBlowersRockets:
        //            return "You can change the direction of Bumps, Blowers and Rockets through" + Environment.NewLine +
        //                   "\"Change Components Direction\" Button";
        //            break;
        //        case NotificationType.HeightOfRope:
        //            return "You can change Rope height: make it longer when pressing the rope's pin;" + Environment.NewLine +
        //                   "shorter with Deletion Mode along the rope";
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException("notificationType");
        //    }
        //}

        
        public void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.Seconds - lastNotificationStartTimeStamp >= oneNotificationPeriod)
            {
                if (_notifications.Count > 0)
                    _notifications.RemoveAt(0);
                lastNotificationStartTimeStamp = 0;
                _y = -40;
            }
        }

        private float _y = -40;
        public void Draw(GameTime gameTime)
        {
            if (StaticData.GameSessionMode == SessionMode.DesignMode && StaticData.IsNotification)
            {
                if (_notifications.Count > 0)
                {
                    if (lastNotificationStartTimeStamp != 0)
                    {
                        if (_y < 0)
                            _y += 0.5f;
                        int startX = 150;
                        Visual2D vis = new Visual2D(new Vector3(startX, (int) _y, 0), 600, 40, _notifications[0]);
                        vis.Draw(gameTime);

                        //var spriteBatch = StaticData.EngineManager.Game1.SpriteBatch;
                        //spriteBatch.Begin();
                        //spriteBatch.DrawString(StaticData.EngineManager.Game1.Font,
                        //                                                      _notifications[0],
                        //                                                      new Vector2(startX + 20, 5), Color.White);
                        //spriteBatch.End();
                    }
                    else
                    {
                        lastNotificationStartTimeStamp = gameTime.TotalGameTime.Seconds;
                    }
                }
            }
        }
    }
}
