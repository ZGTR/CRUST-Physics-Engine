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
using CRUSTEngine.ProjectEngines.AuthoringTool;
using CRUSTEngine.ProjectEngines.PCGEngine.Generators;

namespace CRUSTEngine.FormsManipualtion
{
    public partial class SetGrammarDialog : Form
    {
        public SetGrammarDialog()
        {
            InitializeComponent();
            StreamReader sr = new StreamReader(@"C:\CTREngine\cut_the_rope_level_gen_pAuthoring.bnf");
            tbxGrammarString.Text = sr.ReadToEnd();
            sr.Close();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            string grammarStr = tbxGrammarString.Text;//.Replace("\r\n", Environment.NewLine);
            StreamWriter sw = new StreamWriter(@"C:\CTREngine\cut_the_rope_level_gen_pAuthoring.bnf");
            sw.WriteLine(grammarStr);
            sw.Close();
            DesignEnhanceManager.EnhanceDesign(true);
            this.Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
