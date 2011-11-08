//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

using Rackspace.CloudFiles.Domain.Request.Interfaces;

namespace Rackspace.CloudFiles.Domain.Request
{
    #region
    using System;
    using Request.Interfaces;
    using Utils;
    #endregion

    /// <summary>
    /// A class to represent getting a public container's information in a web request
    /// </summary>
    public class GetPublicContainerInformation : IAddToWebRequest
    {
        private readonly string _cdnManagementUrl;
        private readonly string _containerName;

        public GetPublicContainerInformation(string cdnManagementUrl,  string containerName)
        {
            if (String.IsNullOrEmpty(cdnManagementUrl) || String.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException();
            }

            _cdnManagementUrl = cdnManagementUrl;
            _containerName = containerName;
        }

        /// <summary>
        /// Creates the corresponding URI for this request using cdn management url and container name.
        /// </summary>
        /// <returns>A new URI</returns>
        public Uri CreateUri()
        {
            return new Uri(_cdnManagementUrl + "/" + _containerName.Encode() + "?enabled_only=true");
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