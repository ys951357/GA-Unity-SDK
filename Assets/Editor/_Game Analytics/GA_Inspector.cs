/// <summary>
/// The inspector for the GA prefab.
/// </summary>

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using System;

[CustomEditor(typeof(GA))]
public class GA_Inspector : Editor
{
	private GUIContent _documentationLink		= new GUIContent("Help", "Opens the Game Analytics Unity3D wrapper documentation page in your browser. https://beta.gameanalytics.com/docs/unity3d.html");
	private GUIContent _checkForUpdates			= new GUIContent("Updates", "Checks if you have the newest version of the Game Analytics Unity3D wrapper.");
	private GUIContent _publicKeyLabel			= new GUIContent("Game Key", "Your Game Analytics public key - copy/paste from the GA website.");
	private GUIContent _privateKeyLabel			= new GUIContent("Secret Key", "Your Game Analytics private key - copy/paste from the GA website.");
	private GUIContent _closeHint				= new GUIContent("-", "Closes the current hint. This hint will not be displayed again. The \"Reset Hints\" button, in the Editor submenu below, will reset all closed hints so they will appear again.");
	private GUIContent _resetHints				= new GUIContent("Reset Hints", "Reset all closed hints so they will appear again.");
	private GUIContent _build					= new GUIContent("Build", "The current version of the game. Updating the build name for each test version of the game will allow you to filter by build when viewing your data on the GA website.");
	private GUIContent _displayHints			= new GUIContent("Display Hints", "Displays a box with hints, tips, and warnings at the top of the GA inspector.");
	private GUIContent _debugMode				= new GUIContent("Debug Mode", "Show additional debug messages from GA in the unity editor console.");
	private GUIContent _runInEditor				= new GUIContent("Run In Editor Play Mode", "Submit data to the Game Analytics server while playing your game in the Unity editor.");
	private GUIContent _trackTarget				= new GUIContent("Track Target", "The transform target used for special events, such as any submitted feedback or bug reports. Events will use this transforms's position (necessary for generating heatmaps for these events).");
	private GUIContent _customUserID			= new GUIContent("Custom User ID", "If enabled no data will be submitted until a custom user ID is provided. This is useful if you have your own log-in system, which ensures you have a unique user ID.");
	private GUIContent _qa						= new GUIContent("Quality Assurance", "This tab shows options for QA.");
	private GUIContent _qaSystemSpecs			= new GUIContent("Submit System Info", "Submit system information to the Game Analytics server when the game starts.");
	private GUIContent _qaFpsAverage			= new GUIContent("Submit Average FPS", "Submit the average frames per second.");
	private GUIContent _qaFpsCritical			= new GUIContent("Submit Critical FPS", "Submit a message whenever the frames per second falls below a certain threshold. The location of the Track Target will be used for critical FPS events.");
	private GUIContent _qaFpsCriticalThreshold	= new GUIContent("FPS<", "Frames per second threshold.");
	private GUIContent _qaErrorHandling			= new GUIContent("Error Handling", "Defines error and exception messages to be submitted to the Game Analytics server.");
	private GUIContent _qaMaxErrorCount			= new GUIContent("Max Error Count", "The maximum number of errors and exceptions tracked per session. It is a good idea to keep this number relatively low, so as to not submit a huge number of repeating exceptions.");
	private GUIContent _qaSubmitErrors			= new GUIContent("Submit Errors", "Submit error and exception messages to the Game Analytics server. Useful for getting relevant data when the game crashes, etc.");
	private GUIContent _qaErrorSubmitStackTrace	= new GUIContent("Submit Stack Trace", "Automatic error and exception messages sent to the Game Analytics server will include the error stack trace.");
	private GUIContent _qaErrorSystemSpecs		= new GUIContent("Submit System Info", "Automatic error and exception messages sent to the Game Analytics server will include system information.");
	private GUIContent _gd						= new GUIContent("Game Design", "This tab shows options for Game Design.");
	private GUIContent _gdSceneChange			= new GUIContent("Submit Level Transitions", "Submit special event to the Game Analytics server whenever a new level (scene) is loaded.");
	private GUIContent _data					= new GUIContent("Data", "This tab shows options related to how and when data is sent to Game Analytics.");
	private GUIContent _allowRoaming			= new GUIContent("Submit While Roaming", "If enabled and using a mobile device (iOS or Android), data will be submitted to the Game Analytics servers while the mobile device is roaming (internet connection via carrier data network).");
	private GUIContent _archiveData				= new GUIContent("Archive Data", "If enabled data will be archived when an internet connection is not available. When an internet connection is established again, any archived data will be sent.");
	private GUIContent _archiveMaxSize			= new GUIContent("Size<", "Indicates the maximum disk space used for archiving data in bytes.");
	private GUIContent _gui						= new GUIContent("Feedback GUI", "The Game Analytics user feedback GUI. Allows players to submit feedback and bug reports to the Game Analytics server.");
	private GUIContent _guiEnabled				= new GUIContent("GUI enabled", "Enable GUI for submitting feedback and bug reports. The location of the Track Target will be used for feedback events.");
	private GUIContent _general					= new GUIContent("General", "This tab shows general options which are relevant for a wide variety of messages sent to Game Analytics.");
	private GUIContent _editor					= new GUIContent("Editor", "This tab shows options which determine how the Game Analytics wrapper behaves in the editor.");
	private GUIContent _trackedEvents			= new GUIContent("Drag'n'Drop Tracker", "Drag game objects or prefabs to the empty container to automatically track any of the predefined events.");
	private GUIContent _removeTracker			= new GUIContent("-", "Stop automatic tracking for this game object or prefab.");
	
	static private GA_Tracker[] _gaTrackerObjects;
	
	override public void OnInspectorGUI ()
	{
		GA ga = target as GA;
		
		// we have to break prefab connection because it causes problems with this custom inspector
		// if persistent this is the real GA prefab so we skip this step
		if (!EditorUtility.IsPersistent (target ))
		{
			PrefabUtility.DisconnectPrefabInstance(ga.gameObject);
		}
		
		// Playing around with styles
		//GUIStyle style = new GUIStyle(GUI.skin.button);
		//style.normal.textColor = Color.red;
		// GUI.skin doesn't seem to work very well here..
		// GUI.skin = (GUISkin)Resources.Load("_Game Analytics/GA_Skin", typeof(GUISkin));
		
		EditorGUI.indentLevel = 1;
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
		EditorGUILayout.HelpBox("Game Analytics Unity3D Wrapper v." + GA.VERSION, UnityEditor.MessageType.None);
		if (GUILayout.Button(_documentationLink, GUILayout.MaxWidth(60)))
		{
			Application.OpenURL("https://beta.gameanalytics.com/docs/unity3d.html");
		}
		if (GUILayout.Button(_checkForUpdates, GUILayout.MaxWidth(60)))
		{
			Application.OpenURL("https://github.com/GameAnalytics/Unity-Wrapper/downloads");
		}
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		if (ga.DisplayHints)
		{
			GA.HelpInfo helpInfo = ga.GetHelpMessage();
			MessageType msgType = ConvertMessageType(helpInfo.MsgType);
			EditorGUILayout.HelpBox(helpInfo.Message, msgType);
			
			GUI.enabled = msgType == MessageType.Info;
			if (GUILayout.Button(_closeHint, GUILayout.MaxWidth(17)))
			{
				ga.ClosedHints.Add(helpInfo.HelpType);
			}
			GUI.enabled = true;
		}
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("", GUILayout.Width(7));
	    GUILayout.Label(_publicKeyLabel, GUILayout.Width(75));
		ga.PublicKey = EditorGUILayout.TextField("", ga.PublicKey);
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("", GUILayout.Width(7));
	    GUILayout.Label(_privateKeyLabel, GUILayout.Width(75));
		ga.PrivateKey = EditorGUILayout.TextField("", ga.PrivateKey);
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("", GUILayout.Width(7));
	    GUILayout.Label(_build, GUILayout.Width(75));
		ga.Build = EditorGUILayout.TextField("", ga.Build);
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
		
		ga.EditorFoldOut = EditorGUILayout.Foldout(ga.EditorFoldOut, _editor);
		
		if (ga.EditorFoldOut)
		{
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(27));
		    GUILayout.Label(_displayHints, GUILayout.Width(150));
			ga.DisplayHints = EditorGUILayout.Toggle("", ga.DisplayHints, GUILayout.MaxWidth(38));
			GUI.enabled = ga.ClosedHints.Count > 0;
			if (GUILayout.Button(_resetHints, GUILayout.MaxWidth(90)))
			{
				ga.ClosedHints.Clear();
			}
			GUI.enabled = true;
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(27));
		    GUILayout.Label(_debugMode, GUILayout.Width(150));
			ga.DebugMode = EditorGUILayout.Toggle("", ga.DebugMode);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(27));
		    GUILayout.Label(_runInEditor, GUILayout.Width(150));
		    ga.RunInEditorPlayMode = EditorGUILayout.Toggle("", ga.RunInEditorPlayMode);
			GUILayout.EndHorizontal();
		}
		
		EditorGUILayout.Space();
		
		ga.GeneralFoldOut = EditorGUILayout.Foldout(ga.GeneralFoldOut, _general);
		
		if (ga.GeneralFoldOut)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("", GUILayout.Width(27));
			GUILayout.Label(_trackTarget, GUILayout.Width(150));
			ga.TrackTarget = (Transform)EditorGUILayout.ObjectField(ga.TrackTarget, typeof(Transform), true, GUILayout.MaxWidth(150));
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(27));
		    GUILayout.Label(_customUserID, GUILayout.Width(150));
		    ga.CustomUserID = EditorGUILayout.Toggle("", ga.CustomUserID);
			GUILayout.EndHorizontal();
		}
		
		EditorGUILayout.Space();
		
		ga.DataFoldOut = EditorGUILayout.Foldout(ga.DataFoldOut, _data);
		
		if (ga.DataFoldOut)
		{
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(27));
		    GUILayout.Label(_archiveData, GUILayout.Width(150));
		    ga.ArchiveData = EditorGUILayout.Toggle("", ga.ArchiveData, GUILayout.Width(36));
			GUI.enabled = ga.ArchiveData;
			GUILayout.Label(_archiveMaxSize, GUILayout.Width(40));
			
			int tmpMaxArchiveSize = 0;
			if (int.TryParse(EditorGUILayout.TextField(ga.ArchiveMaxFileSize.ToString(), GUILayout.Width(48)), out tmpMaxArchiveSize))
			{
				ga.ArchiveMaxFileSize = Mathf.Max(Mathf.Min(tmpMaxArchiveSize, 2000), 0);
			}
			
			GUI.enabled = true;
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(27));
		    GUILayout.Label(_allowRoaming, GUILayout.Width(150));
		    ga.AllowRoaming = EditorGUILayout.Toggle("", ga.AllowRoaming);
			GUILayout.EndHorizontal();
		}
		
		EditorGUILayout.Space();
		
		ga.QAFoldOut = EditorGUILayout.Foldout(ga.QAFoldOut, _qa);
		
		if (ga.QAFoldOut)
		{
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(27));
		    GUILayout.Label(_qaSystemSpecs, GUILayout.Width(150));
		    ga.IncludeSystemSpecs = EditorGUILayout.Toggle("", ga.IncludeSystemSpecs);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(27));
		    GUILayout.Label(_qaFpsAverage, GUILayout.Width(150));
		    ga.SubmitFpsAverage = EditorGUILayout.Toggle("", ga.SubmitFpsAverage);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(27));
		    GUILayout.Label(_qaFpsCritical, GUILayout.Width(150));
		    ga.SubmitFpsCritical = EditorGUILayout.Toggle("", ga.SubmitFpsCritical, GUILayout.Width(35));
			GUI.enabled = ga.SubmitFpsCritical;
		    GUILayout.Label(_qaFpsCriticalThreshold, GUILayout.Width(35));
			
			int tmpFpsCriticalThreshold = 0;
			if (int.TryParse(EditorGUILayout.TextField(ga.FpsCriticalThreshold.ToString(), GUILayout.Width(32)), out tmpFpsCriticalThreshold))
			{
				ga.FpsCriticalThreshold = Mathf.Max(Mathf.Min(tmpFpsCriticalThreshold, 99), 5);
			}
			
			GUI.enabled = true;
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("", GUILayout.Width(15));
			ga.ErrorFoldOut = EditorGUILayout.Foldout(ga.ErrorFoldOut, _qaErrorHandling);
			GUILayout.EndHorizontal();
			
			if (ga.ErrorFoldOut)
			{
				GUILayout.BeginHorizontal();
			    GUILayout.Label("", GUILayout.Width(47));
			    GUILayout.Label(_qaSubmitErrors, GUILayout.Width(150));
			    ga.SubmitErrors = EditorGUILayout.Toggle("", ga.SubmitErrors);
				GUILayout.EndHorizontal();
				
				GUI.enabled = ga.SubmitErrors;
								
				GUILayout.BeginHorizontal();
			    GUILayout.Label("", GUILayout.Width(47));
			    GUILayout.Label(_qaErrorSubmitStackTrace, GUILayout.Width(150));
			    ga.SubmitErrorStackTrace = EditorGUILayout.Toggle("", ga.SubmitErrorStackTrace);
				GUILayout.EndHorizontal();
								
				GUILayout.BeginHorizontal();
			    GUILayout.Label("", GUILayout.Width(47));
			    GUILayout.Label(_qaErrorSystemSpecs, GUILayout.Width(150));
			    ga.SubmitErrorSystemInfo = EditorGUILayout.Toggle("", ga.SubmitErrorSystemInfo);
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
			    GUILayout.Label("", GUILayout.Width(47));
			    GUILayout.Label(_qaMaxErrorCount, GUILayout.Width(150));
				
				int tmpMaxErrorCount = 0;
				if (int.TryParse(EditorGUILayout.TextField(ga.MaxErrorCount.ToString(), GUILayout.Width(32)), out tmpMaxErrorCount))
				{
					ga.MaxErrorCount = Mathf.Max(Mathf.Min(tmpMaxErrorCount, 99), 1);
				}
				GUILayout.EndHorizontal();
				
				GUI.enabled = true;
			}
		}
		
		EditorGUILayout.Space();
		
		ga.GDFoldOut = EditorGUILayout.Foldout(ga.GDFoldOut, _gd);
		
		if (ga.GDFoldOut)
		{
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(27));
		    GUILayout.Label(_gdSceneChange, GUILayout.Width(150));
		    ga.IncludeSceneChange = EditorGUILayout.Toggle("", ga.IncludeSceneChange);
			GUILayout.EndHorizontal();
			
			if (_gaTrackerObjects == null)
			{
				_gaTrackerObjects = FindObjectsOfTypeIncludingAssets(typeof(GA_Tracker)) as GA_Tracker[];
			}
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("", GUILayout.Width(15));
			ga.TrackFoldOut = EditorGUILayout.Foldout(ga.TrackFoldOut, _trackedEvents);
			GUILayout.EndHorizontal();
			
			if (ga.TrackFoldOut)
			{
				foreach (GA_Tracker gaT in _gaTrackerObjects)
				{
					if (gaT != null)
					{
						GUILayout.BeginHorizontal();
					    GUILayout.Label("", GUILayout.Width(47));
						
						GameObject newGo = EditorGUILayout.ObjectField(gaT.gameObject, typeof(GameObject), true, GUILayout.Width(150)) as GameObject;
						
						if (newGo != gaT.gameObject)
						{
							GA_Tracker gaTracker = newGo.GetComponent<GA_Tracker>();
							if (gaTracker == null)
							{
								gaTracker = newGo.AddComponent<GA_Tracker>();
							}
						}
					
						if (GUILayout.Button(_removeTracker, new GUILayoutOption[] { GUILayout.MaxWidth(15), GUILayout.MaxHeight(15) }))
						{
							DestroyImmediate(gaT, true);
						}
						else
						{
							if (PrefabUtility.GetPrefabObject(gaT))
							{
						    	GUILayout.Label("(Prefab)", GUILayout.Width(47));
							}
							
							GUILayout.EndHorizontal();
							
							GUILayout.BeginHorizontal();
							GUILayout.Label("", GUILayout.Width(67));
							gaT.TrackedEventsFoldOut = EditorGUILayout.Foldout(gaT.TrackedEventsFoldOut, new GUIContent(gaT.name + " Events: " + gaT.ListTrackedEvents(), "The automatically tracked events for " + gaT.name + "."));
							GUILayout.EndHorizontal();
							
							if (gaT.TrackedEventsFoldOut)
							{
								for (int i = 0; i < Enum.GetValues(typeof(GA_Tracker.EventType)).Length; i++)
								{
									GA_Tracker.EventType eventType = (GA_Tracker.EventType)i;
									
									bool isEnabled = gaT.TrackedEvents.Contains(eventType);
									
									GUILayout.BeginHorizontal();
									GUILayout.Label("", GUILayout.Width(87));
									bool toggle = EditorGUILayout.Toggle(isEnabled, GUILayout.Width(25));
									GUILayout.Label(new GUIContent(eventType.ToString(), GA_Tracker.EventTooltips[eventType]));
									GUILayout.EndHorizontal();
									
									if (isEnabled != toggle)
									{
										if (toggle)
											gaT.TrackedEvents.Add(eventType);
										else
											gaT.TrackedEvents.Remove(eventType);
									}
								}
							}
							EditorGUILayout.Space();
						}
					}
				}
				
				GUILayout.BeginHorizontal();
				GUILayout.Label("", GUILayout.Width(47));
				GameObject goInstantiate = null;
				goInstantiate = (GameObject)EditorGUILayout.ObjectField(goInstantiate, typeof(GameObject), true, GUILayout.Width(150));
				GUILayout.EndHorizontal();
				
				if (goInstantiate != null)
				{
					GA_Tracker gaTracker = goInstantiate.GetComponent<GA_Tracker>();
					if (gaTracker == null)
					{
						gaTracker = goInstantiate.AddComponent<GA_Tracker>();
					}
				}
			}
		}
		
		EditorGUILayout.Space();
		
		ga.GuiFoldOut = EditorGUILayout.Foldout(ga.GuiFoldOut, _gui);
		
		if (ga.GuiFoldOut)
		{
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(27));
		    GUILayout.Label(_guiEnabled, GUILayout.Width(150));
		    ga.GuiEnabled = EditorGUILayout.Toggle("", ga.GuiEnabled);
			GUILayout.EndHorizontal();
		}
		
		if (GUI.changed) {
			_gaTrackerObjects = FindObjectsOfTypeIncludingAssets(typeof(GA_Tracker)) as GA_Tracker[];
            EditorUtility.SetDirty(ga);
        }
	}
	
	private MessageType ConvertMessageType(GA.MessageTypes msgType)
	{
		switch (msgType)
		{
			case GA.MessageTypes.Error:
				return MessageType.Error;
			case GA.MessageTypes.Info:
				return MessageType.Info;
			case GA.MessageTypes.Warning:
				return MessageType.Warning;
			default:
				return MessageType.None;
		}
	}
}