using CustomerReviews.Core.Events;
using CustomerReviews.Events.Data.Handlers;
using Microsoft.Practices.Unity;
using VirtoCommerce.Platform.Core.Bus;
using VirtoCommerce.Platform.Core.Modularity;

namespace CustomerReviews.Events.Web
{
    public class Module : ModuleBase
    {
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        public override void Initialize()
        {
            base.Initialize();

            var eventHandlerRegistrar = _container.Resolve<IHandlerRegistrar>();
            /*eventHandlerRegistrar.RegisterHandler<CustomerReviewChangingEvent>(async (message, token) => await _container.Resolve<AdjustPropertiesCustomerReviewChangingEvent>().Handle(message)); */
            eventHandlerRegistrar.RegisterHandler<CustomerReviewChangedEvent>(async (message, token) => await _container.Resolve<OpenUrlCustomerReviewChangedEvent>().Handle(message));
        }
    }
}
