using SimpleLinkShrink.Data.Entity;

namespace SimpleLinkShrink.Data
{
    public interface IRepository
    {
        Task<Shortlink> GenerateShortlink(string targetUrl);
        Task<string> GetTargetUrl(string alias);
    }
}
