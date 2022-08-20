using CommonTools.Logger;
using MarketManager.Core.Models;

namespace MarketManager.Core
{
    public class MarketManager
    {
        private readonly ILogger _logger;
        private readonly SecretConfig _serviceConfig;

        public IMarketClient[] MarketClients { get; private set; }

        public MarketManager(ILogger logger, SecretConfig secretConfig)
        {
            _logger = logger;
            _serviceConfig = secretConfig;

            InitClients(_serviceConfig);
        }

        private void InitClients(SecretConfig serviceConfig)
        {
            throw new NotImplementedException();
        }
    }
}