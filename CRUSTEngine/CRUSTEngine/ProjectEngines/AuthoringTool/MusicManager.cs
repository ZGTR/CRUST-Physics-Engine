using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;

namespace CRUSTEngine.ProjectEngines.AuthoringTool
{
    public class MusicManager
    {
        private static Song _song;

        public static void Play()
        {
            if (false)
            {
                _song = StaticData.EngineManager.Game1.Content.Load<Song>(@"Music\Cut The Rope");
                MediaPlayer.Play(_song); // this will start the song playing
                MediaPlayer.IsRepeating = true;
            }
        }

        public static void Stop()
        {
            MediaPlayer.Stop();
        }
    }
}
