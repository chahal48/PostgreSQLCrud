using Microsoft.Extensions.Options;
using PostgreSQLCrudDAL.Interface;
using PostgreSQLCrudDAL.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostgreSQLCrudDAL.DataAccess
{
    /// <summary>
    /// Used to seprate class internal methods and inheritance
    /// </summary>
    public partial class SQLProfession : IProfession
    {
        private readonly ConnectionSetting _connection;

        /// <summary>
        /// Extend SQLProfession class, to get connection string
        /// </summary>
        /// <param name="connection"></param>
        public SQLProfession(IOptions<ConnectionSetting> connection)
        {
            _connection = connection.Value;
        }
    }
}
