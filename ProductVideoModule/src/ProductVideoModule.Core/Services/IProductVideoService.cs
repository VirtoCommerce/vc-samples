using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ProductVideoModule.Core.Models;

namespace ProductVideoModule.Core.Services
{
    public interface IProductVideoService
    {
        Task<IEnumerable<VideoLink>> GetVideoPage(int page = 0, int size = 10);
    }
}
