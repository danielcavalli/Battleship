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
		type = "Clear";
		StopCoroutine("Clear");
	}
	void OnGUI()
	{
		switch(type)
		{
			case "Outside":
				GUI.Box(new Rect(0, Screen.height*0.5f, Screen.width, Screen.height*0.1f), "You can't put your ship outside the battle's area");
				StartCoroutine("Clear", 3);
				break;
			case "Clear":
				break;
		}
	}
}
