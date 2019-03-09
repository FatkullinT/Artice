﻿using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Artice.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Artice.Core
{
	public abstract class UpdateHandler : IUpdateHandler
	{
		public virtual Task<bool> CheckRequest(HttpRequest request)
		{
			return Task.FromResult(request.Method == HttpMethod.Post.Method);
		}

		public virtual async Task<IncomingMessage> HandleAsync(HttpContext context)
		{
			using (Stream receiveStream = context.Request.Body)
			{
				using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
				{
					var content = await readStream.ReadToEndAsync();
					var incomingMessage = await ConvertJsonContent(content);
					await MakeResponse(context);
					return incomingMessage;
				}
			}
		}

		protected virtual Task<IncomingMessage> ConvertJsonContent(string content)
		{
			throw new NotImplementedException();
		}

		protected virtual Task MakeResponse(HttpContext context)
		{
			context.Response.StatusCode = StatusCodes.Status200OK;
			return Task.CompletedTask;
		}
	}
}