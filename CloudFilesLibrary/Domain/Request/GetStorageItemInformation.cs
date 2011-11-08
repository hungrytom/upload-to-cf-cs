//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

namespace Rackspace.CloudFiles.Domain.Request
{

    using System;
    using Interfaces;
    using Exceptions;
    using Utils;

    /// <summary>
    /// A class to represent getting a set of public containers in a web request
    /// </summary>
    public class GetStorageItemInformation : IAddToWebRequest
    {
        private readonly string _storageUrl;
        private readonly string _containerName;
        private readonly string _storageItemName;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetStorageItemInformation"/> class.
        /// </summary>
        /// <param name="storageUrl">The customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">The name of the container where the storage item is located</param>
        /// <param name="storageItemName">The name of the storage item to add meta information too</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        /// <exception cref="ContainerNameException">Thrown when the container name is invalid</exception>
        /// <exception cref="StorageItemNameException">Thrown when the object name is invalid</exception>
        public GetStorageItemInformation(string storageUrl, string containerName, string storageItemName)
        {
            if (string.IsNullOrEmpty(storageUrl)
                || string.IsNullOrEmpty(containerName)
                || string.IsNullOrEmpty(storageItemName))
            {
                throw new ArgumentNullException();
            }

            if (!ContainerNameValidator.Validate(containerName))
            {
                throw new ContainerNameException();
            }
            if (!ObjectNameValidator.Validate(storageItemName))
            {
                throw new StorageItemNameException();
            } 
            
            _storageUrl = storageUrl;
            _containerName = containerName;
            _storageItemName = storageItemName;
        }

        /// <summary>
        /// Creates the corresponding URI for this request using storage item name.
        /// </summary>
        /// <returns>A new URI</returns>
        public Uri CreateUri()
        {
            return new Uri(_storageUrl + "/" + _containerName.Encode() + "/" + _storageItemName.Encode());
        }

        /// <summary>
        /// Applies the appropiate method to the specified request for this implementation.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Apply(ICloudFilesRequest request)
        {
            request.Method = "HEAD";
        }
    }
}