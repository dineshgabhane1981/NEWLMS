using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSBL.DBModels
{
    public class TblEnquiry
    {
        public int Id { get; set; }

        public string FullName { get; set; }
        public string EmailId { get; set; }
        public string ContactNo { get; set; }
        public string CourseName { get; set; }
        public string Country { get; set; }
        public Boolean isShared { get; set; }
    }
}
