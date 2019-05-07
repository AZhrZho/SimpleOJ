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
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            Application.Application.LoadApp(typeof(Application.Judge.JudgeController));
            Application.Application.LoadApp(typeof(Application.Register.RegisterController));
            Application.Application.LoadApp(typeof(Application.Login.LoginController));
            Application.Application.LoadApp(typeof(Application.GetProblems.Controller));
            Application.Application.LoadApp(typeof(Application.GetPosts.Controller));
            Data.Problems = Data.GetProblems();
            Console.WriteLine(Program.TimeLabel() + "加载题目列表...");
            Net.HttpServer server = new Net.HttpServer(8081);
            server.HttpGotRequest += Server_HttpGotRequest;
            server.Start();
            Console.WriteLine(Program.TimeLabel() + "SimpleOJ已启动.");
        }

        private Net.HttpResponseArgs Server_HttpGotRequest(Net.HttpArgs args)
        {
            return Application.Application.Handle(args);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.DataSource = Data.Problems;
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var problem = listBox1.SelectedItem as ProblemInfo;
            label1.Text = string.Empty;
            label1.Text += "题目名称：" + problem.Name + Environment.NewLine;
            label1.Text += "题目描述：" + problem.Discription + Environment.NewLine;
            label1.Text += "限制用时：" + problem.TimeLimit + Environment.NewLine;
            label1.Text += "样例输入：" + Environment.NewLine + problem.SampleInput + Environment.NewLine;
            label1.Text += "样例输出：" + Environment.NewLine + problem.SampleOutput + Environment.NewLine;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            new FrmAdd().ShowDialog();
            Data.Problems = Data.GetProblems();
            listBox1.DataSource = Data.Problems;
        }
    }
}
