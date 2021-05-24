using System;
using System.Collections.Generic;
using System.Text;
using ProductVideoModule.Core.Models;
using VirtoCommerce.Platform.Core.Events;

namespace ProductVideoModule.Core.Events
{
    public class ProductVideoAddedEvent : GenericChangedEntryEvent<VideoLink>
    {
        public ProductVideoAddedEvent(IEnumerable<GenericChangedEntry<VideoLink>> changedEntries) : base(changedEntries)
        {
        }
    }
}
