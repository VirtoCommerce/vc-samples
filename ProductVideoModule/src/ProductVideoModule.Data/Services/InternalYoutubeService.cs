using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.Options;
using ProductVideoModule.Core;
using ProductVideoModule.Core.Services;
using VirtoCommerce.Platform.Core.Exceptions;

namespace ProductVideoModule.Data.Services
{
    public class InternalYoutubeService : IInternalYoutubeService
    {
        private readonly YouTubeService _youtubeService;

        public InternalYoutubeService(IOptions<ProductVideoModuleOptions> moduleOptions)
        {
            if (moduleOptions.Value.YoutubeApiKey is null)
                throw new PlatformException("Platform configuration file must contain the section named as 'ExternalYoutubeApi'. This section consist of YoutubeApiKey property.");

            _youtubeService = new YouTubeService(new BaseClientService.Initializer { ApiKey = moduleOptions.Value.YoutubeApiKey });
        }

        public async Task<SearchListResponse> SearchByExternalApi(string keyWord)
        {
            var searchListRequest = _youtubeService.Search.List("snippet");
            searchListRequest.Q = keyWord; // Replace with your search term.
            searchListRequest.MaxResults = 10;

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();

            return searchListResponse;
        }
    }
}
