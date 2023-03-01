using PostgreSQLCrudEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostgreSQLCrudDAL.Interface
{
    /// <summary>
    /// Abstract methods of SQLContact class
    /// </summary>
    public interface IContact
    {
        List<ContactEntity> GetAllContact();
        ContactEntity GetContactByID(int id);
        public bool CheckEmailAlreadyExists(string email);
        bool AddContact(ContactEntity contactEntity);
        bool UpdateContact(ContactEntity contactEntity);
        bool DeleteContact(int id);
    }
}
