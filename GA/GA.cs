/// <summary>
/// This is the main Game Analytics class, and the only class which the user should have to manually instanciate (add to a game object).
/// This class sets up and starts the Game Analytics service.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GA : MonoBehaviour
{	
	#region public static values
	
	/// <summary>
	/// The version of the GA Unity Wrapper plugin
	/// </summary>
	public static string VERSION = "1.0";
	
	/// <summary>
	/// Gets the instance
	/// </summary>
	/// <value>
	/// The instance
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
	/// The BUILD
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
	/// Gets a value indicating whether to send data while on a roaming network with a mobile device.
	/// </summary>
	/// <value>
	/// SUBMITERRORS true/false
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
	/// SUBMITERRORS true/false
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
	public bool AllowRoaming;
	public bool ArchiveData;
	
	public List<GA_HeatMap> HeatMaps;
	
	//These values are used for the GA_Inspector only
	public bool QAFoldOut;
	public bool GDFoldOut;
	public bool BFoldOut;
	public bool PFoldOut;
	public bool HMFoldOut;
	public bool MobileFoldOut;
	
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
			Debug.LogError("GA Error: Public key and/or private key not set. Closing GA.");
			Destroy(gameObject);
			return;
		}
		
		_instance = this;
		DontDestroyOnLoad(this);
	}
	
	/// <summary>
	/// Setup involving other components
	/// </summary>
	public void Start ()
	{
		GA_Submit.SetupKeys(PublicKey, PrivateKey);
		
		Application.RegisterLogCallback(GA_Debug.HandleLog);
		
		gameObject.AddComponent<GA_SpecialEvents>();
		
		if (IncludeSystemSpecs)
		{
			GA_Queue.AddItem(GA_GenericInfo.GetGenericInfo(), GA_Submit.CategoryType.GA_Log);
			RunCoroutine(GA_Queue.SubmitQueue());
		}
		
		//If we can get an internet connection then add any archived data to the submit queue
		if  (ArchiveData &&
			(Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || 
			(Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork && GA.ALLOWROAMING)))
		{
			List<GA_Submit.Item> items = GA_Archive.GetArchivedData();
			
			foreach (GA_Submit.Item item in items)
			{
				GA_Queue.AddItem(item.Parameters, item.Type);
			}
		}
		
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
			Debug.LogError("GA Error: No GA instance exists. Drag the GA game object into your scene.");
			return null;
		}
		
		return _instance.StartCoroutine(routine);
	}
	
	#endregion
}