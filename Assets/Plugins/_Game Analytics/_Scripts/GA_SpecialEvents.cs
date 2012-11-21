/// <summary>
/// This class handles special events unique to the Unity Wrapper, such as submitting level/scene changes, and delaying application quit
/// until data has been sent.
/// </summary>

using UnityEngine;
using System.Collections;

public class GA_SpecialEvents : MonoBehaviour
{
	#region private values
	
	private float _lastLevelStartTime = 0f;
	
	private int _frameCountAvg = 0;
	private float _lastUpdateAvg = 0f;
	private int _frameCountCrit = 0;
	private float _lastUpdateCrit = 0f;
	
	#endregion
	
	#region unity derived methods
	
	public void Awake ()
	{
		SceneChange();
	}
	
	public void Update()
	{
		//average FPS
		if (GA.SUBMITAVERAGEFPS)
		{
			_frameCountAvg++;
		}
		
		//critical FPS
		if (GA.SUBMITCRITICALFPS)
		{
			_frameCountCrit++;
		}
	}
	
	public void OnLevelWasLoaded ()
	{
		SceneChange();
	}
	
	public void OnApplicationQuit ()
	{
#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
		if (!GA_Queue.QUITONSUBMIT)
		{
			SubmitAverageFPS();
			GA_Tracker.SendTrackingData();
			
			GA_Queue.QUITONSUBMIT = true;
			GA_Design.NewEvent("GA:ExitGame");
			GA_Queue.SubmitQueue();
			Application.CancelQuit();
		}
#endif
	}
	
	public void SubmitAverageFPS()
	{
		//average FPS
		if (GA.SUBMITAVERAGEFPS)
		{
			float timeSinceUpdate = Time.time - _lastUpdateAvg;
			float fpsSinceUpdate = _frameCountAvg / timeSinceUpdate;
			_lastUpdateAvg = Time.time;
			_frameCountAvg = 0;
			
			if (fpsSinceUpdate > 0)
			{
				//Uses track target
				if (GA.TRACKTARGET != null)
				{
					GA_Quality.NewEvent("GA:AverageFPS", ((int)fpsSinceUpdate).ToString(), GA.TRACKTARGET.position.x, GA.TRACKTARGET.position.y, GA.TRACKTARGET.position.z);
				}
				else
				{
					GA_Quality.NewEvent("GA:AverageFPS", ((int)fpsSinceUpdate).ToString());
				}
			}
		}
	}
	
	public void SubmitCriticalFPS()
	{
		//average FPS
		if (GA.SUBMITCRITICALFPS)
		{
			float timeSinceUpdate = Time.time - _lastUpdateCrit;
			float fpsSinceUpdate = _frameCountCrit / timeSinceUpdate;
			_lastUpdateCrit = Time.time;
			_frameCountCrit = 0;
			
			if (fpsSinceUpdate <= GA.FPSCRITICALTHRESHOLD)
			{
				//Uses track target
				if (GA.TRACKTARGET != null)
				{
					GA_Quality.NewEvent("GA:CriticalFPS", _frameCountCrit.ToString(), GA.TRACKTARGET.position.x, GA.TRACKTARGET.position.y, GA.TRACKTARGET.position.z);
				}
				else
				{
					GA_Quality.NewEvent("GA:CriticalFPS", _frameCountCrit.ToString());
				}
			}
		}
	}
	
	#endregion
	
	#region private methods
	
	private void SceneChange()
	{
		SubmitAverageFPS();
		GA_Tracker.SendTrackingData();
		
		if (GA.INCLUDESCENECHANGE)
		{
			GA_Design.NewEvent("GA:LevelStarted", Time.time - _lastLevelStartTime);
		}
		_lastLevelStartTime = Time.time;
	}
	
	#endregion
}
