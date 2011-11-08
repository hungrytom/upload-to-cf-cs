//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Rackspace.CloudFiles.Domain.Response.Interfaces;
using Rackspace.CloudFiles.Utils;

namespace Rackspace.CloudFiles.Domain.Response
{
    /// <summary>
    /// Represents the response information from a CloudFiles request
    /// </summary>
    public class CloudFilesResponse : ICloudFilesResponse
    {
        private readonly HttpWebResponse _webResponse;
        private readonly IList<string> _contentbody = new List<string>();
        private readonly MemoryStream memstream = new MemoryStream( );
        private Stream Getstream()
        {
            memstream.Seek(0, 0);
            return memstream;
        }
        public CloudFilesResponse(HttpWebResponse webResponse)
        {
            _webResponse = webResponse;
            CopyToMemory(_webResponse.GetResponseStream(), memstream);
            if (HasTextBody())
            try
            {
                GetBody(Getstream());
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
            
        }

        private bool HasTextBody()
        {
            return (_webResponse.ContentType.Contains("application/json") ||
                _webResponse.ContentType=="application/xml"||
                Regex.Match(_webResponse.ContentType,@"^application\/xml; charset=utf-?8$").Success ||
                _webResponse.ContentType.Contains("text/plain") && _webResponse.ContentLength == -1) ||
                _webResponse.ContentType.Contains("text/plain");
        }

        private void CopyToMemory(Stream input, Stream output)
        {
            var buffer = new byte[32768];
            while (true)
            {
                int read = input.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                {
                    output.Seek(0, 0);
                    return;
                }
                output.Write(buffer, 0, read);
            }
            
        }
        private void GetBody(Stream stream)
        {

            var reader = new StreamReader(stream);
            
            string line;
            while((line = reader.ReadLine())!= null)
            {
                _contentbody.Add(line);
            }
            
        }
        /// <summary>
        /// A property representing the HTTP Status code returned from cloudfiles
        /// </summary>
        public HttpStatusCode Status { get { return _webResponse.StatusCode; } }

        /// <summary>
        /// A collection of key-value pairs representing the headers returned from the create container request
        /// </summary>
        public WebHeaderCollection Headers { get { return _webResponse.Headers; } }

        public void Close()
        {
            _webResponse.Close();
        }

        /// <summary>
        /// dictionary of meta tags assigned to this storage item
        /// </summary>
        public Dictionary<string, string> Metadata
        {
            get
            {
                var tags = new Dictionary<string, string>();
                foreach (string s in _webResponse.Headers.Keys)
                {
                    // LB- 2011-08-04
                    // previously, this code used s.LastIndexOf("-") to determine where the metadata
                    // item name starts, followed by tags.Add(s.Substring(...).
                    // That assumed that you never had metadata with a name like "audio-bit-rate"
                    //   1) your metadata item name would result in "rate"
                    //   2) If you also had an item named "video-bit-rate" the tags.Add(...) method
                    //      would throw an exception because the key "rate" already exists.
                    if (s.StartsWith(Constants.META_DATA_HEADER, StringComparison.InvariantCultureIgnoreCase))
                    {
                        tags[s.Substring(Constants.META_DATA_HEADER.Length)] = _webResponse.Headers[s];
                    }
                }
                return tags;
            }
        }

        public string Method
        {
            get { return _webResponse.Method; }
        }

        public HttpStatusCode StatusCode
        {
            get { return _webResponse.StatusCode; }
        }

        public string StatusDescription
        {
            get { return _webResponse.StatusDescription; }
        }

        public IList<string> ContentBody
        {
            get
            {
                return _contentbody;
            }
        }

        public string ContentType
        {
            get { return 
                _webResponse.ContentType; }
        }

        public string ETag
        {
            get { return _webResponse.Headers[Constants.ETAG]; }
            set { _webResponse.Headers[Constants.ETAG] = value; }
        }

        public long ContentLength
        {
            get { return _webResponse.ContentLength; }
        }

        public DateTime LastModified
        {
            get { return _webResponse.LastModified; }
        }

        public Stream GetResponseStream()
        {
            
            return  Getstream();
        }

        public void Dispose()
        {
            memstream.Close();
            _webResponse.Close();
        }

        public event Connection.ProgressCallback Progress;
    }
}