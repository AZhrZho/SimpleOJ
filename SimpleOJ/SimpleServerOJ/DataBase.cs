using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServerOJ
{
    class DataBase
    {
        /// <summary>
        /// 数据库名
        /// </summary>
        private static string Name { get; set; } = "simpleoj";
        /// <summary>
        /// 数据库地址
        /// </summary>
        private static string Host { get; set; } = "127.0.0.1";
        /// <summary>
        /// 数据库端口
        /// </summary>
        private static int Port { get; set; } = 3306;
        /// <summary>
        /// 用户名
        /// </summary>
        private static string UserName { get; set; } = "root";
        /// <summary>
        /// 密码
        /// </summary>
        private static string PassWord { get; set; } = "";

        private static string ConnStr { get; set; }

        public static MySqlConnection GetConnection()
        {
            if (ConnStr == null) throw new Exception("数据库未初始化");
            else return new MySqlConnection(ConnStr);
        }

        public static void Initialise(string name, string host, int port, string user, string password)
        {
            ConnStr = string.Format(
                "Database = {0}; datasource = {1}; port = {2}; user={3};pwd={4};SslMode = none; charset=utf8;"
                , name, host, port, user, password);
        }

        public static void Initialise()
        {
            if (Name == null || Host == null || UserName == null || PassWord == null) return;
            Initialise(Name, Host, Port, UserName, PassWord);
        }
    }
}
