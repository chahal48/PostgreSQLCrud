using Microsoft.Extensions.Options;
using PostgreSQLCrudDAL.Interface;
using PostgreSQLCrudDAL.Setting;

namespace PostgreSQLCrudDAL.DataAccess
{
    /// <summary>
    /// Used to seprate class internal methods and inheritance
    /// </summary>
    public partial class NpgSQLProfession : IProfession
    {
        private readonly ConnectionSetting _connection;

        /// <summary>
        /// Extend SQLProfession class, to get connection string
        /// </summary>
        /// <param name="connection"></param>
        public NpgSQLProfession(IOptions<ConnectionSetting> connection)
        {
            _connection = connection.Value;
        }
    }
}
