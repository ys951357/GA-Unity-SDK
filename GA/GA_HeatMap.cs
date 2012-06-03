// The following is the result of some very early experimentation / ideas on how to handle heatmaps in a cool way.
// Since the team is doing other work at this time, heatmaps have been pushed to a later stage, and as such this
// code is likely to be changed completely at a later stage, when the heatmap system is designed.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GA_HeatMap : ScriptableObject
{
	public enum TrackingType { Death, Movement, Spawn, Custom }
	
	/// <summary>
	/// The custom name of this heat map
	/// </summary>
	public string Name;
	
	/// <summary>
	/// Inspector value only. Determines if the heat map submenu is folded or not
	/// </summary>
	public bool FoldOut;
	
	/// <summary>
	/// The unity game object which this heat map tracks
	/// </summary>
	public GameObject TargetObject;
	
	/// <summary>
	/// The type of tracking
	/// </summary>
	public TrackingType TrackType;
	
	/// <summary>
	/// The custom script which has the custom event to track 
	/// </summary>
	public MonoBehaviour CustomScript;
	
	/// <summary>
	/// The index of the custom event in the inspector
	/// </summary>
	public int CustomEventIndex;
	
	/// <summary>
	/// The custom event used to track for the heat map
	/// </summary>
	public string CustomEvent;
}