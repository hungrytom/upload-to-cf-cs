//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

namespace Rackspace.CloudFiles.Domain
{
    #region Using
    using System;
    #endregion

    /// <summary>
    /// Encapsulates customer's account information including the container count and bytes used.
    /// </summary>
    public class AccountInformation
    {
        /// <summary>
        /// Initializes a new instance of AccountInformation class for customer
        /// </summary>
        /// <param name="containerCount">The number of containers a customer owns</param>
        /// <param name="bytesUsed">The bytes used by a customer</param>
        /// <exception cref="System.ArgumentNullException">Thrown when any of the reference arguments are null</exception>
        public AccountInformation(string containerCount, string bytesUsed)
        {
            if (string.IsNullOrEmpty(containerCount) ||
                string.IsNullOrEmpty(bytesUsed))
            {
                throw new ArgumentNullException();
            }

            this.ContainerCount = int.Parse(containerCount);
            this.BytesUsed = long.Parse(bytesUsed);
        }

        /// <summary>
        /// Gets or sets the number of the containers a customer owns
        /// </summary>
        /// <return>number of containers</return>
        public int ContainerCount { get; set; }

        /// <summary>
        /// Gets or sets the bytes used by a customer
        /// </summary>
        /// <return>number of bytes</return>
        public long BytesUsed { get; set; }
    }
}