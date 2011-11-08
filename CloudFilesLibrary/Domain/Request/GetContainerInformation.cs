//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

using Rackspace.CloudFiles.Domain.Request.Interfaces;
using Rackspace.CloudFiles.Exceptions;
using Rackspace.CloudFiles.Utils;

namespace Rackspace.CloudFiles.Domain.Request
{
    #region Using
    using System;
    using Request.Interfaces;
    using Exceptions;
    using Utils;
    #endregion

    /// <summary>
    /// A class to represent getting a container's information in a web request
    /// </summary>
    public class GetContainerInformation : IAddToWebRequest
    {
        private readonly string _storageUrl;
        private readonly string _containerName;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetContainerInformation"/> class.
        /// </summary>
        /// <param name="storageUrl">The customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">The name of the container where the storage item is located</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        /// <exception cref="ContainerNameException">Thrown when the container name is invalid</exception>
        public GetContainerInformation(string storageUrl,  string containerName)
        {
            if (string.IsNullOrEmpty(storageUrl) || string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException();
            }

            if (!ContainerNameValidator.Validate(containerName)) throw new ContainerNameException();

            _storageUrl = storageUrl;
            _containerName = containerName;
        }

        /// <summary>
        /// Creates the corresponding URI for this request using this container.
        /// </summary>
        /// <returns>A new URI</returns>
        public Uri CreateUri()
        {
            return new Uri(_storageUrl + "/" + _containerName.Encode());
        }

        /// <summary>
        /// Applies the method to the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Apply(ICloudFilesRequest request)
        {
            request.Method = "HEAD";
        }
    }

    /// <summary>
    /// A class to represent getting a container's serialized information in a web request
    /// </summary>
    public class GetContainerInformationSerialized : IAddToWebRequest
    {
        private readonly string _storageUrl;
        private readonly string _containerName;
        private readonly Format _format;

        /// <summary>
        /// GetContainerInformationSerialized constructor
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters are null</exception>
        public GetContainerInformationSerialized(string storageUrl, string containerName, Format format)
        {
            if (string.IsNullOrEmpty(storageUrl) || string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException();
            }

            if (!ContainerNameValidator.Validate(containerName))
            {
                throw new ContainerNameException();
            }

            _storageUrl = storageUrl;
            _containerName = containerName;
            _format = format;
        }

        /// <summary>
        /// Creates the corresponding URI for this request using this container and format indicator.
        /// </summary>
        /// <returns>A new URI</returns>
        public Uri CreateUri()
        {
            return new Uri(_storageUrl + "/" + _containerName.Encode() + "?format=" + EnumHelper.GetDescription(_format));
        }

        /// <summary>
        /// Applies the appropiate method to the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Apply(ICloudFilesRequest request)
        {
             request.Method = "GET";
        }
    }
}