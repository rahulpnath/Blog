using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using _500px.DataModel;

namespace _500px.Common
{
 public  class Oauth500px
    {
        private string consumerKey;

        private string consumerSecret;
        public bool IsAuthenticated { get; set; }
        private string OAuthAccessUrl;
        private string OAuthRequestUrl;
        private string OAuthAuthorizeUrl ;
        private string OAuthCallbackUrl;
        private string OAuthSignatureMethod;
        private string OAuthSignatureMethodName;
        private string OAuthVersion;
        private RequestType requestType;
        private Dictionary<string, string> AuthorizationParameters;
        private OauthToken Token;
        private string Data;

        public Oauth500px(string ConsumerKey,string ConsumerSecret,string CallbackUrl, string RequestUrl, string AuthorizeUrl, string AccessUrl)
        {
            consumerKey = ConsumerKey;
            consumerSecret = ConsumerSecret;
            OAuthAccessUrl = AccessUrl;
            OAuthAuthorizeUrl = AuthorizeUrl;
            OAuthRequestUrl = RequestUrl;
            OAuthCallbackUrl = CallbackUrl;
            OAuthSignatureMethod = "HMAC-SHA1";
            OAuthSignatureMethodName = "HMAC_SHA1";
            OAuthVersion = "1.0";
            requestType = RequestType.POST;
            Token = new OauthToken();
        }

      public class OauthToken
       {
           public string Token { get; set; }

           public string SecretCode { get; set; }

           public string Verifier { get; set; }
       }

      public enum RequestType
        {
            GET,
            POST,
            DELETE
        }

      public  class OauthParameter
        {
            public const string OauthCallback = "oauth_callback";
            public const string OauthConsumerKey = "oauth_consumer_key";
            public const string OauthNonce = "oauth_nonce";
            public const string OauthSignatureMethod = "oauth_signature_method";
            public const string OauthTimestamp = "oauth_timestamp";
            public const string OauthVersion = "oauth_version";
            public const string OauthSignature = "oauth_signature";
            public const string OauthToken = "oauth_token";
            public const string OauthVerifier = "oauth_verifier";
        }

        public Oauth500px MakeRequest(RequestType RequestType)
        {
            this.requestType = RequestType;
            return this;
        }

        private string Nonce()
        {
            return new Random().Next(1000000000).ToString();
        }

        private string TimeStamp()
        {
            return Math.Round((DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime()).TotalSeconds).ToString();
        }

     public async Task<bool> Authenticate()
     {
         try
         {
             await this.MakeRequest(Oauth500px.RequestType.POST).RequestToken();
             await this.MakeRequest(RequestType.POST).AuthorizeToken();
             await this.MakeRequest(RequestType.POST).AccessToken();
             return IsAuthenticated= true;
         }
         catch (Exception e)
         {
             return IsAuthenticated = false;
         }
         
     }
        public async Task<OauthToken> RequestToken()
        {
            AuthorizationParameters = new Dictionary<string, string>()
                                                                     {
                                                                         {
                                                                             OauthParameter.OauthCallback, OAuthCallbackUrl
                                                                         },
                                                                         {OauthParameter.OauthConsumerKey, consumerKey},
                                                                         {OauthParameter.OauthNonce, Nonce()},
                                                                         {
                                                                             OauthParameter.OauthSignatureMethod,
                                                                             OAuthSignatureMethod
                                                                         },
                                                                         {OauthParameter.OauthTimestamp, TimeStamp()},
                                                                         {OauthParameter.OauthVersion, OAuthVersion}
                                                                     };
            string response = await this.MakeRequest(RequestType.POST).Sign(OAuthRequestUrl,String.Empty).ExecuteRequest(OAuthRequestUrl);
            
            if (response != null)
            {
                String[] keyValPairs = response.Split('&');

                for (int i = 0; i < keyValPairs.Length; i++)
                {
                    String[] splits = keyValPairs[i].Split('=');
                    switch (splits[0])
                    {
                        case "oauth_token":
                            Token.Token = splits[1];
                            break;
                        case "oauth_token_secret":
                            Token.SecretCode = splits[1];
                            break;
                    }
                }
            }
            return Token;
        }

        public Oauth500px WithData(string data)
        {
            Data = data;
            return this;
        }
        public async Task<OauthToken> AuthorizeToken()
        {
            var tempAuthorizeUrl = OAuthAuthorizeUrl + "?oauth_token=" + Token.Token;

            System.Uri StartUri = new Uri(tempAuthorizeUrl);
            System.Uri EndUri = new Uri(OAuthCallbackUrl);

            var auth =
                await
                WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, StartUri, EndUri);

            var responseData = auth.ResponseData;
            responseData = responseData.Replace("http://www.bing.com/?", "");
            var split = responseData.Split('&');
            var keyValue = split[1].Split('=');
            Token.Verifier = keyValue[1];
            return Token;
        }

        public async Task<OauthToken> AccessToken()
     {
         AuthorizationParameters = new Dictionary<string, string>()
                                                                     {
                                                                         {OauthParameter.OauthConsumerKey, consumerKey},
                                                                         {OauthParameter.OauthNonce, Nonce()},
                                                                         {
                                                                             OauthParameter.OauthSignatureMethod,
                                                                             OAuthSignatureMethod
                                                                         },
                                                                         {OauthParameter.OauthTimestamp, TimeStamp()},
                                                                         {OauthParameter.OauthToken,Token.Token},
                                                                         {OauthParameter.OauthVerifier,Token.Verifier},
                                                                         {OauthParameter.OauthVersion, OAuthVersion}
                                                                     };
         var response = await this.MakeRequest(RequestType.POST).Sign(OAuthAccessUrl, Token.SecretCode).ExecuteRequest(OAuthAccessUrl);
       if (response != null)
       {
           String[] keyValPairs = response.Split('&');

           for (int i = 0; i < keyValPairs.Length; i++)
           {
               String[] splits = keyValPairs[i].Split('=');
               switch (splits[0])
               {
                   case "oauth_token":
                       Token.Token = splits[1];
                       break;
                   case "oauth_token_secret":
                       Token.SecretCode = splits[1];
                       break;
               }
           }
       }

       return Token;
     }

        private Oauth500px Sign(string Url, string tokenSecret)
        {
            String SigBaseStringParams = String.Join("&", AuthorizationParameters.Select(key => key.Key + "=" + Uri.EscapeDataString(key.Value)));
            String SigBaseString = requestType.ToString() + "&";
            SigBaseString += Uri.EscapeDataString(Url) + "&" + Uri.EscapeDataString(SigBaseStringParams);

            IBuffer KeyMaterial = CryptographicBuffer.ConvertStringToBinary(consumerSecret + "&" + tokenSecret, BinaryStringEncoding.Utf8);
            MacAlgorithmProvider HmacSha1Provider = MacAlgorithmProvider.OpenAlgorithm(OAuthSignatureMethodName);
            CryptographicKey MacKey = HmacSha1Provider.CreateKey(KeyMaterial);
            IBuffer DataToBeSigned = CryptographicBuffer.ConvertStringToBinary(SigBaseString, BinaryStringEncoding.Utf8);
            IBuffer SignatureBuffer = CryptographicEngine.Sign(MacKey, DataToBeSigned);
            String Signature = CryptographicBuffer.EncodeToBase64String(SignatureBuffer);
            AuthorizationParameters.Add(OauthParameter.OauthSignature, Signature);
            return this;
        }

        private async Task<String> ExecuteRequest(String Url)
        {
            string DataToPost = string.Empty;
            if (AuthorizationParameters != null)
            {
                 DataToPost = "OAuth " +
                                    String.Join(", ",
                                                AuthorizationParameters.Select(
                                                    key =>
                                                    key.Key +
                                                    (string.IsNullOrEmpty(key.Value)
                                                         ? string.Empty
                                                         : "=\"" + Uri.EscapeDataString(key.Value) + "\"")));
                AuthorizationParameters.Clear();
            }
            

            string  m_PostResponse = string.Empty;
            try
            {
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(Url);
                Request.Method = requestType.ToString();
                Request.Headers["Authorization"] = DataToPost;
                HttpWebResponse Response = (HttpWebResponse)await Request.GetResponseAsync();
                StreamReader ResponseDataStream = new StreamReader(Response.GetResponseStream());
                m_PostResponse = await ResponseDataStream.ReadToEndAsync();
            }
            catch (Exception Err)
            {

            }

            return m_PostResponse;
        }
      
        public async Task<T> ExecuteRequest<T>(string Url,Dictionary<string,string> Parameters) where T:class 
        {
            AuthorizationParameters = new Dictionary<string, string>()
                                          {
                                              {OauthParameter.OauthConsumerKey, consumerKey},
                                              {OauthParameter.OauthNonce, Nonce()},
                                              {OauthParameter.OauthSignatureMethod, OAuthSignatureMethod},
                                              {OauthParameter.OauthTimestamp, TimeStamp()},
                                              {OauthParameter.OauthToken, Token.Token},
                                              {OauthParameter.OauthVersion, OAuthVersion}
                                          };
                      
            string RequestUrl;
            if (Parameters != null && Parameters.Count > 0)
            {
                RequestUrl = Url + "?" +
                             String.Join("&",
                             Parameters.Select(a => a.Key + (string.IsNullOrEmpty(a.Value) ?string.Empty : "=" + Uri.EscapeDataString(a.Value))).ToArray());
                foreach (var parameter in Parameters)
                {
                    AuthorizationParameters.Add(parameter.Key,parameter.Value);
                }
            }
            else
                RequestUrl = Url;
            var response = await this.MakeRequest(requestType).Sign(Url,Token.SecretCode).ExecuteRequest(RequestUrl);
            if (string.IsNullOrEmpty(response))
                return null;
            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T));
            T dat = json.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(response))) as T;
            return dat;
        }

      public async Task<T> ExecuteNonAuthorizedRequest<T>(string Url,Dictionary<string,string> Parameters )  where T: class
      {
           string RequestUrl;
            if (Parameters != null && Parameters.Count > 0)
            {
                RequestUrl = Url + "?" +
                             String.Join("&",
                             Parameters.Select(a => a.Key + (string.IsNullOrEmpty(a.Value) ?string.Empty : "=" + Uri.EscapeDataString(a.Value))).ToArray());
                foreach (var parameter in Parameters)
                {
                    AuthorizationParameters.Add(parameter.Key,parameter.Value);
                }
            }
            else
                RequestUrl = Url;
            var response = await this.MakeRequest(requestType).ExecuteRequest(RequestUrl);
            if (string.IsNullOrEmpty(response))
                return null;
            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T));
            T dat = json.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(response))) as T;
            return dat;
      }
    }
}
