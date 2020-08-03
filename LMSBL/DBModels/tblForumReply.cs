using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSBL.DBModels
{
    public class tblForumReply
    {
        public int ForumId { get; set; }
        public int UserId { get; set; }
        public string ForumReply { get; set; }
        public DateTime CreatedDate { get; set; }

        public string UserName { get; set; }
        public string ProfileImage { get; set; }

        public List<tblForumReply> lstReply { get; set; }
    }
}
