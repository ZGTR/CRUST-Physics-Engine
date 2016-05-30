using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CRUSTEngine.ProjectEngines.PCGEngine
{
    public class EngineShotsManager
    {
        public int DirNr { private set; get; }
        public int ImageCounter { private set; get; }
        private Game1 _game1 = null;

        public EngineShotsManager()
        {
            this.SetNewDirNumber();
        }

        public static void ShowXNAWindow()
        {
            try
            {
                using (Game1 game = new Game1())
                {
                    game.Run();
                }
            }
            catch (Exception)
            {
            }
        }

        private int SetNewDirNumber()
        {
            for (int i = 0; i < 10000; i++)
            {
                if (!Directory.Exists(i.ToString()))
                {
                    Directory.CreateDirectory(i.ToString());
                    DirNr = i;
                    break;
                }
            }
            return DirNr;
        }

        public void TakeEngineShot(bool shouldUpdate = true)
        {
            //if (_game1 == null)
            //{
            //    _game1 = new Game1();
            //}

            ////using (_game1)
            //{
            //    //_game1 = new Game1();
            //    _game1.SetOpacity(0);
            //    //_game1.IsDrawJustOneTime = true;
            //    _game1.ShouldUpdate = shouldUpdate;
            //    _game1.DrawMe();
                //_game1.Run();
                GameManager.SaveFrame(DirNr, ImageCounter);
                ImageCounter++;
            //}
        }

        public void TakeEngineShotWhileEngineRunning(String imageFileName)
        {
            GameManager.SaveFrame(imageFileName);
        }

        public void TakeEngineShot(String imageFileName)
        {
            GameManager.SaveFrame(imageFileName);
            //using (Game1 game1 = new Game1())
            //{
            //    try
            //    {
            //        game1.SetOpacity(0);
            //        game1.IsDrawJustOneTime = true;
            //        game1.ShouldUpdate = false;
            //        game1.Run();
            //        GameManager.SaveFrame(imageFileName);
            //    }
            //    catch (Exception e)
            //    {

            //    }
            //}
        }
    }
}

    