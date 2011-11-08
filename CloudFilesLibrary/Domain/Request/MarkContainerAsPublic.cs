//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

namespace Rackspace.CloudFiles.Domain.Request
{
    using System;
    using Interfaces;
    using Utils;

    /// <summary>
    /// A class to represent marking a container public in a web request
    /// </summary>
    public class MarkContainerAsPublic : IAddToWebRequest
    {
        private readonly string _cdnManagementUrl;
        private readonly string _containerName;
        private readonly int _timeToLiveInSeconds;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkContainerAsPublic"/> class to add this container to the list of containers to be served up publicly.
        /// </summary>
        /// <param name="cdnManagementUrl">The URL that will be used for accessing content from CloudFS</param>
        /// <param name="containerName">The name of the container to make public on the CDN</param>
        public MarkContainerAsPublic(string cdnManagementUrl, string containerName) : this(cdnManagementUrl, containerName, -200)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkContainerAsPublic"/> class to add this container to the list of containers to be served up publicly.
        /// </summary>
        /// <param name="cdnManagementUrl">The URL that will be used for accessing content from CloudFS</param>
        /// <param name="containerName">The name of the container to make public on the CDN</param>
        /// <param name="timeToLiveInSeconds">The maximum time (in seconds) content should be kept alive on the CDN before it checks for freshness.</param>
        public MarkContainerAsPublic(string cdnManagementUrl, string containerName, int timeToLiveInSeconds)
        {
            if (string.IsNullOrEmpty(cdnManagementUrl)
              || string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException();
            }

            _cdnManagementUrl = cdnManagementUrl;
            _containerName = containerName;
            _timeToLiveInSeconds = timeToLiveInSeconds;
        }

        /// <summary>
        /// Creates the corresponding URI for this request.
        /// </summary>
        /// <returns>A new URI</returns>
        public Uri CreateUri()
        {
            return new Uri(_cdnManagementUrl + "/" + _containerName.Encode());
        }

        /// <summary>
        /// Applies the appropiate method and headers to the specified request for this implementation.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Apply(ICloudFilesRequest request)
        {
            request.Method = "PUT";
            request.Headers.Add(Constants.X_CDN_ENABLED, "true".Capitalize());
            if (_timeToLiveInSeconds > -1) request.Headers.Add(Constants.X_CDN_TTL, _timeToLiveInSeconds.ToString());
        }
    }
}