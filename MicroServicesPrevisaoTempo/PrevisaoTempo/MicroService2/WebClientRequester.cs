using System.Net;

namespace MicroService2
{
    class WebClientRequester : IRequester
    {
        private WebClient _client;

        public WebClientRequester()
        {
            _client = new WebClient();
        }

        public string Request(string url)
        {
            return _client.DownloadString(url);
        }
    }
}