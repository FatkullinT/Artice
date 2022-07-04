using System.Collections.Generic;

namespace Artice.Core.Models
{
    public class UpdatesResponse<TUpdate>
    {
        public IEnumerable<TUpdate> Updates { get; set; }

        public Dictionary<string, string> ContextData { get; set; }
    }
}