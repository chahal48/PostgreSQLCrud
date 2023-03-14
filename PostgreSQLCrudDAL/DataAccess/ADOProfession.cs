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
    /// Implementation of IProfession abstract methods
    /// </summary>
    public partial class NpgSQLProfession
    {
        /// <summary>
        /// Call stored procedure to get list of profession
        /// </summary>
        /// <returns></returns>
        public List<ProfessionEntity> GetAllProfession()
        {
            List<ProfessionEntity> ListProfession = new List<ProfessionEntity>();
            using (ADOExecution exec = new ADOExecution(_connection.SQLString))
            {
                using (IDataReader dr = exec.ExecuteReader(CommandType.Text, "select * from udf_GetAllProfession();"))
                {
                    while (dr.Read())
                    {
                        ListProfession.Add(new ProfessionEntity
                        {
                            ProfessionID = Convert.ToInt32(dr["_id"]),
                            Profession = Convert.ToString(dr["_profession"]),
                            Description = Convert.ToString(dr["_description"]),
                            LastModified = Convert.ToDateTime(dr["_lastmodified"])
                        });
                    }
                }
            }
            return ListProfession;
        }
        /// <summary>
        /// Call stored procedure to get profession details by Id
        /// </summary>
        /// <returns></returns>
        public ProfessionEntity GetProfessionById(int id)
        {
            ProfessionEntity professionEntity = new ProfessionEntity();
            using (ADOExecution exec = new ADOExecution(_connection.SQLString))
            {
                using (IDataReader dr = exec.ExecuteReader(CommandType.Text, "select * from udf_getprofessionbyid(:_pId);",
                    new NpgsqlParameter("_pId", id)))
                {
                    while (dr.Read())
                    {
                        professionEntity = new ProfessionEntity
                        {
                            ProfessionID = Convert.ToInt32(dr["_id"]),
                            Profession = Convert.ToString(dr["_profession"]),
                            Description = Convert.ToString(dr["_description"])
                        };
                    }
                }
            }
            return professionEntity;
        }
        /// <summary>
        /// Call stored procedure to add new profession and its details
        /// </summary>
        /// <param name="professionEntity"></param>
        /// <returns></returns>
        public bool AddProfession(ProfessionEntity professionEntity)
        {
            using (ADOExecution exec = new ADOExecution(_connection.SQLString))
            {
                var obj = exec.ExecuteScalar(CommandType.Text, "select udf_addpofession(:_profession,:_description);",
                    new NpgsqlParameter("_profession", professionEntity.Profession),
                    new NpgsqlParameter("_description", professionEntity.Description));

                return ReturnBool(obj);
            }
        }
        /// <summary>
        /// Call stored procedure to update contact details
        /// </summary>
        /// <param name="professionEntity"></param>
        /// <returns></returns>
        public bool UpdateProfession(ProfessionEntity professionEntity)
        {
            using (ADOExecution exec = new ADOExecution(_connection.SQLString))
            {
                var obj = exec.ExecuteScalar(CommandType.Text, "select udf_updateprofession(:_pId,:_profession,:_description);",
                    new NpgsqlParameter("_pId", professionEntity.ProfessionID),
                    new NpgsqlParameter("_profession", professionEntity.Profession),
                    new NpgsqlParameter("_description", professionEntity.Description));

                return ReturnBool(obj);
            }
        }
        /// <summary>
        /// Call stored procedure to delete profession details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteProfession(int id)
        {
            using (ADOExecution exec = new ADOExecution(_connection.SQLString))
            {
                var obj = exec.ExecuteScalar(CommandType.Text, "select udf_deleteprofession(:_pId);",
                    new NpgsqlParameter("_pId", id));

                return ReturnBool(obj);
            }
        }
        /// <summary>
        /// Call stored procedure to check profession existence
        /// </summary>
        /// <param name="profession"></param>
        /// <returns></returns>
        public bool CheckProfessionAlreadyExists(string profession)
        {
            int Result = 1;
            using (ADOExecution exec = new ADOExecution(_connection.SQLString))
            {
                var obj = exec.ExecuteScalar(CommandType.Text, "select udf_checkprofessionalreadyexists(:_profession);",
                    new NpgsqlParameter("_profession", profession));

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
