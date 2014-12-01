using UnityEngine; // 41 Post - Created by DimasTheDriver on Aug/25/2013. Part of the 'Unity3D: Styling a GUI toggle' post. Available at: http://www.41post.com/?p=5230 
using System.Collections;

public class SkinnedToggleGUI : MonoBehaviour 
{
	public GUISkin testGUIskin;
	
	private bool firstToggle = false; 
	private bool secondToggle = false;
	private bool thirdToggle = false;
	
	void Start () 
	{
		if(testGUIskin == null)
		{
			Debug.LogError("Please assign a GUIskin on the editor!");
			enabled = false;
			return;
		}
	}
	
	void OnGUI () 
	{
		firstToggle = GUI.Toggle(new Rect(32,16,64,64), firstToggle, "Simple Toggle", testGUIskin.customStyles[0]);
		secondToggle = GUI.Toggle(new Rect(32,96,64,64), secondToggle, "Toggle With Hover", testGUIskin.customStyles[1]);
		thirdToggle = GUI.Toggle(new Rect(32,176,64,64), thirdToggle, "Complete Toggle", testGUIskin.customStyles[2]);
	}
}
