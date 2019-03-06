using System;

namespace Artice.Core
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class HandlerRouteAttribute : Attribute
	{
		public string Route { get; }

		public HandlerRouteAttribute(string route)
		{
			Route = route;
		}
	}
}