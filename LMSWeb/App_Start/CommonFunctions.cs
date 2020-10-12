using LMSBL.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace LMSWeb.App_Start
{
    public class CommonFunctions
    {
        Exceptions newException = new Exceptions();
        string key = "b14ca5898a4e4133bbce2ea2315a1916";
        public string GetEncodePassword(string password)
        {
            try
            {
                string encodePassword = string.Empty;
                MD5 md5 = new MD5CryptoServiceProvider();
                UTF8Encoding encoder = new UTF8Encoding();
                Byte[] originalBytes = encoder.GetBytes(password);
                Byte[] encodedBytes = md5.ComputeHash(originalBytes);
                password = BitConverter.ToString(encodedBytes).Replace("-", "");
                encodePassword = password.ToLower();
                return encodePassword;
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                return null;
            }
        }
        public string EncryptString(string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public string DecryptString(string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}