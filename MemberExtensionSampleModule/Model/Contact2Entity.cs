using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VirtoCommerce.CustomerModule.Data.Model;

namespace ContactExtModule.Web.Model
{

    public class Contact2Entity : ContactDataEntity
    {
        [StringLength(128)]
        public string JobTitle { get; set; }
    }
}