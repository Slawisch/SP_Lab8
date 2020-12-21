using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SP_Lab8
{
    public partial class Form1 : Form
    {
        private RegistryKey hkColors;
        private Color currentColor = DefaultBackColor;
        private Color pickedColor = DefaultBackColor;
        private Color defaultColor = DefaultBackColor;
        public Form1()
        {
            InitializeComponent();
            
            RegistryKey hkCurrentUser = Registry.CurrentUser;
            RegistryKey hkControlPanel = hkCurrentUser.OpenSubKey("Control Panel");
            hkColors = hkControlPanel.OpenSubKey("Colors",true);
            object bgColor = hkColors.GetValue("Background");
            
            string[] currentColorStrings = bgColor.ToString().Split(' ');
            
            currentColor = Color.FromArgb(Int32.Parse(currentColorStrings[0]), Int32.Parse(currentColorStrings[1]),
                Int32.Parse(currentColorStrings[2]));
            pickedColor = currentColor;
            defaultColor = currentColor;

            panel2.BackColor = currentColor;
            panel1.BackColor = pickedColor;
            panel3.BackColor = defaultColor;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            hkColors.SetValue("Background", $"{pickedColor.R} {pickedColor.G} {pickedColor.B}");
            currentColor = pickedColor;
            panel2.BackColor = currentColor;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = pickedColor;
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            pickedColor = colorDialog1.Color;
            panel1.BackColor = pickedColor;
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            hkColors.SetValue("Background", $"{defaultColor.R} {defaultColor.G} {defaultColor.B}");
            currentColor = defaultColor;
            panel2.BackColor = currentColor;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("Apply changes?",
                "Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information) == DialogResult.No) {
                hkColors.SetValue("Background", $"{defaultColor.R} {defaultColor.G} {defaultColor.B}");
            }
        }
    }
}