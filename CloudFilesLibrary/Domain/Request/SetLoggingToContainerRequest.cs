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

    public class SetLoggingToContainerRequest : IAddToWebRequest
    {
        private readonly string _publiccontainer;
        private readonly string _cdnManagmentUrl;
        private readonly bool _loggingenabled;

        /// <summary>
        /// A class to represent setting logging to a container with a web request
        /// </summary>
        /// <param name="publiccontainer">The publiccontainer.</param>
        /// <param name="cdnManagmentUrl">The CDN managment URL.</param>
        /// <param name="loggingenabled">if set to <c>true</c> [loggingenabled].</param>
        public SetLoggingToContainerRequest(string publiccontainer, string cdnManagmentUrl, bool loggingenabled)
        {
            _publiccontainer = publiccontainer;
            _cdnManagmentUrl = cdnManagmentUrl;
            _loggingenabled = loggingenabled;

            if (String.IsNullOrEmpty(publiccontainer))
            {
                throw new ArgumentNullException();
            }

            if (!ContainerNameValidator.Validate(publiccontainer))
            {
                throw new ContainerNameException();
            }
        }

        /// <summary>
        /// Creates the corresponding URI for this request.
        /// </summary>
        /// <returns>A new URI</returns>
        public Uri CreateUri()
        {
            return  new Uri(_cdnManagmentUrl + "/" + _publiccontainer.Encode());
        }

        /// <summary>
        /// Applies the appropiate method and headers to the specified request for this implementation.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Apply(ICloudFilesRequest request)
        {
            request.Method = "POST";
            string enabled = _loggingenabled ? "True" : "False";

            request.Headers.Add("X-Log-Retention", enabled);
        }
    }
}
