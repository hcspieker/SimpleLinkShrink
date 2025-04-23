using SimpleLinkShrink.Data.Entity;

namespace SimpleLinkShrink.Data
{
    public interface IRepository
    {
        Task<Shortlink> Create(string targetUrl);
        Task<Shortlink> Get(string alias);
        Task Delete(int id);
    }
}
