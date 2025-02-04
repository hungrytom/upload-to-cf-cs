///
/// See COPYING file for licensing information
///

namespace Rackspace.CloudFiles.Utils
{
    /// <summary>
    /// This class initializes the constants which would be utilized all over the application.
    /// </summary>
    public static class Constants
    {
        public static string X_CDN_MANAGEMENT_URL = "X-CDN-Management-URL";
        public static string X_CDN_URI = "X-CDN-URI";
        public static string X_CDN_SSL_URI = "X-CDN-SSL-URI";
		public static string X_CDN_STREAMING_URI = "X-CDN-Streaming-URI";
        public static string X_CDN_ENABLED = "X-CDN-Enabled";
        public static string X_CDN_TTL = "X-TTL";
        public const string X_AUTH_TOKEN = "X-Auth-Token";
        public const string USER_AGENT = "csharp-cloudfiles";
        public const string META_DATA_HEADER = "X-Object-Meta-";
        public const int CHUNK_SIZE = 4096; //in bytes

        // The timeout is now set to a caller defined value (in IAddToWebRequest.Apply()) or defaults
        // to System.Threading.Timeout.Infinite
        //public const int CONNECTION_TIMEOUT = 2147483646; //Maximum value maintained so that connection does not timeout.
        
        public const string X_AUTH_USER = "X-Auth-User";
        public const string X_AUTH_KEY = "X-Auth-Key";
        public const string X_ACCOUNT_CONTAINER_COUNT = "X-Account-Container-Count";
        public const string X_ACCOUNT_BYTES_USED = "X-Account-Bytes-Used";
        public const string X_STORAGE_URL = "X-Storage-Url";
        public const string X_CONTAINER_STORAGE_OBJECT_COUNT = "X-Container-Object-Count";
        public const string X_CONTAINER_BYTES_USED = "X-Container-Bytes-Used";
        public const string X_PURGE_EMAIL = "X-Purge-Email";
        public const string ETAG = "ETag";
        public const int MAXIMUM_META_KEY_LENGTH = 128;
        public const int MAXIMUM_META_VALUE_LENGTH = 256;
        public const string CONTENT_LENGTH_HEADER = "Content-Length";
        public const string CONTENT_TYPE_HEADER = "Content-Type";
        public const string US_AUTHURL = "https://auth.api.rackspacecloud.com/v1.0";
        public const string UK_AUTHURL = "https://lon.auth.api.rackspacecloud.com/v1.0";
        public const string X_LOG_RETENTION = "X-Log-Retention";
        public const string X_CONTAINTER_META_DATA_HEADER = "X-Container-Meta-";
    }
}