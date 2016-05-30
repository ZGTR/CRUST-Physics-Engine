using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CRUSTEngine.ProjectEngines.GraphicsEngine;

using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Gui;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay
{
    [Serializable]
    public class EntraManager: IUpdatableComponent
    {
        public EntraAgentSimple EntraAgentSimple;
        [NonSerialized]
        private Bitmap _bitmapToDraw;
        [NonSerialized]
        private EntraResult _entraResult;
        [NonSerialized] 
        private Thread _thread;
        private bool _isFirstTime;


        public EntraManager()
        {
            this.EntraAgentSimple = new CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.EntraAgentSimple();
            _thread = null;
            _isFirstTime = true;
            _bitmapToDraw = null;
        }

        public void Update(GameTime gameTime)
        {
            if (StaticData.IsEntraActivated)
            {
                if (_isFirstTime || !_thread.IsAlive)
                {
                    _thread = new Thread(() =>
                        {
                            _bitmapToDraw = RunEntraAgent(StaticData.EngineManager, out _entraResult);
                        });
                    _thread.Start();
                    _isFirstTime = false;
                }
            }
        }

        //private void KillFinishedThreads()
        //{
        //    if (_threads.Count > 1)
        //    {
        //        int kkk = 0;
        //    }
        //    _threadsToKill.Clear();
        //    foreach (Thread thread in _threads)
        //    {
        //        if (!thread.IsAlive)
        //            _threadsToKill.Add(thread);
        //    }

        //    foreach (Thread thread in _threadsToKill)
        //    {
        //        thread.Abort();
        //        _threads.Remove(thread);
        //    }
        //}

        //private void KillOverloadedThreads()
        //{
        //    if (_threads.Count > 3)
        //    {
        //        for (int i = 0; i < _threads.Count - 2; i++)
        //        {
        //            _threads.RemoveAt(0);
        //        }
        //    }
        //}

        public void Draw(GameTime gameTime)
        {
            UpdateUI();
            if (_bitmapToDraw != null)
            {
                var texToDraw = GetTexture2DFromBitmap(StaticData.EngineManager.Game1.GraphicsDevice, _bitmapToDraw);
                Visual2D vis =
                    new Visual2D(new Rectangle(0, 0, StaticData.LevelFarWidth, StaticData.LevelFarHeight),
                                 texToDraw);
                vis.Draw(gameTime);
            }
        }

        private void UpdateUI()
        {
            if (_entraResult != null)
            {
                if (_entraResult.IsPlayable)
                {
                    StaticData.CtrLevelDesigner.lbIsLevelPlayable.Text = @"            Yes            ";
                    StaticData.CtrLevelDesigner.lbIsLevelPlayable.BackColor =
                        System.Drawing.Color.LightGreen;
                }
                else
                {
                    StaticData.CtrLevelDesigner.lbIsLevelPlayable.Text = @"             No             ";
                    StaticData.CtrLevelDesigner.lbIsLevelPlayable.BackColor =
                        System.Drawing.Color.LightCoral;
                }
                StaticData.CtrLevelDesigner.lbIsLevelPlayable.ForeColor = System.Drawing.Color.White;
            }
        }

        public Bitmap RunEntraAgent(EngineManager engineState, out EntraResult eRes)
        {
            this.EntraAgentSimple = new EntraAgentSimple();
            eRes = this.EntraAgentSimple.CheckPlayability(engineState);
            Bitmap b = EntraDrawer.GetPolyOnlyBitmap(eRes);

            if (StaticData.IsEntraPathActivated)
            {
                StaticData.EngineManager.EntraPathManager.RunAgent();
            }
            return b;
        }

        public static Texture2D GetTexture2DFromBitmap(GraphicsDevice device, Bitmap bitmap)
        {
            Texture2D tex = new Texture2D(device, bitmap.Width, bitmap.Height, true, SurfaceFormat.Color);

            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                              ImageLockMode.ReadOnly, bitmap.PixelFormat);

            int bufferSize = data.Height * data.Stride;

            //create data buffer 
            byte[] bytes = new byte[bufferSize];

            // copy bitmap data into buffer
            Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);

            // copy our buffer to the texture
            tex.SetData(bytes);

            // unlock the bitmap data
            bitmap.UnlockBits(data);

            return tex;
        }
    }
}
