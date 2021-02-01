using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Auto2FADisabler
{
    public class DiscordThings
    {
        [JsonProperty("code")]
        public string code { get; set; }

    }

    class Program
    {
        public static string Get2FACode(string Token, string password, string UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36")
        {
            var Req = (HttpWebRequest)WebRequest.Create($"https://discord.com/api/v8/users/@me/mfa/codes");

            var postData = Properties.Text.password.Replace("my1337pass", password);

            var data = Encoding.ASCII.GetBytes(postData);

            Req.Method = "POST";
            Req.UserAgent = UserAgent;
            Req.ContentType = "application/json";
            Req.Headers.Add("authorization", Token);
            Req.ContentLength = data.Length;

            using (var stream = Req.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)Req.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString;
        }




        public static string Disable2FA(string Token, string code, string UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36")
        {
            var Req = (HttpWebRequest)WebRequest.Create($"https://discord.com/api/v8/users/@me/mfa/totp/disable");

            var postData = Properties.Text.DisableMFA.Replace("urcode", code);

            var data = Encoding.ASCII.GetBytes(postData);

            Req.Method = "POST";
            Req.UserAgent = UserAgent;
            Req.ContentType = "application/json";
            Req.Headers.Add("authorization", Token);
            Req.ContentLength = data.Length;

            using (var stream = Req.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)Req.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString;
        }

        static void Main(string[] args)
        {


            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Coded by https://github.com/HS-Dev-org/ organisation, have fun :)");
            Thread.Sleep(2500);
            Console.Clear();
            Console.ResetColor();
            Console.Write("Token : "); string token = Console.ReadLine(); Console.Clear(); Console.Write("Password : "); string password = Console.ReadLine();



            try
            {
                string _2faasjson = Get2FACode(token, password);
                dynamic deserialized = JObject.Parse(_2faasjson);


                string backupcode1 = deserialized.backup_codes[0].code;


                string mfadisablereturned = Disable2FA(token, backupcode1);


                dynamic newtoken = JObject.Parse(mfadisablereturned);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Succesfully disabled 2FA : " + newtoken.token);
            }
            catch (Exception)
            {

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("An error occured");
            }

            
            Thread.Sleep(-1);
        }
    }
}
