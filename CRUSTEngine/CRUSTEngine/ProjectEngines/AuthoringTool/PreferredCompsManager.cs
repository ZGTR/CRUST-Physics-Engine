using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Bubble;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Blower;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.PCGEngine;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs;
using Color = Microsoft.Xna.Framework.Color;

namespace CRUSTEngine.ProjectEngines.AuthoringTool
{
    [Serializable]
    public class PreferredCompsManager : IUpdatableComponent
    {
        private const String GrammarRest = @"<comps>::=<comp>|<comp><comps>
<comp>::=<rope>|<blower>|<rocket>|<bump>|<bubble>
<rope>::= rope(<x>,<y>,<length>)
<rocket>::=rocket(<x>,<y>,<rocket_dir>)
<blower>::= blower(<x>,<y>,<blower_dir>)
<bump>::=bump(<x>,<y>,<bump_dir>)
<bubble> ::= bubble(<x>, <y>)

<x>::= 200 | 220 | 240 | 260 | 280 | 300 | 320 | 340 | 360 | 380 | 400 | 420 |  440 | 460 | 480 | 500 | 520 | 540 
<y>::= 40 | 60 | 80 | 100 | 120 | 140 |  160 | 180 | 200 | 220 | 240 | 260 | 280 | 300 | 320 | 340 | 360 | 380 | 400 | 420 |  440 | 460
<length>::= 100 | 130 | 160 | 190 | 220 | 250 | 270

<blower_dir>::= 0 | 4 
<bump_dir>::= 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7
<rocket_dir>::= 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7
<isExist>::= 0 | 1
<depth>::= 30 | 60 | 90 | 120 | 150 | 180";


        private List<Visual2D> _comps;
        private ColorsProvider _colorProvider;
        public PreferredCompsManager()
        {
            _comps = new List<Visual2D>();
            _colorProvider = new ColorsProvider(Color.White, Color.Gray);
        }

        public void ToggleComponentSetterState(Visual2D vis)
        {
            if (CanBePreferred(vis))
            {
                if (vis != null)
                {
                    if (_comps.Contains(vis))
                    {
                        _comps.Remove(vis);
                    }
                    else
                    {
                        _comps.Add(vis);
                    }
                }
            }
        }

        private bool CanBePreferred(Visual2D vis)
        {
            if (vis is FrogRB
                || IsRopePin(vis)
                || vis is RocketCarrierService
                || vis is BubbleService
                || vis is BlowerService
                || vis is BumpRigid)
                return true;
            return false;
        }

        private bool IsRopePin(Visual2D vis)
        {
            foreach (SpringService service in StaticData.EngineManager.SpringsManagerEngine.ListOfServices)
            {
                if (vis == service.Masses[0])
                    return true;
            }
            return false;
        }

        public String GetPrefCompsToGrammarFile()
        {
            String strFinalGrammar = String.Empty;
            String compLevelBase = String.Empty;
            String compShouldPres = String.Empty;
            String compAlreadyPres = EngineStateManager.GetEngineStateFactStringWithSpaceDelimiterGEVAStyle(_comps);
            int compPresCounter = 0;
            if (!compAlreadyPres.Contains("frog"))
            {
                compShouldPres += "<frog>";
            }
            if (!compAlreadyPres.Contains("rope"))
            {
                compShouldPres += "<rope>";
                compPresCounter++;
            }
            if (!compAlreadyPres.Contains("blower"))
            {
                compShouldPres += "<blower>";
                compPresCounter++;
            }
            if (!compAlreadyPres.Contains("rocket"))
            {
                compShouldPres += "<rocket>";
                compPresCounter++;
            }
            if (!compAlreadyPres.Contains("bump"))
            {
                compShouldPres += "<bump>";
                compPresCounter++;
            }
            if (!compAlreadyPres.Contains("bubble"))
            {
                compShouldPres += "<bubble>";
                compPresCounter++;
            }
            

            if (compPresCounter < 5)
            {
                compLevelBase = "<level>::=<cookie><comps_design><comps>";
            }
            else
            {
                compLevelBase = "<level>::=<cookie><comps_design>";
            }

            strFinalGrammar = compLevelBase;
            strFinalGrammar += Environment.NewLine
                               +
                               @"<cookie>::=cookie(<x>,<y>)
<frog>::=frog(<x>,<y>)
<comps_design>::="
                               + compAlreadyPres + compShouldPres;

            strFinalGrammar += Environment.NewLine
                + GrammarRest;
            return strFinalGrammar;
        }

        public void Update(GameTime gameTime)
        {
            // Nothing to do here
        }

        public void Draw(GameTime gameTime)
        {
            for (int i = 0; i < _comps.Count; i++)
            {
                Visual2D vis = _comps[i];
                var newRect = new Rectangle(vis.RectangleArea.X - 10, vis.RectangleArea.Y - 10,
                                            vis.RectangleArea.Width + 20, vis.RectangleArea.Height + 20);
                Visual2D visBorder = new Visual2D(newRect, TextureType.Border);
                Color newColor = _colorProvider.ColorifyDrawing(gameTime);
                visBorder.Draw(gameTime, newColor);
            }
        }
    }
}
