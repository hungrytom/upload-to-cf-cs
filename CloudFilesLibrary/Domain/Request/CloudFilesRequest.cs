//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

namespace Rackspace.CloudFiles.Domain.Request
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;
    using Interfaces;
    using Response;
    using Response.Interfaces;
    using Utils;

    /// <summary>
    /// Wraps requests to optionally handle proxy credentials and SSL.
    /// </summary>
    public class CloudFilesRequest : ICloudFilesRequest
    {
        private readonly HttpWebRequest _httpWebRequest;

        private readonly ProxyCredentials _proxyCredentials;

        private TimeSpan? _timeout;

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudFilesRequest"/> class.
        /// </summary>
        /// <param name="uri">The URI for a creating a web request.</param>
        public CloudFilesRequest(Uri uri)
            : this(WebRequest.Create(uri) as HttpWebRequest)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudFilesRequest"/> class without proxy creditials provided.
        /// </summary>
        /// <param name="request">The request being sent to the server</param>
        public CloudFilesRequest(HttpWebRequest request)
            : this(request, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudFilesRequest"/> class with proxy creditials provided.
        /// </summary>
        /// <param name="request">The request being sent to the server</param>
        /// <param name="proxyCredentials">Proxy credentials</param>
        /// <exception cref="System.ArgumentNullException">Thrown when any of the reference arguments are null</exception>
        public CloudFilesRequest(HttpWebRequest request, ProxyCredentials proxyCredentials)
        {
            if (request == null)
            {
                throw new ArgumentNullException();
            }

            _httpWebRequest = request;
            _proxyCredentials = proxyCredentials;
            if(request.Headers == null)
            {
                request.Headers = new WebHeaderCollection();
            }
        }

        private event Connection.ProgressCallback Progress;

        /// <summary>
        /// Gets the content stream.
        /// </summary>
        /// <value>The content stream.</value>
        public Stream ContentStream { get; private set; }

        /// <summary>
        /// Gets the type of the request.
        /// </summary>
        /// <value>The type of the request.</value>
        public Type RequestType
        {
            get { return _httpWebRequest.GetType(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to send chunked data
        /// </summary>
        /// <value><c>true</c> if [send chunked]; otherwise, <c>false</c>.</value>
        public bool SendChunked
        {
            get { return _httpWebRequest.SendChunked; }
            set { _httpWebRequest.SendChunked = value; }
        }

        /// <summary>
        /// Gets the request URI.
        /// </summary>
        /// <value>The request URI.</value>
        public Uri RequestUri
        {
            get { return _httpWebRequest.RequestUri; }
        }

        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        /// <value>The method.</value>
        public string Method
        {
            get { return _httpWebRequest.Method; }
            set { _httpWebRequest.Method = value; }
        }

        /// <summary>
        /// Gets the HTTP headers.
        /// </summary>
        /// <value>The headers.</value>
        public WebHeaderCollection Headers
        {
            get { return _httpWebRequest.Headers; }
            set { _httpWebRequest.Headers = value; }
        }

        /// <summary>
        /// Gets the length of the request content.
        /// </summary>
        /// <value>The length of the content.</value>
        public long ContentLength
        {
            get { return _httpWebRequest.ContentLength; }
            private set { _httpWebRequest.ContentLength = value; }
        }

        /// <summary>
        /// Gets or sets the RangeTo
        /// </summary>
        /// <value>The RangeTo.</value>
        public int RangeTo { get; set; }

        /// <summary>
        /// Gets or sets the RangeFrom
        /// </summary>
        /// <value>The RangeFrom.</value>
        public int RangeFrom { get; set; }

        /// <summary>
        /// Gets or sets the request ContentType
        /// </summary>
        /// <value>The ContentType.</value>
        public string ContentType
        {
            get { return _httpWebRequest.ContentType; }
            set { _httpWebRequest.ContentType = value; }
        }

        /// <summary>
        /// Gets or sets IfModifiedSince timestamp
        /// </summary>
        /// <value>The IfModifiedSince property.</value>
        public DateTime IfModifiedSince
        {
            get { return _httpWebRequest.IfModifiedSince; }
            set { _httpWebRequest.IfModifiedSince = value; }
        }

        /// <summary>
        /// Gets the ETag in headers.
        /// </summary>
        /// <value>The ETag value.</value>
        public string ETag
        {
            get { return this.Headers[Constants.ETAG]; }
            private set { this.Headers.Add(Constants.ETAG, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to allow write stream buffering.
        /// </summary>
        /// <value><c>true</c> if [allow write stream buffering]; otherwise, <c>false</c>.</value>
        public bool AllowWriteStreamBuffering
        {
            get { return _httpWebRequest.AllowWriteStreamBuffering; }
            set { _httpWebRequest.AllowWriteStreamBuffering = value; }
        }

        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        /// <value>The user agent.</value>
        public string UserAgent
        {
            get { return _httpWebRequest.UserAgent; }
            set { _httpWebRequest.UserAgent = value; }
        }

        /// <summary>
        /// Gets/Sets the request timeout.
        /// </summary>
        /// <remarks>
        /// If the specified value (converted to Milliseconds) exceeds Int32.MaxValue
        /// or is less than 0 and not equal to System.Threading.Timeout.Infinite,
        /// an ArgumentOutOfRangeException will be thrown.
        /// Setting the Timeout to null causes the System.Threading.Timeout.Inifinite
        /// value to be used.
        /// </remarks>
        public TimeSpan? Timeout
        {
            get { return _timeout; }
            set
            {
                if (value.HasValue)
                {
                    if (value.Value.TotalMilliseconds > Int32.MaxValue)
                    {
                        throw new ArgumentOutOfRangeException("Timeout.TotalMilliseconds cannot exceed Int32.MaxValue");
                    }

                    if ((value.Value.TotalMilliseconds < 0) &&
                        (value.Value.TotalMilliseconds != System.Threading.Timeout.Infinite))
                    {
                        throw new ArgumentOutOfRangeException("Timeout is less than 0 and is not Infinite");
                    }
                }

                _timeout = value;
            }
        }

        /// <summary>
        /// Stringifies bytes to MD5.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>An MD5 string</returns>
        private static string StringifyMD5(IEnumerable<byte> bytes)
        {
            var result = new StringBuilder();

            foreach (byte b in bytes)
            {
                result.AppendFormat("{0:x2}", b);
            }

            return result.ToString();
        }

        /// <summary>
        /// Sets the content from a stream into the request.
        /// </summary>
        /// <param name="stream">The stream of content.</param>
        /// <param name="progress">The progress callback to report on the amount of data that's been uploaded.</param>
        public void SetContent(Stream stream, Connection.ProgressCallback progress)
        {
            this.ContentStream = stream;
            this.ContentLength = stream.Length;
            this.Progress = progress;
            this.ETag = StringifyMD5(new MD5CryptoServiceProvider().ComputeHash(ContentStream));
            ContentStream.Seek(0, 0);
        }

        /// <summary>
        /// Gets the request stream.
        /// </summary>
        /// <returns>The request stream.</returns>
        public Stream GetRequestStream()
        {
            return _httpWebRequest.GetRequestStream();
        }

        /// <summary>
        /// Get the cloud files web response from the current web request.
        /// </summary>
        /// <returns>A HttpWebRequest object that has all the information to make a request against CloudFiles</returns>
        public ICloudFilesResponse GetResponse()
        {
            _httpWebRequest.Timeout = (_timeout.HasValue) ? (int)_timeout.Value.TotalMilliseconds : (System.Threading.Timeout.Infinite);
            _httpWebRequest.UserAgent = Constants.USER_AGENT;

            ////   HandleIsModifiedSinceHeaderRequestFieldFor(_httpWebRequest);

            HandleRangeHeader(_httpWebRequest);

            if (_httpWebRequest.ContentLength > 0)
            {
                AttachBodyToWebRequest(_httpWebRequest);
            }

            HandleProxyCredentialsFor(_httpWebRequest);
            return new CloudFilesResponse((HttpWebResponse)_httpWebRequest.GetResponse());
        }

        /// <summary>
        /// Handles the range header.
        /// </summary>
        /// <param name="webrequest">The webrequest.</param>
        private void HandleRangeHeader(HttpWebRequest webrequest)
        {
            if (RangeFrom != 0 && RangeTo == 0)
            {
                webrequest.AddRange("bytes", RangeFrom);
            }
            else if (RangeFrom == 0 && RangeTo != 0)
            {
                webrequest.AddRange("bytes", RangeTo);
            }
            else if (RangeFrom != 0 && RangeTo != 0)
            {
                webrequest.AddRange("bytes", RangeFrom, RangeTo);
            }
        }

        /// <summary>
        /// Handles the proxy credentials for a web request.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        private void HandleProxyCredentialsFor(WebRequest httpWebRequest)
        {
            if (_proxyCredentials == null)
            {
                return;
            }

            var loProxy = new WebProxy(_proxyCredentials.ProxyAddress, true);

            if (_proxyCredentials.ProxyUsername.Length > 0)
            {
                loProxy.Credentials = new NetworkCredential(_proxyCredentials.ProxyUsername, _proxyCredentials.ProxyPassword, _proxyCredentials.ProxyDomain);
            }

            httpWebRequest.Proxy = loProxy;
        }

        /// <summary>
        /// Attaches the body to web request.
        /// </summary>
        /// <param name="request">The request.</param>
        private void AttachBodyToWebRequest(WebRequest request)
        {
            using (var webstream = request.GetRequestStream())
            {
                var buffer = new byte[Constants.CHUNK_SIZE];

                int amt;
                while ((amt = ContentStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    webstream.Write(buffer, 0, amt);

                    // Fire the progress event
                    if (this.Progress != null)
                    {
                        this.Progress(amt);
                    }
                }

                ContentStream.Close();
                webstream.Flush();
            }
        }

    }

}