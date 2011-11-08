/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 3/22/2010
 * Time: 2:46 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Rackspace.CloudFiles.Exceptions
{
	/// <summary>
	/// Description of InsecureUrlException.
	/// </summary>
	public class InsecureUrlException:Exception
	{
		public InsecureUrlException(string message):base(message)
		{
		}
	}
}
