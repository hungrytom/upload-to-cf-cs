//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

namespace Rackspace.CloudFiles.Domain.Request
{

    #region Using
    using System;
    using Request.Interfaces;
    #endregion

    /// <summary>
    /// A class to represent getting a set of public containers in a web request
    /// </summary>
    public class GetPublicContainers : IAddToWebRequest
    {
        private readonly string _cdnManagementUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPublicContainers"/> class.
        /// </summary>
        /// <param name="cdnManagementUrl">The CDN management URL.</param>
        public GetPublicContainers(string cdnManagementUrl)
        {
            if (string.IsNullOrEmpty(cdnManagementUrl))
            {
                throw new ArgumentNullException();
            }

            _cdnManagementUrl = cdnManagementUrl;

        }

        /// <summary>
        /// Creates the corresponding URI for this request.
        /// </summary>
        /// <returns>A new URI</returns>
        public Uri CreateUri()
        {
            return  new Uri(_cdnManagementUrl + "?enabled_only=true");
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