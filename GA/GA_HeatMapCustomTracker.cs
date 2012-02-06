// The following is the result of some very early experimentation / ideas on how to handle heatmaps in a cool way.
// Since the team is doing other work at this time, heatmaps have been pushed to a later stage, and as such this
// code is likely to be changed completely at a later stage, when the heatmap system is designed.

using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

public class GA_HeatMapCustomTracker : MonoBehaviour
{
	public GA_HeatMap HeatMap;
	
	void Start()
	{
		if (HeatMap.TrackType == GA_HeatMap.TrackingType.Custom && HeatMap.CustomEvent != null && HeatMap.CustomEvent.Length > 0 &&
		    HeatMap.CustomScript != null && GetComponent(HeatMap.CustomScript.GetType()) != null)
		{
			EventInfo eventInfo = HeatMap.CustomScript.GetType().GetEvent(HeatMap.CustomEvent);
			MethodInfo methodInfo = typeof(GA_HeatMapCustomTracker).GetMethod("CustomHeatMapCallback");
			Delegate handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, methodInfo);
			
			eventInfo.AddEventHandler(GetComponent(HeatMap.CustomScript.GetType()), handler);
		}
	}
	
	public void CustomHeatMapCallback(object sender, System.EventArgs e)
	{
		GA_Design.NewEvent(HeatMap.Name+":"+gameObject.name, null, transform.position.x, transform.position.y, transform.position.z);
	}
}
