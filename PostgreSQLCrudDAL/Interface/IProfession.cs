using PostgreSQLCrudEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostgreSQLCrudDAL.Interface
{
    /// <summary>
    /// Abstract methods of SQLProfession class
    /// </summary>
    public interface IProfession
    {
        List<ProfessionEntity> GetAllProfession();
        ProfessionEntity GetProfessionById(int id);
        public bool CheckProfessionAlreadyExists(string profession);
        bool AddProfession(ProfessionEntity professionEntity);
        bool UpdateProfession(ProfessionEntity professionEntity);
        bool DeleteProfession(int id);
    }
}
