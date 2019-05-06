using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SimpleServerOJ.Application
{
    class BaseModel
    {
        /// <summary>
        /// 数据表名
        /// </summary>
        protected virtual string TableName { get; }
        /// <summary>
        /// 判断数据表中是否存在指定数据
        /// </summary>
        /// <param name="limit">查找条件</param>
        /// <returns></returns>
        public bool Find(Dictionary<string, object> limit)
        {
            var conn = DataBase.GetConnection();
            conn.Open();
            var findstr = string.Format("select * from {0} where", TableName);
            StringBuilder sb = new StringBuilder(findstr);
            foreach (var kv in limit)
            {
                var key = kv.Key;
                if ((!"<>=".Contains(key.TrimEnd(' ').Last())) && (!key.ToUpper().Contains(" LIKE"))) key += '=';
                sb.AppendFormat(" {0}'{1}' and", key, kv.Value);
            }
            sb.Remove(sb.Length - 3, 3);
            findstr = sb.ToString();
            MySqlCommand cmd = new MySqlCommand(findstr, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            var result = false;
            if (reader.Read()) result = true;
            conn.Close();
            return result;
        }
        /// <summary>
        /// 判断数据表中是否存在指定数据
        /// </summary>
        /// <param name="key">查找键</param>
        /// <param name="value">查找值</param>
        /// <returns></returns>
        public bool Find(string key, string value)
        {
            var conn = DataBase.GetConnection();
            conn.Open();
            var findstr = string.Format("select * from {0} where {1}='{2}'", TableName, key, value);
            MySqlCommand cmd = new MySqlCommand(findstr, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            var result = false;
            if (reader.Read()) result = true;
            conn.Close();
            return result;
        }
        /// <summary>
        /// 从数据表中获取指定数据
        /// </summary>
        /// <param name="key">查找键</param>
        /// <param name="value">查找值</param>
        /// <returns></returns>
        public List<Dictionary<string, object>> Select(string key, string value)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            var conn = DataBase.GetConnection();
            conn.Open();
            var restric = new string[4];
            restric[2] = TableName;
            var table = conn.GetSchema("Columns", restric);
            List<string> colums = new List<string>();
            foreach (DataRow r in table.Rows)
            {
                colums.Add(r["column_name"].ToString());
            }
            var findstr = string.Format("select * from {0} where {1}='{2}'", TableName, key, value);
            MySqlCommand cmd = new MySqlCommand(findstr, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Dictionary<string, object> dt = new Dictionary<string, object>();
                foreach (var cn in colums)
                {
                    dt.Add(cn, reader[cn]);
                }
                result.Add(dt);
            }
            reader.Dispose();
            cmd.Dispose();
            conn.Close();
            return result;
        }
        /// <summary>
        /// 从数据表中获取指定数据
        /// </summary>
        /// <param name="limit">查找条件</param>
        /// <returns></returns>
        public List<Dictionary<string, object>> Select(Dictionary<string, object> limit)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            var conn = DataBase.GetConnection();
            conn.Open();
            var restric = new string[4];
            restric[2] = TableName;
            var table = conn.GetSchema("Columns", restric);
            List<string> colums = new List<string>();
            foreach (DataRow r in table.Rows)
            {
                colums.Add(r["column_name"].ToString());
            }
            var udstr = string.Format("select * from {0} where", TableName);
            StringBuilder sb = new StringBuilder(udstr);
            foreach (var kv in limit)
            {
                var key = kv.Key;
                if ((!"<>=".Contains(key.TrimEnd(' ').Last())) && (!key.ToUpper().Contains(" LIKE"))) key += '=';
                sb.AppendFormat(" {0}'{1}' and", key, kv.Value);
            }
            sb.Remove(sb.Length - 3, 3);
            udstr = sb.ToString();
            MySqlCommand cmd = new MySqlCommand(udstr, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Dictionary<string, object> dt = new Dictionary<string, object>();
                foreach (var cn in colums)
                {
                    dt.Add(cn, reader[cn]);
                }
                result.Add(dt);
            }
            reader.Dispose();
            return result;
        }
        /// <summary>
        /// 将数据插入到数据表
        /// </summary>
        /// <param name="data">要插入的数据</param>
        /// <returns></returns>
        public bool Insert(Dictionary<string, object> data)
        {
            return InsertGetID(data) == -1 ? false : true;
        }
        /// <summary>
        /// 将数据插入到数据表，并得到当前自增数
        /// </summary>
        /// <param name="data">要插入的数据</param>
        /// <returns></returns>
        public long InsertGetID(Dictionary<string, object> data)
        {
            var connection = DataBase.GetConnection();
            connection.Open();
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into ");
            sb.Append(TableName + " set ");
            foreach (var c in data)
            {
                sb.Append(c.Key + "=");
                sb.Append("'" + c.Value + "' ,");
            }
            sb.Remove(sb.Length - 1, 1);
            MySqlCommand cmd = new MySqlCommand(sb.ToString(), connection);
            cmd.ExecuteNonQuery();
            long result = cmd.LastInsertedId;
            cmd.Dispose();
            connection.Close();
            return result;
        }
        /// <summary>
        /// 从数据表中删除符合条件的数据
        /// </summary>
        /// <param name="limit">查找条件</param>
        /// <returns>受影响的数据的数量</returns>
        public long Delete(Dictionary<string, object> limit)
        {
            //返回删除的数据条数
            var conn = DataBase.GetConnection();
            conn.Open();
            var delstr = string.Format("delect from {0} where", TableName);
            StringBuilder sb = new StringBuilder(delstr);
            foreach (var kv in limit)
            {
                var key = kv.Key;
                if ((!"<>=".Contains(key.TrimEnd(' ').Last())) && (!key.ToUpper().Contains(" LIKE"))) key += '=';
                sb.AppendFormat(" {0}'{1}' and", key, kv.Value);
            }
            sb.Remove(sb.Length - 3, 3);
            delstr = sb.ToString();
            MySqlCommand cmd = new MySqlCommand(delstr, conn);
            long result = cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
            return result;
        }
        /// <summary>
        /// 从数据表中删除符合条件的数据
        /// </summary>
        /// <param name="key">查找键</param>
        /// <param name="value">查找值</param>
        /// <returns>受影响的数据的数量</returns>
        public int Delete(string key, string value)
        {
            //返回删除的数据条数
            var conn = DataBase.GetConnection();
            conn.Open();
            var findstr = string.Format("delete from {0} where {1}='{2}'", TableName, key, value);
            MySqlCommand cmd = new MySqlCommand(findstr, conn);
            var result = cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
            return result;
        }
        /// <summary>
        /// 更新数据表中的数据
        /// </summary>
        /// <param name="key">查找键</param>
        /// <param name="value">查找值</param>
        /// <param name="newdata">新数据</param>
        /// <returns>受影响的数据数量</returns>
        public long Update(string key, string value, Dictionary<string, object> newdata)
        {
            string udstr = string.Format("update {0} set ", TableName);
            StringBuilder sb = new StringBuilder(udstr);
            foreach (var u in newdata)
            {
                sb.AppendFormat("{0} = '{1}',", u.Key, u.Value);
            }
            sb.Remove(sb.Length - 1, 1);
            sb.AppendFormat(" where {0} = '{1}'", key, value);
            udstr = sb.ToString();
            var conn = DataBase.GetConnection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(udstr, conn);
            long result = cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
            return result;
        }
        /// <summary>
        /// 更新数据表中的数据
        /// </summary>
        /// <param name="limit">查找条件</param>
        /// <param name="newdata">新数据</param>
        /// <returns>受影响的数据的数量</returns>
        public long Update(Dictionary<string, object> limit, Dictionary<string, object> newdata)
        {
            string udstr = string.Format("update {0} set ", TableName);
            StringBuilder sb = new StringBuilder(udstr);
            foreach (var u in newdata)
            {
                sb.AppendFormat("{0} = '{1}',", u.Key, u.Value);
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(" where ");
            foreach (var kv in limit)
            {
                var key = kv.Key;
                if ((!"<>=".Contains(key.TrimEnd(' ').Last())) && (!key.ToUpper().Contains(" LIKE"))) key += '=';
                sb.AppendFormat(" {0}'{1}' and", key, kv.Value);
            }
            sb.Remove(sb.Length - 3, 3);
            udstr = sb.ToString();
            var conn = DataBase.GetConnection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(udstr, conn);
            long result = cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
            return result;
        }
    }
}
