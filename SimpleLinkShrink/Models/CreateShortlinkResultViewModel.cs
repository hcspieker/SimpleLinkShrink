namespace SimpleLinkShrink.Models
{
    public class CreateShortlinkResultViewModel
    {
        public string TargetUrl { get; set; }
        public string InternalUrl { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public CreateShortlinkResultViewModel()
        {
            TargetUrl = string.Empty;
            InternalUrl = string.Empty;
        }
    }
}
