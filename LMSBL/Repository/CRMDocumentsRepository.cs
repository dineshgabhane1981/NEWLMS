using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMSBL.Common;
using LMSBL.DBModels.CRMNew;
using LMSBL.DBModels;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Web.Script.Serialization;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon;
using System.Configuration;
using System.Collections.Specialized;
using Amazon.S3.IO;

namespace LMSBL.Repository
{
    public class CRMDocumentsRepository
    {
        DataRepository db = new DataRepository();
        Exceptions newException = new Exceptions();

        string AWSAccessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
        string AWSSecretKey = ConfigurationManager.AppSettings["AWSSecretKey"];
        string AWSBucketName = ConfigurationManager.AppSettings["AWSBucketName"];

        string abc = string.Empty;       
        public bool AddEnquiryDocument(int ClientId, string fileBase64String, string fileName)
        {
            bool result = false;
            try
            {

                IAmazonS3 client = new AmazonS3Client(AWSAccessKey, AWSSecretKey, RegionEndpoint.EUWest3);
                TransferUtility utility = new TransferUtility(client);
                TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();
                var clientName = GetUserFolderName(ClientId);
                string path = @"clients/" + clientName;

                S3DirectoryInfo di = new S3DirectoryInfo(client, AWSBucketName, path);
                if (!di.Exists)
                {
                    di.Create();
                }


                request.BucketName = AWSBucketName;
                request.Key = path + "/" + fileName;

                byte[] newBytes = Convert.FromBase64String(fileBase64String);
                MemoryStream ms = new MemoryStream(newBytes, 0, newBytes.Length);
                ms.Write(newBytes, 0, newBytes.Length);
                request.InputStream = ms;
                utility.Upload(request);
                using (var context = new CRMContext())
                {
                    tblCRMDocument objCRMDocument = new tblCRMDocument();
                    objCRMDocument.ClientId = ClientId;
                    objCRMDocument.DocumentName = fileName;
                    objCRMDocument.UpdatedDate = DateTime.Now;
                    objCRMDocument.UpdatedBy = ClientId;
                    context.tblCRMDocuments.Add(objCRMDocument);
                    context.SaveChanges();
                }

            }
            catch (Exception ex)
            {

                newException.AddException(ex);
            }

            return result;
        }

        public string GetUserFolderName(int UserId)
        {
            string name = string.Empty;
            try
            {
                using (var context = new CRMContext())
                {
                    var lstResult = (from a in context.tblCRMUsers
                                     join b in context.tblCRMClients on a.ClientId equals b.ClientID
                                     where a.Id == UserId
                                     select new
                                     {
                                         AdviserCompanyName = b.ClientID + "/" + a.Id

                                     }).Select(x => x.AdviserCompanyName);
                    var result = lstResult.FirstOrDefault();
                    name = result;
                }
            }
            catch (Exception ex)
            {

                newException.AddException(ex);
            }
            return name;
        }
    }
}
