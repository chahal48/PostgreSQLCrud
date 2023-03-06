using Microsoft.Extensions.Options;
using PostgreSQLCrudDAL.Interface;
using PostgreSQLCrudDAL.Setting;

namespace PostgreSQLCrudDAL.DataAccess
{
    /// <summary>
    /// Used to seprate class internal methods and inheritance
    /// </summary>
    public partial class NpgSQLContact : IContact
    {
        private readonly ConnectionSetting _connection;

        /// <summary>
        /// Extend SQLContact class, to get connection string
        /// </summary>
        /// <param name="connection"></param>
        public NpgSQLContact(IOptions<ConnectionSetting> connection)
        {
            _connection = connection.Value;
        }
    }
}
