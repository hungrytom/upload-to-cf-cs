///
/// See COPYING file for licensing information
///

using System;

namespace Rackspace.CloudFiles.Exceptions
{
    /// <summary>
    /// This exception is thrown when the Range header contains invalid range values
    /// </summary>
    public class InvalidRangeHeaderException : Exception
    {

        /// <summary>
        /// The default constructor
        /// </summary>
        public InvalidRangeHeaderException()
        {
        }

        /// <summary>
        /// An explicit constructor for describing the acceptable range header format
        /// </summary>
        /// <param name="msg">Used to more explicitly indicate the reason for failure</param>
        public InvalidRangeHeaderException(string msg):base(msg) {}     
    }
}