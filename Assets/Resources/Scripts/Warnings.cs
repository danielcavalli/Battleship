using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Warnings : MonoBehaviour {

	public Text WarningText; 
	int clear = 0;
	public static string type;
	void Start()
	{
		WarningText = gameObject.GetComponent<Text>();
	}
	IEnumerator Clear(float waitTime) 
	{
		yield return new WaitForSeconds(waitTime);
		type = "clear";
		StopCoroutine("Clear");
	}
	void OnGUI()
	{
		switch(type)
		{
			case "outside":
				GUI.Box(new Rect(0, Screen.height*0.5f, Screen.width, Screen.height*0.1f), "You can't put your ship outside the battle's area");
				StartCoroutine("Clear", 3);
				break;
			case "same_place":
				GUI.Box(new Rect(0, Screen.height*0.5f, Screen.width, Screen.height*0.1f), "You can't put two ships in the same place");
				StartCoroutine("Clear", 3);
				break;
			case "clear":
				break;
		}
	}
}
