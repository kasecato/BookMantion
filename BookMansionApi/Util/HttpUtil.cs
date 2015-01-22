using BookMansionApi.Exceptions;
using BookMansionApi.Model.Http;
using BookMansionApi.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace BookMansionApi.Util
{
    public sealed class HttpUtil
    {
        #region > Public Method

        public static IAsyncOperation<HttpResponseEntity> PostAsync(Uri uri, string content)
        {
            return SendRequest(uri, HttpMethod.Post, content).AsAsyncOperation<HttpResponseEntity>();
        }

        public static IAsyncOperation<HttpResponseEntity> PostAsync(Uri uri, string username, string password, string content)
        {
            return SendRequest(uri, HttpMethod.Post, content, username, password).AsAsyncOperation<HttpResponseEntity>();
        }

        public static IAsyncOperation<HttpResponseEntity> GetAsync(Uri uri)
        {
            return SendRequest(uri, HttpMethod.Get).AsAsyncOperation<HttpResponseEntity>();
        }

        public static IAsyncOperation<HttpResponseEntity> GetAsync(Uri uri, string username, string password)
        {
            return SendRequest(uri, HttpMethod.Get, null, username, password).AsAsyncOperation<HttpResponseEntity>();
        }

        #endregion

        #region > Private Method

        private async static Task<HttpResponseEntity> SendRequest(Uri uri, HttpMethod method, string content = null, string username = null, string password = null)
        {
            /**********************************************************************
             * Validation
             **********************************************************************/
            if (!NetworkUtil.IsNetworkAvailable())
            {
                throw new HttpException(BookManExceptionType.Internet);
            }

            /**********************************************************************
             * Initialize
             **********************************************************************/
            HttpClientHandler handler = new HttpClientHandler()
            {
                Proxy = WebRequest.DefaultWebProxy,
                UseProxy = true,
                UseCookies = true,
                PreAuthenticate = false,
                AllowAutoRedirect = true,
                UseDefaultCredentials = false,
                AutomaticDecompression = System.Net.DecompressionMethods.GZip,
                MaxAutomaticRedirections = 50,
                MaxRequestContentBufferSize = 2147483647,
            };

            /**********************************************************************
             * Proxy
             **********************************************************************/
            bool isNeedProxyCredential = !String.IsNullOrEmpty(username) &&
                                         !String.IsNullOrEmpty(password);
            if (isNeedProxyCredential)
            {
                handler.Credentials = new NetworkCredential(username, password);
            }

            /**********************************************************************
             * Timeout
             **********************************************************************/
            HttpClient client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromMilliseconds(DefaultValue.Timeout),
            };

            /**********************************************************************
             * Make Request
             **********************************************************************/
            HttpRequestMessage request = new HttpRequestMessage()
            {
                Method = method,
                RequestUri = uri,
            };

            if (method == HttpMethod.Post)
            {
                request.Content = new StringContent(content, Encoding.UTF8);
            }

            /**********************************************************************
             * Send HTTP Request
             **********************************************************************/
            try
            {
                HttpResponseMessage response = await client.SendAsync(request);
                return await ToHttpResponseEntity(response, content);
            }
            catch (HttpRequestException ex)
            {
                return ToHttpResponseEntity(ex);
            }
            catch (TaskCanceledException)
            {
                throw new HttpException(BookManExceptionType.Timeout);
            }
        }

        private static async Task<HttpResponseEntity> ToHttpResponseEntity(HttpResponseMessage response, string content)
        {
            /**********************************************************************
             * Response Message
             **********************************************************************/
            HttpResponseEntity result = new HttpResponseEntity()
            {
                Version = response.Version.ToString(),
                StatusCode = (int)response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                IsSuccessStatusCode = response.IsSuccessStatusCode,
            };

            result.Headers = new Dictionary<string, IEnumerable<string>>();
            foreach (var row in response.Headers)
            {
                result.Headers.Add(row.Key, row.Value);
            }

            using (Stream responseStream = await response.Content.ReadAsStreamAsync())
            {
                Encoding charset = Encoding.GetEncoding(response.Content.Headers.ContentType.CharSet);
                result.Content = new StreamReader(responseStream, charset).ReadToEnd();
                result.ContentType = response.Content.Headers.ContentType.ToString();
                result.ContentLength = (response.Content.Headers.ContentLength != null) ? (long)response.Content.Headers.ContentLength : 0;
            }

            /**********************************************************************
             * Request Message
             **********************************************************************/
            result.RequestMessage = new HttpRequestEntity()
            {
                Method = response.RequestMessage.Method.ToString(),
                Version = response.RequestMessage.Version.ToString(),
                Properties = response.RequestMessage.Properties,
                RequestUri = response.RequestMessage.RequestUri,
            };

            result.RequestMessage.Headers = new Dictionary<string, IEnumerable<string>>();
            foreach (var row in response.RequestMessage.Headers)
            {
                result.RequestMessage.Headers.Add(row.Key, row.Value);
            }

            if (response.RequestMessage.Content != null)
            {
                result.RequestMessage.Content = content;
                result.RequestMessage.ContentType = response.RequestMessage.Content.Headers.ContentType.ToString();
                result.RequestMessage.ContentLength = (response.RequestMessage.Content.Headers.ContentLength != null) ? (long)response.RequestMessage.Content.Headers.ContentLength : 0;
            }

            return result;
        }

        private static HttpResponseEntity ToHttpResponseEntity(HttpRequestException exception)
        {
            WebException webexception = exception.InnerException as WebException;

            /**********************************************************************
             * Response Message
             **********************************************************************/
            HttpResponseEntity result = new HttpResponseEntity()
            {
                ReasonPhrase = webexception.Status.ToString()
            };

            if (webexception.Response == null)
            {
                throw new HttpException(BookManExceptionType.Dns);
            }

            result.ContentLength = webexception.Response.ContentLength;
            result.ContentType = webexception.Response.ContentType;
            result.Headers = new Dictionary<string, IEnumerable<string>>();
            foreach (string key in webexception.Response.Headers.AllKeys)
            {
                IEnumerable<string> value = new List<string>(1) { webexception.Response.Headers[key] };
                result.Headers.Add(key, value);
            }

            return result;
        }

        #endregion
    }
}
