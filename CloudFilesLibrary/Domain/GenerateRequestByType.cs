//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

using System;
using System.Web;
using Rackspace.CloudFiles.Domain.Request.Interfaces;
using Rackspace.CloudFiles.Domain.Response.Interfaces;
using Rackspace.CloudFiles.Utils;

namespace Rackspace.CloudFiles.Domain.Request
{
   
    public class GenerateRequestByType
    {
        private readonly IRequestFactory _requestfactory;
        private readonly IResponseFactory _responsefactory;
		
        public GenerateRequestByType():this(new RequestFactory(), new ResponseFactory() ){}
		public GenerateRequestByType(IRequestFactory requestfactory):this(requestfactory, new ResponseFactory()){}
		
        public GenerateRequestByType(IRequestFactory requestfactory, IResponseFactory responsefactory)
        {
            _requestfactory = requestfactory;
			_responsefactory = responsefactory;
        }
		private void AddAuthHeaderToRequest(ICloudFilesRequest cfrequest, string authtoken){
			cfrequest.Headers.Add(Constants.X_AUTH_TOKEN, HttpUtility.UrlEncode(authtoken));
		}
		 
        private ICloudFilesResponse commonSubmit(IAddToWebRequest requesttype, Func<ICloudFilesRequest> requeststrategy, string authtoken)
        {
            var cfrequest = requeststrategy.Invoke();
			//only way I've figured out how to make auth header logic conditional, this is a smell and in need of a better pattern
			if (!String.IsNullOrEmpty(authtoken))
				AddAuthHeaderToRequest(cfrequest, authtoken); 
			
            requesttype.Apply(cfrequest);
				
           	var response = _responsefactory.Create(cfrequest);
           	return response;
        }
        public ICloudFilesResponse Submit (IAddToWebRequest requesttype,  string authtoken)
        {
			return commonSubmit(requesttype, ()=>_requestfactory.Create(requesttype.CreateUri()), authtoken);
        }
        public ICloudFilesResponse Submit(IAddToWebRequest requesttype)
        {
			return commonSubmit(requesttype,()=> _requestfactory.Create(requesttype.CreateUri()), "");
        }
        public ICloudFilesResponse Submit(IAddToWebRequest requesttype, string authtoken, ProxyCredentials credentials)
        {
           return commonSubmit(requesttype, ()=> _requestfactory.Create(requesttype.CreateUri(),credentials),authtoken );   
        }
    }
}