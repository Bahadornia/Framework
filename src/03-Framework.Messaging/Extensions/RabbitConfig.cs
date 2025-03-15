namespace App.Framework.Messaging.Extensions
{
    public class RabbitConfig
    {
        public string BaseUrl { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string EndPoint { get; set; } = default!;
    }
}