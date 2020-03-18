namespace MicroServiceBase.Communication
{
    public struct MessagingConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VHost { get; set; }
    }
}