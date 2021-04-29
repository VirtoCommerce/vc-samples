using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.AModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.BModule.Data.Model
{   
    public class SeamlessEntity2: SeamlessEntity
    {
        
        public string NewField { get; set; }
    }
}
