using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSBL.DBModels
{
    public class TblUserGroup
    {
        public int userid { get; set; }
        public int GroupId { get; set; }
        public virtual ICollection<TblUserGroup> tblUserGroup { get; set; }

        
    }
}
