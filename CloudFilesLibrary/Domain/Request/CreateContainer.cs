//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

using System.Linq;

namespace Rackspace.CloudFiles.Domain.Request
{

    using System;
    using Interfaces;
    using Exceptions;
    using Utils;
    using System.Collections.Generic;

    /// <summary>
    /// A class to represent creating a container in a web request
    /// </summary>
    public class CreateContainer : IAddToWebRequest
    {
        private readonly string _storageUrl;
        private readonly string _containerName;
        private Dictionary<string, string> _metadata;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateContainer"/> class.
        /// </summary>
        /// <param name="storageUrl">The customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">The name of the container where the storage item is located</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference arguments are null</exception>
        /// <exception cref="ContainerNameException">Thrown when the container name is invalid</exception>
        /// <exception cref="StorageItemNameException">Thrown when the object name is invalid</exception>
        public CreateContainer(string storageUrl, string containerName)
            : this(storageUrl, containerName, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateContainer"/> class.
        /// </summary>
        /// <param name="storageUrl">The customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">The name of the container to create.</param>
        /// <param name="metadata">The metadata to associate with the new container.</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference arguments are null</exception>
        /// <exception cref="ContainerNameException">Thrown when the container name is invalid</exception>
        /// <exception cref="StorageItemNameException">Thrown when the object name is invalid</exception>
        public CreateContainer(string storageUrl, string containerName, Dictionary<String, String> metadata)
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
            _metadata = metadata;
        }

        /// <summary>
        /// Creates the URI.
        /// </summary>
        /// <returns>A new URI for this container</returns>
        public Uri CreateUri()
        {
            return new Uri(_storageUrl + "/" + _containerName.Encode());
        }

        /// <summary>
        /// Applies the appropiate method to the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Apply(ICloudFilesRequest request)
        {
            request.Method = "PUT";

            if ((_metadata != null) && (_metadata.Count > 0))
            {
                foreach (var m in _metadata.Where(m => (!String.IsNullOrEmpty(m.Key)) && (!String.IsNullOrEmpty(m.Value))))
                {
                    if (m.Key.ToLower().StartsWith(Constants.X_CONTAINTER_META_DATA_HEADER))
                    {
                        // make sure the metadata item isn't just the container metadata prefix string
                        if (m.Key.Length > Constants.X_CONTAINTER_META_DATA_HEADER.Length)
                        {
                            // If the caller already added the container metadata prefix string,
                            // add their key as is.
                            request.Headers.Add(m.Key, m.Value);
                        }
                    }
                    else
                    {
                        request.Headers.Add(Constants.X_CONTAINTER_META_DATA_HEADER + m.Key, m.Value);
                    }
                }
            }

        }
    }
}