//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

namespace Rackspace.CloudFiles.Domain.Request.Interfaces
{
    using System;
    using System.IO;
    using System.Net;
    using Response.Interfaces;

    public interface ICloudFilesRequest
    {
        /// <summary>
        /// Gets the cloud files response for this request.
        /// </summary>
        /// <returns>The response</returns>
        ICloudFilesResponse GetResponse();

        /// <summary>
        /// Gets the request URI.
        /// </summary>
        /// <value>The request URI.</value>
        Uri RequestUri { get; }

        /// <summary>
        /// Gets or sets the HTTP method to use for this request.
        /// </summary>
        /// <value>The method.</value>
        string Method { get; set; }

        /// <summary>
        /// Gets or sets the HTTP headers to use for this request.
        /// </summary>
        /// <value>The headers.</value>
        WebHeaderCollection Headers { get; set; }

        /// <summary>
        /// Gets the length of the content to send for this request.
        /// </summary>
        /// <value>The length of the content.</value>
        long ContentLength { get;  }

        /// <summary>
        /// Gets or sets the RangeTo
        /// </summary>
        /// <value>The RangeTo.</value>
        int RangeTo { get; set; }

        /// <summary>
        /// Gets or sets the RangeFrom
        /// </summary>
        /// <value>The RangeFrom.</value>
        int RangeFrom { get; set; }

        /// <summary>
        /// Gets or sets the ContentType for any content sent in this request 
        /// </summary>
        /// <value>The ContentType.</value>
        string ContentType { get; set; }

        /// <summary>
        /// Gets or sets if modified since.
        /// </summary>
        /// <value>If modified since.</value>
        DateTime IfModifiedSince { get; set; }

        /// <summary>
        /// Gets the ETag
        /// </summary>
        /// <value>The ETag value.</value>
        string ETag { get;  }

        /// <summary>
        /// Gets or sets a value indicating whether to allow write stream buffering.
        /// </summary>
        /// <value><c>true</c> if [allow write stream buffering]; otherwise, <c>false</c>.</value>
        bool AllowWriteStreamBuffering { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to send chunked data
        /// </summary>
        /// <value><c>true</c> if [send chunked]; otherwise, <c>false</c>.</value>
        bool SendChunked { get; set; }

        /// <summary>
        /// Gets the content stream.
        /// </summary>
        /// <value>The content stream.</value>
        Stream ContentStream { get; }

        /// <summary>
        /// Gets/Sets the amount of time to wait for the request to complete.
        /// </summary>
        /// <remarks>Setting Timeout to null causes the System.Threading.Timeout.Infinit value to be used.</remarks>
        TimeSpan? Timeout { get; set; }

        /// <summary>
        /// Gets the request stream.
        /// </summary>
        /// <returns>The request stream.</returns>
        Stream GetRequestStream();

        /// <summary>
        /// Sets the content from a stream into the request.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="progress">The progress callback to report on the amount of data that's been uploaded.</param>
        void SetContent(Stream stream, Connection.ProgressCallback progress);
    }
}