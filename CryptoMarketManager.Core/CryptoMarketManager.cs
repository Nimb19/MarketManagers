namespace CryptoMarketManager.Core
{
    public class CryptoMarketManager
    {
        private readonly SecretConfig _serviceConfig;
        public TYPE Type { get; set; }

        public CryptoMarketManager(SecretConfig secretConfig)
        {
            _serviceConfig = secretConfig;

            InitClients(_serviceConfig);
        }
    }
}