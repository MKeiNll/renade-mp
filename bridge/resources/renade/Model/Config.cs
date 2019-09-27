using Newtonsoft.Json;

namespace renade
{
    public class Config
    {
        [JsonProperty("dbUser")]
        public readonly string DatabaseUser;

        [JsonProperty("dbPass")]        
        public readonly string DatabasePassword;
    }
}