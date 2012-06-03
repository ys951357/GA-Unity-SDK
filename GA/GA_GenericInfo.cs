/// <summary>
/// This class handles user ID, session ID, time stamp, and sends a user message, optionally including system specs, when the game starts
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

public static class GA_GenericInfo
{
	//[iOS only] Uncomment the lines below if the xcode script for OpenUDID is implemented
	//[DllImport ("__Internal")]
	//private static extern string FunctionGetOpenUDID ();
	
	#region public values
	
	/// <summary>
	/// The ID of the user/player. A unique ID will be determined the first time the player plays. If an ID has already been created for a player this ID will be used.
	/// </summary>
	public static string UserID
	{
		get {
			if (_userID == null)
			{
				if (PlayerPrefs.HasKey("GA_uid"))
				{
					_userID = PlayerPrefs.GetString("GA_uid");
				}
				else
				{
					_userID = GetUUID(true);
					PlayerPrefs.SetString("GA_uid", _userID);
					PlayerPrefs.Save();
				}
			}
			return _userID;
		}
	}
	
	/// <summary>
	/// The ID of the current session. A unique ID will be determined when the game starts. This ID will be used for the remainder of the play session.
	/// </summary>
	public static string SessionID
	{
		get {
			if (_sessionID == null)
			{
				_sessionID = GetUUID(false);
			}
			return _sessionID;
		}
	}
	
	/// <summary>
	/// The current UTC date/time in seconds
	/// </summary>
	public static string TimeStamp
	{
		get {
			return ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString(); //DateTime.UtcNow.Subtract(_epochStart).TotalSeconds.ToString();
		}
	}
	
	#endregion
	
	#region private values
	
	private static string _userID;
	private static string _sessionID;
	//private static DateTime _epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
	
	#endregion
	
	#region public methods
	
	/// <summary>
	/// Gets generic system information at the beginning of a play session
	/// </summary>
	/// <param name="inclSpecs">
	/// Determines if all the system specs should be included <see cref="System.Bool"/>
	/// </param>
	/// <returns>
	/// The message to submit to the GA server is a dictionary of all the relevant parameters (containing user ID, session ID, system information, language information, date/time, build version) <see cref="Dictionary<System.String, System.Object>"/>
	/// </returns>
	public static Dictionary<string, object> GetGenericInfo()
	{
		string system = GetSystem();
		string language = Application.systemLanguage.ToString();
		string version = GA.VERSION;
		
		Dictionary<string, object> parameters = new Dictionary<string, object>()
		{
			//{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], UserID },
			//{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], SessionID },
			//{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build], GA.BUILD },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID], "GA:SystemSpecs" }
		};
		
		AddSystemSpecs(parameters);
		
		return parameters;
	}
	
	/// <summary>
	/// Gets a universally unique ID.
	/// </summary>
	/// <returns>
	/// The generated UUID <see cref="System.String"/>
	/// </returns>
	public static string GetUUID(bool isUserID)
	{
		if (isUserID)
		{
			#if UNITY_IPHONE
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				try
				{
					//[iOS only] Uncomment the line below if the xcode script for OpenUDID is implemented
					//return FunctionGetOpenUDID();
				}
				catch (Exception)
				{
					//The xcode script for OpenUDID is not implemented correctly, so we just continue and get the GUID instead 
				}
			}
			#endif
			
			#if UNITY_ANDROID
			return SystemInfo.deviceUniqueIdentifier;
			#endif
		}
		
		return Guid.NewGuid().ToString();
	}
	
	#endregion
	
	#region private methods
	
	/// <summary>
	/// Adds detailed system specifications regarding the users/players device to the parameters. This does not work on IPhone.
	/// </summary>
	/// <param name="parameters">
	/// The parameters which will be sent to the server <see cref="Dictionary<System.String, System.Object>"/>
	/// </param>
	private static void AddSystemSpecs(Dictionary<string, object> parameters)
	{
		#if !UNITY_IPHONE
		Dictionary<string, object> systemSpecsDictionary = new Dictionary<string, object>();
		
		systemSpecsDictionary.Add("os", SystemInfo.operatingSystem);
		systemSpecsDictionary.Add("process_type", SystemInfo.processorType);
		systemSpecsDictionary.Add("process_count", SystemInfo.processorCount);
		systemSpecsDictionary.Add("sys_mem_size", SystemInfo.systemMemorySize);
		systemSpecsDictionary.Add("gfx_mem_size", SystemInfo.graphicsMemorySize);
		systemSpecsDictionary.Add("gfx_name", SystemInfo.graphicsDeviceName);
		systemSpecsDictionary.Add("gfx_vendor", SystemInfo.graphicsDeviceVendor);
		systemSpecsDictionary.Add("gfx_id", SystemInfo.graphicsDeviceID);
		systemSpecsDictionary.Add("gfx_vendor_id", SystemInfo.graphicsDeviceVendorID);
		systemSpecsDictionary.Add("gfx_version", SystemInfo.graphicsDeviceVersion);
		systemSpecsDictionary.Add("gfx_shader_level", SystemInfo.graphicsShaderLevel);
		systemSpecsDictionary.Add("gfx_pixel_fillrate", SystemInfo.graphicsPixelFillrate);
		systemSpecsDictionary.Add("sup_shadows", SystemInfo.supportsShadows);
		systemSpecsDictionary.Add("sup_render_textures", SystemInfo.supportsRenderTextures);
		systemSpecsDictionary.Add("sup_image_effects", SystemInfo.supportsImageEffects);
		
		parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Message], LitJson.JsonMapper.ToJson(systemSpecsDictionary));
		#endif
	}
	
	/// <summary>
	/// Gets the users system type
	/// </summary>
	/// <returns>
	/// String determining the system the user is currently running <see cref="System.String"/>
	/// </returns>
	private static string GetSystem()
	{
		#if UNITY_STANDALONE_OSX
		return "MAC";
		#endif

		#if UNITY_STANDALONE_WIN
		return "PC";
		#endif
		
		#if UNITY_WEBPLAYER
		return "WEBPLAYER";
		#endif
		
		#if UNITY_WII
		return "WII";
		#endif
		
		#if UNITY_IPHONE
		return "IPHONE";
		#endif

		#if UNITY_ANDROID
		return "ANDROID";
		#endif
		
		#if UNITY_PS3
		return "PS3";
		#endif

		#if UNITY_XBOX360
		return "XBOX";
		#endif
	}
	
	#endregion
}
