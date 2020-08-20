using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSBL.DBModels
{
    public class TblGroup
    {
       
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string CreatedOn { get; set; }
        public int userid { get; set; }
        public List<OnlyUserId> userarr { get; set; }   
        public int NoOfUsers { get; set; }
        public int tenantId { get; set; }
        public virtual ICollection<TblGroup> tblgroup { get; set; }
    }

    
}
