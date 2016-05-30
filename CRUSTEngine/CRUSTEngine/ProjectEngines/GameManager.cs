using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CRUSTEngine.FormsManipualtion;
using CRUSTEngine.ProjectEngines.HelperModules;
using Point = System.Drawing.Point;

namespace CRUSTEngine.ProjectEngines
{
    public static class  GameManager
    {
        public static Game1 GameMe;
        public static void InitializeMe()
        {
            Form winForm = (Form)Form.FromHandle(GameMe.Window.Handle);
            Cursor myCursor = CursorHelper.LoadCustomCursor(@"Content\Cursors\cursorHand.ani");
            winForm.Cursor = myCursor;
            winForm.Location = new Point(310, 20);

            GameMe.Window.Title = "CRUST - Ropossum V3.0";
            GameMe.SpriteBatch = new SpriteBatch(GameMe.GraphicsDevice);

            CTRLevelDesigner form1 = new CTRLevelDesigner();
            //CRUSTLevelDesigner form1 = new CRUSTLevelDesigner();
            if (Game1.IsUserDesigner)
            {
                form1.StartPosition = FormStartPosition.Manual;
                form1.Show();
            }

            StaticData.InitializeEngine(GameMe);
        }


        public static void SaveFrame(int dirNr, int count)
        {
            count += 1;
            string counter = count.ToString();

            int w = 900;// this.GraphicsDevice.PresentationParameters.BackBufferWidth;
            int h = 550;// this.GraphicsDevice.PresentationParameters.BackBufferHeight;

            GameMe.DrawMe();

            //pull the picture from the buffer 
            int[] backBuffer = new int[w * h];
            GameMe.GraphicsDevice.GetBackBufferData(backBuffer);

            //copy into a texture 
            Texture2D texture = new Texture2D(GameMe.GraphicsDevice, w, h, false,
                GameMe.GraphicsDevice.PresentationParameters.BackBufferFormat);
            texture.SetData(backBuffer);

            //save to disk 
            Stream stream = File.OpenWrite(dirNr + @"\" + counter + ".jpg");

            texture.SaveAsJpeg(stream, w, h);
            stream.Dispose();

            texture.Dispose();
        }

        public static void SaveFrame(String imageFileName)
        {
            int w = GameMe.GraphicsDevice.PresentationParameters.BackBufferWidth;
            int h = GameMe.GraphicsDevice.PresentationParameters.BackBufferHeight;

            GameMe.DrawMe();

            //pull the picture from the buffer 
            int[] backBuffer = new int[w * h];
            GameMe.GraphicsDevice.GetBackBufferData(backBuffer);

            //copy into a texture 
            Texture2D texture = new Texture2D(GameMe.GraphicsDevice, w, h, false,
                GameMe.GraphicsDevice.PresentationParameters.BackBufferFormat);
            texture.SetData(backBuffer);

            //save to disk 
            Stream stream = File.OpenWrite(imageFileName);

            texture.SaveAsJpeg(stream, w, h);
            stream.Close();
            stream.Dispose();

            texture.Dispose();
        }
    }
}
