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
            foreach (string s in Manipulator.Manipulations.Keys)
            {
                comboBox1.Items.Add(s);
            }
            comboBox1.SelectedIndex = 0;
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
                }
                catch(Exception exception)
                {
                    MessageBox.Show("Unable to load file:\r\n" + exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            int chunkDrawingSize = (int)(600f / (Math.Max(this.world.Map.GetLength(0), this.world.Map.GetLength(1)) / 16));

            panel1.Size = new Size(this.world.Map.GetLength(0) * chunkDrawingSize / 16 + 5, this.world.Map.GetLength(1) * chunkDrawingSize / 16 + 5);
            this.world.Draw(e.Graphics, chunkDrawingSize);
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
            Manipulator.Manipulate(this.world, Manipulator.Manipulations[comboBox1.SelectedItem.ToString()]);
            MessageBox.Show("Manipulation done!");
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            button3.Enabled = Manipulator.Manipulations.Keys.Contains(comboBox1.Text);
        }
    }
}
