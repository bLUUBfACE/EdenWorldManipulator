using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eden_World_Manipulator
{
    public partial class Form1 : Form
    {
        World world;

        public Form1()
        {
            InitializeComponent();
            panel1.Size = new Size(176, 141);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.world = World.LoadWorld(openFileDialog1.FileName);

                    MessageBox.Show(this.world.Name + " loaded successfully!", "World loaded");
                    panel1.Visible = true;
                    panel1.Invalidate();

                    button2.Enabled = true;
                    groupBox1.Enabled = true;
                    groupBox2.Enabled = true;
                }
                catch(Exception exception)
                {
                    MessageBox.Show("Unable to load file:\r\n" + exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.Size = new Size(this.world.Map.GetLength(0) / 2 + 1, this.world.Map.GetLength(1) / 2 + 1);
            this.world.Draw(e.Graphics);
        }        

        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.world.SaveWorld(saveFileDialog1.FileName);
                    MessageBox.Show(this.world.Name + " saved succesfully!");
                }
                catch
                {
                    MessageBox.Show("Unable to save file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            this.world.Manipulate(Manipulator.BottomlessManipulation);
            MessageBox.Show("Manipulation done!");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.world.Manipulate(Manipulator.SphereCreation);
            MessageBox.Show("Creation done!");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.world.Manipulate(Manipulator.CylinderCreation);
            MessageBox.Show("Creation done!");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.world.Manipulate(Manipulator.NaturalManipulation);
            MessageBox.Show("Manipulation done!");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //this.world.Manipulate(Manipulator.SphericBlockTypeManipulation);
            //MessageBox.Show("Manipulation done!");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.world.Manipulate(Manipulator.OceanCreation);
            MessageBox.Show("Creation done!");
        }
    }
}
