using System;

namespace LMSBL.DBModels
{
    public class tblForum
    {
        public int ForumId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ForumType { get; set; }
        public int TenantId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Boolean IsBrodcast { get; set; }
        


    }
}
