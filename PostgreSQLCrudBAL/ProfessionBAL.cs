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
    public class ProfessionBAL
    {
        IProfession _iProfession;
        /// <summary>
        /// injected SQLProfession class
        /// </summary>
        /// <param name="iProfession"></param>
        public ProfessionBAL(IProfession iProfession)
        {
            _iProfession = iProfession;
        }
        public List<ProfessionEntity> GetAllProfession()
        {
            return _iProfession.GetAllProfession();
        }
        public ProfessionEntity GetProfessionById(int id)
        {
            return _iProfession.GetProfessionById(id);
        }
        public bool AddProfession(ProfessionEntity obj)
        {
            return _iProfession.AddProfession(obj);
        }
        public bool DeleteProfession(int id)
        {
            return _iProfession.DeleteProfession(id);
        }
        public bool UpdateProfession(ProfessionEntity obj)
        {
            return _iProfession.UpdateProfession(obj);
        }
        public bool CheckProfessionAlreadyExists(string profession)
        {
            return _iProfession.CheckProfessionAlreadyExists(profession);
        }
    }
}
