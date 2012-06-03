using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class GA_UnitTest : UnityTest
{
	/// <summary>
	/// Makes sure an instance of the GA prefab exists
	/// </summary>
	/// <returns>
	/// A <see cref="IEnumerator"/>
	/// </returns>
	public IEnumerator TestGAInstance()
	{
		if (AssertTrue(GA.INSTANCE != null, "GA Instance test")) { yield break; }
	}
	
	/// <summary>
	/// Pings the GA server to make sure status is ok  
	/// </summary>
	/// <returns>
	/// A <see cref="IEnumerator"/>
	/// </returns>
	public IEnumerator TestGAPing()
	{
		string url = GA_Submit.GetBaseURL(true) + "/ping";
		
		WWW www = new WWW(url);
		
		yield return www;
		
		if (AssertTrue(www.error == null, "www error test")) { yield break; }
		
		Dictionary<string, object> parameters = JsonMapper.ToObject<Dictionary<string, object>>(www.text);
		
		if (AssertTrue(parameters != null, "parameters test")) { yield break; }
		if (AssertEqual(parameters.Count, 2)) { yield break; }
		if (AssertTrue(parameters.ContainsKey("status"), "parameters status test")) { yield break; }
		if (AssertEqual((string)parameters["status"], "ok")) { yield break; }
		if (AssertTrue(parameters.ContainsKey("version"), "parameters version test")) { yield break; }
		if (AssertEqual((string)parameters["version"], "1.0.0")) { yield break; }
	}
	
	/// <summary>
	/// Tests a typical session submit
	/// </summary>
	/// <returns>
	/// A <see cref="IEnumerator"/>
	/// </returns>
	public IEnumerator TestGASessionSubmit()
	{		
		Dictionary<string, object> parameters = new Dictionary<string, object>()
		{
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], "8f64a3b5-84c9-4932-9715-48e9456654b1" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], "e5315601-cedc-4b86-9081-5c61dafdbfaf" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build], GA.BUILD },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Gender], "M" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Birth_year], 1984 },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Country], "USA" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.State], "CA" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Friend_Count], 7 }
		};
		
		yield return StartCoroutine(GA_Submit.Submit(new List<GA_Submit.Item> { new GA_Submit.Item {
			Type = GA_Submit.CategoryType.GA_User,
			Parameters = parameters
		} }, delegate {
			AssertTrue(true, "submit test passed");
		}, delegate {
			Fail("submit test failed");
		}));
	}
	
	/// <summary>
	/// Tests a typical event submit with a value and x, y coordinates
	/// </summary>
	/// <returns>
	/// A <see cref="IEnumerator"/>
	/// </returns>
	public IEnumerator TestGAEventValXYSubmit()
	{		
		Dictionary<string, object> parameters = new Dictionary<string, object>()
		{
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], "8f64a3b5-84c9-4932-9715-48e9456654b1" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], "e5315601-cedc-4b86-9081-5c61dafdbfaf" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build], GA.BUILD },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID], "Test Event Value X Y" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Level], "Test Level 1" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.TimeStamp], "1328514167.19718" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Value], "21.75" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.X], "16f" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Y], "-11.97" }
		};
		
		yield return StartCoroutine(GA_Submit.Submit(new List<GA_Submit.Item> { new GA_Submit.Item {
			Type = GA_Submit.CategoryType.GA_Event,
			Parameters = parameters
		} }, delegate {
			AssertTrue(true, "event val x y submit test passed");
		}, delegate {
			Fail("event val x y submit test failed");
		}));
	}
	
	/// <summary>
	/// Tests a typical event submit with a value, but without coordinates
	/// </summary>
	/// <returns>
	/// A <see cref="IEnumerator"/>
	/// </returns>
	public IEnumerator TestGAEventValSubmit()
	{		
		Dictionary<string, object> parameters = new Dictionary<string, object>()
		{
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], "8f64a3b5-84c9-4932-9715-48e9456654b1" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], "e5315601-cedc-4b86-9081-5c61dafdbfaf" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build], GA.BUILD },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID], "Test Event Value" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Level], "Test Level 2" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.TimeStamp], "1328514167.19718" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Value], "9.3" }
		};
		
		yield return StartCoroutine(GA_Submit.Submit(new List<GA_Submit.Item> { new GA_Submit.Item {
			Type = GA_Submit.CategoryType.GA_Event,
			Parameters = parameters
		} }, delegate {
			AssertTrue(true, "event val submit test passed");
		}, delegate {
			Fail("event val submit test failed");
		}));
	}
	
	/// <summary>
	/// Tests a typical event submit without value or coordinates
	/// </summary>
	/// <returns>
	/// A <see cref="IEnumerator"/>
	/// </returns>
	public IEnumerator TestGAEventSubmit()
	{		
		Dictionary<string, object> parameters = new Dictionary<string, object>()
		{
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], "8f64a3b5-84c9-4932-9715-48e9456654b1" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], "e5315601-cedc-4b86-9081-5c61dafdbfaf" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build], GA.BUILD },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID], "Test Event" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Level], "Test Level 3" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.TimeStamp], "1328514167.19718" }
		};
		
		yield return StartCoroutine(GA_Submit.Submit(new List<GA_Submit.Item> { new GA_Submit.Item {
			Type = GA_Submit.CategoryType.GA_Event,
			Parameters = parameters
		} }, delegate {
			AssertTrue(true, "event submit test passed");
		}, delegate {
			Fail("event submit test failed");
		}));
	}
	
	/// <summary>
	/// Tests a typical crash submit
	/// </summary>
	/// <returns>
	/// A <see cref="IEnumerator"/>
	/// </returns>
	public IEnumerator TestGACrashSubmit()
	{		
		Dictionary<string, object> parameters = new Dictionary<string, object>()
		{
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], "8f64a3b5-84c9-4932-9715-48e9456654b1" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], "e5315601-cedc-4b86-9081-5c61dafdbfaf" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build], GA.BUILD },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID], "TestException" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Message], "Looks like we ran into a TestExeception: This is the stack trace of the TestException." },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.TimeStamp], "1328514167.19718" }
		};
		
		yield return StartCoroutine(GA_Submit.Submit(new List<GA_Submit.Item> { new GA_Submit.Item {
			Type = GA_Submit.CategoryType.GA_Log,
			Parameters = parameters
		} }, delegate {
			AssertTrue(true, "crash submit test passed");
		}, delegate {
			Fail("crash submit test failed");
		}));
	}
	
	/// <summary>
	/// Tests a typical purchase submit
	/// </summary>
	/// <returns>
	/// A <see cref="IEnumerator"/>
	/// </returns>
	public IEnumerator TestGAPurchaseSubmit()
	{		
		Dictionary<string, object> parameters = new Dictionary<string, object>()
		{
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], "e5315601-cedc-4b86-9081-5c61dafdbfaf" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], "8f64a3b5-84c9-4932-9715-48e9456654b1" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build], GA.BUILD },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Currency], "USD" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Amount], "99" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID], "WeaponUpgrade:RocketLauncher" }
		};
		
		yield return StartCoroutine(GA_Submit.Submit(new List<GA_Submit.Item> { new GA_Submit.Item {
			Type = GA_Submit.CategoryType.GA_Purchase,
			Parameters = parameters
		} }, delegate {
			AssertTrue(true, "purchase submit test passed");
		}, delegate {
			Fail("purchase submit test failed");
		}));
	}
	
	/// <summary>
	/// Tests multiple message submit
	/// </summary>
	/// <returns>
	/// A <see cref="IEnumerator"/>
	/// </returns>
	public IEnumerator TestGASubmitMultiple()
	{		
		Dictionary<string, object> parameters = new Dictionary<string, object>()
		{
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], "e5315601-cedc-4b86-9081-5c61dafdbfaf" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], "8f64a3b5-84c9-4932-9715-48e9456654b1" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build], GA.BUILD },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Currency], "USD" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Amount], "5600" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID], "Rocket Launcher Upgrade." },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.TimeStamp], "1328514167.19718" }
		};
		
		Dictionary<string, object> parameters2 = new Dictionary<string, object>()
		{
			//{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], "e5315601-cedc-4b86-9081-5c61dafdbfaf" },
			//{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], "8f64a3b5-84c9-4932-9715-48e9456654b1" },
			//{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build], GA.BUILD },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Currency], "USD" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Amount], "9300" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID], "Power Bandana Unlock." },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.TimeStamp], "1328514167.19718" }
		};
		
		Dictionary<string, object> parameters3 = new Dictionary<string, object>()
		{
			//{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], "e5315601-cedc-4b86-9081-5c61dafdbfaf" },
			//{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], "8f64a3b5-84c9-4932-9715-48e9456654b1" },
			//{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build], GA.BUILD },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Currency], "USD" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Amount], "6050" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID], "Refresh Talent Skills." },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.TimeStamp], "1328514167.19718" }
		};
		
		yield return StartCoroutine(GA_Submit.Submit(new List<GA_Submit.Item> { new GA_Submit.Item {
			Type = GA_Submit.CategoryType.GA_Purchase,
			Parameters = parameters
		}, new GA_Submit.Item {
			Type = GA_Submit.CategoryType.GA_Purchase,
			Parameters = parameters2
		}, new GA_Submit.Item {
			Type = GA_Submit.CategoryType.GA_Purchase,
			Parameters = parameters3
		} }, delegate {
			AssertTrue(true, "multiple submit test passed");
		}, delegate {
			Fail("multiple submit test failed");
		}));
	}
}