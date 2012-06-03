// The following is the result of some very early experimentation / ideas on how to handle heatmaps in a cool way.
// Since the team is doing other work at this time, heatmaps have been pushed to a later stage, and as such this
// code is likely to be changed completely at a later stage, when the heatmap system is designed.

using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(GA_HeatMapTracker))]
public class GA_HeatMapTrackerInspector : Editor
{
	private GUIContent _heatmapsLabel	= new GUIContent("Heat Maps:", "The names of the Heat Maps connected to this tracker.");
	private GUIContent _trackingLabel	= new GUIContent("Track Types:", "The tracking types of this tracker.");
	
	override public void OnInspectorGUI ()
	{
		GA_HeatMapTracker gahmt = target as GA_HeatMapTracker;
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("", GUILayout.Width(7));
	    GUILayout.Label(_heatmapsLabel, GUILayout.Width(100));
		string hmLbl = "";
		if (gahmt.DeathHeatMap != null && gahmt.IsTrackingDeath)
		{
			hmLbl = gahmt.DeathHeatMap.Name;
		}
		if (gahmt.MovementHeatMap != null && gahmt.IsTrackingMovement)
		{
			if (hmLbl.Length > 0)
				hmLbl += ", " + gahmt.MovementHeatMap.Name;
			else
				hmLbl = gahmt.MovementHeatMap.Name;
		}
		if (gahmt.SpawnHeatMap != null && gahmt.IsTrackingSpawn)
		{
			if (hmLbl.Length > 0)
				hmLbl += ", " + gahmt.SpawnHeatMap.Name;
			else
				hmLbl = gahmt.SpawnHeatMap.Name;
		}
	    GUILayout.Label(hmLbl);
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();	
	    GUILayout.Label("", GUILayout.Width(7));
	    GUILayout.Label(_trackingLabel, GUILayout.Width(100));
		string trackLbl = "";
		if (gahmt.IsTrackingDeath)
		{
			trackLbl = "Death";
		}
		if (gahmt.IsTrackingMovement)
		{
			if (trackLbl.Length > 0)
				trackLbl += ", Movement";
			else
				trackLbl = "Movement";
		}
		if (gahmt.IsTrackingSpawn)
		{
			if (trackLbl.Length > 0)
				trackLbl += ", Spawn";
			else
				trackLbl = "Spawn";
		}
	    GUILayout.Label(trackLbl);
		GUILayout.EndHorizontal();
	}
}
