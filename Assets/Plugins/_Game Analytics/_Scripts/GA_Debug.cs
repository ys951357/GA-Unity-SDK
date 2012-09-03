/// <summary>
/// This class handles error and exception messages, and makes sure they are added to the Quality category 
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GA_Debug
{
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
        if (GA.SUBMITERRORS && type == LogType.Exception || type == LogType.Error)
		{
			GA_Quality.NewErrorEvent("GA Error:" + type.ToString(), logString + " " + stackTrace);
		}
    }
	
	/// <summary>
	/// Only used if GA_GUI script is added to the GA object (for testing)
	/// Deletes old messages in from the GUI.
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
