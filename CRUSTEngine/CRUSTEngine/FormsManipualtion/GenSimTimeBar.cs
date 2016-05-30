using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CRUSTEngine.ProjectEngines;
using CRUSTEngine.ProjectEngines.AuthoringTool;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers.GameModes;
using CRUSTEngine.ProjectEngines.PCGEngine;
using CRUSTEngine.ProjectEngines.PCGEngine.Actions;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.GenSim;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.GenSim.GevaInterpreter;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.Ryse;

namespace CRUSTEngine.FormsManipualtion
{
    public partial class GenSimTimeBarForm : Form
    {

        private Graphics g;
        private Pen penPoint;
        private Pen pen;
        private bool _deletionMode = false;
        private int minTSConst = 17*60;
        private int maxTSConst = 17*60 + 6*60;
        private int minTS;
        private int maxTS;
        private bool _moveMode;
        private int _currentCPX;
        private bool _catchedMove = false;
        private int currVal = 0;
        public List<ActionTimePair> PairCTP = new List<ActionTimePair>();
        public List<int> ctpGraphics = new List<int>();
        private int currX, currY;

        public GenSimTimeBarForm()
        {
            InitializeComponent();
            this.comboBox1.SelectedIndex = 0;
            this.lbTSValue.Text = (minTSConst).ToString();
            this.tbMinTS.Text = (minTSConst).ToString();
            this.tbMaxTS.Text = (maxTSConst).ToString();
            g = this.pictureBox1.CreateGraphics();
            pen = new Pen(Color.Black, 4);
            penPoint = new Pen(Color.Red, 6);
            minTS = Int32.Parse(tbMinTS.Text);
            maxTS = Int32.Parse(tbMaxTS.Text);
        }

        private void bSetParamsTS_Click(object sender, EventArgs e)
        {
            this.PairCTP.Clear();
            this.ctpGraphics.Clear();
            //this.trbrTime.Minimum = Int32.Parse(tbMinTS.Text);
            //this.trbrTime.Maximum = Int32.Parse(tbMaxTS.Text);
            if (Int32.Parse(tbMinTS.Text) >= 1020)
            {
                minTS = Int32.Parse(tbMinTS.Text);
            }
            else
            {
                tbMinTS.Text = (minTSConst).ToString();
                minTS = minTSConst;
            }
            maxTS = Int32.Parse(tbMaxTS.Text);
        }

        private void tbTime_Scroll(object sender, EventArgs e)
        {
            lbTSValue.Text = trbrTime.Value.ToString();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            currX = e.X;
            if (currX > 10 && currX < this.pictureBox1.Width - 10)
            {
                int totalWidth = this.pictureBox1.Width - 20;
                g.Clear(Color.White);

                g.DrawLine(pen, 10, 10, this.pictureBox1.Width - 10, 10);

                DrawLastPoints();

                if (!_deletionMode)
                {
                    penPoint.Color = GetCompColor(GetCompType());
                    g.DrawEllipse(penPoint, currX, 5, 10, 10);
                }
                else
                {
                    penPoint.Color = Color.Magenta;
                    g.DrawEllipse(penPoint, currX, 5, 10, 10);
                }

                float posTrack = (currX - 10)/(float) (totalWidth);

                currVal = (int) ((posTrack*(maxTS - minTS)) + minTS);

                lbTSValue.Text = (currVal).ToString() + "=" + String.Format("{0:0.00}", currVal/(float) 60) + "sec";
            }
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            g.Clear(Color.White);

            g.DrawLine(pen, 10, 10, this.pictureBox1.Width - 10, 10);

            DrawLastPoints();
        }

        private void DrawLastPoints()
        {
            int index = 0;
            foreach (int compTimePair in this.ctpGraphics)
            {
                penPoint.Color = GetCompColor((PairCTP[index] as ActionTimePair).EType);
                g.DrawEllipse(penPoint, compTimePair, 5, 10, 10);
                index++;
            }
        }

        private Color GetCompColor(EventType aType)
        {
            switch (aType)
            {
                case EventType.BlowerPress:
                    return Color.DodgerBlue;
                    break;
                case EventType.RopeCut:
                    return Color.SandyBrown;
                    break;
                case EventType.BubblePinch:
                    return Color.Gray;
                    break;
                case EventType.RocketPress:
                    return Color.OrangeRed;
                    break;
                case EventType.BumperInteraction:
                    return Color.Orange;
                    break;
                case EventType.OmNomFeed:
                    return Color.LimeGreen;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("eType");
            }
            return Color.Black;
        }

        private EventType GetCompType()
        {
            if (this.comboBox1.SelectedIndex == 0)
            {
                return EventType.RopeCut;
            }
            if (this.comboBox1.SelectedIndex == 1)
            {
                return EventType.RocketPress;
            }
            if (this.comboBox1.SelectedIndex == 2)
            {
                return EventType.BlowerPress;
            }
            if (this.comboBox1.SelectedIndex == 3)
            {
                return EventType.BubblePinch;
            }
            if (this.comboBox1.SelectedIndex == 4)
            {
                return EventType.BumperInteraction;
            }
            if (this.comboBox1.SelectedIndex == 5)
            {
                return EventType.OmNomFeed;
            }
            return EventType.RopeCut;
        }

        private void bDeleteComp_Click(object sender, EventArgs e)
        {
            _deletionMode = !_deletionMode;
            if (_deletionMode)
            {
                this.bDeleteComp.Text = "Deletion On";
            }
            else
            {
                this.bDeleteComp.Text = "Deletion Off";
            }
        }

        private void bRunAgent_Click(object sender, EventArgs e)
        {
            //Thread t = new Thread(() =>
            //    {
            Game1 game = StaticData.EngineManager.Game1;
            PairCTP.Sort(PairCTPComparator);
            GenSimAgentWrapper agent = new GenSimAgentWrapper(this.PairCTP, 20, false);
            agent.ScatterComps();

            StaticData.EngineManager.Game1 = game;
            if (agent.WAgent.IsSuccess)
            {
                LevelBuilder.CreateRestedLevel(agent.WAgent.LevelStr, false);
                CTRLevelDesigner.SaveEngineState();
                StaticData.GameSessionMode = SessionMode.PlayingMode;
                LivePlayabilitySimulator simulator = new LivePlayabilitySimulator(StaticData.EngineManager);
                simulator.SimulateSameWindow(agent.WAgent.Actions);
            }
            else
            {
                MessageBox.Show("Max number of try is reached. Run the agent again.");
            }

            //});
            //t.Start();

        }

        private int PairCTPComparator(CATimePair x, CATimePair y)
        {
            if (x.KeyTime > y.KeyTime)
            {
                return 1;
            }
            if (x.KeyTime == y.KeyTime)
            {
                return 0;
            }
            //if (x.KeyTime < y.KeyTime)
            {
                return -1;
            }
        }

        private void bClear_Click(object sender, EventArgs e)
        {
            this.PairCTP.Clear();
            this.ctpGraphics.Clear();
        }

        private void bMoveMode_Click(object sender, EventArgs e)
        {
            _moveMode = !_moveMode;
            if (_moveMode)
            {
                this.bMoveMode.Text = "Move On";
            }
            else
            {
                this.bMoveMode.Text = "Move Off";
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (_catchedMove)
            {
                int indexX = ctpGraphics.IndexOf(_currentCPX);
                ctpGraphics[indexX] = currX;
                PairCTP[indexX].KeyTime = currVal;
                _catchedMove = false;
            }
        }

        private void pictureBox1_Click(object sender, MouseEventArgs e)
        {
            if (_deletionMode)
            {
                foreach (int cp in ctpGraphics)
                {
                    if (Math.Abs(cp - currX) < 10)
                    {
                        PairCTP.RemoveAt(ctpGraphics.IndexOf(cp));
                        ctpGraphics.Remove(cp);
                        break;
                    }
                }
            }
            if (_moveMode && !_catchedMove)
            {
                foreach (int cp in ctpGraphics)
                {
                    if (Math.Abs(cp - currX) < 10)
                    {
                        _currentCPX = cp;
                        _catchedMove = true;
                        break;
                    }
                }
            }
            else
            {
                PairCTP.Add(new ActionTimePair(GetCompType(), currVal));
                ctpGraphics.Add(currX);
            }
        }
    }
}
