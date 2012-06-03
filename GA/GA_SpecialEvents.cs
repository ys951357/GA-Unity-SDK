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
	
	#endregion
	
	#region unity derived methods
	
	public void Awake ()
	{
		SceneChange();
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
			GA_Queue.QUITONSUBMIT = true;
			GA_Design.NewEvent("GA: Game Over");
			GA_Queue.SubmitQueue();
			Application.CancelQuit();
		}
#endif
	}
	
	#endregion
	
	#region private methods
	
	private void SceneChange()
	{
		if (GA.INCLUDESCENECHANGE)
		{
			GA_Design.NewEvent("GA:LevelEnded", Time.time - _lastLevelStartTime);
		}
		_lastLevelStartTime = Time.time;
	}
	
	#endregion
}
