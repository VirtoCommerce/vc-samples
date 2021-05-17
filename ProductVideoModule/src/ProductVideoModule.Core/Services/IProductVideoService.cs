using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ProductVideoModule.Core.Models;

namespace ProductVideoModule.Core.Services
{
    public interface IProductVideoService
    {
        Task<VideoLink[]> GetByIdsAsync(string[] videoLinksIds);
        Task SaveVideoLinksAsync(VideoLink[] items);
        Task DeleteVideoLinksAsync(string[] ids);
    }
}
