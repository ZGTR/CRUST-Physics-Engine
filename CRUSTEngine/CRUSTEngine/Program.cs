using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using ClipperLib;
using CRUSTEngine.ProjectEngines.Starters;
using Microsoft.Xna.Framework;
using CRUSTEngine.Database;
using CRUSTEngine.FormsManipualtion;
using CRUSTEngine.ProjectEngines;
using CRUSTEngine.ProjectEngines.AuthoringTool;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Blower;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers.GameModes;
using CRUSTEngine.ProjectEngines.PCGEngine;
using CRUSTEngine.ProjectEngines.PCGEngine.Generators;
using CRUSTEngine.ProjectEngines.PCGEngine.Generators.GenManagers;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.MusicBased;
using Point = Microsoft.Xna.Framework.Point;

namespace CRUSTEngine
{
    using Polygon = List<IntPoint>;
    using Polygons = List<List<IntPoint>>;

#if WINDOWS || XBOX
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            args = new string[] {"99", "0", "0"};
            new StarterManager().Start(args);
        }
    }
#endif
}