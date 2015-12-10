using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Collections;

public class DatabaseManager : MonoBehaviour {
	public string db_url;
	public static string playerLogged;
	public static string matchName = "";
	public static int Turn = 1;
	public string Tiles;
	public int attacks;
	public int[] ships = new int[5];

	
	void Update()
	{
		Debug.Log (Tiles);
		Debug.Log (attacks);
	}
	
	public void Login()
	{

		StartCoroutine(LogInRoutine());
	}
	
	public void Subscribe()
	{
		Debug.Log("Started");
		GameObject[] pass = GameObject.FindGameObjectsWithTag("subscribe_password");
		if (pass[0].GetComponent<InputField>().text == pass[1].GetComponent<InputField>().text)
			StartCoroutine(SubscribeRoutine());
		else
			Debug.Log("Hue");
	}
	
	public IEnumerator CreateMatchRoutine(string boolean)
	{
		WWWForm form = new WWWForm();
		form.AddField("username", playerLogged);
		form.AddField("matchName", matchName);
		form.AddField("justChecking", boolean);
		WWW webRequest = new WWW(db_url + "CreateMatch.php",form);
		yield return webRequest;
		Debug.Log(webRequest.text);
		Debug.Log ("WebRequest" + webRequest.text);
		if (webRequest.text.Trim () == "MatchReady") {
			Application.LoadLevel("Game");
		} else if (webRequest.text.Trim () != "Error" && matchName == "") {
			matchName = webRequest.text.Trim ();
		} else {
			StartCoroutine(CreateMatchRoutine("TRUE"));
		}
	}
	
	public IEnumerator LogInRoutine()
	{
		string usernameText =  GameObject.FindGameObjectWithTag("login_username").GetComponent<InputField>().text;
		string passwordText =  GameObject.FindGameObjectWithTag("login_password").GetComponent<InputField>().text;
		WWWForm form = new WWWForm ();
		form.AddField ("username",usernameText);
		form.AddField ("password", passwordText);
		WWW webRequest = new WWW (db_url + "Login.php", form);
		yield return webRequest;
		if (webRequest.text.Trim() == "TRUE")
		{
			playerLogged = usernameText;
			StartCoroutine(CreateMatchRoutine("FALSE"));
			Debug.Log(webRequest.text);
		}
		
	}
	
	public IEnumerator SubscribeRoutine()
	{
		string usernameText = GameObject.FindGameObjectWithTag("subscribe_username").GetComponent<InputField>().text;
		string passwordText = GameObject.FindGameObjectWithTag("subscribe_password").GetComponent<InputField>().text;
		WWWForm form = new WWWForm();
		form.AddField("username", usernameText);
		form.AddField("password", passwordText);
		form.AddField("state", "0");
		form.AddField("status", "Mehh");
		WWW webRequest = new WWW(db_url + "Subscribe.php", form);
		yield return webRequest;
		if (webRequest.text == "New record created successfully") { Debug.Log("deu bom!"); }
		else Debug.Log(webRequest.text);
	}
	
	bool CheckConnection(string URL)
	{
		try
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
			request.Timeout = 5000;
			request.Credentials = CredentialCache.DefaultNetworkCredentials;
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			
			if (response.StatusCode == HttpStatusCode.OK) return true;
			else return false;
		}
		catch
		{
			return false;
		}
	}
	
	public void Attack()
	{
		StartCoroutine (SendAttackRoutine ());
	}
	
	public IEnumerator sendAttackStatusRoutine(string[] t)
	{
		WWWForm form = new WWWForm();
		form.AddField("matchName",matchName);
		form.AddField("player",playerLogged);
		form.AddField("tile","Mano deu certo");
		WWW webRequest = new WWW(db_url + "SendAttackStatus.php", form);
		yield return webRequest;
		if(webRequest.text.Trim() == "Bum")Debug.Log("deus e pai e criador");
	}
	
	
	public IEnumerator checkAttackStatusRoutine()
	{
		WWWForm form = new WWWForm();
		form.AddField("matchName",matchName);
		form.AddField("player",playerLogged);
		
		WWW webRequest = new WWW(db_url + "CheckAttackStatus.php", form);
		yield return webRequest;
		if (webRequest.text.Trim () == "Nada") {
			StartCoroutine (checkAttackStatusRoutine ());
		} else {
			Debug.Log(webRequest.text.Trim());
			string[] tiles = webRequest.text.Trim().Split(',');
			StartCoroutine(sendAttackStatusRoutine(tiles));
		}
	}
	
	
	public IEnumerator SendAttackRoutine()
	{
		WWWForm form = new WWWForm();
		form.AddField("matchName",matchName);
		form.AddField("player",playerLogged);
		form.AddField("tile", Tiles);
		
		WWW webRequest = new WWW(db_url + "SendAttack.php", form);
		yield return webRequest;
		
		if (webRequest.text == "Attack sent") { 
			Tiles = "";
			StartCoroutine(checkAttackStatusRoutine());
		}
		
		else Debug.Log(webRequest.text);
	}
	
}
