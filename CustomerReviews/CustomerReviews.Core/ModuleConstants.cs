namespace CustomerReviews.Core
{
    public static class ModuleConstants
    {
        public static class Security
        {
            public static class Permissions
            {
                public const string CustomerReviewRead = "customerReview:read",
                                    CustomerReviewUpdate = "customerReview:update",
                                    CustomerReviewDelete = "customerReview:delete";
            }
        }
    }
}
