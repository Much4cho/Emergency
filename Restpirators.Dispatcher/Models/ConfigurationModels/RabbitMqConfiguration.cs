namespace Restpirators.Dispatcher.Models.ConfigurationModels
{
    public class RabbitMqConfiguration
    {
        public const string ConfigurationKey = "RabbitMqConfiguration";

        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
        public ExchangesConfiguration Exchanges { get; set; }

        public class ExchangesConfiguration
        {
            public string Fanout { get; set; }
        }

    }
}