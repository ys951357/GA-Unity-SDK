using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
public class GA_ExampleManager : MonoBehaviour
{
	private string _exampleGameKey = "0c3c6e8c8896556a947a1f7c06c5df06";
	private string _exampleSecretKey = "f0e52c8b2422b39c0e3ed4be19792b6d2c81ef0a";
	
	[SerializeField]
	public static bool HasShownTutorialWindow;
	
	void Start()
	{
		if (Application.isPlaying)
		{
			GA.API.Submit.SetupKeys(_exampleGameKey, _exampleSecretKey);
			
			GA.Log("Changed GameAnalytics Game Key and Secret Key for this example game.");
		}
		else
		{
			if (!HasShownTutorialWindow)
			{
				HasShownTutorialWindow = true;
				GA_ExampleTutorial tutorial = ScriptableObject.CreateInstance<GA_ExampleTutorial>();
				tutorial.ShowUtility();
				tutorial.position = new Rect(150, 150, 420, 340);
			}
		}
	}
}
