using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SimpleGPS
{
    [DataContract]
    public class LocationQueryResponse
    {
        [DataMember]
        public string authenticationResultCode { get; set; }
        [DataMember]
        public string brandLogoUri { get; set; }
        [DataMember]
        public string copyright { get; set; }
        [DataMember]
        public string statusCode { get; set; }
        [DataMember]
        public string statusDescription { get; set; }
        [DataMember]
        public string traceId { get; set; }

        [DataMember]
        public ResourceSet[] resourceSets { get; set; }

        [DataContract]
        public class ResourceSet
        {
            [DataMember]
            public int estimatedTotal { get; set; }

            [DataMember]
            public Resource[] resources { get; set; }

            [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1", Name = "Location")]
            public class Resource
            {
                [DataMember]
                public string __type { get; set; }

                [DataMember]
                public double[] bbox { get; set; }

                [DataMember]
                public string name { get; set; }

                [DataMember]
                public Point point { get; set; }

                [DataContract]
                public class Point
                {
                    [DataMember]
                    public string type { get; set; }

                    [DataMember]
                    public string[] coordinates { get; set; }
                }

                [DataMember]
                public Address address { get; set; }

                [DataContract]
                public class Address
                {
                    [DataMember]
                    public string addressLine { get; set; }
                    [DataMember]
                    public string adminDistrict { get; set; }
                    [DataMember]
                    public string adminDistrict2 { get; set; }
                    [DataMember]
                    public string countryRegion { get; set; }
                    [DataMember]
                    public string formattedAddress { get; set; }
                    [DataMember]
                    public string locality { get; set; }
                    [DataMember]
                    public string postalCode { get; set; }
                }

                [DataMember]
                public string confidence { get; set; }

                [DataMember]
                public string entityType { get; set; }
            }

        }
    }
}
