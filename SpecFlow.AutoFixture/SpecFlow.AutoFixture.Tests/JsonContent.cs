using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace SpecFlow.AutoFixture.Tests
{
    public class JsonContent : StringContent
    {
        public JsonContent(object obj) : base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json") { }
    }
}
