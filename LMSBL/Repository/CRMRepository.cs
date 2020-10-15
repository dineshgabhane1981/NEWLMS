using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMSBL.Common;
using LMSBL.DBModels;
using LMSBL.DBModels.CRMNew;
using System.Data.Entity.Migrations;

namespace LMSBL.Repository
{
    public class CRMRepository
    {
        DataRepository db = new DataRepository();
        Exceptions newException = new Exceptions();
        public List<tblCRMClient> GetClientById(int ClientId)
        {
            tblCRMClient objCRMClient = new tblCRMClient();
            try
            {
                db.parameters.Clear();
                db.AddParameter("@ClientID", SqlDbType.Int, ClientId);
                DataSet ds = db.FillData("sp_CRMClientsGetById");
                List<tblCRMClient> clientDetails = ds.Tables[0].AsEnumerable().Select(dr => new tblCRMClient
                {
                    ClientID = Convert.ToInt32(dr["ClientID"]),
                    ClientName = Convert.ToString(dr["ClientName"]),
                    ClientLogo = Convert.ToString(dr["ClientLogo"]),                    
                    IsActive = Convert.ToBoolean(dr["IsActive"]),                    
                    CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                    isLMS = Convert.ToBoolean(dr["isLMS"])
                    //TenantId = Convert.ToString(dr["TenantId"])
                    //Duration = Convert.ToInt32(dr["Duration"])


                }).ToList();
                return clientDetails;
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
        }

        public bool UpdateCRMClient(int ClientId,string clientlogo)
        {
            bool result = false;
            using (var context = new CRMContext())
            {
                var objcrmclient = context.tblCRMClients.FirstOrDefault(a => a.ClientID == ClientId);
                objcrmclient.ClientLogo = clientlogo;
                context.tblCRMClients.AddOrUpdate(objcrmclient);
                context.SaveChanges();
                result = true;
            }
            return result;

        }
    }
}
