using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Linq;
using System.Text.RegularExpressions;
using System;

namespace UCWA.WindowsPhone.Model
{
    public class UCWAHelper
    {
        public string UserName { get; set; }

        public string Domain { get; set; }

        private string password;

        private const string autoDiscoverUrl = @"https://lyncdiscover.{0}";

        private bool isUserAuthenticated;
        private Action onLoginCompleted;

        RestClient ucwaClient;

        RestRequest request;

        public UCWAHelper()
        {

        }

        public UCWAHelper(string userName, string password, Action loginCompleted)
        {
            this.UserName = userName;
            this.password = password;
            this.onLoginCompleted = loginCompleted;
            this.Domain = this.UserName.Split(new string[] { "@" }, System.StringSplitOptions.RemoveEmptyEntries)[1];
            this.InitializeConnection();
        }

        private void InitializeConnection()
        {
            ucwaClient = new RestClient();
            request = new RestRequest(string.Format(autoDiscoverUrl, this.Domain));
            request.AddHeader("Accept", "application/json");
            ucwaClient.ExecuteAsync(request, this.ParseAutoDiscoveryResponse);
        }

        private string user;
        private string xframe;

        private void ParseAutoDiscoveryResponse(IRestResponse response)
        {
            var resp = JsonConvert.DeserializeObject(response.Content) as JObject;
            user = (string)(resp["_links"]["user"].First as JProperty).Value;
            xframe = (string)(resp["_links"]["xframe"].First as JProperty).Value;

            this.Authenticate(user, null, null);
        }

        private void Authenticate(string authenticateUrl, string authenticateToken, string authenticateTokenType)
        {
            // Make a GET request to get the ouath url
            request = new RestRequest(authenticateUrl);
            request.AddHeader("Accept", "application/json");
            if (!string.IsNullOrEmpty(userToken))
            {
                request.AddHeader("Authorization", String.Format("{0} {1}", authenticateTokenType, authenticateToken));
            }
            ucwaClient.ExecuteAsync(request, this.ParseAuthenticateResponse);
        }



        private void ParseAuthenticateResponse(IRestResponse response)
        {
            // check if the request is succesfull
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                GetOauthUrl(response);
            }

            else if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Check if user is already authenticated
                if (!isUserAuthenticated)
                {
                    GetApplicationsUrl(response);
                }
                else
                {
                    this.CreateNewApplications(applications);
                }
            }

        }

        private string applications;
        private string applicationsUser;

        private void GetApplicationsUrl(IRestResponse response)
        {
            var resp = JsonConvert.DeserializeObject(response.Content) as JObject;
            applicationsUser = (string)(resp["_links"]["self"].First as JProperty).Value;
            xframe = (string)(resp["_links"]["xframe"].First as JProperty).Value;
            applications = (string)(resp["_links"]["applications"].First as JProperty).Value;
            this.CreateNewApplications(applications);
        }

        private void CreateNewApplications(string applications)
        {
            request = new RestRequest(applications, Method.POST);
            if (CheckIfSameDomain(applications, oauthUrl))
            {
                request.AddHeader("Authorization", String.Format("{0} {1}", applicationTokenType, applicationToken));
            }
            var applicationBody = @"""UserAgent"":""{0}"",""EndpointId"":""{1}"",""Culture"":""en-US""";
            request.RequestFormat = DataFormat.Xml;
            request.AddParameter(
                "application/json",
               "{" + string.Format(applicationBody, "UCWAWindowsPhoneSample", Guid.NewGuid().ToString()) + "}",
                ParameterType.RequestBody);
            request.AddHeader("Accept", "application/json");
            ucwaClient.ExecuteAsync(request, this.CreateNewApplicationsResponse);
        }

        private bool CheckIfSameDomain(string url1, string url2)
        {
            // Check if the token is for the correct domain
            return new Uri(url1).Host == new Uri(url2).Host;
        }

        private void CreateNewApplicationsResponse(IRestResponse response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // We are on a split-domain scenario. We need to re-authenticate with the new oauth url
                isUserAuthenticated = true;
                GetOauthUrl(response);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                // New applciation created
                this.ParseApplicationCreatedResponse(response);
            }
                
        }

        public string UserFullName { get; set; }

        public string Title { get; set; }

        public string Department { get; set; }

        private void ParseApplicationCreatedResponse(IRestResponse response)
        {
            var resp = JsonConvert.DeserializeObject(response.Content) as JObject;
            UserFullName = (string)(resp["_embedded"]["me"]["name"] as JValue).Value;
            Title = (string)(resp["_embedded"]["me"]["title"] as JValue).Value;
            Department = (string)(resp["_embedded"]["me"]["department"] as JValue).Value;
            this.onLoginCompleted();
        }

        string oauthUrl;

        private void GetOauthUrl(IRestResponse response)
        {
            // parse the url to get the OAuth url
            var authenticateHeader = response.Headers.FirstOrDefault(a => a.Name == "WWW-Authenticate");
            var oauthPattern = ".*MsRtcOAuth href=\"(?<oauthUrl>.*)\",grant_type.*";
            Regex reg = new Regex(oauthPattern);
            var oauthMatch = reg.Match((string)authenticateHeader.Value);
            var oauthGroup = oauthMatch.Groups["oauthUrl"];
            if (oauthGroup != null)
            {
                 oauthUrl = oauthGroup.Value;
                // Send a POST request to the Oauth Url 
                this.GetToken(oauthUrl);
            }
        }

        private void GetToken(string oauthUrl)
        {
            request = new RestRequest(oauthUrl, Method.POST);
            request.AddHeader("Accept", "application/json");
            request.RequestFormat = DataFormat.Xml;

            request.AddParameter(
                "application/x-www-form-urlencoded;charset='utf-8'",
                string.Format(@"grant_type=password&username={0}&password={1}", UserName, password),
                ParameterType.RequestBody);
            ucwaClient.ExecuteAsync(request, this.ParseGetToken);
        }

        string userToken, applicationToken;
        string userTokenType, applicationTokenType;

        private void ParseGetToken(IRestResponse response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Get the token
                var resp = JsonConvert.DeserializeObject(response.Content) as JObject;
                if (!isUserAuthenticated)
                {
                    // We do not know if the userToken and applicationToken would be in different
                    // domain. Hence we are asigning both as the same. We will update the 
                    // application token if it is on a seperate domain
                    userToken = applicationToken = (string)(resp["access_token"] as JValue).Value;
                    userTokenType = applicationTokenType= (string)(resp["token_type"] as JValue).Value;
                    this.Authenticate(user, userToken, userTokenType);
                }
                else
                {
                    applicationToken = (string)(resp["access_token"] as JValue).Value;
                    applicationTokenType = (string)(resp["token_type"] as JValue).Value;
                    this.CreateNewApplications(applications);
                }

            }
        }

    }
}
