//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

namespace Rackspace.CloudFiles.Domain.Request
{
    using System;
    using Request.Interfaces;
    using Exceptions;
    using Utils;

    /// <summary>
    /// A class to represent deleting a container in a web request
    /// </summary>
    public class DeleteContainer : IAddToWebRequest
    {
        private readonly string _url;
        private readonly string _containerName;
        private readonly string[] _emailAddresses;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteContainer"/> class.
        /// </summary>
        /// <param name="url">The customer unique url or public url to interact with cloudfiles</param>
        /// <param name="containerName">The name of the container where the storage item is located</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        /// <exception cref="ContainerNameException">Thrown when the container name is invalid</exception>
        /// <exception cref="StorageItemNameException">Thrown when the object name is invalid</exception>
        public DeleteContainer(string url,  string containerName) : this(url, containerName, null)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteContainer"/> class.
        /// </summary>
        /// <param name="url">The customer unique url or public url to interact with cloudfiles</param>
        /// <param name="containerName">The name of the container where the storage item is located</param>
        /// <param name="emailAddresses">The email addressese to notify once deletion is complete</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        /// <exception cref="ContainerNameException">Thrown when the container name is invalid</exception>
        /// <exception cref="StorageItemNameException">Thrown when the object name is invalid</exception>
        public DeleteContainer(string url, string containerName, string[] emailAddresses)
        {
            _url = url;
            _containerName = containerName;
            _emailAddresses = emailAddresses;

            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException();
            }

            if (!ContainerNameValidator.Validate(containerName))
            {
                throw new ContainerNameException();
            }
        }

        /// <summary>
        /// Creates the URI.
        /// </summary>
        /// <returns>A new URI for this container</returns>
        public Uri CreateUri()
        {
            return new Uri(_url + "/" + _containerName.Encode());
        }

        /// <summary>
        /// Applies the apropiate method to the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Apply(ICloudFilesRequest request)
        {
            request.Method = "DELETE";
            if(_emailAddresses != null && _emailAddresses.Length > 0)
            {
                request.Headers.Add(Constants.X_PURGE_EMAIL, string.Join(",", _emailAddresses));
            }
        }
    }
}