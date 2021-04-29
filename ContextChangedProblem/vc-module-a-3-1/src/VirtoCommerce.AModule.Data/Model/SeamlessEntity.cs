using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.AModule.Data.Model
{   
    public class SeamlessEntity:Entity
    {
        public int Numeric { get; set; }
        public int NewNumeric { get; set; }
    }
}
