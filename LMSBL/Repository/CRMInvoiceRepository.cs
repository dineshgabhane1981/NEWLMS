using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMSBL.Common;
using LMSBL.DBModels.CRMNew;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace LMSBL.Repository
{

    public class CRMInvoiceRepository
    {
        DataRepository db = new DataRepository();
        Exceptions newException = new Exceptions();

        public string GetInvoiceNumber(int ClientId)
        {
            string invoiceNo = string.Empty;
            using (var context = new CRMContext())
            {
                var result = context.tblCRMInvoices.Where(x => x.ClientId == ClientId).OrderByDescending(y => y.InvoiceNumber).FirstOrDefault();
                if (result != null)
                {
                    invoiceNo = result.InvoiceNumber;
                    var theNumber = invoiceNo.Substring(4, invoiceNo.Length - 4);
                    invoiceNo = "INV-" + (Convert.ToInt32(theNumber) + 1);
                }
                else
                {
                    invoiceNo = "INV-1";
                }
            }
            return invoiceNo;
        }

        public List<SelectListItem> GetCRMCurriencies()
        {
            List<SelectListItem> lstCRMCurriencies = new List<SelectListItem>();

            List<tblCRMCurrency> lstCRMCurrency = new List<tblCRMCurrency>();
            using (var context = new CRMContext())
            {
                lstCRMCurrency = context.tblCRMCurrencies.Select(a => a).ToList();
            }
            foreach (var currency in lstCRMCurrency)
            {
                lstCRMCurriencies.Add(new SelectListItem
                {
                    Text = Convert.ToString(currency.CurrencyName),
                    Value = Convert.ToString(currency.CurrencyName)
                });
            }

            return lstCRMCurriencies;
        }

        public bool SaveInvoice(tblCRMInvoice ObjCRMInvoivce, List<tblCRMInvoiceItem> ObjCRMInvoiceItemLST)
        {
            bool status = false;
            using (var context = new CRMContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.tblCRMInvoices.AddOrUpdate(ObjCRMInvoivce);
                        context.SaveChanges();

                        var oldItems = context.tblCRMInvoiceItems.Where(x => x.InvoiceId == ObjCRMInvoivce.InvoiceId).ToList();

                        foreach (var item in oldItems)
                        {
                            context.tblCRMInvoiceItems.Remove(item);
                            context.SaveChanges();
                        }

                        int count = 1;
                        foreach (var item in ObjCRMInvoiceItemLST)
                        {
                            item.InvoiceId = ObjCRMInvoivce.InvoiceId;
                            item.ItemId = count;
                            context.tblCRMInvoiceItems.AddOrUpdate(item);
                            context.SaveChanges();
                            count++;
                        }

                        transaction.Commit();
                        status = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        newException.AddException(ex);
                        throw ex;
                    }
                }
            }
            return status;
        }
        public List<tblCRMInvoice> GetInvoices(int ClientId)
        {
            List<tblCRMInvoice> ObjCRMInvoices = new List<tblCRMInvoice>();
            using (var context = new CRMContext())
            {
                ObjCRMInvoices = context.tblCRMInvoices.Where(x => x.ClientId == ClientId).ToList();
            }
            return ObjCRMInvoices;
        }

        public tblCRMInvoice GetInvoiceForEdit(int Id)
        {
            tblCRMInvoice ObjCRMInvoices = new tblCRMInvoice();
            using (var context = new CRMContext())
            {
                ObjCRMInvoices = context.tblCRMInvoices.FirstOrDefault(x => x.InvoiceId == Id);
            }
            return ObjCRMInvoices;
        }

        public List<tblCRMInvoiceItem> GetInvoiceItemForEdit(int Id)
        {
            List<tblCRMInvoiceItem> ObjCRMInvoiceItem = new List<tblCRMInvoiceItem>();
            using (var context = new CRMContext())
            {
                ObjCRMInvoiceItem = context.tblCRMInvoiceItems.Where(x => x.InvoiceId == Id).ToList();
            }
            return ObjCRMInvoiceItem;
        }

        public bool MarkPaidInvoice(int invoiceId, int updatedId)
        {
            bool status = false;
            try
            {
                using (var context = new CRMContext())
                {
                    tblCRMInvoice invoice = new tblCRMInvoice();
                    invoice = context.tblCRMInvoices.FirstOrDefault(x => x.InvoiceId == invoiceId);

                    if (invoice != null)
                    {
                        invoice.Status = "Paid";
                        invoice.UpdatedBy = updatedId;
                        invoice.UpdatedOn = DateTime.Now;
                        context.tblCRMInvoices.AddOrUpdate(invoice);
                        context.SaveChanges();
                        status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
            }
            return status;
        }
    }
}
