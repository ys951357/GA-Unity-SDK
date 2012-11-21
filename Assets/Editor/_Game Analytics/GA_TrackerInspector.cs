/// <summary>
/// The inspector for the GA prefab.
/// </summary>

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using System;

[CustomEditor(typeof(GA_Tracker))]
public class GA_TrackerInspector : Editor
{
	override public void OnInspectorGUI ()
	{
		GA_Tracker gaTracker = target as GA_Tracker;
		
		GUILayout.Label("Tracked Events: " + gaTracker.ListTrackedEvents());
	}
}