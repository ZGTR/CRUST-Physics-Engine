using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CRUSTEngine.ProjectEngines.GraphicsEngine
{
    [Serializable]
    public class ColorsProvider
    {
        private Color[] _colorArr;
        private double _counterToColor2 = 0;
        private Color Color1;
        private Color Color2;

        public ColorsProvider(Color c1, Color c2)
        {
            this.Color1 = c1;
            this.Color2 = c2;
            int rangeR, rangeG, rangeB;
            if (this.Color2.B - this.Color1.B > 0)
                rangeR = Math.Abs(this.Color1.R - this.Color2.R) / 5;
            else
                rangeR = -Math.Abs(this.Color1.R - this.Color2.R) / 5;
            if (this.Color2.B - this.Color1.B > 0)
                rangeG = Math.Abs(this.Color1.G - this.Color2.G) / 5;
            else
                rangeG = -Math.Abs(this.Color1.G - this.Color2.G) / 5;
            if (this.Color2.B - this.Color1.B > 0)
                rangeB = Math.Abs(this.Color1.B - this.Color2.B) / 5;
            else
                rangeB = -Math.Abs(this.Color1.B - this.Color2.B) / 5;

            _colorArr = new Color[5];

            _colorArr[0] = Color1;
            _colorArr[1] = new Color(Color1.R + 2 * rangeR, Color1.G + 2 * rangeG, Color1.B + 2 * rangeB);
            _colorArr[2] = new Color(Color1.R + 3 * rangeR, Color1.G + 3 * rangeG, Color1.B + 3 * rangeB);
            _colorArr[3] = new Color(Color1.R + 4 * rangeR, Color1.G + 4 * rangeG, Color1.B + 4 * rangeB);
            _colorArr[4] = new Color(Color1.R + 5 * rangeR, Color1.G + 5 * rangeG, Color1.B + 5 * rangeB);
        }

        public Color ColorifyDrawing(GameTime gametime)
        {
            if (gametime.TotalGameTime.Seconds % 2 == 0)
            {
                _counterToColor2 += 1;
                if (_counterToColor2 > 4)
                {
                    _counterToColor2 = 0;
                }
                //if (_counterToColor2 < _colorArr.Length - 1)
                //{
                //    return Color.White;
                //}
                //else
                //{
                //return _colorArr[(int) _counterToColor2];
                return _colorArr[(int) 0];
                //}
            }
            return new Color(0, 0, 0, 1);
        }
    }
}

