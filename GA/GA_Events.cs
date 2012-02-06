using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GA_Events
{
	#region public methods
	
	public static void NewEvent(string eventName, string eventValue, float x, float y)
	{
		CreateNewEvent(eventName, eventValue, x, y);
	}
	
	public static void NewEvent(string eventName, string eventValue)
	{
		CreateNewEvent(eventName, eventValue, null, null);
	}
	
	public static void NewEvent(string eventName)
	{
		CreateNewEvent(eventName, null, null, null);
	}
	
	#endregion
	
	#region private methods
	
	/// <summary>
	/// Adds a custom event to the submit queue (see GA_Queue)
	/// </summary>
	/// <param name="eventName">
	/// Identifies the event so this should be as descriptive as possible. PickedUpAmmo might be a good event name. EventTwo is a bad event name! <see cref="System.String"/>
	/// </param>
	/// <param name="eventValue">
	/// A value relevant to the event. F.x. if the player picks up some shotgun ammo the eventName could be "PickedUpAmmo" and this value could be "Shotgun". This can be null <see cref="System.Nullable<System.Single>"/>
	/// </param>
	/// <param name="x">
	/// The x coordinate of the event occurence. This can be null <see cref="System.Nullable<System.Single>"/>
	/// </param>
	/// <param name="y">
	/// The y coordinate of the event occurence. This can be null <see cref="System.Nullable<System.Single>"/>
	/// </param>
	private static void CreateNewEvent(string eventName, string eventValue, float? x, float? y)
	{
		Dictionary<string, object> parameters = new Dictionary<string, object>()
		{
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], GA_GenericInfo.SessionID },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID], eventName },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Level], Application.loadedLevelName },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.TimeStamp], GA_GenericInfo.TimeStamp }
		};
		
		if (eventValue != null && eventValue != "")
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Value], eventValue.ToString());
		}
		
		if (x.HasValue)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.X], x.ToString());
		}
		
		if (y.HasValue)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Y], y.ToString());
		}
		
		GA_Queue.AddItem(parameters, GA_Submit.CategoryType.GA_Event);
	}
	
	#endregion
}