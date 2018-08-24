using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VirtoCommerce.CustomerModule.Data.Model;

namespace MemberExtensionSampleModule.Web.Model
{

    public class Contact2DataEntity : ContactDataEntity
    {
        [StringLength(128)]
        public string JobTitle { get; set; }

        public override void Patch(MemberDataEntity memberDataEntity)
        {
            base.Patch(memberDataEntity);
            var target = memberDataEntity as Contact2DataEntity;
            if (target != null)
            {
                target.JobTitle = JobTitle;
            }

        }
    }
}
