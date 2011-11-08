//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Net;
using Rackspace.CloudFiles.Utils;

namespace Rackspace.CloudFiles.Domain
{
    public class StorageItemInformation
    {
        private readonly WebHeaderCollection headers;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="headers">collection of headers assigned to this storage item</param>
        public StorageItemInformation(WebHeaderCollection headers)
        {
            this.headers = headers;
        }

        /// <summary>
        /// entity tag used to determine if any content changed in transfer - http://en.wikipedia.org/wiki/HTTP_ETag
        /// </summary>
        public string ETag
        {
            get { return headers[Constants.ETAG]; }
        }

        /// <summary>
        /// http content type of the storage item
        /// </summary>
        public string ContentType
        {
            get { return headers[Constants.CONTENT_TYPE_HEADER]; }
        }

        /// <summary>
        /// http content length of the storage item
        /// </summary>
        public string ContentLength
        {
            get { return headers[Constants.CONTENT_LENGTH_HEADER]; }
        }

        /// <summary>
        /// dictionary of meta tags assigned to this storage item
        /// </summary>
        public Dictionary<string, string> Metadata
        {
            get
            {
                var tags = new Dictionary<string, string>();
                foreach (string s in from string s in headers.Keys where s.StartsWith(Constants.META_DATA_HEADER, System.StringComparison.InvariantCultureIgnoreCase) where s.Length > Constants.META_DATA_HEADER.Length select s)
                {
                    tags[s.Substring(Constants.META_DATA_HEADER.Length)] = headers[s];
                }
                return tags;
            }
        }
    }
}