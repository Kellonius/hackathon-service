using Hackathon_Service.Services;

namespace Hackathon_Service.Models.Epic
{
    public class Reference
    {
        public string display { get; set; }
        public string reference { get; set; }

        public T Get<T>()
        {
            return new HttpService("").Get<T>(reference);
        }
    }
}