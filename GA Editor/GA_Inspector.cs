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
	private GUIContent _publicKeyLabel	= new GUIContent("Public Key", "Your Game Analytics public key - copy/paste from the GA website.");
	private GUIContent _privateKeyLabel	= new GUIContent("Private Key", "Your Game Analytics private key - copy/paste from the GA website.");
	private GUIContent _build			= new GUIContent("Build", "The current version of the game. Updating the build name for each test version of the game will allow you to filter by build when viewing your data on the GA website.");
	private GUIContent _debugMode		= new GUIContent("Debug Mode", "Show additional debug messages from GA in the unity editor console.");
	private GUIContent _runInEditor		= new GUIContent("Run In Editor Play Mode", "Submit data to the Game Analytics server while playing your game in the Unity editor.");
	private GUIContent _qa				= new GUIContent("Quality Assurance", "This tab shows options for QA.");
	private GUIContent _qaSystemSpecs	= new GUIContent("Submit System Info", "Submit system information to the Game Analytics server.");
	private GUIContent _qaSubmitErrors	= new GUIContent("Submit Errors", "Submit error messages to the Game Analytics server. Useful for getting relevant data when the game crashes, etc.");
	private GUIContent _gd				= new GUIContent("Game Design", "This tab shows options for Game Design.");
	private GUIContent _gdSceneChange	= new GUIContent("Submit Level Transitions", "Submit special event to the Game Analytics server whenever a new level (scene) is loaded.");
	private GUIContent _b				= new GUIContent("Business", "This tab shows options for Business.");
	private GUIContent _p				= new GUIContent("Players", "This tab shows options for Players.");
	private GUIContent _hm				= new GUIContent("Heat Maps", "This tab shows options for Heat Maps.");
	private GUIContent _hmAddHm			= new GUIContent("Add Heat Map", "Add a new heat map tracker, which will submit data to the Game Analytics server.");
	private GUIContent _hmRemoveHm		= new GUIContent("-", "Delete this heat map.");
	private GUIContent _hmName			= new GUIContent("Name", "The name that best identifies what this heat map is tracking.");
	private GUIContent _hmGameObject	= new GUIContent("Game Object", "The game object that will be tracked by this heat map. If a scene object is used the heat map will track only that instance of the object. If a prefab is used the heat map will track any objects of that type.");
	private GUIContent _hmTracking		= new GUIContent("Tracking", "Indicates what the heat map will track. 'Death' tracks when the object is destroyed. 'Movement' tracks the objects movement. 'Spawn' tracks when the object is created. 'Custom' tracks a user defined event from a custom script.");
	private GUIContent _hmCustomScript	= new GUIContent("Custom Script", "The custom script which is responsible for what should be tracked");
	private GUIContent _hmCustomMethod	= new GUIContent("Custom Event", "The custom event which is fired when something should be tracked. GA will subscribe to this event and trigger the heat map.");
	private GUIContent _mobile			= new GUIContent("Mobile", "This tab shows options for mobile devices only (iOS and Android).");
	private GUIContent _allowRoaming	= new GUIContent("Submit While Roaming", "If enabled data will be submitted to the Game Analytics servers while the mobile device is roaming (internet connection via carrier data network).");
	private GUIContent _archiveData		= new GUIContent("Archive Data", "If enabled data will be archived if an internet connection is not available. The next time the game is started and an internet connection is available any archived data will be sent.");
	
	private List<GA_HeatMap> _heatMapsToRemove;
    private string[] _hmTrackOptions = new string[] { "Death", "Movement", "Spawn", "Custom" };
	
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
		
		// The following is the result of some very early experimentation / ideas on how to handle heatmaps in a cool way.
		// Since the team is doing other work at this time, heatmaps have been pushed to a later stage, and as such this
		// code is likely to be changed completely at a later stage, when the heatmap system is designed.
		
		ga.HMFoldOut = EditorGUILayout.Foldout(ga.HMFoldOut, _hm);
		
		if (ga.HMFoldOut)
		{	
			if (ga.HeatMaps == null)
			{
				ga.HeatMaps = new List<GA_HeatMap>();
			}
			if (_heatMapsToRemove == null)
			{
				_heatMapsToRemove = new List<GA_HeatMap>();
			}
			
			EditorGUILayout.Space();
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(27));
			if (GUILayout.Button(_hmAddHm, GUILayout.MaxWidth(100)))
			{
				GA_HeatMap newHM = GA_HeatMap.CreateInstance<GA_HeatMap>();
				newHM.Name = "Heat Map";
				ga.HeatMaps.Add(newHM);
			}
			GUILayout.EndHorizontal();
			
			if (ga.HeatMaps != null)
			{
				List<MonoBehaviour> scriptsToRemoveFromReplacedGOs = new List<MonoBehaviour>();
				List<MonoBehaviour> scriptsToRemoveFromNewTracking = new List<MonoBehaviour>();
				bool updateHeatMapTracker = false;
				
				for (int i = 0; i < ga.HeatMaps.Count; i++)
				{
					EditorGUILayout.Space();
					GUILayout.BeginHorizontal();
			    	GUILayout.Label("", GUILayout.Width(17));
					ga.HeatMaps[i].FoldOut = EditorGUILayout.Foldout(ga.HeatMaps[i].FoldOut, "Heat Map: " + ga.HeatMaps[i].Name);
					if (GUILayout.Button(_hmRemoveHm, GUILayout.MaxWidth(17)))
					{
						_heatMapsToRemove.Add(ga.HeatMaps[i]);
					}
					GUILayout.EndHorizontal();
					
					if (ga.HeatMaps[i].FoldOut)
					{
						
						//*** get corresponding custom tracker for custom events
						GA_HeatMapCustomTracker hmctExists = null;
						if (ga.HeatMaps[i].TargetObject != null)
						{
							GA_HeatMapCustomTracker[] hmcts = ga.HeatMaps[i].TargetObject.GetComponents<GA_HeatMapCustomTracker>();
							foreach (GA_HeatMapCustomTracker hmct in hmcts)
							{
								if (hmct.HeatMap == ga.HeatMaps[i])
									hmctExists = hmct;
							}
						}
						
						//*** heat map name ***
						EditorGUILayout.Space();
						GUILayout.BeginHorizontal();
					    GUILayout.Label("", GUILayout.Width(37));
						ga.HeatMaps[i].Name = EditorGUILayout.TextField(_hmName, ga.HeatMaps[i].Name);
						GUILayout.EndHorizontal();
						
						//*** game object ***
						GUILayout.BeginHorizontal();
					    GUILayout.Label("", GUILayout.Width(47));
					    GUILayout.Label(_hmGameObject);
						GameObject tmpTargetObject = ga.HeatMaps[i].TargetObject;
						ga.HeatMaps[i].TargetObject = (GameObject)EditorGUILayout.ObjectField(ga.HeatMaps[i].TargetObject, typeof(GameObject), true);
						GUILayout.EndHorizontal();
						
						if (tmpTargetObject != ga.HeatMaps[i].TargetObject)
						{
							if (ga.HeatMaps[i].TrackType != GA_HeatMap.TrackingType.Custom)
								updateHeatMapTracker = true;
							
							if (tmpTargetObject != null && tmpTargetObject.GetComponent<GA_HeatMapTracker>() != null)
							{
								scriptsToRemoveFromReplacedGOs.Add(tmpTargetObject.GetComponent<GA_HeatMapTracker>());
							}
							
							if (hmctExists != null)
							{
								DestroyImmediate(hmctExists, true);
							}
						}
						
						//*** track type ***
						GUILayout.BeginHorizontal();
					    GUILayout.Label("", GUILayout.Width(47));
					    GUILayout.Label(_hmTracking);
						GA_HeatMap.TrackingType tmpTrackType = ga.HeatMaps[i].TrackType;
						ga.HeatMaps[i].TrackType = (GA_HeatMap.TrackingType)EditorGUILayout.Popup((int)ga.HeatMaps[i].TrackType, _hmTrackOptions);
						GUILayout.EndHorizontal();
						
						if (tmpTrackType != ga.HeatMaps[i].TrackType)
						{
							if (ga.HeatMaps[i].TargetObject != null)
							{
								if (ga.HeatMaps[i].TargetObject.GetComponent<GA_HeatMapTracker>() != null)
								{
									SetupHeatMapTracker(ga.HeatMaps[i], tmpTrackType, false);
									
									if (tmpTrackType != GA_HeatMap.TrackingType.Custom && ga.HeatMaps[i].TrackType == GA_HeatMap.TrackingType.Custom)
									{
										scriptsToRemoveFromNewTracking.Add(ga.HeatMaps[i].TargetObject.GetComponent<GA_HeatMapTracker>());
									
										//setting updateHeatMapTracker to true here causes errors because of ResetToPrefabState
										//so instead we just update what is needed:
										foreach (GA_HeatMap hm in ga.HeatMaps)
										{
											if (hm.TrackType != GA_HeatMap.TrackingType.Custom && hm.TargetObject != null)
											{
												if (hm.TargetObject.GetComponent<GA_HeatMapTracker>() == null)
												{
													hm.TargetObject.AddComponent<GA_HeatMapTracker>();
												}
												
												SetupHeatMapTracker(hm, hm.TrackType, true);
											}
										}
									}
									else
									{
										updateHeatMapTracker = true;
									}
								}
								
								if (hmctExists != null)
								{
									if (tmpTrackType == GA_HeatMap.TrackingType.Custom && ga.HeatMaps[i].TrackType != GA_HeatMap.TrackingType.Custom)
									{
										DestroyImmediate(hmctExists, true);
									}
								}
							}
						}
						
						//*** Custom tracking ***
						if (ga.HeatMaps[i].TrackType == GA_HeatMap.TrackingType.Custom)
						{
							GUILayout.BeginHorizontal();
						    GUILayout.Label("", GUILayout.Width(47));
						    GUILayout.Label(_hmCustomScript);
							ga.HeatMaps[i].CustomScript = (MonoBehaviour)EditorGUILayout.ObjectField(ga.HeatMaps[i].CustomScript, typeof(MonoBehaviour), true);
							GUILayout.EndHorizontal();
							
							if (ga.HeatMaps[i].CustomScript != null)
							{
								Type t = ga.HeatMaps[i].CustomScript.GetType();
								/*MethodInfo[] miAdd = t.GetMethods();
								MethodInfo[] miRemove = t.BaseType.GetMethods();
								MethodInfo[] mi = new MethodInfo[miAdd.Length - miRemove.Length + 1];
								string[] _hmCustomMethodOptions = new string[miAdd.Length - miRemove.Length + 1];
								int k = 0;
								for (int u = 0; u < miAdd.Length; u++)
								{
									bool exclude = false;
									foreach (MethodInfo m in miRemove)
									{
										if (m.Name.Equals(miAdd[u].Name))
											exclude = true;
									}
									if (!exclude)
									{
										_hmCustomMethodOptions[k] = miAdd[u].Name;
										mi[k] = miAdd[u];
										k++;
									}
								}*/
								MethodInfo[] mi = t.GetMethods();
								List<string> _hmCustomMethodOptionsList = new List<string>();
								for (int u = 0; u < mi.Length; u++)
								{
									if (mi[u].Name.StartsWith("add_"))
									{
										_hmCustomMethodOptionsList.Add(mi[u].Name.Substring(4));
									}
								}
								string[] _hmCustomMethodOptions = _hmCustomMethodOptionsList.ToArray();
								
								GUILayout.BeginHorizontal();
							    GUILayout.Label("", GUILayout.Width(47));
							    GUILayout.Label(_hmCustomMethod);
								ga.HeatMaps[i].CustomEventIndex = EditorGUILayout.Popup(ga.HeatMaps[i].CustomEventIndex, _hmCustomMethodOptions);
								if (ga.HeatMaps[i].CustomEventIndex >= _hmCustomMethodOptions.Length)
								{
									ga.HeatMaps[i].CustomEventIndex = 0;
								}
								if (ga.HeatMaps[i].CustomEventIndex < _hmCustomMethodOptions.Length)
								{
									ga.HeatMaps[i].CustomEvent = _hmCustomMethodOptions[ga.HeatMaps[i].CustomEventIndex];
								}
								GUILayout.EndHorizontal();
							}
						}
						
						// *** Heat Map Custom Trackers ***
						if (ga.HeatMaps[i].TrackType == GA_HeatMap.TrackingType.Custom && ga.HeatMaps[i].TargetObject != null)
						{
							if (hmctExists == null)
							{
								hmctExists = ga.HeatMaps[i].TargetObject.AddComponent<GA_HeatMapCustomTracker>();
								hmctExists.HeatMap = ga.HeatMaps[i];
							}
						}
					}
				}
						
				// *** Heat Map Trackers ***
				if (updateHeatMapTracker)
				{
					foreach (GA_HeatMap hm in ga.HeatMaps)
					{
						if (hm.TrackType != GA_HeatMap.TrackingType.Custom && hm.TargetObject != null)
						{
							if (hm.TargetObject.GetComponent<GA_HeatMapTracker>() == null)
							{
								hm.TargetObject.AddComponent<GA_HeatMapTracker>();
							}
							
							SetupHeatMapTracker(hm, hm.TrackType, true);
						}
					}
				}
				
				// Remove unwanted scripts and update scripts
				if (_heatMapsToRemove.Count > 0 || scriptsToRemoveFromReplacedGOs.Count > 0 || scriptsToRemoveFromNewTracking.Count > 0)
				{
					List<MonoBehaviour> scriptsToRemove = new List<MonoBehaviour>();
					List<MonoBehaviour> scriptsToPreserve = new List<MonoBehaviour>();
					foreach (GA_HeatMap hm in ga.HeatMaps)
					{
						if (hm.TargetObject != null && hm.TargetObject.GetComponent<GA_HeatMapTracker>() != null)
						{
							if (_heatMapsToRemove.Contains(hm))
							{
								scriptsToRemove.Add(hm.TargetObject.GetComponent<GA_HeatMapTracker>());
							}
							else
							{
								scriptsToPreserve.Add(hm.TargetObject.GetComponent<GA_HeatMapTracker>());
							}
							
							if (hm.TrackType != GA_HeatMap.TrackingType.Custom &&
							    scriptsToRemoveFromNewTracking.Contains(hm.TargetObject.GetComponent<GA_HeatMapTracker>()))
							{
								scriptsToRemoveFromNewTracking.Remove(hm.TargetObject.GetComponent<GA_HeatMapTracker>());
							}
						}
					}
					scriptsToRemove.AddRange(scriptsToRemoveFromReplacedGOs);
					
					foreach (MonoBehaviour mb in scriptsToRemove)
					{
						if (!scriptsToPreserve.Contains(mb))
							DestroyImmediate(mb, true);
					}
					
					foreach (MonoBehaviour mb in scriptsToRemoveFromNewTracking)
					{
						DestroyImmediate(mb, true);
					}
					
					for (int i = 0; i < _heatMapsToRemove.Count; i++)
					{
						GA_HeatMap hm = _heatMapsToRemove[i];
						
						ga.HeatMaps.Remove(hm);
						
						if (hm.TargetObject != null)
						{
							if (hm.TargetObject.GetComponent<GA_HeatMapTracker>() != null)
								SetupHeatMapTracker(hm, hm.TrackType, false);
						
							GA_HeatMapCustomTracker hmctExists = null;
							GA_HeatMapCustomTracker[] hmcts = hm.TargetObject.GetComponents<GA_HeatMapCustomTracker>();
							foreach (GA_HeatMapCustomTracker hmct in hmcts)
							{
								if (hmct.HeatMap == hm)
									hmctExists = hmct;
							}
							
							if (hmctExists != null)
							{
								DestroyImmediate(hmctExists, true);
							}
						}
					}
					_heatMapsToRemove.Clear();
				}
			}
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
	}
	
	/// <summary>
	/// Sets tracking status of non-custom heat maps. Includes both prefab and scene objects.
	/// </summary>
	/// <param name="hm">
	/// The heat map <see cref="GA_HeatMap"/>
	/// </param>
	/// <param name="trackType">
	/// The tracking type to enable/disable <see cref="GA_HeatMap.TrackingType"/>
	/// </param>
	/// <param name="state">
	/// The new true/false state for the tracking type <see cref="System.Boolean"/>
	/// </param>
	private void SetupHeatMapTracker(GA_HeatMap hm, GA_HeatMap.TrackingType trackType, bool state)
	{
		switch (trackType)
		{
			case GA_HeatMap.TrackingType.Death:
				hm.TargetObject.GetComponent<GA_HeatMapTracker>().IsTrackingDeath = state;
				if (state) hm.TargetObject.GetComponent<GA_HeatMapTracker>().DeathHeatMap = hm;
				break;
			case GA_HeatMap.TrackingType.Movement:
				hm.TargetObject.GetComponent<GA_HeatMapTracker>().IsTrackingMovement = state;
				if (state) hm.TargetObject.GetComponent<GA_HeatMapTracker>().MovementHeatMap = hm;
				break;
			case GA_HeatMap.TrackingType.Spawn:
				hm.TargetObject.GetComponent<GA_HeatMapTracker>().IsTrackingSpawn = state;
				if (state) hm.TargetObject.GetComponent<GA_HeatMapTracker>().SpawnHeatMap = hm;
				break;
		}
		
		GA_HeatMapTracker[] hmtList = FindObjectsOfType(typeof(GA_HeatMapTracker)) as GA_HeatMapTracker[];
		foreach (GA_HeatMapTracker hmt in hmtList)
		{
			PrefabUtility.ResetToPrefabState(hmt);
			//EditorUtility.ResetToPrefabState(hmt); //Deprecated
		}
	}
}