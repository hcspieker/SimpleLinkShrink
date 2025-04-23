namespace SimpleLinkShrink.Models
{
    public class CreateShortlinkResultViewModel
    {
        public string TargetUrl { get; set; }
        public string InternalAccessUrl { get; set; }
        public string InternalDeleteUrl { get;  set; }
        public DateTime? ExpirationDate { get; set; }

        public CreateShortlinkResultViewModel()
        {
            TargetUrl = string.Empty;
            InternalAccessUrl = string.Empty;
            InternalDeleteUrl = string.Empty;
        }
    }
}
