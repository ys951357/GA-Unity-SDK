/// <summary>
/// This class handles any GUI related to the Game Analytics service, such as user feedback
/// </summary>

using UnityEngine;
using System.Collections;

public class GA_Gui : MonoBehaviour
{
	public enum WindowType { None, MessageTypeWindow, FeedbackWindow, BugWindow };
	
	// feedback/bug report windows
	private Texture _gaLogo;
	private WindowType _windowType = WindowType.None;
	
	private bool _popUpError = false;
	
	private Rect _messageTypeWindowRect;
	private Rect _feedbackWindowRect;
	private Rect _bugWindowRect;
	
	private string _topic = "";
	private string _details = "";
	
	void Start ()
	{
		_gaLogo = (Texture)Resources.Load("_Game Analytics/_Images/gaLogo", typeof(Texture));
		
		_messageTypeWindowRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 75, 400, 150);
		
		_feedbackWindowRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 150, 400, 300);
		_bugWindowRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 150, 400, 300);
	}
	
	void OnGUI ()
	{
		Texture2D onNormalSkin = GUI.skin.window.onNormal.background;
		GUI.skin.window.onNormal.background = GUI.skin.window.normal.background;
		
		// feedback/bug report windows
		if (GA.GUIENABLED || _windowType != WindowType.None)
		{
			switch (_windowType)
			{
				case WindowType.None:
					if (GUI.Button(new Rect(Screen.width - 72, Screen.height - 72, 64, 64), _gaLogo))
					{
						_windowType = WindowType.MessageTypeWindow;
					}
					break;
				case WindowType.MessageTypeWindow:
					_messageTypeWindowRect = GUI.Window(0, _messageTypeWindowRect, MessageTypeWindow, "");
					break;
				case WindowType.FeedbackWindow:
					_feedbackWindowRect = GUI.Window(0, _feedbackWindowRect, FeedbackWindow, "");
					break;
				case WindowType.BugWindow:
					_bugWindowRect = GUI.Window(0, _bugWindowRect, BugWindow, "");
					break;
			}
		}
		
		GUI.skin.window.onNormal.background = onNormalSkin;
	}
	
	#region feedback/bug report windows
	
	void MessageTypeWindow(int windowID)
	{
		GUI.Label(new Rect(10, 15, 380, 50), "Help improve this game by submitting feedback/bug reports");
		
		if (GUI.Button(new Rect(10, 50, 185, 90), "Feedback"))
		{
			_windowType = WindowType.FeedbackWindow;
			GUI.FocusControl("TopicField");
		}
		if (GUI.Button(new Rect(205, 50, 185, 90), "Bug Report"))
		{
			_windowType = WindowType.BugWindow;
			GUI.FocusControl("TopicField");
		}
    }
	
	void FeedbackWindow(int windowID)
	{
		int heightAdd = 0;
		
		GUI.Label(new Rect(10, 15, 380, 50), "Submit feedback");
		
		GUI.Label(new Rect(10, 50, 380, 20), "Topic*");
		GUI.SetNextControlName("TopicField");
		_topic = GUI.TextField(new Rect(10, 70, 380, 20), _topic, 50);
		
		GUI.Label(new Rect(10, 100, 380, 20), "Details*");
		_details = GUI.TextArea(new Rect(10, 120, 380, 130), _details, 400);
		
		if (GUI.Button(new Rect(10, 260 + heightAdd, 185, 30), "Cancel"))
		{
			_topic = "";
			_details = "";
			_windowType = WindowType.None;
		}
		if (GUI.Button(new Rect(205, 260 + heightAdd, 185, 30), "Submit"))
		{
			if (_topic.Length > 0 && _details.Length > 0)
			{
				Vector3 target = Vector3.zero;
				if (GA.TRACKTARGET)
				{
					target = GA.TRACKTARGET.position;
				}
				
				GA_Quality.NewEvent("Feedback:"+_topic, _details, target.x, target.y, target.z);
				
				_topic = "";
				_details = "";
				_windowType = WindowType.None;
			}
			else
			{
				// _topic and details must be filled out
			}
		}
    }
	
	void BugWindow(int windowID)
	{
		int heightAdd = 0;
		
		if (_popUpError)
			GUI.Label(new Rect(10, 10, 385, 50), "Oops! It looks like an error just occurred! Please fill out this form with as many details as possible (what you were doing, etc.).");
		else
			GUI.Label(new Rect(10, 15, 380, 50), "Submit bug report");
		
		GUI.Label(new Rect(10, 50, 380, 20), "Topic*");
		GUI.SetNextControlName("TopicField");
		_topic = GUI.TextField(new Rect(10, 70, 380, 20), _topic, 50);
		
		GUI.Label(new Rect(10, 100, 380, 20), "Details*");
		_details = GUI.TextArea(new Rect(10, 120, 380, 130), _details, 400);
		
		if (GUI.Button(new Rect(10, 260 + heightAdd, 185, 30), "Cancel"))
		{
			_topic = "";
			_details = "";
			_windowType = WindowType.None;
			_popUpError = false;
		}
		if (GUI.Button(new Rect(205, 260 + heightAdd, 185, 30), "Submit"))
		{
			if (_topic.Length > 0 && _details.Length > 0)
			{
				Vector3 target = Vector3.zero;
				if (GA.TRACKTARGET)
				{
					target = GA.TRACKTARGET.position;
				}
				
				if (_popUpError)
					GA_Debug.SubmitError("Crash Report:"+_topic, _details);
				else
					GA_Quality.NewEvent("Bug Report:"+_topic, _details, target.x, target.y, target.z);
				
				_topic = "";
				_details = "";
				_windowType = WindowType.None;
				_popUpError = false;
			}
			else
			{
				// _topic and details must be filled out
			}
		}
    }
	
	#endregion
	
	public void OpenBugGUI()
	{
		_windowType = WindowType.BugWindow;
		_popUpError = true;
	}
}