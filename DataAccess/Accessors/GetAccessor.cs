using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DataAccess.Accessors
{
    internal class GetAccessor
    {

        internal static string Download(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;

            WebRequest webRequest   = WebRequest.Create(url);
            WebResponse webResponse = webRequest.GetResponse();

            using (var webStream = webResponse.GetResponseStream())
            using (var inStream  = new StreamReader(webStream))
            {
                return inStream.ReadToEnd(); 
            }
        }
    }
}
