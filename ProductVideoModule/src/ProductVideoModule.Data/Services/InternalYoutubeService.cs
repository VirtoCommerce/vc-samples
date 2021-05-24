using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.Options;
using ProductVideoModule.Core;
using ProductVideoModule.Core.Models;
using ProductVideoModule.Core.Services;
using VirtoCommerce.Platform.Core.Exceptions;

namespace ProductVideoModule.Data.Services
{
    public class InternalYoutubeService : IInternalYoutubeService
    {
        private readonly YouTubeService _youtubeService;
        private readonly IProductVideoService _productVideoService;

        private readonly HttpClient _httpClient;
        private const string EXTERNAL_URL = "https://img.youtube.com/vi/{0}/0.jpg";
        private readonly Regex PATTERN = new Regex(@"^.*(?:(?:youtu\.be\/|v\/|vi\/|u\/\w\/|embed\/)|(?:(?:watch)?\?v(?:i)?=|\&v(?:i)?=))([^#\&\?]+).*");

        public InternalYoutubeService(IOptions<ProductVideoModuleOptions> moduleOptions, IProductVideoService productVideoService)
        {
            if (moduleOptions.Value.YoutubeApiKey is null)
                throw new PlatformException("Platform configuration file must contain the section named as 'ExternalYoutubeApi'. This section consist of YoutubeApiKey property.");

            _youtubeService = new YouTubeService(new BaseClientService.Initializer { ApiKey = moduleOptions.Value.YoutubeApiKey });
            _productVideoService = productVideoService;
            _httpClient = new HttpClient();
        }

        public async Task<SearchListResponse> SearchByExternalApi(string keyword)
        {
            var searchListRequest = _youtubeService.Search.List("snippet");
            searchListRequest.Q = keyword; // Replace with your search term.
            searchListRequest.MaxResults = 10;

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();

            return searchListResponse;
        }

        public async Task CheckVideosExsistence(IEnumerable<VideoLink> videoLinks)
        {
            var statusesDict = new Dictionary<string, VideoLinkStatus>();

            foreach (var link in videoLinks)
            {
                var videoId = PATTERN.Match(link.Url).Groups[1].Value;
                var response = await _httpClient.GetAsync(string.Format(EXTERNAL_URL, videoId));

                if (response.IsSuccessStatusCode)
                    statusesDict.Add(link.Id, VideoLinkStatus.Verified);
            }

            await _productVideoService.ChangeVideoLinksStatuses(statusesDict);
        }
    }
}
