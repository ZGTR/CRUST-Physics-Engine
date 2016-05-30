using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CRUSTEngine.ProjectEngines;
using CRUSTEngine.ProjectEngines.PCGEngine;
using CRUSTEngine.ProjectEngines.PCGEngine.Generators;

namespace CRUSTEngine.FormsManipualtion
{
    public partial class SetLevelDialog : Form
    {
        public SetLevelDialog()
        {
            InitializeComponent();
        }

        private void bUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            if (op.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(op.FileName);
                string levelStr = sr.ReadLine();
                tbxLevelString.Text = levelStr;
            }
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            string levelStr = tbxLevelString.Text;
            LevelBuilder.CreateRestedLevel(levelStr, false);
            //StaticData.EngineManager = new EngineManager(StaticData.EngineManager.Game1);
            //var LevelGeneratorEngine = new LevelGenerator(levelStr);
            //LevelGeneratorEngine.GenerateLevel();
            this.Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
