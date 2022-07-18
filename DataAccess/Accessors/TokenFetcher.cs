using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DataStructures;

namespace DataAccess.Accessors
{
    class TokenFetcher
    {


        private const string LOGIN_LINK = "https://pr2hub.com/login.php";

        private readonly byte[] AES_KEY = { 85, 74, 47, 106, 110, 70, 42, 119, 82, 48, 113, 82, 75, 47, 100, 72 };
        private readonly byte[] AES_IV  = { 38, 99, 57, 42, 121, 42, 53, 112, 61, 49, 85, 78, 120, 47, 84, 114 };
        private readonly string LOGIN_CODE = "eisjI1dHWG4vVTAtNjB0Xw";

        public TokenInfo Result { get; private set; }


        internal TokenFetcher(string username, string password, string version)
        {
            Result = new TokenInfo();

            var content  = GetContent(username, password, version);
            var response = Post(LOGIN_LINK, content).Result;

            ParseResponse(response);
        }


        private Dictionary<string, string> GetContent(string username, string password, string version)
        {
            var json = JsonConvert.SerializeObject(new
            {
                remember   = true,
                build      = version,
                domain     = "cdn.jiggmin.com",
                login_id   = 0,
                user_pass  = password,
                user_name  = username,
                login_code = LOGIN_CODE,
                server = new
                {
                    population  = 0,
                    server_name = "Derron",
                    guild_id    = "0",
                    happy_hour  = 0,
                    address     = "45.76.24.255",
                    server_id   = "1",
                    port        = 9160,
                    status      = "open",
                    tournament  = "0"
                }
            });

            return new Dictionary<string, string>()
            {
                { "i",     GetLoginString(json) },
                { "build", version              },
                { "token", ""                   }
            };
        }

        private string GetLoginString(string json)
        {
            byte[] encrypted;

            // Create an Aes object with the specified key and iv.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key     = AES_KEY;
                aesAlg.IV      = AES_IV;
                aesAlg.Padding = PaddingMode.Zeros;
                aesAlg.Mode    = CipherMode.CBC;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(json);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }

            return Convert.ToBase64String(encrypted);
        }
       
        private async Task<string> Post(string url, Dictionary<string, string> values)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Referer", "https://pr2hub.com/");

            var content  = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(url, content).ConfigureAwait(false);
            var responseFromServer = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return responseFromServer;
        }

        private void ParseResponse(string jsonString)
        {
            if(string.IsNullOrWhiteSpace(jsonString))
                return;

            try
            {
                var json    = JObject.Parse(jsonString);
                var success = json?.GetValue("success")?.Value<bool>() ?? false;

                Result.Token    = json?.GetValue("token")?.Value<string>() ?? string.Empty;
                Result.Success  = success && !string.IsNullOrWhiteSpace(Result.Token);
                Result.ErrorMsg = success 
                                ? json?.GetValue("message")?.Value<string>() ?? string.Empty
                                : json?.GetValue("error")?.Value<string>()    ?? string.Empty;
            }
            catch (Exception ex)
            {
                Result.Success  = false;
                Result.ErrorMsg = ex.Message;
                return;
            }
        }


    }
}
