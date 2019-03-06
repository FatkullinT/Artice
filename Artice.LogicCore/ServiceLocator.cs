using System;
using Microsoft.Extensions.DependencyInjection;

namespace Artice.LogicCore
{
	public class ServiceLocator : IServiceLocator
	{
		private readonly IServiceProvider _serviceProvider;
		private IServiceScope _scope;

		public ServiceLocator(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public T Get<T>()
		{
			return _scope != null ? _scope.ServiceProvider.GetService<T>() : _serviceProvider.GetService<T>();
		}

		public void CreateScope()
		{
			_scope = _serviceProvider.CreateScope();
		}

		public void DeleteScope()
		{
			_scope.Dispose();
			_scope = null;
			GC.Collect();
		}
	}
}