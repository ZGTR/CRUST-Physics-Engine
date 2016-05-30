using System;
using System.Drawing;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPath
{
    [Serializable]
    public class EntraPathManager: IUpdatableComponent
    {
        [NonSerialized]
        private Texture2D _texToDraw;
        [NonSerialized]
        private Thread _thread;
        [NonSerialized]
        private Bitmap _bitmapPath = null;
        private bool firstTime = true;

        public EntraPathManager()
        {
            _thread = null;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void RunAgent()
        {
            if (StaticData.IsEntraActivated && StaticData.IsEntraPathActivated)
            {
                _bitmapPath = RunEntraAgent();
                //if (firstTime || !_thread.IsAlive)
                //{
                //    _thread = new Thread(() =>
                //        {
                //            _bitmapPath = RunEntraAgent();
                //        });
                //    _thread.Start();
                //    firstTime = false;
                //}
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (_bitmapPath != null)
            {
                _texToDraw = EntraManager.GetTexture2DFromBitmap(StaticData.EngineManager.Game1.GraphicsDevice,
                                                                 _bitmapPath);
                Visual2D vis =
                    new Visual2D(new Rectangle(0, 0, StaticData.LevelFarWidth, StaticData.LevelFarHeight),
                                 _texToDraw);
                vis.Draw(gameTime);
            }
        }

        private Bitmap RunEntraAgent()
        {
            //var entraCopy = ObjectSerializer.DeepCopy(StaticData.EngineManager.EntraManager.EntraAgentSimple);
            EntraPathAgent pathAgent = new EntraPathAgent(StaticData.EngineManager.EntraManager.EntraAgentSimple);
            Bitmap b = null;
            int indexPathToDraw = StaticData.EntraPathIndex;
            if (indexPathToDraw > pathAgent.AllPaths.Count - 1)
            {
                indexPathToDraw = pathAgent.AllPaths.Count - 1;
            }
            if (indexPathToDraw < 0)
            {
                indexPathToDraw = 0;
            }
            if (pathAgent.AllPaths.Count > 0)
                b = pathAgent.GetPathBitmap(indexPathToDraw, false);
            return b;
        }
    }
}
