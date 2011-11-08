//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

namespace Rackspace.CloudFiles.Domain.Request.Interfaces
{
    #region Using
    using System;
    using System.Net;
    #endregion

    /// <summary>
    /// Represents a specific type of api request
    /// </summary>
    public interface IAddToWebRequest
    {
        /// <summary>
        /// Creates the corresponding URI for this request.
        /// </summary>
        /// <returns>A new URI</returns>
        Uri CreateUri();

        /// <summary>
        /// Applies the appropiate properties to the specified request for this implementation.
        /// </summary>
        /// <param name="request">The request.</param>
        void Apply(ICloudFilesRequest request);
    }
}