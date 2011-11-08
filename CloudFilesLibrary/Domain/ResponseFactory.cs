//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

using System.Text;
using Rackspace.CloudFiles.Domain.Request.Interfaces;
using Rackspace.CloudFiles.Domain.Response.Interfaces;
using Rackspace.CloudFiles.Utils;

namespace Rackspace.CloudFiles.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public interface IResponseFactory
    {
        ICloudFilesResponse Create(ICloudFilesRequest request);
    }

    /// <summary>
    /// ResponseFactory
    /// </summary>
    public class ResponseFactory : IResponseFactory 
    {
        public ResponseFactory()
        {
            Log.EnsureInitialized();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ICloudFilesResponse Create(ICloudFilesRequest request)
        {
            // this may be very very wrong look at tests closely
         //   if (request.ContentLength>0)
          //      throw new InvalidResponseTypeException(
             //       "The request type is of IRequestWithContentBody. Content body is expected with this request. ");

          
            Log.Debug(this, OutputRequestInformation(request));


            var response = request.GetResponse();
            Log.Debug(this, OutputResponseInformation(response));

            
            response.Close();
            return response;
                      
        }

        private string OutputRequestInformation(ICloudFilesRequest request)
        {
            var output = new StringBuilder();
            output.AppendLine("REQUEST");
            output.Append("Request URL: ");
            output.AppendLine(request.RequestUri.ToString());
            output.Append("Method: ");
            output.AppendLine(request.Method);
            output.AppendLine("Headers: ");
            foreach (var key in request.Headers.AllKeys)
            {
                output.Append(key);
                output.Append(": ");
                output.Append(request.Headers[key]);
                output.AppendLine();
            }

            return output.ToString();
        }

        private string OutputResponseInformation(ICloudFilesResponse response)
        {
            var output = new StringBuilder();
            output.AppendLine("RESPONSE:");
            output.Append("Status Code: ");
            output.AppendLine(response.StatusCode.ToString());
            output.Append("Status Description: ");
            output.AppendLine(response.StatusDescription);
            output.AppendLine("Headers: ");
            foreach (var key in response.Headers.AllKeys)
            {
                output.Append(key);
                output.Append(": ");
                output.Append(response.Headers[key]);
                output.AppendLine();
            }

            return output.ToString();
        }
    }
}