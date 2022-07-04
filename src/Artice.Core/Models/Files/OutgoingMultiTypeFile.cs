using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Artice.Core.Models.Files
{
    public class OutgoingMultiTypeFile : IOutgoingFile
    {
        private readonly Dictionary<Type, IFile> _innerFiles;

        public IReadOnlyDictionary<Type, IFile> InnerFiles => _innerFiles;

        public OutgoingMultiTypeFile(params IFile[] files)
        {
            _innerFiles = new Dictionary<Type, IFile>();
            foreach (var file in files)
            {
                if (file is OutgoingMultiTypeFile outgoingMultiTypeFile)
                {
                    foreach (var innerFile in outgoingMultiTypeFile.InnerFiles.Values)
                    {
                        Add(innerFile);
                    }
                }
                else
                {
                    Add(file);
                }
            }
        }

        private void Add(IFile file)
        {
            var type = file.GetType();
            if (_innerFiles.ContainsKey(type))
            {
                _innerFiles[type] = file;
            }
            else
            {
                _innerFiles.Add(file.GetType(), file);
            }
        }

        public Task<string> GetNameAsync(CancellationToken cancellationToken = default)
        {
            return (GetIfExist<ILocalFile>()
                    ?? GetIfExist<IWebFile>()
                    ?? GetIfExist<IOutgoingFile>()
                    ?? _innerFiles.Values.First())
                .GetNameAsync(cancellationToken);
        }

        public Task<Stream> OpenReadStreamAsync(CancellationToken cancellationToken = default)
        {
            return (GetIfExist<ILocalFile>()
                    ?? GetIfExist<IWebFile>()
                    ?? GetIfExist<IOutgoingFile>()
                    ?? _innerFiles.Values.First())
                .OpenReadStreamAsync(cancellationToken);
        }

        public IFile Prefer<TFile>()
            where TFile : class, IFile
        {
            return (IFile) GetIfExist<TFile>() ?? this;
        }

        public TFile GetIfExist<TFile>()
            where TFile : class, IFile
        {
            TFile resultFile = null;

            if (_innerFiles.TryGetValue(typeof(TFile), out var fileValue))
                resultFile = fileValue as TFile;

            return resultFile ?? _innerFiles.Values.FirstOrDefault(f => f is TFile) as TFile;
        }
    }
}