using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GA_Purchases : MonoBehaviour
{
	/// <summary>
	/// Used for player purchases
	/// </summary>
	/// <param name="purchaseName">
	/// The purchase identifier. F.x. "Rocket Launcher Upgrade" <see cref="System.String"/>
	/// </param>
	/// <param name="amount">
	/// The cost of the purchase in cent <see cref="System.Int32"/>
	/// </param>
	public static void NewPurchase(string purchaseName, int amount)
	{
		Dictionary<string, object> parameters = new Dictionary<string, object>()
		{
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], GA_GenericInfo.SessionID },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], GA_GenericInfo.UserID },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Amount], amount.ToString() },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.PurchaseID], purchaseName },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.TimeStamp], GA_GenericInfo.TimeStamp }
		};
		
		GA_Queue.AddItem(parameters, GA_Submit.CategoryType.GA_Purchase);
	}
}
