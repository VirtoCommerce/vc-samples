using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace External.CustomerReviewsModule.Web.Security
{
    public static class PredefinedPermissions
    {
        public const string CustomerReviewRead = "External.CustomerReviewsModule:read";
        public const string CustomerReviewUpdate = "External.CustomerReviewsModule:update";
        public const string CustomerReviewDelete = "External.CustomerReviewsModule:delete";
    }
}
