using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSiS_Proj
{
    public partial class plotChoose : Form
    {
        public Form1 form;
        public plotChoose()
        {
            InitializeComponent();
            this.ControlBox = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CpuForm form = new CpuForm();
            
            
            form.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Ram ram = new Ram();
            ram.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            GpuForm gpuForm = new GpuForm();
            gpuForm.Show();
            this.Close();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            NetForm netForm = new NetForm();
            netForm.Show();
            this.Close();
        }
    }
}
