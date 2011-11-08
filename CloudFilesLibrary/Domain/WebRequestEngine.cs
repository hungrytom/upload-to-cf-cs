//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

using Rackspace.CloudFiles.Domain.Request.Interfaces;
using Rackspace.CloudFiles.Domain.Response.Interfaces;

namespace Rackspace.CloudFiles.Domain.Request
{
    public interface IWebRequestEngine
    {
        ICloudFilesResponse Submit(ICloudFilesRequest request);
       
    }
}