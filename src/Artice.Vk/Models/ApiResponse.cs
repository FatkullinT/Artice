using Newtonsoft.Json;

namespace Artice.Vk.Models
{
	public class ApiResponse<TResponse>
	{
		/// <summary>
		/// Gets the result object.
		/// </summary>
		[JsonProperty("response", Required = Required.Default)]
		public TResponse Response { get; set; }

        /// <summary>
        /// Error details.
        /// </summary>
        [JsonProperty("error", Required = Required.Default)]
        public Error Error { get; set; }
    }
}