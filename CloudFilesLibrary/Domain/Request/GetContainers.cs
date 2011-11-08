//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------


using System.Text;
using Rackspace.CloudFiles.Utils;

namespace Rackspace.CloudFiles.Domain.Request
{
    #region Using
    using System;
    using Request.Interfaces;
    using System.Collections.Generic;
    #endregion

    /// <summary>
    /// A class to represent getting a set of containers in a web request
    /// </summary>
    public class GetContainers : IAddToWebRequest
    {
        private readonly string _storageUrl;
        private readonly StringBuilder _stringBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetContainers"/> class.
        /// </summary>
        /// <param name="storageUrl">The customer unique url to interact with cloudfiles</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public GetContainers(string storageUrl) : this(storageUrl, null){}

        /// <summary>
        /// Initializes a new instance of the <see cref="GetContainers"/> class.
        /// </summary>
        /// <param name="storageUrl">The customer unique url to interact with cloudfiles</param>
        /// <param name="requestParameters">dictionary of parameter filters to place on the request url</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public GetContainers(string storageUrl, Dictionary<GetListParameters, string> requestParameters)
        {
            _storageUrl = storageUrl;

            if (string.IsNullOrEmpty(storageUrl))
            {
                throw new ArgumentNullException();
            }

            // Stop here if no request parameters
            if (requestParameters == null || requestParameters.Count <= 0)
            {
                return;
            }

            _stringBuilder = new StringBuilder();

            foreach (GetListParameters param in requestParameters.Keys)
            {
                var paramName = param.ToString().ToLower();

                if (param == GetListParameters.Limit)
                {
                    int.Parse(requestParameters[param]);
                }

                if (_stringBuilder.Length > 0)
                {
                    _stringBuilder.Append("&");
                }
                else
                {
                    _stringBuilder.AppendFormat("?");
                }

                _stringBuilder.Append(paramName + "=" + requestParameters[param].Encode());
            }
        }

        /// <summary>
        /// Creates the corresponding URI for this request.
        /// </summary>
        /// <returns>A new URI</returns>
        public Uri CreateUri()
        {
            var uri = new Uri(string.Format("{0}{1}", _storageUrl, _stringBuilder));
            return uri;
        }

        /// <summary>
        /// Applies the appropiate method to the specified request for this implementation.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Apply(ICloudFilesRequest request)
        {
            request.Method = "GET";
        }
    }
}