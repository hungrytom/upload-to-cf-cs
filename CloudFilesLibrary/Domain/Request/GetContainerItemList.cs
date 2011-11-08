//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

namespace Rackspace.CloudFiles.Domain.Request
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Request.Interfaces;
    using Exceptions;
    using Utils;

    /// <summary>
    /// A class to represent getting a container's item list in a web request
    /// </summary>
    public class GetContainerItemList : IAddToWebRequest
    {
        private readonly string _storageUrl;
        private readonly string _containerName;
        private readonly StringBuilder _stringBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetContainerItemList"/> class.
        /// </summary>
        /// <param name="storageUrl">The customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">The name of the container where the storage item is located</param>
        /// <param name="requestParameters">dictionary of parameter filters to place on the request url</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        /// <exception cref="ContainerNameException">Thrown when the container name is invalid</exception>
        public GetContainerItemList(
            string storageUrl,
            string containerName, 
            Dictionary<GetListParameters, string> requestParameters)
        {
            _storageUrl = storageUrl;
            _containerName = containerName;

            if (string.IsNullOrEmpty(storageUrl) || string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException();
            }

            if (!ContainerNameValidator.Validate(containerName))
            {
                throw new ContainerNameException();
            }

            // Stop here if no request parameters
            if (requestParameters == null || requestParameters.Count <= 0)
            {
                return;
            } 

            _stringBuilder = new StringBuilder();

            foreach (var param in requestParameters.Keys)
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
        /// Initializes a new instance of the <see cref="GetContainerItemList"/> class.
        /// </summary>
        /// <param name="storageUrl">The customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">The name of the container where the storage item is located</param>
        public GetContainerItemList(string storageUrl, string containerName)
            : this(storageUrl, containerName, null)
        {
        }

        /// <summary>
        /// Creates the corresponding URI using this container and item list parameters.
        /// </summary>
        /// <returns>A new URI</returns>
        public Uri CreateUri()
        {
           return new Uri(_storageUrl + "/" + _containerName.Encode() + _stringBuilder);
        }

        /// <summary>
        /// Applies the appropiate request method.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Apply(ICloudFilesRequest request)
        {
            request.Method = "GET";
        }
    }
}