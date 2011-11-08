//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

using Rackspace.CloudFiles.Domain.Request.Interfaces;
using Rackspace.CloudFiles.Exceptions;
using Rackspace.CloudFiles.Utils;

namespace Rackspace.CloudFiles.Domain.Request
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Request.Interfaces;
    using Exceptions;
    using Utils;

    /// <summary>
    /// A class to represent putting a storage item with a web request
    /// </summary>
    public class PutStorageItem : IAddToWebRequest
    {

        private readonly string _storageUrl;
        private readonly string _containerName;
        private readonly string _remoteStorageItemName;
        private readonly Stream _fileToSend;
        private readonly Dictionary<string, string> _metadata;
        private readonly string _fileUrl;

        public event Connection.ProgressCallback Progress;

        /// <summary>
        /// Initializes a new instance of the <see cref="PutStorageItem"/> class for putting an item without a file stream.
        /// </summary>
        /// <param name="storageUrl">The customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">The name of the container where the storage item is located</param>
        /// <param name="remoteStorageItemName">The name of the storage item to add meta information too</param>
        /// <param name="localFilePath">The path of the file to put into cloudfiles</param>
        public PutStorageItem(string storageUrl, string containerName, string remoteStorageItemName, string localFilePath)
            : this(storageUrl, containerName, remoteStorageItemName, localFilePath, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PutStorageItem"/> class for putting an item without meta data.
        /// </summary>
        /// <param name="storageUrl">The customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">The name of the container where the storage item is located</param>
        /// <param name="remoteStorageItemName">The name of the storage item to add meta information too</param>
        /// <param name="filestream">The file stream of the file to put into cloudfiles</param>
        public PutStorageItem(string storageUrl, string containerName, string remoteStorageItemName, Stream filestream)
            : this(storageUrl, containerName, remoteStorageItemName, filestream, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PutStorageItem"/> class for putting an item with meta data.
        /// </summary>
        /// <param name="storageUrl">the customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">the name of the container where the storage item is located</param>
        /// <param name="remoteStorageItemName">the name of the storage item to add meta information too</param>
        /// <param name="fileToSend">the file stream of the file to put into cloudfiles</param>
        /// <param name="metadata">dictionary of meta tags to apply to the storage item</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        /// <exception cref="ContainerNameException">Thrown when the container name is invalid</exception>
        /// <exception cref="StorageItemNameException">Thrown when the object name is invalid</exception>
        public PutStorageItem(string storageUrl, string containerName,
            string remoteStorageItemName,
            Stream fileToSend,
            Dictionary<string, string> metadata)
        {
            if (string.IsNullOrEmpty(storageUrl)
                || string.IsNullOrEmpty(containerName)
                || fileToSend == null
                || string.IsNullOrEmpty(remoteStorageItemName))
            {
                throw new ArgumentNullException();
            }

            if (!ContainerNameValidator.Validate(containerName))
            {
                throw new ContainerNameException();
            }
            if (!ObjectNameValidator.Validate(remoteStorageItemName))
            {
                throw new StorageItemNameException();
            }

            _fileUrl = CleanUpFilePath(remoteStorageItemName);
            _storageUrl = storageUrl;
            _containerName = containerName;
            _remoteStorageItemName = remoteStorageItemName;
            _metadata = metadata;
            _fileToSend = fileToSend;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PutStorageItem"/> class for putting an item with a local file.
        /// </summary>
        /// <param name="storageUrl">The customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">The name of the container where the storage item is located</param>
        /// <param name="remoteStorageItemName">The name of the storage item to add meta information too</param>
        /// <param name="localFilePath">The path of the file to put into cloudfiles</param>
        /// <param name="metadata">Dictionary of meta tags to apply to the storage item</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        /// <exception cref="ContainerNameException">Thrown when the container name is invalid</exception>
        /// <exception cref="StorageItemNameException">Thrown when the object name is invalid</exception>
        public PutStorageItem(string storageUrl, string containerName, string remoteStorageItemName,
            string localFilePath,
            Dictionary<string, string> metadata)
        {
            _storageUrl = storageUrl;
            _containerName = containerName;
            _remoteStorageItemName = remoteStorageItemName;
            _metadata = metadata;

            if (string.IsNullOrEmpty(storageUrl)
                || string.IsNullOrEmpty(containerName)
                || string.IsNullOrEmpty(localFilePath)
                || string.IsNullOrEmpty(remoteStorageItemName))
            {
                throw new ArgumentNullException();
            }

            if (!ContainerNameValidator.Validate(containerName))
            {
                throw new ContainerNameException();
            }

            if (!ObjectNameValidator.Validate(remoteStorageItemName))
            {
                throw new StorageItemNameException();
            }

            _fileUrl = CleanUpFilePath(localFilePath);
            _fileToSend = new FileStream(_fileUrl, FileMode.Open, FileAccess.Read);
        }

        /// <summary>
        /// Content type for the storage item
        /// </summary>
        /// <returns>
        /// Storage item's content type
        /// </returns>
        private string ContentType()
        {
            if (String.IsNullOrEmpty(_fileUrl) || _fileUrl.IndexOf(".") < 0)
            {
                return "application/octet-stream";
            }

            return MimeHelper.GetMimeType(_fileUrl);
        }

        /// <summary>
        /// Cleans up file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>Cleaned file path</returns>
        private static string CleanUpFilePath(string filePath)
        {
            return filePath.StripSlashPrefix().Replace(@"file:\\\", "");
        }

        /// <summary>
        /// Creates the corresponding URI for this request using the storage item info.
        /// </summary>
        /// <returns>A new URI</returns>
        public Uri CreateUri()
        {
            return new Uri(_storageUrl + "/" + _containerName.Encode() + "/" + _remoteStorageItemName.StripSlashPrefix().Encode());
        }

        /// <summary>
        /// Applies the appropiate properties to the specified request for this implementation.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Apply(ICloudFilesRequest request)
        {
            _fileToSend.Position = 0;
            request.Method = "PUT";

            if (_metadata != null && _metadata.Count > 0)
            {
                foreach (var m in _metadata)
                {
                    if ((String.IsNullOrEmpty(m.Key)) || (String.IsNullOrEmpty(m.Value)))
                    {
                        continue;
                    }

                    if (m.Key.StartsWith(Constants.META_DATA_HEADER, StringComparison.InvariantCultureIgnoreCase))
                    {
                        // make sure the metadata item isn't just the container metadata prefix string
                        if (m.Key.Length > Constants.META_DATA_HEADER.Length)
                        {
                            // If the caller already added the container metadata prefix string,
                            // add their key as is.
                            request.Headers.Add(m.Key, m.Value);
                        }
                    }
                    else
                    {
                        request.Headers.Add(Constants.META_DATA_HEADER + m.Key, m.Value);
                    }
                }
            }

            request.AllowWriteStreamBuffering = false;
            request.ContentType = ContentType();
            request.SetContent(_fileToSend, Progress);
        }

    }
}