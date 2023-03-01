using Npgsql;
using PostgreSQLCrudDAL.ADO;
using PostgreSQLCrudEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostgreSQLCrudDAL.DataAccess
{
    /// <summary>
    /// Implementation of IContact abstract methods
    /// </summary>
    public partial class SQLContact
    {
        /// <summary>
        /// Call stored procedure to get list of Contact
        /// </summary>
        /// <returns></returns>
        public List<ContactEntity> GetAllContact()
        {
            List<ContactEntity> ListContact = new List<ContactEntity>();
            using (ADOExecution exec = new ADOExecution(_connection.SQLString))
            {
                using (IDataReader dr = exec.ExecuteReader(CommandType.StoredProcedure, "usp_GetAllContact"))
                {
                    while (dr.Read())
                    {
                        ListContact.Add(new ContactEntity
                        {
                            ContactID = Convert.ToInt32(dr["ContactId"]),
                            fName = Convert.ToString(dr["FirstName"]),
                            lName = Convert.ToString(dr["LastName"]),
                            emailAddr = Convert.ToString(dr["EmailAddress"]),
                            Company = Convert.ToString(dr["Company"]),
                            Category = (Category)Convert.ToInt32(dr["Category"]),
                            Profession = Convert.ToString(dr["Profession"]),
                            ProfessionID = Convert.ToInt32(dr["ProfessionId"]),
                            Gender = (Gender)Convert.ToInt32(dr["Gender"]),
                            DOB = (DateTime)dr["DOB"],
                            ModeSlack = Convert.ToBoolean(dr["ModeSlack"]),
                            ModeWhatsapp = Convert.ToBoolean(dr["ModeWhatsapp"]),
                            ModePhone = Convert.ToBoolean(dr["ModePhone"]),
                            ModeEmail = Convert.ToBoolean(dr["ModeEmail"]),
                            ContactImage = Convert.ToString(dr["ContactImage"]),
                            LastModified = Convert.ToDateTime(dr["LastModified"])
                        });
                    }
                }
            }
            return ListContact;
        }
        /// <summary>
        /// Call stored procedure to get contact details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContactEntity GetContactByID(int id)
        {
            ContactEntity contactEntity = new ContactEntity();
            using (ADOExecution exec = new ADOExecution(_connection.SQLString))
            {
                using (IDataReader dr = exec.ExecuteReader(CommandType.StoredProcedure, "usp_GetContactById",
                    new NpgsqlParameter("@ContactId", id)))
                {
                    while (dr.Read())
                    {
                        contactEntity = new ContactEntity
                        {
                            ContactID = Convert.ToInt32(dr["ContactId"]),
                            fName = Convert.ToString(dr["FirstName"]),
                            lName = Convert.ToString(dr["LastName"]),
                            emailAddr = Convert.ToString(dr["EmailAddress"]),
                            Company = Convert.ToString(dr["Company"]),
                            Category = (Category)Convert.ToInt32(dr["Category"]),
                            Profession = Convert.ToString(dr["Profession"]),
                            ProfessionID = Convert.ToInt32(dr["ProfessionId"]),
                            Gender = (Gender)Convert.ToInt32(dr["Gender"]),
                            DOB = (DateTime)dr["DOB"],
                            ModeSlack = Convert.ToBoolean(dr["ModeSlack"]),
                            ModeWhatsapp = Convert.ToBoolean(dr["ModeWhatsapp"]),
                            ModePhone = Convert.ToBoolean(dr["ModePhone"]),
                            ModeEmail = Convert.ToBoolean(dr["ModeEmail"]),
                            ContactImage = Convert.ToString(dr["ContactImage"])
                        };
                    }
                }
            }
            return contactEntity;
        }
        /// <summary>
        /// Call stored procedure to add new contact and its details
        /// </summary>
        /// <param name="contactEntity"></param>
        /// <returns></returns>
        public bool AddContact(ContactEntity contactEntity)
        {
            using (ADOExecution exec = new ADOExecution(_connection.SQLString))
            {
                int Result = exec.ExecuteNonQuery(CommandType.StoredProcedure, "usp_AddContact",
                    new NpgsqlParameter("@ProfessionId", contactEntity.ProfessionID),
                    new NpgsqlParameter("@FirstName", contactEntity.fName),
                    new NpgsqlParameter("@LastName", contactEntity.lName),
                    new NpgsqlParameter("@EmailAddress", contactEntity.emailAddr),
                    new NpgsqlParameter("@Company", contactEntity.Company),
                    new NpgsqlParameter("@Category", contactEntity.Category),
                    new NpgsqlParameter("@Gender", contactEntity.Gender),
                    new NpgsqlParameter("@DOB", contactEntity.DOB),
                    new NpgsqlParameter("@ModeSlack", contactEntity.ModeSlack),
                    new NpgsqlParameter("@ModeEmail", contactEntity.ModeEmail),
                    new NpgsqlParameter("@ModePhone", contactEntity.ModePhone),
                    new NpgsqlParameter("@ModeWhatsapp", contactEntity.ModeWhatsapp),
                    new NpgsqlParameter("@ContactImage", contactEntity.ContactImage));

                return ReturnBool(Result);
            }
        }
        /// <summary>
        /// Call stored procedure to update contact details
        /// </summary>
        /// <param name="contactEntity"></param>
        /// <returns></returns>
        public bool UpdateContact(ContactEntity contactEntity)
        {
            using (ADOExecution exec = new ADOExecution(_connection.SQLString))
            {
                int Result = exec.ExecuteNonQuery(CommandType.StoredProcedure, "usp_UpdateContact",
                    new NpgsqlParameter("@ContactId", contactEntity.ContactID),
                    new NpgsqlParameter("@ProfessionId", contactEntity.ProfessionID),
                    new NpgsqlParameter("@FirstName", contactEntity.fName),
                    new NpgsqlParameter("@LastName", contactEntity.lName),
                    new NpgsqlParameter("@EmailAddress", contactEntity.emailAddr),
                    new NpgsqlParameter("@Company", contactEntity.Company),
                    new NpgsqlParameter("@Category", contactEntity.Category),
                    new NpgsqlParameter("@Gender", contactEntity.Gender),
                    new NpgsqlParameter("@DOB", contactEntity.DOB),
                    new NpgsqlParameter("@ModeSlack", contactEntity.ModeSlack),
                    new NpgsqlParameter("@ModeEmail", contactEntity.ModeEmail),
                    new NpgsqlParameter("@ModePhone", contactEntity.ModePhone),
                    new NpgsqlParameter("@ModeWhatsapp", contactEntity.ModeWhatsapp),
                    new NpgsqlParameter("@ContactImage", contactEntity.ContactImage));

                return ReturnBool(Result);
            }
        }
        /// <summary>
        /// Call stored procedure to delete contact details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteContact(int id)
        {
            using (ADOExecution exec = new ADOExecution(_connection.SQLString))
            {
                int Result = exec.ExecuteNonQuery(CommandType.StoredProcedure, "usp_DeleteContact",
                    new NpgsqlParameter("@ContactId", id));

                return ReturnBool(Result);
            }
        }
        /// <summary>
        /// Call stored procedure to check email existence
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool CheckEmailAlreadyExists(string email)
        {
            int Result = 1;
            using (ADOExecution exec = new ADOExecution(_connection.SQLString))
            {
                var obj = exec.ExecuteScalar(CommandType.StoredProcedure, "usp_CheckEmailAlreadyExists",
                    new NpgsqlParameter("@Email", email));

                if (obj != null)
                {
                    Result = Convert.ToInt32(obj);
                }

                return Result == 0;
            }
        }
        /// <summary>
        /// procedure success/failure healper method
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool ReturnBool(int result)
        {
            return result > 0;
        }
    }
}
