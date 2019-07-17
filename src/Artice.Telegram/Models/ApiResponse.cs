using Newtonsoft.Json;

namespace Artice.Telegram.Models
{
	public class ApiResponse<TResponse>
	{
		/// <summary>
		/// Gets a value indicating whether the request was successful.
		/// </summary>
		[JsonProperty("ok", Required = Required.Always)]
		public bool Ok { get; set; }

		/// <summary>
		/// Gets the result object.
		/// </summary>
		[JsonProperty("result", Required = Required.Default)]
		public TResponse ResultObject { get; set; }

		/// <summary>
		/// Gets the error message.
		/// </summary>
		[JsonProperty("description", Required = Required.Default)]
		public string Message { get; set; }

		/// <summary>
		/// Gets the error code.
		/// </summary>
		[JsonProperty("error_code", Required = Required.Default)]
		public int Code { get; set; }
	}
}