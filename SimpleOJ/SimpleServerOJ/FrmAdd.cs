using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleServerOJ
{
    public partial class FrmAdd : Form
    {
        public FrmAdd()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Data.AddProblem(tb_in.Text, tb_out.Text, new ProblemInfo
            {
                Discription = tb_discription.Text,
                Name = tb_name.Text,
                SampleInput = tb_sample_in.Text,
                SampleOutput = tb_sample_out.Text,
                TimeLimit = Convert.ToInt32(tb_limit.Text)
            });
            Close();
        }
    }
}
