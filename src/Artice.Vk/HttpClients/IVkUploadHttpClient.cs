using System.Threading;
using System.Threading.Tasks;
using Artice.Core.Models.Files;
using Artice.Vk.Models;

namespace Artice.Vk.HttpClients
{
    public interface IVkUploadHttpClient
    {
        Task<UploadResponse> PostAsync(string uploadUrl, string fieldName, IFile file, CancellationToken cancellationToken);
    }
}