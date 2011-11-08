//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

using Rackspace.CloudFiles.Domain.Request.Interfaces;

namespace Rackspace.CloudFiles.Domain.Request
{

    #region
    using System;
    using System.IO;
    using Request.Interfaces;
    using Utils;
    #endregion

    /// <summary>
    /// A class to represent putting a storage directory with a web request
    /// </summary>
    public class PutStorageDirectory:IAddToWebRequest
    {
        private readonly string _storageurl;
        private readonly string _containername;
        private readonly string _objname;

        /// <summary>
        /// Initializes a new instance of the <see cref="PutStorageDirectory"/> class to "upsert" a new storage directory.
        /// </summary>
        /// <param name="storageurl">The storageurl.</param>
        /// <param name="containername">The containername.</param>
        /// <param name="objname">The objname.</param>
        public PutStorageDirectory(string storageurl, string containername, string objname)
        {
            _storageurl = storageurl;
            _containername = containername;
            _objname = objname;
        }

        /// <summary>
        /// Creates the corresponding URI for this request.
        /// </summary>
        /// <returns>A new URI</returns>
        public Uri CreateUri()
        {
             return new Uri(_storageurl + "/" + _containername.Encode() + "/" + _objname.StripSlashPrefix().Encode());
        }

        /// <summary>
        /// Applies the appropiate properties to the specified request for this implementation.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Apply(ICloudFilesRequest request)
        {
            request.Method = "PUT";
            request.ContentType = "application/directory";
            request.SetContent(new MemoryStream(new byte[0]), delegate { }); 
        }
    }
}