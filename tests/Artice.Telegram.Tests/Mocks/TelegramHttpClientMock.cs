using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using Artice.Core.Models.Files;
using Artice.Testing.Core;
using Moq;

namespace Artice.Telegram.Tests.Mocks
{
	public class TelegramHttpClientMock : BaseMock<ITelegramHttpClient, TelegramHttpClientMock>
	{
		public void VerifyPost<TResponseModel>(string methodName, Expression<Func<Dictionary<string, object>, bool>> match)
		{
			Verify(client => client.PostAsync<TResponseModel>(methodName, It.Is(match), It.IsAny<CancellationToken>()), Times.Once);
		}

        public void VerifyPostFiles<TResponseModel>(string methodName, Expression<Func<Dictionary<string, string>, bool>> matchParams, Expression<Func<IEnumerable<KeyValuePair<string, IFile>>, bool>> matchFiles, Times times)
        {
            Verify(client => client.PostFilesAsync<TResponseModel>(methodName, It.Is(matchParams), It.Is(matchFiles), It.IsAny<CancellationToken>()), times);
        }

        public void VerifyPostFiles<TResponseModel>(string methodName, Expression<Func<Dictionary<string, string>, bool>> matchParams, Expression<Func<IEnumerable<KeyValuePair<string, IFile>>, bool>> matchFiles)
        {
            VerifyPostFiles<TResponseModel>(methodName, matchParams, matchFiles, Times.Once());
        }

        public void VerifyGet<TResponseModel>(string methodName, Expression<Func<Dictionary<string, object>, bool>> match)
		{
			Verify(client => client.GetAsync<TResponseModel>(methodName, It.Is(match), It.IsAny<CancellationToken>()), Times.Once);
		}
	}
}