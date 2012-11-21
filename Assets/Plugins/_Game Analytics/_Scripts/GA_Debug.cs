/// <summary>
/// This class handles error and exception messages, and makes sure they are added to the Quality category 
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public static class GA_Debug
{
	private static int _errorCount = 0;
	
	private static bool _showLogOnGUI = true;
	public static List<string> Messages;
	
	/// <summary>
	/// If SubmitErrors is enabled on the GA object this makes sure that any exceptions or errors are submitted to the GA server
	/// </summary>
	/// <param name="logString">
	/// Contains both a message and an exception identifier, so we split it up <see cref="System.String"/>
	/// </param>
	/// <param name="stackTrace">
	/// The exception stack trace <see cref="System.String"/>
	/// </param>
	/// <param name="type">
	/// The type of the log message (we only submit errors and exceptions to the GA server) <see cref="LogType"/>
	/// </param>
	public static void HandleLog(string logString, string stackTrace, LogType type)
	{
		//Only used if the GA_DebugGUI script is added to the GA object (for testing)
		if (_showLogOnGUI)
		{
			if (Messages == null)
			{
				Messages = new List<string>();
			}
			Messages.Add(logString);
			
			//Not necessary for small tests
			//GA.RunCoroutine(DeleteMsg());
		}
		
		//We only submit exceptions and errors
        if (GA.SUBMITERRORS && _errorCount < GA.MAXERRORCOUNT && (type == LogType.Exception || type == LogType.Error))
		{
			// Might be worth looking into: http://www.doogal.co.uk/exception.php
			
			_errorCount++;
			
			bool errorSubmitted = false;
			
			string eventID = "";
			
			try
			{
				eventID = logString.Split(':')[0];
			}
			catch
			{
				eventID = logString;
			}
			
			if (GA.SUBMITSTACKTRACE)
			{
				SubmitError(eventID, stackTrace);
				errorSubmitted = true;
			}
			
			if (GA.SUBMITERRORSYSTEMINFO)
			{
				List<Dictionary<string, object>> systemspecs = GA_GenericInfo.GetGenericInfo(eventID);
			
				foreach (Dictionary<string, object> spec in systemspecs)
				{
					GA_Queue.AddItem(spec, GA_Submit.CategoryType.GA_Log, false);
				}
				
				errorSubmitted = true;
			}
			
			if (!errorSubmitted)
			{
				SubmitError(eventID, logString);
			}
		}
    }
	
	public static void SubmitError(string eventName, string message)
	{
		Vector3 target = Vector3.zero;
		if (GA.TRACKTARGET)
		{
			target = GA.TRACKTARGET.position;
		}
		
		GA_Quality.NewErrorEvent(eventName, message, target.x, target.y, target.z);
	}
	
	/// <summary>
	/// Only used if GA_GUI script is added to the GA object (for testing)
	/// Deletes old messages from the GUI.
	/// </summary>
	/// <returns>
	/// The message.
	/// </returns>
	public static IEnumerator DeleteMsg()
	{
		yield return new WaitForSeconds(10);
		if (Messages.Count > 0)
		{
			Messages.RemoveAt(0);
		}
	}
}
