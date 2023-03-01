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
    public partial class SQLContact : IContact
    {
        private readonly ConnectionSetting _connection;

        /// <summary>
        /// Extend SQLContact class, to get connection string
        /// </summary>
        /// <param name="connection"></param>
        public SQLContact(IOptions<ConnectionSetting> connection)
        {
            _connection = connection.Value;
        }
    }
}
