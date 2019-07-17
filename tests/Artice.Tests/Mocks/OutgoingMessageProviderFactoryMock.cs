using Artice.Core.OutgoingMessages;
using Artice.Testing.Core;
using Moq;

namespace Artice.Tests.Mocks
{
	public class OutgoingMessageProviderFactoryMock : BaseMock<IOutgoingMessageProviderFactory, OutgoingMessageProviderFactoryMock>
	{
		public OutgoingMessageProviderFactoryMock(bool returnsRequestedProvider = false)
		{
			if (returnsRequestedProvider)
			{
				Setup(factory => factory.GetProvider(It.IsAny<string>()))
					.Returns<string>((id) => new OutgoingMessageProviderMock(id).Object);
			}
		}

		public OutgoingMessageProviderMock AddProvider(string providerId)
		{
			var providerMock = new OutgoingMessageProviderMock(providerId);
			Setup(factory => factory.GetProvider(providerId)).Returns(() => providerMock.Object);
			return providerMock;
		}
	}
}