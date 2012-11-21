/// <summary>
/// Add to a game object or prefab to set up Game Analytic's automatic event tracking.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GA_Tracker : MonoBehaviour
{
	public enum EventType { Start, OnDestroy, OnMouseDown, OnLevelWasLoaded, OnTriggerEnter, OnCollisionEnter, OnControllerColliderHit }
	
	public static Dictionary<EventType, string> EventTooltips = new Dictionary<EventType, string>()
	{
		{ EventType.Start, "Send an event when the Start method is run. Use this for tracking spawning of the object" },
		{ EventType.OnDestroy, "Send an event when the OnDestroy method is run. Use this for tracking \"death\" of the object." },
		{ EventType.OnMouseDown, "Send an event when the OnMouseDown method is run. Use this for tracking when the player performs a click/touch on the object." },
		{ EventType.OnLevelWasLoaded, "Send an event when the OnLevelWasLoaded method is run. Use this for tracking when a new level is loaded." },
		{ EventType.OnTriggerEnter, "Send an event when the OnTriggerEnter method is run. Use this for tracking when something (f.x. the player) enters a trigger area." },
		{ EventType.OnCollisionEnter, "Send an event when the OnCollisionEnter method is run. Use this for tracking when objects collide." },
		{ EventType.OnControllerColliderHit, "Send an event when the OnControllerColliderHit method is run. Use this for tracking when a controller hits a collider while performing a Move." }
	};
	
	[SerializeField]
	public List<EventType> TrackedEvents = new List<EventType>();
	
	public bool TrackedEventsFoldOut = true;
	
	private static Dictionary<string, int>[] _trackCounter;
	
	void Awake()
	{
		if (_trackCounter == null)
		{
			_trackCounter = new Dictionary<string, int>[Enum.GetValues(typeof(EventType)).Length];
			for (int i = 0; i < _trackCounter.Length; i++)
			{
				_trackCounter[i] = new Dictionary<string, int>();
			}
		}
	}
	
	void Start()
	{
		if (TrackedEvents.Contains(EventType.Start))
		{
			if (_trackCounter[(int)EventType.Start].ContainsKey(gameObject.name))
				_trackCounter[(int)EventType.Start][gameObject.name]++;
			else
				_trackCounter[(int)EventType.Start].Add(gameObject.name, 1);
			//GA_Design.NewEvent("Instantiated:"+gameObject.name, null, transform.position.x, transform.position.y, transform.position.z);
		}
	}
	
	void OnDestroy()
	{
		if (TrackedEvents.Contains(EventType.OnDestroy))
		{
			if (_trackCounter[(int)EventType.OnDestroy].ContainsKey(gameObject.name))
				_trackCounter[(int)EventType.OnDestroy][gameObject.name]++;
			else
				_trackCounter[(int)EventType.OnDestroy].Add(gameObject.name, 1);
			//GA_Design.NewEvent("Destroyed:"+gameObject.name, null, transform.position.x, transform.position.y, transform.position.z);
		}
	}
	
	void OnMouseDown()
	{
		if (TrackedEvents.Contains(EventType.OnMouseDown))
		{
			if (_trackCounter[(int)EventType.OnMouseDown].ContainsKey(gameObject.name))
				_trackCounter[(int)EventType.OnMouseDown][gameObject.name]++;
			else
				_trackCounter[(int)EventType.OnMouseDown].Add(gameObject.name, 1);
			//GA_Design.NewEvent("MouseDown:"+gameObject.name, null, transform.position.x, transform.position.y, transform.position.z);
		}
	}
	
	public void OnLevelWasLoaded ()
	{
		if (TrackedEvents.Contains(EventType.OnLevelWasLoaded))
		{
			GA_Design.NewEvent("LevelLoaded:"+gameObject.name, null, transform.position.x, transform.position.y, transform.position.z);
		}
	}
	
	public void OnTriggerEnter ()
	{
		if (TrackedEvents.Contains(EventType.OnTriggerEnter))
		{
			if (_trackCounter[(int)EventType.OnTriggerEnter].ContainsKey(gameObject.name))
				_trackCounter[(int)EventType.OnTriggerEnter][gameObject.name]++;
			else
				_trackCounter[(int)EventType.OnTriggerEnter].Add(gameObject.name, 1);
		}
	}
	
	public void OnCollisionEnter ()
	{
		if (TrackedEvents.Contains(EventType.OnCollisionEnter))
		{
			if (_trackCounter[(int)EventType.OnCollisionEnter].ContainsKey(gameObject.name))
				_trackCounter[(int)EventType.OnCollisionEnter][gameObject.name]++;
			else
				_trackCounter[(int)EventType.OnCollisionEnter].Add(gameObject.name, 1);
		}
	}
	
	public void OnControllerColliderHit ()
	{
		if (TrackedEvents.Contains(EventType.OnControllerColliderHit))
		{
			if (_trackCounter[(int)EventType.OnControllerColliderHit].ContainsKey(gameObject.name))
				_trackCounter[(int)EventType.OnControllerColliderHit][gameObject.name]++;
			else
				_trackCounter[(int)EventType.OnControllerColliderHit].Add(gameObject.name, 1);
		}
	}
	
	public static void SendTrackingData()
	{
		if (_trackCounter != null)
		{
			for (int i = 0; i < _trackCounter.Length; i++)
			{
				if (_trackCounter[i].Count > 0)
				{
					List<string> keys = new List<string>(_trackCounter[i].Keys);
					foreach (string key in keys)
					{
						GA_Design.NewEvent(((GA_Tracker.EventType)i).ToString() + ":" + key, _trackCounter[i][key]);
						_trackCounter[i].Remove(key);
					}
				}
			}
		}
	}
	
	public string ListTrackedEvents()
	{
		string listedEvents = "";
		TrackedEvents.Sort();
		foreach (EventType eventType in TrackedEvents)
		{
			if (listedEvents.Equals(""))
				listedEvents += eventType.ToString();
			else
				listedEvents += ", " + eventType.ToString();
		}
		
		if (!listedEvents.Equals(""))
			return listedEvents;
		else
			return "(None)";
	}
}