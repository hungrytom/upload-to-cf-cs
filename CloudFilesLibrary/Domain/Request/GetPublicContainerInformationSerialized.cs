//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

using Rackspace.CloudFiles.Domain.Request.Interfaces;
using Rackspace.CloudFiles.Utils;

namespace Rackspace.CloudFiles.Domain.Request
{
    #region
    using System;
    using Request.Interfaces;
    using Utils;
    #endregion

    /// <summary>
    /// A class to represent getting a public container's serialized information in a web request
    /// </summary>
    public class GetPublicContainersInformationSerialized: IAddToWebRequest
    {
        private readonly string _cdnManagementurl;
        private readonly Format _format;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPublicContainersInformationSerialized"/> class.
        /// </summary>
        /// <param name="cdnManagementurl">The CDN managementurl.</param>
        /// <param name="format">The format.</param>
        public GetPublicContainersInformationSerialized(string cdnManagementurl, Format format)
        {
            _cdnManagementurl = cdnManagementurl;
            _format = format;
        }

        /// <summary>
        /// Creates the corresponding URI for this request using the format descriptor.
        /// </summary>
        /// <returns>A new URI</returns>
        public Uri CreateUri()
        {
            var endurl = ("?format=" + EnumHelper.GetDescription(_format) + "&enabled_only=true");
            return new Uri(_cdnManagementurl + endurl);
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