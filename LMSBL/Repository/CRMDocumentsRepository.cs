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
                    objCRMDocument.DocumentName = "CV";
                    objCRMDocument.DocumentLink = clientName + "/" + fileName;
                    objCRMDocument.DocumentfileName = fileName;
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

        public bool AddDocument(tblCRMDocument objCRMDocument, string fileBase64String, string FileName)
        {
            bool result = false;
            try
            {
                IAmazonS3 client = new AmazonS3Client(AWSAccessKey, AWSSecretKey, RegionEndpoint.EUWest3);
                TransferUtility utility = new TransferUtility(client);
                TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();
                var clientName = GetUserFolderName(objCRMDocument.ClientId);
                string path = @"clients/" + clientName;

                S3DirectoryInfo di = new S3DirectoryInfo(client, AWSBucketName, path);
                if (!di.Exists)
                {
                    di.Create();
                }
                request.BucketName = AWSBucketName;
                request.Key = path + "/" + FileName;

                byte[] newBytes = Convert.FromBase64String(fileBase64String);
                MemoryStream ms = new MemoryStream(newBytes, 0, newBytes.Length);
                ms.Write(newBytes, 0, newBytes.Length);
                request.InputStream = ms;
                utility.Upload(request);
                using (var context = new CRMContext())
                {
                    objCRMDocument.DocumentLink = clientName + "/" + FileName;
                    objCRMDocument.DocumentfileName = FileName;
                    context.tblCRMDocuments.Add(objCRMDocument);
                    context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
            }
            return result;
        }

        public List<tblCRMDocument> GetCRMDocumentList(int ClientId)
        {
            List<tblCRMDocument> lstDocument = new List<tblCRMDocument>();
            using (var context = new CRMContext())
            {
                lstDocument = context.tblCRMDocuments.Where(x => x.ClientId == ClientId).ToList();
            }
            return lstDocument;
        }

        public string DownloadFileFromS3(int id)
        {
            string returnLocation = string.Empty;
            try
            {

                using (var context = new CRMContext())
                {
                    var objDoc = context.tblCRMDocuments.FirstOrDefault(x => x.DocumentId == id);
                    var filelink = objDoc.DocumentLink;
                    string _FilePath = ConfigurationManager.AppSettings["SharedLocation"];
                    string FileLocation = _FilePath + "/" + objDoc.DocumentfileName;
                    FileStream fs = File.Create(FileLocation);
                    fs.Close();
                    string path = @"clients/" + filelink;
                    IAmazonS3 client = new AmazonS3Client(AWSAccessKey, AWSSecretKey, RegionEndpoint.EUWest3);
                    TransferUtility fileTransferUtility = new TransferUtility(client);
                    fileTransferUtility.Download(FileLocation, AWSBucketName, path);
                    fileTransferUtility.Dispose();
                    returnLocation = _FilePath;
                }
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
            }

            return returnLocation;
        }

        public string DownloadAllFileFromS3(int clientId)
        {
            string returnLocation = string.Empty;
            try
            {

                using (var context = new CRMContext())
                {
                    string _FilePath = ConfigurationManager.AppSettings["SharedLocation"];
                    var objUser = context.tblCRMUsers.FirstOrDefault(item => item.Id == clientId);
                    var FolderName = objUser.FirstName + "_" + objUser.LastName;
                    var objDoc = context.tblCRMDocuments.Where(x => x.ClientId == clientId && x.IsDocumentLocation!=true).ToList();
                    if (!Directory.Exists(_FilePath + "/" + FolderName))
                    {
                        Directory.CreateDirectory(_FilePath + "/" + FolderName);
                    }
                    else
                    {
                        Directory.Delete(_FilePath + "/" + FolderName,true);
                        Directory.CreateDirectory(_FilePath + "/" + FolderName);
                    }
                    foreach (var docItem in objDoc)
                    {
                        var filelink = docItem.DocumentLink;
                        string FileLocation = _FilePath + "/"+ FolderName + "/" + docItem.DocumentfileName;
                        FileStream fs = File.Create(FileLocation);
                        fs.Close();
                        string path = @"clients/" + filelink;
                        IAmazonS3 client = new AmazonS3Client(AWSAccessKey, AWSSecretKey, RegionEndpoint.EUWest3);
                        TransferUtility fileTransferUtility = new TransferUtility(client);
                        fileTransferUtility.Download(FileLocation, AWSBucketName, path);
                        fileTransferUtility.Dispose();

                    }
                    string startPath = _FilePath + "/" + FolderName;
                    string zipPath = _FilePath + "/" + FolderName + ".zip";
                    if (File.Exists(zipPath))
                    {
                        File.Delete(zipPath);
                    }
                    ZipFile.CreateFromDirectory(startPath, zipPath);
                    returnLocation = FolderName + ".zip";
                }
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
            }

            return returnLocation;
        }
        public bool SaveDocumentLink(int id, string link, int ClientId)
        {
            bool status = false;
            try
            {
                using (var context = new CRMContext())
                {
                    tblCRMDocument doc = new tblCRMDocument();
                    doc = context.tblCRMDocuments.FirstOrDefault(x => x.ClientId == id && x.IsDocumentLocation == true);
                    if (doc != null)
                    {
                        doc.DocumentLink = link;
                        doc.UpdatedDate = DateTime.Now;
                        context.tblCRMDocuments.AddOrUpdate(doc);
                        context.SaveChanges();
                    }
                    else
                    {
                        doc = new tblCRMDocument();
                        doc.ClientId = id;
                        doc.DocumentLink = link;
                        doc.DocumentfileName = link;
                        doc.UpdatedDate = DateTime.Now;
                        doc.UpdatedBy = ClientId;
                        doc.IsDocumentLocation = true;
                        context.tblCRMDocuments.Add(doc);
                        context.SaveChanges();
                    }
                    status = true;
                }
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
            }
            return status;
        }

        public string GetDocumentLink(int id)
        {
            string link = string.Empty;
            try
            {
                using (var context = new CRMContext())
                {
                    tblCRMDocument doc = new tblCRMDocument();
                    doc = context.tblCRMDocuments.FirstOrDefault(x => x.ClientId == id && x.IsDocumentLocation == true);
                    if (doc != null)
                    {
                        link = doc.DocumentLink;
                    }                    
                }
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
            }
            return link;
        }
    }
}
