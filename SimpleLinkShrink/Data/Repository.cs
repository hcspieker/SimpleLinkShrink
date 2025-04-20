using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SimpleLinkShrink.Exceptions;
using SimpleLinkShrink.Configuration;
using SimpleLinkShrink.Data.Entity;
using SimpleLinkShrink.Util;

namespace SimpleLinkShrink.Data
{
    public class Repository : IRepository
    {
        private readonly LinkDbContext _context;
        private readonly IRandomStringGenerator _randomStringGenerator;
        private readonly IOptionsMonitor<ShortLinkSettings> _optionsMonitor;

        private ShortLinkSettings _settings => _optionsMonitor.CurrentValue;

        public Repository(LinkDbContext context, IRandomStringGenerator randomStringGenerator, IOptionsMonitor<ShortLinkSettings> optionsMonitor)
        {
            _context = context;
            _randomStringGenerator = randomStringGenerator;
            _optionsMonitor = optionsMonitor;
        }

        public async Task<Shortlink> GenerateShortlink(string targetUrl)
        {
            var entity = new Shortlink
            {
                TargetUrl = targetUrl,
                Alias = _randomStringGenerator.GenerateRandomString(_settings.LinkAliasLength),
                ExpirationDate = DateTime.Now.Add(_settings.LinkExpirationSpan)
            };

            var amountOfRetries = 5;
            var retry = 0;
            while (await _context.Shortlinks.AnyAsync(x => x.Alias == entity.Alias))
            {
                if (retry < amountOfRetries)
                    throw new CreateShortlinkException($"Error while generating a new shortlink alias.");

                retry++;
                entity.Alias = _randomStringGenerator.GenerateRandomString(_settings.LinkAliasLength);
            }

            await _context.Shortlinks.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<string> GetTargetUrl(string alias)
        {
            try
            {
                var result = await _context.Shortlinks.SingleAsync(x => x.Alias == alias);

                return result.TargetUrl;
            }
            catch (InvalidOperationException)
            {
                throw new ShortlinkNotFoundException($"No shortlink was found for the alias {alias}.");
            }
        }
    }
}
