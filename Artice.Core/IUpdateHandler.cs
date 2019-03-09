﻿using System.Threading.Tasks;
using Artice.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Artice.Core
{
	public interface IUpdateHandler
	{
		Task<bool> CheckRequest(HttpRequest request);

		Task<IncomingMessage> HandleAsync(HttpContext context);
	}
}