using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSBL.DBModels.CRMNew
{
    public class EnquiryListing
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public DateTime CreatedDate { get; set; }
        public string IntrestedVisa { get; set; }        
    }
}
