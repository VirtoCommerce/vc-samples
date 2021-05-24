using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using ProductVideoModule.Core.Events;
using ProductVideoModule.Core.Models;
using ProductVideoModule.Core.Services;
using ProductVideoModule.Data.Models;
using ProductVideoModule.Data.Repositories;
using ProductVideoModule.Data.Services;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Events;
using Xunit;

namespace ProductVideoModule.Tests
{
    public class ProductVideoServiceTests
    {
        private Mock<IVideoLinkRepository> _videoLinkRepositoryMock;
        private Mock<IEventPublisher> _eventPublisherMock;
        private IProductVideoService _service;

        public ProductVideoServiceTests()
        {
            _videoLinkRepositoryMock = new Mock<IVideoLinkRepository>();
            _videoLinkRepositoryMock.SetupGet(r => r.UnitOfWork).Returns(Mock.Of<IUnitOfWork>());
            _eventPublisherMock = new Mock<IEventPublisher>();
            _service = new ProductVideoService(
                () => _videoLinkRepositoryMock.Object,
                Mock.Of<IPlatformMemoryCache>(),
                _eventPublisherMock.Object);
        }

        [Fact]
        public async Task SaveVideoLinksAsyncTest()
        {
            // Arrange
            var ids = new string[] { "id1", "id0" };
            var linksToSave = new VideoLink[] {
                new VideoLink(){ Id = ids[0]},
                new VideoLink(){ Id = ids[1]},
            };

            _videoLinkRepositoryMock.Setup(r => r.GetByIdsAsync(ids))
                .ReturnsAsync(new VideoLinkEntity[] { new VideoLinkEntity() { Id = ids[1] } });

            // Act
            await _service.SaveVideoLinksAsync(linksToSave);

            // Assert
            _videoLinkRepositoryMock.Verify(x => x.GetByIdsAsync(It.IsAny<IEnumerable<string>>()), Times.Once);
            _eventPublisherMock.Verify(x => x.Publish(
                It.Is<ProductVideoAddedEvent>(x => x.ChangedEntries.Any(y => y.NewEntry.Id == ids[0])), default
                ), Times.Once);
        }
    }
}
