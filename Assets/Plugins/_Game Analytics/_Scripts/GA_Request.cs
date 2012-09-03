/// <summary>
/// This class handles receiving data from the Game Analytics servers.
/// JSON data is sent using a MD5 hashed authorization header, containing the JSON data and private key. Data received must be in JSON format also.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using LitJson;

public static class GA_Request
{
	/// <summary>
	/// Handlers for success and fail regarding requests sent to the GA server
	/// </summary>
	public delegate void SubmitSuccessHandler(RequestType requestType, JsonData returnParam, SubmitErrorHandler errorEvent);
	public delegate void SubmitErrorHandler(string message);
	
	/// <summary>
	/// Types of requests that can be made to the GA server
	/// </summary>
	public enum RequestType { GA_GetHeatmapEvents, GA_GetHeatmapData }
	
	#region private values
	
	/// <summary>
	/// All the different types of requests
	/// </summary>
	public static Dictionary<RequestType, string> Requests = new Dictionary<RequestType, string>()
	{
		{ RequestType.GA_GetHeatmapEvents, "hm_events" },
		{ RequestType.GA_GetHeatmapData, "hm_data" }
	};
	
	#endregion
	
	#region public methods
	
	public static IEnumerator Request(RequestType requestType, string requestInfo, SubmitSuccessHandler successEvent, SubmitErrorHandler errorEvent)
	{
		//Get the url with the request type
		string url = GA_Submit.GetURL(Requests[requestType]);
		
		//Make the request
		Dictionary<string, object> requestParameters = new Dictionary<string, object>();
		
		switch(requestType)
		{
			case RequestType.GA_GetHeatmapEvents:
				requestParameters.Add("events", requestInfo);
				break;
			case RequestType.GA_GetHeatmapData:
				requestParameters.Add("id", requestInfo);
				break;
		}
		
		//Make a JSON array string out of the request parameters
		string json = JsonMapper.ToJson(requestParameters);
		
		//Prepare the JSON array string for sending by converting it to a byte array
		byte[] data = Encoding.ASCII.GetBytes(json);
		
		//Set the authorization header to contain an MD5 hash of the JSON array string + the private key
		Hashtable headers = new Hashtable();
		headers.Add("Authorization", GA_Submit.CreateMD5Hash(json));
		
		//Try to send the data
		WWW www = new WWW(url, data, headers);
		
		//Wait for response
		yield return www;
		
		if (GA.DEBUG)
		{
			Debug.Log("GA URL: " + url);
			Debug.Log("GA Request: " + json);
			Debug.Log("GA Hash: " + GA_Submit.CreateMD5Hash(json));
		}
		
		try
		{
			if (www.error != null)
			{
				throw new Exception(www.error);
			}
			
			//Get the JSON object from the response
			JsonData returnParam = JsonMapper.ToObject(www.text);
			
			if (returnParam != null)
			{
				if (GA.DEBUG)
				{
					Debug.Log("GA Result: " + www.text);
				}
				
				if (successEvent != null)
				{
					successEvent(requestType, returnParam, errorEvent);
				}
			}
			else
			{
				throw new Exception(www.text);
			}
		}
		catch (Exception e)
		{
			if (errorEvent != null)
			{
				errorEvent(e.Message);
			}
		}
	}
	
	#endregion
}