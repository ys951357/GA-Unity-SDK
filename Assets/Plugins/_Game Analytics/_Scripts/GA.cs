/// <summary>
/// This is the main Game Analytics class. This class sets up and starts the Game Analytics service.
/// The entire Game Analytics Unity3D package, including all scripts, has been designed and implemented by Simon Millard for Game Analytics (www.gameanalytics.com)
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GA : MonoBehaviour
{
	/// <summary>
	/// Types of help given in the help box of the GA inspector
	/// </summary>
	public enum HelpTypes { None, FpsCriticalAndTrackTargetHelp, GuiAndTrackTargetHelp, IncludeSystemSpecsHelp, ProvideCustomUserID };
	public enum MessageTypes { None, Error, Info, Warning };
	
	/// <summary>
	/// A message and message type for the help box displayed on the GUI inspector
	/// </summary>
	public struct HelpInfo
	{
		public string Message;
		public MessageTypes MsgType;
		public HelpTypes HelpType;
	}
	
	#region public static values
	
	/// <summary>
	/// The version of the GA Unity Wrapper plugin
	/// </summary>
	public static string VERSION = "0.2.1";
	
	/// <summary>
	/// Gets the instance
	/// </summary>
	/// <value>
	/// Instance GA
	/// </value>
	public static GA INSTANCE
	{
		get {
			return _instance;
		}
	}
	
	/// <summary>
	/// Gets a value indicating whether to show extra debug messages when data is sent
	/// </summary>
	/// <value>
	/// DEBUG true/false
	/// </value>
	public static bool DEBUG
	{
		get {
			if (_instance != null)
				return _instance.DebugMode;
			else
				return false;
		}
	}
	
	/// <summary>
	/// Gets a value indicating whether to submit data while the game is run in the Unity editor only.
	/// </summary>
	/// <value>
	/// RUNINEDITOR true/false
	/// </value>
	public static bool RUNINEDITOR
	{
		get {
			if (_instance != null)
				return _instance.RunInEditorPlayMode;
			else
				return false;
		}
	}
	
	/// <summary>
	/// Gets the current game build supplied by the user
	/// </summary>
	/// <value>
	/// BUILD string
	/// </value>
	public static string BUILD
	{
		get {
			if (_instance != null)
				return _instance.Build;
			else
				return null;
		}
	}
	
	/// <summary>
	/// Gets a value indicating whether to submit data whenever a scene change occurs. See GA_SpecialEvents.
	/// </summary>
	/// <value>
	/// INCLUDESCENECHANGE true/false
	/// </value>
	public static bool INCLUDESCENECHANGE
	{
		get {
			if (_instance != null)
				return _instance.IncludeSceneChange;
			else
				return false;
		}
	}
	
	/// <summary>
	/// Gets a value indicating whether to submit data whenever an error or exception occurs. See GA_Debug.
	/// </summary>
	/// <value>
	/// SUBMITERRORS true/false
	/// </value>
	public static bool SUBMITERRORS
	{
		get {
			if (_instance != null)
				return _instance.SubmitErrors;
			else
				return false;
		}
	}
	
	/// <summary>
	/// Gets a value indicating whether to submit the stack trace whenever an error or exception occurs. See GA_Debug.
	/// </summary>
	/// <value>
	/// SUBMITSTACKTRACE true/false
	/// </value>
	public static bool SUBMITSTACKTRACE
	{
		get {
			if (_instance != null)
				return _instance.SubmitErrorStackTrace;
			else
				return false;
		}
	}
	
	/// <summary>
	/// Gets a value indicating whether to submit system information whenever an error or exception occurs. See GA_Debug.
	/// </summary>
	/// <value>
	/// SUBMITERRORSYSTEMINFO true/false
	/// </value>
	public static bool SUBMITERRORSYSTEMINFO
	{
		get {
			if (_instance != null)
				return _instance.SubmitErrorSystemInfo;
			else
				return false;
		}
	}
	
	/// <summary>
	/// Gets a value indicating the maximum number of errors or exceptions tracked per session. See GA_Debug.
	/// </summary>
	/// <value>
	/// MAXERRORCOUNT
	/// </value>
	public static int MAXERRORCOUNT
	{
		get {
			if (_instance != null)
				return _instance.MaxErrorCount;
			else
				return 0;
		}
	}
	
	/// <summary>
	/// Gets a value indicating whether to submit average frames per second.
	/// </summary>
	/// <value>
	/// SUBMITAVERAGEFPS true/false
	/// </value>
	public static bool SUBMITAVERAGEFPS
	{
		get {
			if (_instance != null)
				return _instance.SubmitFpsAverage;
			else
				return false;
		}
	}
	
	/// <summary>
	/// Gets a value indicating whether to submit a message whenever the frames per second falls below a certain threshold.
	/// </summary>
	/// <value>
	/// SUBMITCRITICALFPS true/false
	/// </value>
	public static bool SUBMITCRITICALFPS
	{
		get {
			if (_instance != null)
				return _instance.SubmitFpsCritical;
			else
				return false;
		}
	}
	
	/// <summary>
	/// Gets a value indicating the frames per second threshold for when to submit a critical FPS message.
	/// </summary>
	/// <value>
	/// FPSCRITICALTHRESHOLD int
	/// </value>
	public static int FPSCRITICALTHRESHOLD
	{
		get {
			if (_instance != null)
				return _instance.FpsCriticalThreshold;
			else
				return 99;
		}
	}
	
	/// <summary>
	/// Gets a value indicating whether to send data while on a roaming network with a mobile device.
	/// </summary>
	/// <value>
	/// ALLOWROAMING true/false
	/// </value>
	public static bool ALLOWROAMING
	{
		get {
			if (_instance != null)
				return _instance.AllowRoaming;
			else
				return false;
		}
	}
	
	/// <summary>
	/// Gets a value indicating whether to archive data when no internet connection is available. See GA_Archive.
	/// </summary>
	/// <value>
	/// ARCHIVEDATA true/false
	/// </value>
	public static bool ARCHIVEDATA
	{
		get {
			if (_instance != null)
				return _instance.ArchiveData;
			else
				return false;
		}
	}
	
	/// <summary>
	/// Is the user feedback GUI enabled?
	/// </summary>
	/// <value>
	/// GUIENABLED true/false
	/// </value>
	public static bool GUIENABLED
	{
		get {
			if (_instance != null)
				return _instance.GuiEnabled;
			else
				return false;
		}
	}
	
	/// <summary>
	/// Get the transform target for events such as GUI user feedback and bug reports, critical FPS, etc.
	/// </summary>
	/// <value>
	/// GUITARGET Transform
	/// </value>
	public static Transform TRACKTARGET
	{
		get {
			if (_instance != null)
				return _instance.TrackTarget;
			else
				return null;
		}
	}
	
	/// <summary>
	/// Gets the maximum file size on the disk for archiving data in bytes.
	/// </summary>
	/// <value>
	/// ARCHIVEMAXFILESIZE long
	/// </value>
	public static long ARCHIVEMAXFILESIZE
	{
		get {
			if (_instance != null)
				return _instance.ArchiveMaxFileSize;
			else
				return 0;
		}
	}
	
	/// <summary>
	/// If true the GA wrapper will not start sending data automatically until a UserID is specified.
	/// </summary>
	/// <value>
	/// CUSTOMUSERID true/false
	/// </value>
	public static bool CUSTOMUSERID
	{
		get {
			if (_instance != null)
				return _instance.CustomUserID;
			else
				return false;
		}
	}
	
	#endregion
	
	#region public values
	
	public string PublicKey;
	public string PrivateKey;
	public string Build;
	public bool DebugMode;
	public bool RunInEditorPlayMode;
	public bool IncludeSystemSpecs;
	public bool IncludeSceneChange;
	public bool SubmitErrors;
	public int MaxErrorCount = 10;
	public bool SubmitErrorStackTrace;
	public bool SubmitErrorSystemInfo;
	public bool SubmitFpsAverage;
	public bool SubmitFpsCritical;
	public int FpsCriticalThreshold = 30;
	public bool AllowRoaming;
	public bool ArchiveData;
	public bool GuiEnabled;
	public Transform TrackTarget;
	public long ArchiveMaxFileSize = 500;
	public bool CustomUserID;
	
	//These values are used for the GA_Inspector only
	public List<HelpTypes> ClosedHints = new List<HelpTypes>();
	public bool DisplayHints;
	public bool QAFoldOut;
	public bool GDFoldOut;
	public bool DataFoldOut;
	public bool GuiFoldOut;
	public bool ErrorFoldOut;
	public bool GeneralFoldOut;
	public bool EditorFoldOut;
	public bool TrackFoldOut;
	
	#endregion
	
	#region private static values
	
	/// <summary>
	/// This is the only instance of the GA component which should ever exist
	/// </summary>
	private static GA _instance;
	
	#endregion
	
	#region unity derived methods
	
	/// <summary>
	/// Setup this component
	/// </summary>
	public void Awake ()
	{
		//make sure we only have one object with this GA script at any time
		if (_instance != null)
		{
			Destroy(gameObject);
			return;
		}
		
		if (PublicKey.Equals("") || PrivateKey.Equals(""))
		{
			Debug.LogWarning("GA Error: Public key and/or private key not set. Closing GA.");
			Destroy(gameObject);
			return;
		}
		
		_instance = this;
		DontDestroyOnLoad(this);
		
		gameObject.AddComponent<GA_SpecialEvents>();
		gameObject.AddComponent<GA_Gui>();
	}
	
	/// <summary>
	/// Setup involving other components
	/// </summary>
	public void Start ()
	{
		GA_Submit.SetupKeys(PublicKey, PrivateKey);
		
		Application.RegisterLogCallback(GA_Debug.HandleLog);
		
		if (DebugMode)
		{
			Debug.Log("GA Wrapper initialized, waiting for events..");
		}
		
		if (IncludeSystemSpecs)
		{
			List<Dictionary<string, object>> systemspecs = GA_GenericInfo.GetGenericInfo("");
			
			foreach (Dictionary<string, object> spec in systemspecs)
			{
				GA_Queue.AddItem(spec, GA_Submit.CategoryType.GA_Log, false);
			}
		}
		
		//Start the submit queue for sending messages to the server
		if (!CustomUserID && GA_GenericInfo.UserID != string.Empty)
			RunCoroutine(GA_Queue.SubmitQueue());
		
		//If we're playing the unity demo "AngryBots", then add the GA_AngryBots component
		if (Application.loadedLevelName == "AngryBots")
		{
			gameObject.AddComponent("GA_AngryBots");
		}
	}
	
	#endregion
	
	#region public methods
	
	/// <summary>
	/// Starts a new coroutine for the specified method, using the StartCoroutine Unity function.
	/// This is used to run the submits to the Game Analytics server in a seperate routine.
	/// </summary>
	/// <param name="routine">
	/// The method to start in the new coroutine <see cref="IEnumerator"/>
	/// </param>
	/// <returns>
	/// The new coroutine <see cref="Coroutine"/>
	/// </returns>
	public static Coroutine RunCoroutine(IEnumerator routine)
	{
		if (_instance == null)
		{
			Debug.LogWarning("GA Error: No GA instance exists. Drag the GA game object into your scene.");
			return null;
		}
		
		return _instance.StartCoroutine(routine);
	}
	
	/// <summary>
	/// Checks the internet connectivity.
	/// </summary>
	/// <returns>
	/// True if the device is connected to the internet, otherwise false.
	/// </returns>
	public static bool CheckInternetConnectivity()
	{
		#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
		
		try
		{
			System.Net.Sockets.TcpClient clnt = new System.Net.Sockets.TcpClient("www.gameanalytics.com", 80);
			clnt.Close();
			return true;
		}
		catch(System.Exception)
		{
			return false;
		}
		
		#else
		
		if  (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || 
			(Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork && ALLOWROAMING))
		{
			return true;
		}
		else
		{
			return false;
		}
		
		#endif
	}
	
	/// <summary>
	/// Sets a custom user ID.
	/// Make sure each user has a unique user ID. This is useful if you have your own log-in system with unique user IDs.
	/// NOTE: Only use this method if you have enabled "Custom User ID" on the GA inspector!
	/// </summary>
	/// <param name="customID">
	/// The custom user ID - this should be unique for each user
	/// </param>
	public static void SetCustomUserID(string customID)
	{
		if (customID != string.Empty)
		{
			GA_GenericInfo.SetCustomUserID(customID);
			
			RunCoroutine(GA_Queue.SubmitQueue());
		}
	}
	
	public HelpInfo GetHelpMessage()
	{
		if (PublicKey.Equals("") || PrivateKey.Equals(""))
		{
			return new HelpInfo() { Message = "Please fill in your Game Key and Secret Key, obtained from the Game Analytics website where you created your game.", MsgType = MessageTypes.Warning, HelpType = HelpTypes.None };
		}
		else if (Build.Equals(""))
		{
			return new HelpInfo() { Message = "Please fill in a name for your build, representing the current version of the game. Updating the build name for each version of the game will allow you to filter by build when viewing your data on the GA website.", MsgType = MessageTypes.Warning, HelpType = HelpTypes.None };
		}
		else if (SubmitFpsCritical && TrackTarget == null && !ClosedHints.Contains(HelpTypes.FpsCriticalAndTrackTargetHelp))
		{
			return new HelpInfo() { Message = "You have chosen to track critical FPS, but you have not added a Track Target. Adding a Track Target will help you identify the location of critical FPS events, as the Track Target's coordinates are submitted with the event.", MsgType = MessageTypes.Info, HelpType = HelpTypes.FpsCriticalAndTrackTargetHelp };
		}
		else if (GuiEnabled && TrackTarget == null && !ClosedHints.Contains(HelpTypes.GuiAndTrackTargetHelp))
		{
			return new HelpInfo() { Message = "You have chosen to enable the Feedback GUI, but you have not added a Track Target. Adding a Track Target will help you identify the location of player feedback, as the Track Target's coordinates are submitted with the event.", MsgType = MessageTypes.Info, HelpType = HelpTypes.GuiAndTrackTargetHelp };
		}
		else if (CustomUserID && !ClosedHints.Contains(HelpTypes.ProvideCustomUserID))
		{
			return new HelpInfo() { Message = "You have indicated that you will provide a custom user ID - no data will be submitted until it is provided.", MsgType = MessageTypes.Info, HelpType = HelpTypes.ProvideCustomUserID };
		}
		
		#if !UNITY_IPHONE
		if (!IncludeSystemSpecs && !ClosedHints.Contains(HelpTypes.IncludeSystemSpecsHelp))
		{
			return new HelpInfo() { Message = "If you are having trouble identifying the hardware of users with performance issues you might want to enable Submit System Info under the Quality Assurance tab. This way detailed system information will be submitted at the beginning of each play session.", MsgType = MessageTypes.Info, HelpType = HelpTypes.IncludeSystemSpecsHelp };
		}
		#endif
		
		return new HelpInfo() { Message = "No hints to display", MsgType = MessageTypes.None, HelpType = HelpTypes.None };
	}
	
	#endregion
}