using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSBL.DBModels
{
  public  class tblTemplateAssign
    {
      public int TenantId { get; set; }
      public int TemplateId { get; set; }
      public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
