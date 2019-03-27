using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Filters
{
    public class CompressedContent : HttpContent
    {
        private readonly string _encondingType;
        private readonly HttpContent _originalContent;

        public CompressedContent(HttpContent content, string encondingType = "gzip")
        {
            if (content == null)
                throw new ArgumentNullException("content");

            _originalContent = content;
            _encondingType = encondingType.ToLowerInvariant();
            foreach (var header in _originalContent.Headers)
            {
                Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            Headers.ContentEncoding.Add(encondingType);
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            Stream compressedStream = null;
            switch (_encondingType)
            {
                case "gzip":
                    compressedStream = new GZipStream(stream, CompressionMode.Compress, true);
                    break;
                case "deflate":
                    compressedStream = new DeflateStream(stream, CompressionMode.Compress, true);
                    break;
                default:
                    break;
            }
            
            return _originalContent.CopyToAsync(compressedStream).ContinueWith(tsk =>
            {
                if (compressedStream != null)
                {
                    compressedStream.Dispose();
                }
            });
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }
    }
}
