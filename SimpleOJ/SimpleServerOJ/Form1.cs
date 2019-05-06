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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Application.Application.LoadApp(typeof(Application.Judge.JudgeController));
            Net.HttpServer server = new Net.HttpServer(8080);
            server.HttpGotRequest += Server_HttpGotRequest;
            server.Start();
        }

        private Net.HttpResponseArgs Server_HttpGotRequest(Net.HttpArgs args)
        {
            return Application.Application.Handle(args);
        }
    }
}
