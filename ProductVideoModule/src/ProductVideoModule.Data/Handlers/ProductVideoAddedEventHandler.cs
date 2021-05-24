using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductVideoModule.Core.Events;
using ProductVideoModule.Core.Services;
using VirtoCommerce.Platform.Core.Events;

namespace ProductVideoModule.Data.Handlers
{
    public class ProductVideoAddedEventHandler : IEventHandler<ProductVideoAddedEvent>
    {
        private readonly IInternalYoutubeService _internalYoutubeService;

        public ProductVideoAddedEventHandler(IInternalYoutubeService internalYoutubeService)
        {
            _internalYoutubeService = internalYoutubeService;
        }

        public Task Handle(ProductVideoAddedEvent message)
        {
            return _internalYoutubeService.CheckVideosExsistence(message.ChangedEntries.Select(x => x.NewEntry));
        }
    }
}
