using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostgreSQLCrudDAL.ADO
{
    class ADOExecution : IDisposable
    {
        #region NpgSQL helper class

        NpgsqlConnection _con;
        public ADOExecution(string connectionString)
        {
            _con = new NpgsqlConnection();
            _con.ConnectionString = connectionString;
            if (_con.State == System.Data.ConnectionState.Closed)
            {
                _con.Open();
            }

        }
        /// <summary>
        /// Execute Reader helper method.
        /// </summary>
        /// <param name="commandType">CommandType</param>
        /// <param name="commandText">string</param>
        /// <param name="parameters">params</param>
        /// <returns></returns>
        public IDataReader ExecuteReader(System.Data.CommandType commandType, string commandText, params IDataParameter[] parameters)
        {
            // Create and configure a new command.
            IDbCommand com = _con.CreateCommand();
            com.CommandType = commandType;
            com.CommandText = commandText;

            if (parameters != null)
            {
                foreach (IDataParameter parameter in parameters)
                {
                    com.Parameters.Add(parameter);
                }
            }

            return com.ExecuteReader();

        }

        /// <summary>
        /// Execute NonQuery helper menthod
        /// </summary>
        /// <param name="commandType">CommandType</param>
        /// <param name="commandText">string</param>
        /// <param name="parameters">params</param>
        public int ExecuteNonQuery(System.Data.CommandType commandType, string commandText, params IDataParameter[] parameters)
        {
            // Create and configure a new command.
            IDbCommand com = _con.CreateCommand();
            com.CommandType = commandType;
            com.CommandText = commandText;

            if (parameters != null)
            {
                foreach (IDataParameter parameter in parameters)
                {
                    com.Parameters.Add(parameter);
                }
            }

            return com.ExecuteNonQuery();
        }

        /// <summary>
        /// Execute Scalar helper method.
        /// </summary>
        /// <param name="commandType">CommandType</param>
        /// <param name="commandText">string</param>
        /// <param name="parameters">params</param>
        /// <returns>object</returns>
        public object ExecuteScalar(System.Data.CommandType commandType, string commandText, params IDataParameter[] parameters)
        {
            // Create and configure a new command.
            IDbCommand com = _con.CreateCommand();
            com.CommandType = commandType;
            com.CommandText = commandText;

            if (parameters != null)
            {
                foreach (IDataParameter parameter in parameters)
                {
                    com.Parameters.Add(parameter);
                }
            }

            return com.ExecuteScalar();
        }

        /// <summary>
        /// Idisposable member.
        /// </summary>
        public void Dispose()
        {
            if (_con != null)
            {
                _con.Close();
            }
        }

        #endregion
    }
}
