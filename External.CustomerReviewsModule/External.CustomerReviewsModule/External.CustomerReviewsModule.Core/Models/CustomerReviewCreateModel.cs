namespace External.CustomerReviewsModule.Core.Models
{
    public class CustomerReviewCreateModel
    {
        public string AuthorNickname { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
        public string ProductId { get; set; }
    }
}
