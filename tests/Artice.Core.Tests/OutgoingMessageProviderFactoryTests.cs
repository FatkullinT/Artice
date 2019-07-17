using System.Linq;
using Artice.Core.OutgoingMessages;
using Artice.Testing.Core;
using AutoFixture;
using Xunit;

namespace Artice.Core.Tests
{
	public class OutgoingMessageProviderFactoryTests 
	{
		[Fact]
		public void GetProvider_Exists_Success()
		{
			//arrange
			var fixture = new Fixture();
			var providers = fixture.CreateMany<string>(3).Select(name => new OutgoingMessageProviderMock(name).Object).ToArray();
			var expectedProvider = providers.Last();
			var factory = new OutgoingMessageProviderFactory(providers);
			
			//act
			var provider = factory.GetProvider(expectedProvider.MessengerId);
			
			//assert
			Assert.Same(expectedProvider, provider);
		}

		[Fact]
		public void GetProvider_NotExists_Null()
		{
			//arrange
			var fixture = new Fixture();
			var factory = new OutgoingMessageProviderFactory(Enumerable.Empty<IOutgoingMessageProvider>());
			
			//act
			var provider = factory.GetProvider(fixture.Create<string>());
			
			//assert
			Assert.Null(provider);
		}
	}
}