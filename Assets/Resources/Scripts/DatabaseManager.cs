using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Collections;

public class DatabaseManager : MonoBehaviour {
	public string db_url;
    public static string playerLogged;
	public static string matchName = "";
	public string Tiles;
	public int attacks;

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

    public IEnumerator CreateMatchRoutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", playerLogged);
		form.AddField("matchName", matchName);
        WWW webRequest = new WWW(db_url + "CreateMatch.php",form);
        yield return webRequest;
		Debug.Log(webRequest.text);
		if (webRequest.text.Trim () == "MatchReady") {
			Debug.Log("partiu jogar");
		}
		else if (webRequest.text.Trim () != "Error") {
			matchName = webRequest.text;

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
                StartCoroutine(CreateMatchRoutine());
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

	public IEnumerator SendAttackRoutine()
	{
		WWWForm form = new WWWForm();
		form.AddField("matchName",playerLogged);
		form.AddField("player",matchName);
		//form.AddField("tile", Tiles);

		WWW webRequest = new WWW(db_url + "SendAttack.php", form);
		yield return webRequest;

		if (webRequest.text == "Attack sent") { Debug.Log("deu bom!"); }

		else Debug.Log(webRequest.text);
		//Tiles = "";
	}

}
