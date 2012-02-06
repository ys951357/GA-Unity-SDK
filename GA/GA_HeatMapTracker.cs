// The following is the result of some very early experimentation / ideas on how to handle heatmaps in a cool way.
// Since the team is doing other work at this time, heatmaps have been pushed to a later stage, and as such this
// code is likely to be changed completely at a later stage, when the heatmap system is designed.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GA_HeatMapTracker : MonoBehaviour
{
	public bool IsTrackingDeath;
	public bool IsTrackingMovement;
	public bool IsTrackingSpawn;
	
	public GA_HeatMap DeathHeatMap;
	public GA_HeatMap MovementHeatMap;
	public GA_HeatMap SpawnHeatMap;
	
	void Start()
	{
		if (IsTrackingSpawn && SpawnHeatMap != null)
		{
			GA_Design.NewEvent(SpawnHeatMap.Name+":"+gameObject.name, null, transform.position.x, transform.position.y, transform.position.z);
		}
	}
	
	void Update()
	{
		//movement tracking
	}
	
	void OnDestroy()
	{
		if (IsTrackingDeath && DeathHeatMap != null && !Application.isLoadingLevel)
		{
			GA_Design.NewEvent(DeathHeatMap.Name+":"+gameObject.name, null, transform.position.x, transform.position.y, transform.position.z);
		}
	}
}
