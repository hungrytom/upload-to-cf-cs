//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

namespace Rackspace.CloudFiles.Domain.Request
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using Request.Interfaces;
    using Exceptions;
    using Utils;
    #endregion

    /// <summary>
    /// Possible HTTP comparison header fields to apply to a request
    /// </summary>
    public enum RequestHeaderFields
    {
        [Description("If-Match")]
        IfMatch,
        [Description("If-None-Match")]
        IfNoneMatch,
        [Description("If-Modified-Since")]
        IfModifiedSince,
        [Description("If-Unmodified-Since")]
        IfUnmodifiedSince,
        [Description("Range")]
        Range
    }

    /// <summary>
    /// A class to represent getting a storage item in a web request
    /// </summary>
    public class GetStorageItem : IAddToWebRequest
    {
        private readonly string _storageUrl;
        private readonly string _containerName;
        private readonly string _storageItemName;
        private readonly Dictionary<RequestHeaderFields, string> _requestHeaderFields;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetStorageItem"/> class.
        /// </summary>
        /// <param name="storageUrl">the customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">the name of the container where the storage item is located</param>
        /// <param name="storageItemName">the name of the storage item to add meta information too</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        /// <exception cref="ContainerNameException">Thrown when the container name length exceeds the maximum container length allowed</exception>
        public GetStorageItem(string storageUrl, string containerName, string storageItemName) :
            this(storageUrl, containerName, storageItemName, null)
        {
        }

        /// Initializes a new instance of the <see cref="GetStorageItem"/> class with HTTP comparison header fields
        /// <param name="storageUrl">the customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">the name of the container where the storage item is located</param>
        /// <param name="storageItemName">the name of the storage item to add meta information too</param>
        /// <param name="requestHeaderFields">dictionary of request header fields to apply to the request</param>
        /// <exception cref="ContainerNameException">Thrown when the container name is invalid</exception>
        /// <exception cref="StorageItemNameException">Thrown when the object name is invalid</exception>
        public GetStorageItem(string storageUrl, string containerName, string storageItemName, 
            Dictionary<RequestHeaderFields, string> requestHeaderFields)
        {
            _storageUrl = storageUrl;
            _containerName = containerName;
            _storageItemName = storageItemName;
            _requestHeaderFields = requestHeaderFields;

            if (string.IsNullOrEmpty(storageUrl)
                || string.IsNullOrEmpty(containerName)
                || string.IsNullOrEmpty(storageItemName))
            {
                throw new ArgumentNullException();
            }

            if (!ContainerNameValidator.Validate(containerName))
            {
                throw new ContainerNameException();
            }
            if (!ObjectNameValidator.Validate(storageItemName))
            {
                throw new StorageItemNameException();
            }
        }

        /// <summary>
        /// Adds the request field headers to request headers.
        /// </summary>
        /// <param name="requestHeaderFields">The request header fields.</param>
        /// <param name="request">The request.</param>
        private void AddRequestFieldHeadersToRequestHeaders(ICollection<KeyValuePair<RequestHeaderFields, string>> requestHeaderFields,
            ICloudFilesRequest request)
        {
            if (requestHeaderFields == null || requestHeaderFields.Count == 0)
            {
                return;
            }

            foreach(KeyValuePair<RequestHeaderFields, string> item in requestHeaderFields)
            {
                if (!IsSpecialRequestHeaderField(item.Key))
                {
                    request.Headers.Add(EnumHelper.GetDescription(item.Key), item.Value);
                }
                if (item.Key == RequestHeaderFields.IfUnmodifiedSince)
                {
                    request.Headers.Add(EnumHelper.GetDescription(item.Key), String.Format("{0:r}", ParserDateTimeHttpHeader(item.Value)));
                    continue;
                }
                if (item.Key == RequestHeaderFields.IfModifiedSince)
                {
                    request.IfModifiedSince = ParserDateTimeHttpHeader(item.Value);
                    continue;
                }
                if (item.Key == RequestHeaderFields.Range)
                {
                    VerifyAndSplitRangeHeader(request,item.Value);
                    continue;
                }
            }
        }

        /// <summary>
        /// Determines whether {CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}[is a special request header field] [the specified key].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// 	<c>true</c> if [is special request header field] [the specified key]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsSpecialRequestHeaderField(RequestHeaderFields key)
        {
            return key == RequestHeaderFields.IfModifiedSince ||
                   key == RequestHeaderFields.Range ||
                   key == RequestHeaderFields.IfUnmodifiedSince;
        }

        /// <summary>
        /// Verifies the and split range header.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="value">The value.</param>
        private void VerifyAndSplitRangeHeader(ICloudFilesRequest request, string value)
        {
            var r = new Regex("^[0-9]*[-][0-9]*$");

            if (!r.IsMatch(value))
            {
                throw new InvalidRangeHeaderException(
                    "The range must be of the format integer-integer where either integer field is optional.");
            }

            string [] ranged = value.Split('-');

            if (ranged.Length >= 1 && ranged[0].Length > 0)
            {
                request.RangeFrom = int.Parse(ranged[0]);
            }

            if (ranged.Length == 2 && ranged[1].Length > 0)
            {
                if (ranged[0].Length == 0)
                    request.RangeTo = -int.Parse(ranged[1]);
                else
                    request.RangeTo = int.Parse(ranged[1]);
            }
        }

        /// <summary>
        /// Parsers the date time HTTP header.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private DateTime ParserDateTimeHttpHeader(string value)
        {
            try
            {
                return DateTime.Parse(value, CultureInfo.CurrentCulture);    
            }
            catch(FormatException fe)
            {
                throw new DateTimeHttpHeaderFormatException("A Datetime Http Request Header Field is in incorrect format.  Format Exception:" + fe.Message);
            }
        }

        /// <summary>
        /// Creates the corresponding URI for this request using storage item info.
        /// </summary>
        /// <returns>A new URI</returns>
        public Uri CreateUri()
        {
            return new Uri(string.Format("{0}/{1}/{2}",
                    _storageUrl,
                    _containerName.Encode(),
                    _storageItemName.Encode()));
        }

        /// <summary>
        /// Applies the appropiate method and headers to the specified request for this implementation.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Apply(ICloudFilesRequest request)
        {
            request.Method = "GET";
            AddRequestFieldHeadersToRequestHeaders(_requestHeaderFields, request);
        }
    }
}