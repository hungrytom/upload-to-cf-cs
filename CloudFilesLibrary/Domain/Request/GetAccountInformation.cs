//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

using Rackspace.CloudFiles.Domain.Request.Interfaces;
using Rackspace.CloudFiles.Utils;

namespace Rackspace.CloudFiles.Domain.Request
{
    #region Using
    using System;
    using System.ComponentModel;
    using Request.Interfaces;
    using Utils;
    #endregion

    /// <summary>
    /// A class to represent getting account information in a web request
    /// </summary>
    public class GetAccountInformation : IAddToWebRequest
    {
        private readonly string _storageUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAccountInformation"/> class.
        /// </summary>
        /// <param name="storageUrl">The storage URL.</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters are null</exception>
        public GetAccountInformation(string storageUrl)
        {
            _storageUrl = storageUrl;

            if (string.IsNullOrEmpty(storageUrl))
            {
                throw new ArgumentNullException();
            }
        }

        /// <summary>
        /// Creates the URI.
        /// </summary>
        /// <returns>A new URI for this container</returns>
        public Uri CreateUri()
        {
            return new Uri(_storageUrl + "/");
   
        }

        /// <summary>
        /// Applies the apropiate method to the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Apply(ICloudFilesRequest request)
        {
            request.Method = "HEAD";
        }
    }

    public enum Format
    {
        [Description("json")]
        JSON,
        [Description("xml")]
        XML
    }

    /// <summary>
    /// A class to represent getting serialzed account information in a web request
    /// </summary>
    public class GetAccountInformationSerialized : IAddToWebRequest
    {
        private readonly string _storageUrl;
        private readonly Format _format;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAccountInformationSerialized"/> class.
        /// </summary>
        /// <param name="storageUrl">The storage URL.</param>
        /// <param name="format">The requested format to receive for this request <see cref="Format"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters are null</exception>
        public GetAccountInformationSerialized(string storageUrl, Format format)
        {
            if (string.IsNullOrEmpty(storageUrl))
            {
                throw new ArgumentNullException();
            }

            _storageUrl = storageUrl;
            _format = format;
        }

        /// <summary>
        /// Creates the URI.
        /// </summary>
        /// <returns>A new URI for this container</returns>
        public Uri CreateUri()
        {
           return new Uri(_storageUrl + "?format=" + EnumHelper.GetDescription(_format));
        }

        /// <summary>
        /// Applies the apropiate method to the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Apply(ICloudFilesRequest request)
        {
            request.Method = "GET";
        }
    }
}