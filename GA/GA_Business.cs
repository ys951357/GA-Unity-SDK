/// <summary>
/// This class handles business events, such as ingame purchases.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GA_Business : MonoBehaviour
{
	#region public methods
	
	public static void NewEvent(string eventName, string currency, int amount, float x, float y, float z)
	{
		CreateNewEvent(eventName, currency, amount, x, y, z);
	}
	
	public static void NewEvent(string eventName, string currency, int amount)
	{
		CreateNewEvent(eventName, currency, amount, null, null, null);
	}
	
	#endregion
	
	#region private methods
	
	/// <summary>
	/// Used for player purchases
	/// </summary>
	/// <param name="businessID">
	/// The business identifier. F.x. "Rocket Launcher Upgrade" <see cref="System.String"/>
	/// </param>
	/// <param name="currency">
	/// Abbreviation of the currency used for the transaction. F.x. USD (U.S. Dollars) <see cref="System.String"/>
	/// </param>
	/// <param name="amount">
	/// The value of the transaction in the lowest currency unit. F.x. if currency is USD then amount should be in cent <see cref="System.Int32"/>
	/// </param>
	private static void CreateNewEvent(string eventName, string currency, int amount, float? x, float? y, float? z)
	{
		Dictionary<string, object> parameters = new Dictionary<string, object>()
		{
			//{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], GA_GenericInfo.UserID },
			//{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], GA_GenericInfo.SessionID },
			//{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build], GA.BUILD },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID], eventName },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Currency], currency },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Amount], amount.ToString() },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Level], Application.loadedLevelName }
		};
		
		if (x.HasValue)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.X], x.ToString());
		}
		
		if (y.HasValue)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Y], y.ToString());
		}
		
		if (z.HasValue)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Z], z.ToString());
		}
		
		GA_Queue.AddItem(parameters, GA_Submit.CategoryType.GA_Purchase);
	}
	
	#endregion
}