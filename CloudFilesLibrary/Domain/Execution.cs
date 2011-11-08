//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

using System;
using Rackspace.CloudFiles.Utils;

namespace Rackspace.CloudFiles
{
	 
	
 	public static class StartProcess 
	{
		
		public static StartMessage ByLoggingMessage(string startmessage){
			return new StartMessage(startmessage);
		}
	}
	public class StartMessage 
	{
        private readonly string _startmessage;

		public Execution ThenDoing(Action action)
		{
				return new Execution(action, _startmessage);
		}
		public Execution<R> ThenDoing<R>(Func<R> action)
		{
				return new Execution<R>(action, _startmessage);
		}
		
		public  StartMessage(string startmessage)
		{
			_startmessage = startmessage;	
		}
	
 
	}
	
	
	public class Execution<R>{
		private readonly Func<R> _action;
		private readonly string _startmessage;
		
		public Error<R,T> AndIfErrorThrownIs<T>() where T: Exception
		{
			return new Error<R,T>(_action, _startmessage);
		}
		public Execution(Func<R> action, string startmessage){
			
			_action = action;
			_startmessage = startmessage;
		}
	}
	public class Execution
	{
		private readonly Action _action;
		private readonly string _startmessage;
		
		public Error<T> AndIfErrorThrownIs<T>() where T: Exception
		{
			return new Error<T>(_action, _startmessage);
		}
		public Execution(Action action, string startmessage){
			
			_action = action;
			_startmessage = startmessage;
		}
				
	}
	public class Error<R,T> where T: Exception
	{
        private readonly Func<R> _startaction;
        private readonly string _startmessage;
		
		public ErrorAction<R,T> Do(Action<T> erroraction)
		{
			return new ErrorAction<R,T>(_startaction, _startmessage, erroraction);
		}
		
		public Error(Func<R> startaction, string startmessage)
		{
			_startaction = startaction;
			_startmessage = startmessage;	
        }
	}

	public class Error<T> where T: Exception
    {
        private readonly Action _startaction;
        private readonly string _startmessage;
		
		public ErrorAction<T> Do(Action<T> erroraction)
		{
			return new ErrorAction<T>(_startaction, _startmessage, erroraction);
		}
		
        public Error(Action startaction, string startmessage)
		{
			_startaction = startaction;
			_startmessage = startmessage;
		}
	}
	public class ErrorAction<R,T> where T: Exception{
		
		private readonly Func<R> _startaction;
		private readonly Action<T> _erroraction;
		private readonly string _startmessage;

		public ErrorMessage<R,T> AndLogError(string errormessage) 
		{
			return new ErrorMessage<R,T>(_startaction, _startmessage, _erroraction, errormessage);
		}
		public ErrorAction(Func<R> startaction, string startmessage, Action<T> erroraction){
			_startaction = startaction;
			_startmessage = startmessage;
			_erroraction = erroraction;
		}
	}
	public class ErrorAction<T> where T: Exception{
		
		private readonly Action _startaction;
		private readonly Action<T> _erroraction;
		private readonly string _startmessage;

		public ErrorMessage<T> AndLogError(string errormessage) 
		{
			return new ErrorMessage<T>(_startaction, _startmessage, _erroraction, errormessage);
		}
		public ErrorAction(Action startaction, string startmessage, Action<T> erroraction){
			_startaction = startaction;
			_startmessage = startmessage;
			_erroraction = erroraction;
		}
	}
	public class ErrorMessage<R,T>  where T: Exception
	{
		private readonly Func<R> _startaction;
		private readonly Action<T> _erroraction;
		private readonly string _startmessage;
		private readonly string _errormessage;
		
		public R Now()
		{
			Log.Debug(this, _startmessage);
			try
			{ 
				return _startaction.Invoke();
			}
			catch(T ex)
			{	
				_erroraction.Invoke(ex);	
		    		Log.Error(this, _errormessage, ex);
				throw;
			}
				
		}
		public ErrorMessage(Func<R> startaction, string startmessage, Action<T> erroraction, string errormessage){
			
			_startaction =  startaction;
			_startmessage = startmessage;
			_erroraction = erroraction;
			_errormessage = errormessage;
		}
	}
		
	public class ErrorMessage<T>  where T: Exception
	{
		private readonly Action _startaction;
		private readonly Action<T> _erroraction;
		private readonly string _startmessage;
		private readonly string _errormessage;
		
		public void Now()
		{
			Log.Debug(this, _startmessage);
			try
			{ 
				_startaction.Invoke();
			}
			catch(T ex)
			{	
				_erroraction.Invoke(ex);	
		    		Log.Error(this, _errormessage, ex);
				throw;
			}
				
		}
		public ErrorMessage(Action startaction, string startmessage, Action<T> erroraction, string errormessage){
			
			_startaction =  startaction;
			_startmessage = startmessage;
			_erroraction = erroraction;
			_errormessage = errormessage;
		}
			
	}
	
}