using System;
using System.Collections.Generic;
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
        [StringLength(128)]
        public string ContractNumber { get; set; }

        /// <summary>
        /// This method used to convert domain Member type instance to data model
        /// </summary>
        public override MemberDataEntity FromMember(Member member, PrimaryKeyResolvingMap pkMap)
        {
            // Here you can write code for custom mapping
            // supplier properties will be mapped in base method implementation by using value injection
            return base.FromMember(member, pkMap);
        }
        /// <summary>
        /// This method used to convert data type instance to domain model
        /// </summary>
        public override Member ToMember(Member member)
        {
            // Here you can write code for custom mapping
            // supplier properties will be mapped in base method implementation by using value injection
            return base.ToMember(member);
        }
        /// <summary>
        /// This method used to apply changes form other member type instance 
        /// </summary>
        public override void Patch(MemberDataEntity target)
        {
            base.Patch(target);
        }
    }
}