using Microsoft.AspNetCore.Mvc;
using Npgsql;
using PostgreSQLCrudDAL.ADO;
using PostgreSQLCrudDAL.Interface;
using PostgreSQLCrudEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PostgreSQLCrudDAL.DataAccess
{
    /// <summary>
    /// Implementation of IContact abstract methods
    /// </summary>
    public partial class NpgSQLContact
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
                using (IDataReader dr = exec.ExecuteReader(CommandType.Text, "SELECT * from udf_getallcontact();"))
                {
                    while (dr.Read())
                    {
                        ListContact.Add(new ContactEntity
                        {
                            ContactID = Convert.ToInt32(dr["_id"]),
                            fName = Convert.ToString(dr["_firstname"]),
                            lName = Convert.ToString(dr["_lastname"]),
                            emailAddr = Convert.ToString(dr["_emailaddress"]),
                            Company = Convert.ToString(dr["_company"]),
                            Category = Convert.ToString(dr["_category"]),
                            Profession = Convert.ToString(dr["_profession"]),
                            ProfessionID = Convert.ToInt32(dr["_professionId"]),
                            Gender = Convert.ToString(dr["_gender"]),
                            DOB = (DateTime)dr["_dob"],
                            ModeSlack = Convert.ToBoolean(dr["_modeslack"]),
                            ModeWhatsapp = Convert.ToBoolean(dr["_modewhatsapp"]),
                            ModePhone = Convert.ToBoolean(dr["_modephone"]),
                            ModeEmail = Convert.ToBoolean(dr["_modeemail"]),
                            ContactImage = Convert.ToString(dr["_contactimage"]),
                            LastModified = Convert.ToDateTime(dr["_lastmodified"])
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
                using (IDataReader dr = exec.ExecuteReader(CommandType.Text, "SELECT * from udf_getcontactbyid(:_cid);",
                    new NpgsqlParameter("_cid", id)))
                {
                    while (dr.Read())
                    {
                        contactEntity = new ContactEntity
                        {
                            ContactID = Convert.ToInt32(dr["_id"]),
                            fName = Convert.ToString(dr["_firstname"]),
                            lName = Convert.ToString(dr["_lastname"]),
                            emailAddr = Convert.ToString(dr["_emailaddress"]),
                            Company = Convert.ToString(dr["_company"]),
                            Category = Convert.ToString(dr["_category"]),
                            Profession = Convert.ToString(dr["_profession"]),
                            ProfessionID = Convert.ToInt32(dr["_professionId"]),
                            Gender = Convert.ToString(dr["_gender"]),
                            DOB = (DateTime)dr["_dob"],
                            ModeSlack = Convert.ToBoolean(dr["_modeslack"]),
                            ModeWhatsapp = Convert.ToBoolean(dr["_modewhatsapp"]),
                            ModePhone = Convert.ToBoolean(dr["_modephone"]),
                            ModeEmail = Convert.ToBoolean(dr["_modeemail"]),
                            ContactImage = Convert.ToString(dr["_contactimage"]),
                            LastModified = Convert.ToDateTime(dr["_lastmodified"])
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
                var a = contactEntity.Category;
                var b = contactEntity.Gender;

                var obj = exec.ExecuteScalar(CommandType.Text, "select " +
                    "udf_insertcontact(:_pid,:_firstname,:_lastname,:_emailaddress,:_company,:_category,:_gender,:_dob," +
                    ":_modeslack,:_modewhatsapp,:_modeemail,:_modephone,:_contactimage)",
                    new NpgsqlParameter("_pid", contactEntity.ProfessionID),
                    new NpgsqlParameter("_firstname", contactEntity.fName),
                    new NpgsqlParameter("_lastname", contactEntity.lName),
                    new NpgsqlParameter("_emailaddress", contactEntity.emailAddr),
                    new NpgsqlParameter("_company", contactEntity.Company),
                    new NpgsqlParameter("_category", contactEntity.Category),
                    new NpgsqlParameter("_gender", contactEntity.Gender),
                    new NpgsqlParameter("_dob", contactEntity.DOB.Date),
                    new NpgsqlParameter("_modeslack", contactEntity.ModeSlack),
                    new NpgsqlParameter("_modeemail", contactEntity.ModeEmail),
                    new NpgsqlParameter("_modephone", contactEntity.ModePhone),
                    new NpgsqlParameter("_modewhatsapp", contactEntity.ModeWhatsapp),
                    new NpgsqlParameter("_contactimage", contactEntity.ContactImage));

                return ReturnBool(obj);
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
                var obj = exec.ExecuteScalar(CommandType.Text, "select " +
                    "udf_updatecontact(:_cid,:_pid,:_firstname,:_lastname,:_emailaddress,:_company,:_category,:_gender,:_dob," +
                    ":_modeslack,:_modewhatsapp,:_modeemail,:_modephone,:_contactimage)",
                    new NpgsqlParameter("_cid", contactEntity.ContactID),
                    new NpgsqlParameter("_pid", contactEntity.ProfessionID),
                    new NpgsqlParameter("_firstname", contactEntity.fName),
                    new NpgsqlParameter("_lastname", contactEntity.lName),
                    new NpgsqlParameter("_emailaddress", contactEntity.emailAddr),
                    new NpgsqlParameter("_company", contactEntity.Company),
                    new NpgsqlParameter("_category", contactEntity.Category),
                    new NpgsqlParameter("_gender", contactEntity.Gender),
                    new NpgsqlParameter( "_dob", contactEntity.DOB),
                    new NpgsqlParameter("_modeslack", contactEntity.ModeSlack),
                    new NpgsqlParameter("_modeemail", contactEntity.ModeEmail),
                    new NpgsqlParameter("_modephone", contactEntity.ModePhone),
                    new NpgsqlParameter("_modewhatsapp", contactEntity.ModeWhatsapp),
                    new NpgsqlParameter("_contactimage", contactEntity.ContactImage));

                return ReturnBool(obj);
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
                var obj = exec.ExecuteScalar(CommandType.Text, "select udf_deletecontact(:_cid);",
                    new NpgsqlParameter("_cid", id));

                return ReturnBool(obj);
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
                var obj = exec.ExecuteScalar(CommandType.Text, "select udf_checkemailalreadyexists(:_email);",
                    new NpgsqlParameter("_email", email));

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
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool ReturnBool(object obj)
        {
            int result = 0;
            if (obj != null)
            {
                result = Convert.ToInt32(obj);
            }
            return result > 0;
        }
    }
}
