using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSBL.DBModels
{
    public class tblCurriculum
    {
        public int CurriculumId { get; set; }
        public string CurriculumTitle { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int TenantId { get; set; }

        public virtual ICollection<tblCurriculumActivity> TblCurriculumActivity { get; set; }
    }
}