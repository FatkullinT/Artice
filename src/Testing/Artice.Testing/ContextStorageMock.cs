using Artice.Context;
using Artice.Testing.Core;
using Moq;

namespace Artice.Testing
{
	public class ContextStorageMock : BaseMock<IContextStorage, ContextStorageMock>
	{
		public ContextStorageMock(bool returnsRequestedContext = false)
		{
			if (returnsRequestedContext)
			{
				Setup(storage => storage.Get(It.IsAny<Recipient>())).Returns<Recipient>((recipient) =>
					new ChatContextMock().SetRecipient(recipient).Object);
			}
		}

		public ChatContextMock AddContext(Recipient recipient)
		{
			var contextMock = new ChatContextMock().SetRecipient(recipient);
			Setup(storage => storage.Get(It.Is<Recipient>(r => r == recipient))).Returns(() => contextMock.Object);
			return contextMock;
		}
	}
}