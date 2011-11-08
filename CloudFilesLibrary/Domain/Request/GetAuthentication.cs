//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

using Rackspace.CloudFiles.Domain.Request.Interfaces;
using Rackspace.CloudFiles.Utils;

namespace Rackspace.CloudFiles.Domain.Request
{
    #region Using
    using System;
    using Request.Interfaces;
    using Utils;
    #endregion

    /// <summary>
    /// A class to represent getting authentication in a web request
    /// </summary>
    public class GetAuthentication : IAddToWebRequest
    {
        private readonly UserCredentials _userCredentials;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAuthentication"/> class.
        /// </summary>
        /// <param name="userCreds">the UserCredentials instace to use when attempting authentication</param>
        /// <exception cref="System.ArgumentNullException">Thrown when any of the reference arguments are null</exception>
        public GetAuthentication(UserCredentials userCreds)
        {
            if (userCreds == null)
            {
                throw new ArgumentNullException();
            }
            _userCredentials = userCreds;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAuthentication"/> class.
        /// </summary>
        /// <param name="userCreds">the UserCredentials instace to use when attempting authentication</param>
        /// <param name="timeout">The amount of time to wait for the request to complete.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if userCreds parameter is null</exception>
        public GetAuthentication(UserCredentials userCreds, TimeSpan? timeout)
        {
            if (userCreds == null)
            {
                throw new ArgumentNullException();
            }
            _userCredentials = userCreds;
        }

        /// <summary>
        /// Creates the corresponding URI using user credentials for authentication.
        /// </summary>
        /// <returns>A new URI</returns>
        public Uri CreateUri()
        {
            if (_userCredentials == null)
            {
                throw new ArgumentNullException();
            }

            var uri = string.IsNullOrEmpty(_userCredentials.AccountName)
                ? _userCredentials.AuthUrl
                : new Uri(_userCredentials.AuthUrl + "/"
                    + _userCredentials.Cloudversion.Encode() + "/"
                    + _userCredentials.AccountName.Encode() + "/auth");

            return uri;
        }


        /// <summary>
        /// Applies the corresponding method and headers for this authentication request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Apply(ICloudFilesRequest request)
        {
            request.Method = "GET";
            request.Headers.Add(Constants.X_AUTH_USER, _userCredentials.Username.Encode());
            request.Headers.Add(Constants.X_AUTH_KEY, _userCredentials.Api_access_key.Encode());

            request.Timeout = this.Timeout;
        }

        /// <summary>
        /// Gets/Sets the request timeout.
        /// </summary>
        /// <remarks>
        /// If Timeout is set to null, System.Threading.Timeout.Infinite 
        /// will be used as the timeout.
        /// </remarks>
        public TimeSpan? Timeout { get; set; }
    }
}