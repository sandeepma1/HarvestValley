using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugTextHandler : MonoBehaviour
{

	public static DebugTextHandler m_instance = null;
	Text debugText;

	void Awake ()
	{
		m_instance = this;
		debugText = transform.GetComponent <Text> ();
	}

	public void DisplayDebugText (string text)
	{
		debugText.text = text;
	}

	public void Dropped ()
	{
		print ("dropped");
	}

	public void Grabbed ()
	{
		print ("Grabbed");
	}

	public void Removed ()
	{
		print ("Removed");
	}

	public void Changed (Vector2 pos)
	{
		print ("Changed" + pos);
	}
}
