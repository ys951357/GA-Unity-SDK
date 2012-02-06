// The following is the result of some very early experimentation / ideas on how to handle heatmaps in a cool way.
// Since the team is doing other work at this time, heatmaps have been pushed to a later stage, and as such this
// code is likely to be changed completely at a later stage, when the heatmap system is designed.

using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(GA_HeatMapCustomTracker))]
public class GA_HeatMapCustomTrackerInspector : Editor
{
	private GUIContent _heatmapLabel	= new GUIContent("Heat Map:", "The name of the Heat Map connected to this tracker.");
	private GUIContent _scriptLabel	= new GUIContent("Custom Script:", "The custom script containing the event which will trigger this tracker.");
	private GUIContent _eventLabel	= new GUIContent("Custom Event:", "The custom event which will trigger this tracker.");
	
	override public void OnInspectorGUI ()
	{
		GA_HeatMapCustomTracker gahmct = target as GA_HeatMapCustomTracker;
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("", GUILayout.Width(7));
	    GUILayout.Label(_heatmapLabel, GUILayout.Width(100));
	    GUILayout.Label(gahmct.HeatMap.Name);
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("", GUILayout.Width(7));
	    GUILayout.Label(_scriptLabel, GUILayout.Width(100));
	    if (gahmct.HeatMap.CustomScript != null)
			GUILayout.Label(gahmct.HeatMap.CustomScript.GetType().Name);
		else
			GUILayout.Label("<Not Set>");
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("", GUILayout.Width(7));
	    GUILayout.Label(_eventLabel, GUILayout.Width(100));
		if (gahmct.HeatMap.CustomEvent != null && gahmct.HeatMap.CustomEvent.Length > 0)
	    	GUILayout.Label(gahmct.HeatMap.CustomEvent);
		else
			GUILayout.Label("<Not Set>");
		GUILayout.EndHorizontal();
	}
}
