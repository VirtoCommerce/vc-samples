using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Rest;

namespace External.CatalogModule.Web.ApiClient
{
    public class VirtoApiSecurityRequest : ServiceClientCredentials
    {
        public string AppId { get; set; }
        public string SecretKey { get; set; }

        public VirtoApiSecurityRequest(string appId, string secretKey)
        {
            AppId = appId;
            SecretKey = secretKey;
        }

        public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AddAuthorization(request);

            return base.ProcessHttpRequestAsync(request, cancellationToken);
        }


        private void AddAuthorization(HttpRequestMessage request)
        {

            var signature = new ApiRequestSignature { AppId = AppId };

            var parameters = new[]
            {
                    new NameValuePair(null, AppId),
                    new NameValuePair(null, signature.TimestampString)
                };

            signature.Hash = HmacUtility.GetHashString(key => new HMACSHA256(key), SecretKey, parameters);

            request.Headers.Authorization = new AuthenticationHeaderValue("HMACSHA256", signature.ToString());

        }



    }
}

  