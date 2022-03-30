using Newtonsoft.Json.Linq;

namespace ITunesLoader.Interfaces
{
    public interface IITunesService
    {
        IJEnumerable<JToken> GetItunesJson(string searchTerm);
    }
}
