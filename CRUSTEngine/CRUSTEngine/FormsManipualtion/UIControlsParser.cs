using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.PhysicsEngine;

namespace RigidsInMotion.UIDigitaRune
{
    public class UIControlsParser
    {
        public static float GetFloat(TextBox control)
        {
            try
            {
                var textBox = control as TextBox;
                float iOut = 0;
                if (textBox != null) float.TryParse(textBox.Text, out iOut);
                return iOut;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Vector3 GetVector3(TextBox control)
        {
            try
            {
                var textBox = control as TextBox;
                Vector3 vecOut = new Vector3();
                String[] paramArr = textBox.Text.Split(',');
                float x = 0;
                float y = 0;
                float.TryParse(paramArr[0], out x);
                float.TryParse(paramArr[1], out y);
                vecOut = new Vector3(x, y, 0);
                return vecOut;
            }
            catch (Exception)
            {
                return new Vector3(0,0,0);
            }
        }

        public static Material GetMaterial(ComboBox control)
        {
            var dropDown = (control as ComboBox);
            if (dropDown != null) return (Material)(dropDown.SelectedIndex);
            return 0;
        }

        public static bool GetBool(ComboBox control)
        {
            var dropDown = (control as ComboBox);
            if (dropDown != null)
                if (dropDown.SelectedIndex == 0)
                {
                    return true;
                }
            return false;
        }

        public static int GetIndexOfSelection(ComboBox control)
        {
            var dropDown = (control as ComboBox);
            if (dropDown != null)
                if (dropDown.SelectedIndex != -1)
                    return dropDown.SelectedIndex;
            return 0;
        }
    }
}
