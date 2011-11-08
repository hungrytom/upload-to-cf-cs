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
    /// A class to represent deleting a storage item in a web request
    /// </summary>
    public class DeleteStorageItem : IAddToWebRequest
    {
        private readonly string _url;
        private readonly string _containerName;
        private readonly string _storageItemName;
        private readonly string[] _emailAddresses;

        public DeleteStorageItem(string url, string containerName, string storageItemName) : this(url, containerName, storageItemName, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteStorageItem"/> class.
        /// </summary>
        /// <param name="url">the customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">the name of the container where the storage item is located</param>
        /// <param name="storageItemName">the name of the storage item to add meta information too</param>
        /// <param name="emailAddresses">the email addresses to notify who</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        /// <exception cref="ContainerNameException">Thrown when the container name is invalid</exception>
        /// <exception cref="StorageItemNameException">Thrown when the object name is invalid</exception>
        public DeleteStorageItem(string url,  string containerName, string storageItemName, string[] emailAddresses)
        {
            if (string.IsNullOrEmpty(url)
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

            _url = url;
            _containerName = containerName;
            _storageItemName = storageItemName;
            _emailAddresses = emailAddresses;
        }

        /// <summary>
        /// Creates the URI.
        /// </summary>
        /// <returns>A new URI for this container</returns>
        public Uri CreateUri()
        {
            return new Uri(_url + "/" + _containerName.Encode() + "/" + _storageItemName.StripSlashPrefix().Encode());
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