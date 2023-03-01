using PostgreSQLCrudDAL.Interface;
using PostgreSQLCrudEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostgreSQLCrudBAL
{
    /// <summary>
    /// Business logic Class
    /// </summary>
    public class ContactBAL
    {
        IContact _iContact;
        /// <summary>
        /// injected SQLContact class
        /// </summary>
        /// <param name="iContact"></param>
        public ContactBAL(IContact iContact)
        {
            _iContact = iContact;
        }
        public List<ContactEntity> GetAllContact()
        {
            return _iContact.GetAllContact();
        }
        public ContactEntity GetContactByID(int id)
        {
            return _iContact.GetContactByID(id);
        }
        public bool AddContact(ContactEntity obj)
        {
            return _iContact.AddContact(obj);
        }
        public bool UpdateContact(ContactEntity obj)
        {
            return _iContact.UpdateContact(obj);
        }
        public bool DeleteContact(int id)
        {
            return _iContact.DeleteContact(id);
        }
        public bool CheckEmailAlreadyExists(string email)
        {
            return _iContact.CheckEmailAlreadyExists(email);
        }
    }
}
