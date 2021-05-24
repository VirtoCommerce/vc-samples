using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.YouTube.v3.Data;
using ProductVideoModule.Core.Models;

namespace ProductVideoModule.Core.Services
{
    public interface IInternalYoutubeService
    {
        Task<SearchListResponse> SearchByExternalApi(string keyword);

        Task CheckVideosExsistence(IEnumerable<VideoLink> videoLinks);
    }
}
