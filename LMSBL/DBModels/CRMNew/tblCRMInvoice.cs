namespace LMSBL.DBModels.CRMNew
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblCRMInvoice
    {
        [Key]
        public int InvoiceId { get; set; }

        public int ClientId { get; set; }

        [StringLength(50)]
        public string InvoiceNumber { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public DateTime? InvoiceDueDate { get; set; }

        [StringLength(250)]
        public string Reference { get; set; }

        [StringLength(50)]
        public string GSTNumber { get; set; }

        [StringLength(50)]
        public string InvoiceCurrency { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SubTotal { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? GSTRate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? InvoiceTotal { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public bool? IsAdvance { get; set; }
    }
}
