using System;
using System.Web;
using Rackspace.CloudFiles.Exceptions;

namespace Rackspace.CloudFiles.Utils
{
    public static class StringHelper
    {
        public static string Capitalize(this String wordToCapitalize)
        {
            if (String.IsNullOrEmpty(wordToCapitalize))
                throw new ArgumentNullException();

            return char.ToUpper(wordToCapitalize[0]) + wordToCapitalize.Substring(1);
        }

        public static string Capitalize(this bool booleanValue)
        {
            return booleanValue ? "True" : "False";
        }

        public static string Encode(this string stringToEncode)
        {
            if (stringToEncode == null)  
                throw new ArgumentNullException();

            return HttpUtility.UrlEncode(stringToEncode).Replace("+", "%20").Replace("%3a", ":");
        }

        public static string StripSlashPrefix(this string path)
        {
            return path[0] == '/' ? path.Substring(1, path.Length - 1) : path;
        }
        public static string MakeServiceNet(this string str){
        	if(!str.StartsWith("https://"))
        		throw new InsecureUrlException("You tried to set your url of "+ str+" to http and not https");
        	return "https://snet-" +str.Substring(8);
        }
    }
}