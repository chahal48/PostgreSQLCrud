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
    public partial class SQLProfession
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
                using (IDataReader dr = exec.ExecuteReader(CommandType.StoredProcedure, "usp_GetAllProfession"))
                {
                    while (dr.Read())
                    {
                        ListProfession.Add(new ProfessionEntity
                        {
                            ProfessionID = Convert.ToInt32(dr["ProfessionId"]),
                            Profession = Convert.ToString(dr["Profession"]),
                            Description = Convert.ToString(dr["Description"]),
                            LastModified = Convert.ToDateTime(dr["LastModified"])
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
                using (IDataReader dr = exec.ExecuteReader(CommandType.StoredProcedure, "usp_GetProfessionById",
                    new NpgsqlParameter("@ProfessionId", id)))
                {
                    while (dr.Read())
                    {
                        professionEntity = new ProfessionEntity
                        {
                            ProfessionID = Convert.ToInt32(dr["ProfessionId"]),
                            Profession = Convert.ToString(dr["Profession"]),
                            Description = Convert.ToString(dr["Description"])
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
                int Result = exec.ExecuteNonQuery(CommandType.StoredProcedure, "usp_AddProfession",
                    new NpgsqlParameter("@Profession", professionEntity.Profession),
                    new NpgsqlParameter("@Description", professionEntity.Description));

                return ReturnBool(Result);
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
                int Result = exec.ExecuteNonQuery(CommandType.StoredProcedure, "usp_UpdateProfession",
                    new NpgsqlParameter("@ProfessionId", professionEntity.ProfessionID),
                    new NpgsqlParameter("@Profession", professionEntity.Profession),
                    new NpgsqlParameter("@Description", professionEntity.Description));

                return ReturnBool(Result);
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
                int Result = exec.ExecuteNonQuery(CommandType.StoredProcedure, "usp_DeleteProfession",
                    new NpgsqlParameter("@ProfessionId", id));

                return ReturnBool(Result);
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
                var obj = exec.ExecuteScalar(CommandType.StoredProcedure, "usp_CheckProfessionAlreadyExists",
                    new NpgsqlParameter("@Profession", profession));

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
