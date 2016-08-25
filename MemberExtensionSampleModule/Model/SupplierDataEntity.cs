using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VirtoCommerce.CustomerModule.Data.Model;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Platform.Core.Common;

namespace MemberExtensionSampleModule.Web.Model
{
    /// <summary>
    /// Represent persistent member type
    /// </summary>
    public class SupplierDataEntity : MemberDataEntity
    {
        public SupplierDataEntity()
        {
            Reviews = new NullCollection<SupplierReviewDataEntity>();
        }

        [StringLength(128)]
        public string ContractNumber { get; set; }

        public ObservableCollection<SupplierReviewDataEntity> Reviews { get; set; }

        /// <summary>
        /// This method used to convert domain Member type instance to data model
        /// </summary>
        public override MemberDataEntity FromModel(Member member, PrimaryKeyResolvingMap pkMap)
        {
            var retVal = base.FromModel(member, pkMap) as SupplierDataEntity;
            var supplier = member as Supplier;
            if (supplier != null && !supplier.Reviews.IsNullOrEmpty())
            {
                retVal.Reviews = new ObservableCollection<SupplierReviewDataEntity>();
                foreach(var review in supplier.Reviews)
                {
                    var reviewDataEntity = new SupplierReviewDataEntity();
                    pkMap.AddPair(review, reviewDataEntity);
                    retVal.Reviews.Add(reviewDataEntity.FromModel(review));
                }
            }

            // Here you can write code for custom mapping
            // supplier properties will be mapped in base method implementation by using value injection
            return retVal;
        }
        /// <summary>
        /// This method used to convert data type instance to domain model
        /// </summary>
        public override Member ToModel(Member member)
        {
            // Here you can write code for custom mapping
            // supplier properties will be mapped in base method implementation by using value injection
            var retVal = base.ToModel(member) as Supplier;
            if (retVal != null)
            {
                retVal.Reviews = this.Reviews.OrderBy(x => x.Id).Select(x => x.ToModel(new SupplierReview())).ToList();
            }
            return retVal;
        }
        /// <summary>
        /// This method used to apply changes form other member type instance 
        /// </summary>
        public override void Patch(MemberDataEntity target)
        {
            base.Patch(target);

            var suplierDataEntity = target as SupplierDataEntity;
            if (suplierDataEntity != null && !this.Reviews.IsNullCollection())
            {
                 this.Reviews.Patch(suplierDataEntity.Reviews, (sourceReview, targetReview) => sourceReview.Patch(targetReview));
            }

        }
    }
}