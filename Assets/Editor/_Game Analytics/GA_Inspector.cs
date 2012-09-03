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
	private GUIContent _publicKeyLabel			= new GUIContent("Game Key", "Your Game Analytics public key - copy/paste from the GA website.");
	private GUIContent _privateKeyLabel			= new GUIContent("Secret Key", "Your Game Analytics private key - copy/paste from the GA website.");
	private GUIContent _build					= new GUIContent("Build", "The current version of the game. Updating the build name for each test version of the game will allow you to filter by build when viewing your data on the GA website.");
	private GUIContent _debugMode				= new GUIContent("Debug Mode", "Show additional debug messages from GA in the unity editor console.");
	private GUIContent _runInEditor				= new GUIContent("Run In Editor Play Mode", "Submit data to the Game Analytics server while playing your game in the Unity editor.");
	private GUIContent _trackTarget				= new GUIContent("Track Target", "The transform target used for special events, such as any submitted feedback or bug reports. Events will use this transforms's position (necessary for generating heatmaps for these events).");
	private GUIContent _qa						= new GUIContent("Quality Assurance", "This tab shows options for QA.");
	private GUIContent _qaSystemSpecs			= new GUIContent("Submit System Info", "Submit system information to the Game Analytics server.");
	private GUIContent _qaSubmitErrors			= new GUIContent("Submit Errors", "Submit error messages to the Game Analytics server. Useful for getting relevant data when the game crashes, etc.");
	private GUIContent _qaFpsAverage			= new GUIContent("Submit Average FPS", "Submit the average frames per second.");
	private GUIContent _qaFpsCritical			= new GUIContent("Submit Critical FPS", "Submit a message whenever the frames per second falls below a certain threshold. The location of the Track Target will be used for critical FPS events.");
	private GUIContent _qaFpsCriticalThreshold	= new GUIContent("FPS<", "Frames per second threshold.");
	private GUIContent _gd						= new GUIContent("Game Design", "This tab shows options for Game Design.");
	private GUIContent _gdSceneChange			= new GUIContent("Submit Level Transitions", "Submit special event to the Game Analytics server whenever a new level (scene) is loaded.");
	private GUIContent _b						= new GUIContent("Business", "This tab shows options for Business.");
	private GUIContent _p						= new GUIContent("Players", "This tab shows options for Players.");
	private GUIContent _mobile					= new GUIContent("Mobile", "This tab shows options for mobile devices only (iOS and Android).");
	private GUIContent _allowRoaming			= new GUIContent("Submit While Roaming", "If enabled data will be submitted to the Game Analytics servers while the mobile device is roaming (internet connection via carrier data network).");
	private GUIContent _archiveData				= new GUIContent("Archive Data", "If enabled data will be archived when an internet connection is not available. The next time the game is started and an internet connection is available any archived data will be sent.");
	private GUIContent _gui						= new GUIContent("Feedback GUI", "The Game Analytics user feedback GUI. Allows players to submit feedback and bug reports to the Game Analytics server.");
	private GUIContent _guiEnabled				= new GUIContent("GUI enabled", "Enable GUI for submitting feedback and bug reports. The location of the Track Target will be used for feedback events.");
	
	override public void OnInspectorGUI ()
	{
		GA ga = target as GA;
		
		// we have to break prefab connection because it causes problems with this custom inspector
		// if persistent this is the real GA prefab so we skip this step
		if (!EditorUtility.IsPersistent (target ))
		{
			PrefabUtility.DisconnectPrefabInstance(ga.gameObject);
		}
		
		EditorGUI.indentLevel = 1;
		EditorGUILayout.Space();
		
		ga.PublicKey = EditorGUILayout.TextField(_publicKeyLabel, ga.PublicKey);
		ga.PrivateKey = EditorGUILayout.TextField(_privateKeyLabel, ga.PrivateKey);
		
		EditorGUILayout.Space();
		
		ga.Build = EditorGUILayout.TextField(_build, ga.Build);
		
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("", GUILayout.Width(7));
	    GUILayout.Label(_debugMode, GUILayout.Width(150));
		ga.DebugMode = EditorGUILayout.Toggle("", ga.DebugMode);
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("", GUILayout.Width(7));
	    GUILayout.Label(_runInEditor, GUILayout.Width(150));
	    ga.RunInEditorPlayMode = EditorGUILayout.Toggle("", ga.RunInEditorPlayMode);
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("", GUILayout.Width(7));
	    GUILayout.Label(_archiveData, GUILayout.Width(150));
	    ga.ArchiveData = EditorGUILayout.Toggle("", ga.ArchiveData);
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("", GUILayout.Width(7));
		GUILayout.Label(_trackTarget, GUILayout.Width(150));
		ga.TrackTarget = (Transform)EditorGUILayout.ObjectField(ga.TrackTarget, typeof(Transform), true, GUILayout.MaxWidth(150));
		GUILayout.EndHorizontal();
		
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
		    GUILayout.Label(_qaSubmitErrors, GUILayout.Width(150));
		    ga.SubmitErrors = EditorGUILayout.Toggle("", ga.SubmitErrors);
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
		    GUILayout.Label(_qaFpsCriticalThreshold, GUILayout.Width(35));
			
			int tmpFpsCriticalThreshold = 0;
			if (int.TryParse(EditorGUILayout.TextField(ga.FpsCriticalThreshold.ToString(), GUILayout.Width(32)), out tmpFpsCriticalThreshold))
			{
				ga.FpsCriticalThreshold = Mathf.Max(Mathf.Min(tmpFpsCriticalThreshold, 99), 5);
			}
			
			GUILayout.EndHorizontal();
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
		}
		
		EditorGUILayout.Space();
		
		ga.BFoldOut = EditorGUILayout.Foldout(ga.BFoldOut, _b);
		
		if (ga.BFoldOut)
		{
		}
		
		EditorGUILayout.Space();
		
		ga.PFoldOut = EditorGUILayout.Foldout(ga.PFoldOut, _p);
		
		if (ga.PFoldOut)
		{
		}
		
		EditorGUILayout.Space();
		
		ga.MobileFoldOut = EditorGUILayout.Foldout(ga.MobileFoldOut, _mobile);
		
		if (ga.MobileFoldOut)
		{
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(27));
		    GUILayout.Label(_allowRoaming, GUILayout.Width(150));
		    ga.AllowRoaming = EditorGUILayout.Toggle("", ga.AllowRoaming);
			GUILayout.EndHorizontal();
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
	}
}