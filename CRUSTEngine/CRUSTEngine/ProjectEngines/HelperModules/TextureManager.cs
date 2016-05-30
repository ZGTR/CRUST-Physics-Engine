using System;
using Microsoft.Xna.Framework.Graphics;
using CRUSTEngine.ProjectEngines.GraphicsEngine;

namespace CRUSTEngine.ProjectEngines.HelperModules
{
    [Serializable]
    public class TextureManager
    {

        public static Texture2D GetTextureByType(TextureType textureType)
        {
            return GetTextureByTypeRealComps(textureType);
            //return GetTextureByTypeSquaredRocketCompsWhiteBG(textureType);
        }

        private static Texture2D GetTextureByTypeRealComps(TextureType textureType)
        {
            Texture2D textureToReturn;
            Game1 game = StaticData.EngineManager.Game1;
            switch (textureType)
            {
                case TextureType.Border:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/Border3");
                    break;
                case TextureType.Red:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Red");
                    break;
                case TextureType.Notification01:
                    textureToReturn = game.Content.Load<Texture2D>(@"Notifications/Notif01");
                    break;
                case TextureType.Notification02:
                    textureToReturn = game.Content.Load<Texture2D>(@"Notifications/Notif02");
                    break;
                case TextureType.Pin:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/Pin");
                    break;
                case TextureType.PinRing:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/PinRing");
                    break;
                case TextureType.Bump:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/Bump");
                    break;
                case TextureType.Rocket0:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/Rocket0");
                    break;
                case TextureType.Rocket1:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/Rocket1");
                    break;
                case TextureType.Rocket2:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/Rocket2");
                    break;
                case TextureType.Rocket3:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/Rocket3");
                    break;
                case TextureType.Rocket4:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/Rocket4");
                    break;
                case TextureType.Rocket5:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/Rocket5");
                    break;
                case TextureType.Rocket6:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/Rocket6");
                    break;
                case TextureType.Rocket7:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/Rocket7");
                    break;
                case TextureType.Level1:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/Level1");
                    break;
                case TextureType.Level2:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/Level2");
                    break;
                case TextureType.RealRope:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/RealRope");
                    break;
                case TextureType.RoundedWhiteRectangle:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/RoundedWhiteRectangle");
                    break;
                case TextureType.DefaultBox:
                    textureToReturn = game.Content.Load<Texture2D>(@"RigidsTextures/DefaultBox");
                    break;
                case TextureType.DefaultCircle:
                    textureToReturn = game.Content.Load<Texture2D>(@"RigidsTextures/DefaultCircle");
                    break;
                case TextureType.CenterSmartTag:
                    textureToReturn = game.Content.Load<Texture2D>(@"SmartTags/CenterSmartTag");
                    break;
                case TextureType.BoxSmartTag:
                    textureToReturn = game.Content.Load<Texture2D>(@"SmartTags/BoxSmartTag");
                    break;
                case TextureType.Transparent:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/Transparent");
                    break;
                case TextureType.White:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/White");
                    break;
                case TextureType.Rocket:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/Rocket");
                    break;
                case TextureType.BubbleWithoutCandy:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/BubbleWithoutCandy");
                    break;
                case TextureType.BubbleWithCandy:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/BubbleWithCandy");
                    break;
                case TextureType.WaterWave:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/WaterWave");
                    break;
                case TextureType.BasicBackGround:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/BasicBackGround");
                    break;
                case TextureType.PlainWhite:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/PlainWhite");
                    break;
                case TextureType.VisualWater:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/VisualWater");
                    break;
                case TextureType.CirclePE:
                    textureToReturn = game.Content.Load<Texture2D>(@"ParticleEngine/circle");
                    break;
                case TextureType.DiamondPE:
                    textureToReturn = game.Content.Load<Texture2D>(@"ParticleEngine/diamond");
                    break;
                case TextureType.StarPE:
                    textureToReturn = game.Content.Load<Texture2D>(@"ParticleEngine/star");
                    break;
                case TextureType.BlowerNorth:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/BlowerNorth");
                    break;
                case TextureType.BlowerSouth:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/BlowerSouth");
                    break;
                case TextureType.BlowerEast:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/BlowerEast");
                    break;
                case TextureType.BlowerWest:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/BlowerWest");
                    break;
                case TextureType.FrogMouthOpen:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/FrogWithBase");
                    break;
                case TextureType.FrogWithCookie:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/FrogWithCookie");
                    break;
                case TextureType.FrogWithoutCookie:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/FrogWithoutCookie");
                    break;
                case TextureType.VoidActionNotif:
                    textureToReturn = game.Content.Load<Texture2D>(@"Notifications/Actions/VA");
                    break;
                case TextureType.RopeCutNotif:
                    textureToReturn = game.Content.Load<Texture2D>(@"Notifications/Actions/RC");
                    break;
                case TextureType.BlowerPressNotif:
                    textureToReturn = game.Content.Load<Texture2D>(@"Notifications/Actions/BB");
                    break;
                case TextureType.RocketPressNotif:
                    textureToReturn = game.Content.Load<Texture2D>(@"Notifications/Actions/RP");
                    break;
                case TextureType.BubblePinchNotif:
                    textureToReturn = game.Content.Load<Texture2D>(@"Notifications/Actions/BP");
                    break;
                case TextureType.ActionsBG:
                    textureToReturn = game.Content.Load<Texture2D>(@"Notifications/Actions/ActionsBG");
                    break;
                default:
                    textureToReturn = game.Content.Load<Texture2D>(@"RigidsTextures/DefaultBox");
                    break;
            }
            return textureToReturn;
        }

        private static Texture2D GetTextureByTypeRealFrogCompsWhiteBG(TextureType textureType)
        {
            Texture2D textureToReturn;
            Game1 game = StaticData.EngineManager.Game1;
            switch (textureType)
            {
                case TextureType.Level2:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/White");
                    break;
                case TextureType.FrogMouthOpen:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/FrogWithBase");
                    break;
                default:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Transparent");
                    break;
            }
            return textureToReturn;
        }

        private static Texture2D GetTextureByTypeSquaredFrogCompsWhiteBG(TextureType textureType)
        {
            Texture2D textureToReturn;
            Game1 game = StaticData.EngineManager.Game1;
            switch (textureType)
            {
                case TextureType.Level2:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/White");
                    break;
                case TextureType.FrogMouthOpen:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Green");
                    break;
                default:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Transparent");
                    break;
            }
            return textureToReturn;
        }

        private static Texture2D GetTextureByTypeSquaredRocketCompsWhiteBG(TextureType textureType)
        {
            Texture2D textureToReturn;
            Game1 game = StaticData.EngineManager.Game1;
            switch (textureType)
            {
                case TextureType.Level2:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/White");
                    break;
                case TextureType.Rocket:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Red");
                    break;
                default:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Transparent");
                    break;
            }
            return textureToReturn;
        }

        private static Texture2D GetTextureByTypeRealRocketCompsWhiteBG(TextureType textureType)
        {
            Texture2D textureToReturn;
            Game1 game = StaticData.EngineManager.Game1;
            switch (textureType)
            {
                case TextureType.Level2:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/White");
                    break;
                case TextureType.Rocket:
                    textureToReturn = game.Content.Load<Texture2D>(@"CTR/Rocket");
                    break;
                default:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Transparent");
                    break;
            }
            return textureToReturn;
        }

        private static Texture2D GetTextureByTypeSquaredColoredCompsWhiteBG(TextureType textureType)
        {
            Texture2D textureToReturn;
            Game1 game = StaticData.EngineManager.Game1;
            switch (textureType)
            {
                case TextureType.Notification01:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Transparent");
                    break;
                case TextureType.Notification02:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Transparent");
                    break;
                case TextureType.Pin:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Transparent");
                    break;
                case TextureType.PinRing:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Yellow");
                    break;
                case TextureType.Bump:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Orange");
                    break;
                case TextureType.Rocket0:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Red");
                    break;
                case TextureType.Level1:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/White");
                    break;
                case TextureType.Level2:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/White");
                    break;
                case TextureType.RealRope:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Brown");
                    break;
                case TextureType.DefaultBox:
                    textureToReturn = game.Content.Load<Texture2D>(@"RigidsTextures/DefaultBox");
                    break;
                case TextureType.DefaultCircle:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Pink");
                    break;
                case TextureType.CenterSmartTag:
                    textureToReturn = game.Content.Load<Texture2D>(@"SmartTags/CenterSmartTag");
                    break;
                case TextureType.BoxSmartTag:
                    textureToReturn = game.Content.Load<Texture2D>(@"SmartTags/BoxSmartTag");
                    break;
                case TextureType.Transparent:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Transparent");
                    break;
                case TextureType.White:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/White");
                    break;
                case TextureType.Rocket:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Red");
                    break;
                case TextureType.BubbleWithoutCandy:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Purple");
                    break;
                case TextureType.WaterWave:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Transparent");
                    break;
                case TextureType.BasicBackGround:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/White");
                    break;
                case TextureType.PlainWhite:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/PlainWhite");
                    break;
                case TextureType.VisualWater:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/LightYellow");
                    break;
                case TextureType.CirclePE:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Transparent");
                    break;
                case TextureType.DiamondPE:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Transparent");
                    break;
                case TextureType.StarPE:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Transparent");
                    break;
                case TextureType.BlowerNorth:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Blue");
                    break;
                case TextureType.BlowerSouth:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Blue");
                    break;
                case TextureType.BlowerEast:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Blue");
                    break;
                case TextureType.BlowerWest:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Blue");
                    break;
                case TextureType.FrogMouthOpen:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Green");
                    break;
                case TextureType.FrogWithCookie:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Green");
                    break;
                case TextureType.FrogWithoutCookie:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Green");
                    break;
                default:
                    textureToReturn = game.Content.Load<Texture2D>(@"RigidsTextures/DefaultBox");
                    break;
            }
            return textureToReturn;
        }

        private static Texture2D GetTextureByTypeSquaredRedComps(TextureType textureType)
        {
            Texture2D textureToReturn;
            Game1 game = StaticData.EngineManager.Game1;
            switch (textureType)
            {
                case TextureType.Notification01:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Transparent");
                    break;
                case TextureType.Notification02:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Transparent");
                    break;
                case TextureType.Pin:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Transparent");
                    break;
                case TextureType.PinRing:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Red");
                    break;
                case TextureType.Bump:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Red");
                    break;
                case TextureType.Rocket0:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Red");
                    break;
                case TextureType.Level1:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/White");
                    break;
                case TextureType.Level2:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/White");
                    break;
                case TextureType.RealRope:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Transparent");
                    break;
                case TextureType.DefaultBox:
                    textureToReturn = game.Content.Load<Texture2D>(@"RigidsTextures/DefaultBox");
                    break;
                case TextureType.DefaultCircle:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Red");
                    break;
                case TextureType.CenterSmartTag:
                    textureToReturn = game.Content.Load<Texture2D>(@"SmartTags/CenterSmartTag");
                    break;
                case TextureType.BoxSmartTag:
                    textureToReturn = game.Content.Load<Texture2D>(@"SmartTags/BoxSmartTag");
                    break;
                case TextureType.Transparent:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Transparent");
                    break;
                case TextureType.White:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/White");
                    break;
                case TextureType.Rocket:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Red");
                    break;
                case TextureType.BubbleWithoutCandy:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Red");
                    break;
                case TextureType.WaterWave:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Transparent");
                    break;
                case TextureType.BasicBackGround:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/White");
                    break;
                case TextureType.PlainWhite:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/PlainWhite");
                    break;
                case TextureType.VisualWater:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Transparent");
                    break;
                case TextureType.CirclePE:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Transparent");
                    break;
                case TextureType.DiamondPE:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Transparent");
                    break;
                case TextureType.StarPE:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Transparent");
                    break;
                case TextureType.BlowerNorth:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Red");
                    break;
                case TextureType.BlowerSouth:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Red");
                    break;
                case TextureType.BlowerEast:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Red");
                    break;
                case TextureType.BlowerWest:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Red");
                    break;
                case TextureType.FrogMouthOpen:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Red");
                    break;
                case TextureType.FrogWithCookie:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Red");
                    break;
                case TextureType.FrogWithoutCookie:
                    textureToReturn = game.Content.Load<Texture2D>(@"Components/Red");
                    break;
                default:
                    textureToReturn = game.Content.Load<Texture2D>(@"RigidsTextures/DefaultBox");
                    break;
            }
            return textureToReturn;
        }
    }
}
