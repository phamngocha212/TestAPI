using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApplication4
{
    class Program
    {
        static void Main(string[] args)
        {
            string username = "108admin";
            string password = "67890@a";

            string pattern = "01GTKT0/002";
            string serial = "BV/19E";
            string fkey = "SG05320015880";
            //string invNo = "5";

            string[] listIdInv = new string[] { "1122", "1123" };
            string jsonData = string.Empty;
            string jsonDataImportAndPublish = string.Empty;
            string jsonDataAdjInv = string.Empty;
            string jsonDataReReplace = string.Empty;
            string jsonDataCanInv = string.Empty;
            string result = string.Empty;
            string xmlData = string.Empty;
            string xmlDataAdjInv = string.Empty;
            string xmlDataReplaceinv = string.Empty;
            string xmlCancelInv = string.Empty;
            string uri = "http://bv108admin.v-invoice.vn/";
                //"http://localhost:9108/";
            //"http://ixvadmin.v-invoice.vn/";
            // http://ekgisadmin.v-invoice.vn/
            try
            {
                xmlData = File.ReadAllText("C:\\Projects\\V-Invoice\\04. Customers\\04. 108\\API\\ImportAndPublish.xml");
                xmlDataAdjInv = File.ReadAllText("C:\\Projects\\V-Invoice\\04. Customers\\04. 108\\API\\AdjustInv.xml");
                xmlDataReplaceinv = File.ReadAllText("C:\\Projects\\V-Invoice\\04. Customers\\04. 108\\API\\ReplaceInv.xml");

                jsonDataImportAndPublish = String.Format("{{\"xmlData\":\"{0}\",\"username\":\"{1}\",\"password\":\"{2}\",\"pattern\":\"{3}\",\"serial\":\"{4}\",\"convert\":0}}", xmlData, username, password, pattern, serial);
                jsonDataAdjInv = String.Format("{{\"xmlData\":\"{0}\",\"username\":\"{1}\",\"password\":\"{2}\",\"pattern\":\"{3}\",\"fkey\":\"{4}\",\"convert\":0}}", xmlDataAdjInv, username, password, pattern, fkey);
                //jsonDataAdjInv = String.Format("{{\"xmlData\":\"{0}\",\"username\":\"{1}\",\"password\":\"{2}\",\"pattern\":\"{3}\",\"serial\":\"{4}\",\"fkey\":\"{5}\",\"invNo\":\"{6}\",\"convert\":0}}", xmlDataAdjInv, username, password, pattern, serial, fkey, invNo);
                jsonDataReReplace = String.Format("{{\"xmlData\":\"{0}\",\"username\":\"{1}\",\"password\":\"{2}\",\"pattern\":\"{3}\",\"fkey\":\"{4}\",\"convert\":0}}", xmlDataReplaceinv, username, password, pattern, fkey);
                //jsonDataReReplace = String.Format("{{\"xmlData\":\"{0}\",\"username\":\"{1}\",\"password\":\"{2}\",\"pattern\":\"{3}\",\"serial\":\"{4}\",\"fkey\":\"{5}\",\"invNo\":\"{6}\",\"convert\":0}}", xmlDataReplaceinv, username, password, pattern, serial, fkey, invNo);
                jsonDataCanInv = String.Format("{{\"username\":\"{0}\",\"password\":\"{1}\",\"pattern\":\"{2}\",\"fkey\":\"{3}\",\"convert\":0}}", username, password, pattern, fkey);
                
                //uri = uri + "api/business/cancelInv";
                uri = uri + "api/publish/ImportAndPublishInv";

                //1. Import and Publish Invoice
                result = CallApi(uri,
                    username,
                    password,
                    jsonDataImportAndPublish);

                Console.WriteLine(result);
                //Console.WriteLine(result);
                string s = "Hello api";
                Console.Write(s);

                //2. Update customer
                //xmlData = File.ReadAllText("C:\\Projects\\V-Invoice\\01. Documents\\05. Test API\\UpdateCus.xml");
                //jsonData = string.Format("{{\"xmlData\":\"{0}\",\"convert\":0}}", xmlData);
                //uri = uri + "api/Publish/UpdateCus";

                //result = CallApi(uri,
                //    username,
                //    password,
                //    jsonData);

                //var jsonData = String.Format("{{'cusCode':'{0}', 'fromDate':'{1}', 'toDate':'{2}'}}", "0985146892", "10/09/2016", "15/09/2016");
                //result = CallApi("http://viettel.v-invoice.vn/api/portal/listInvByCus",
                //    username,
                //    password,
                //    jsonData);

            }
            catch (Exception e)
            {
                throw e;
            }

            Console.ReadKey();
        }

        static string CallApi(string apiUri, string username, string password, string jsonData)
        {
            string URI = apiUri;

            WebClient client = new WebClient();
            client.Credentials = new NetworkCredential(username, password);
            client.Headers.Add("content-type", "application/json");
            DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - epochStart;
            string epoch = Convert.ToInt32(timeSpan.TotalSeconds).ToString();

            //Mã duy nhất
            string nonce = Guid.NewGuid().ToString("N").ToLower();

            //Tạo dữ liệu mã hóa
            string signatureRawData = String.Format("{0}{1}{2}", "POST", epoch, nonce);

            MD5 md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(signatureRawData));
            var signature = Convert.ToBase64String(hash);

            client.Encoding = System.Text.Encoding.UTF8;

            //Tạo dữ liệu Authentication
            string value = string.Format("{0}:{1}:{2}:{3}:{4}", signature, nonce, epoch, username, password);
            client.Headers.Add("Authentication", value);

            return client.UploadString(URI, jsonData);
        }

        private static void ShowMessageA(string message)
        {            
            Console.WriteLine(message + " hello!");
        }

        private static string GetMessage()
        {
            return "This is a message";
        }
    }
}
