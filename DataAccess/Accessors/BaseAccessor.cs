using System.IO;
using System.Net;
using System.Text;

namespace DataAccess
{
    internal class PostAccessor
    {

        internal string Result { get; set; }


        internal void Access(string link, string query)
        {
            WebRequest request = CreateRequest(link, query.Length);
            SendRequest(request, query);
            WebResponse response = request.GetResponse();
            Result = ReadResponse(response);

            response.Close();
        }


        private WebRequest CreateRequest(string link, int queryLength)
        {
            WebRequest request = WebRequest.Create(link);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = queryLength;

            return request;
        }

        private static void SendRequest(WebRequest request, string query)
        {
            byte[] byteQuery = Encoding.UTF8.GetBytes(query);
            Stream dataStream = request.GetRequestStream();

            dataStream.Write(byteQuery, 0, byteQuery.Length);
            dataStream.Close();
        }

        private static string ReadResponse(WebResponse respond)
        {
            using (Stream dataStream = respond?.GetResponseStream())
            using (StreamReader reader = new StreamReader(dataStream)) 
            { 
                return reader.ReadToEnd();
            }
        }


    }
}
