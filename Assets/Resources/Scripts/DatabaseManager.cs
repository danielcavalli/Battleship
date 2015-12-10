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
	public string fromDbTile;
	public int attacks;
	public int[] ships = new int[5];
	public GameObject loading;

	void Start(){
		Debug.Log(getResultString("20,13,16",new int[]{5,12,13,23,20}));
	}

	public string getResultString(string t, int[] s){
		bool b = false;
		string r = "";
		string[] m = t.Split(',');
		foreach(string c in m){
			for (int i = 0; i< s.Length;i++) {
				if(c == s[i].ToString()){
					r += "t,";
					b = true;
					break;
				}
			}
			if(!b)r+="f,";
			b = false;
		}
		return r.Remove((r.LastIndexOf(",")));
	}

	public void Login()
	{
		loading.SetActive (true);
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
		Debug.LogWarning("Ta entrando na routine");
        WWWForm form = new WWWForm();
        form.AddField("username", playerLogged);
		form.AddField("matchName", matchName);
		form.AddField("justChecking", boolean);
        WWW webRequest = new WWW(db_url + "CreateMatch.php",form);
        yield return webRequest;
		Debug.Log(webRequest.text);
		if (webRequest.text.Trim () == "MatchReady") {
			Application.LoadLevel("Game");
		} else if (webRequest.text.Trim () != "Error" && matchName == "") {
			matchName = webRequest.text.Trim ();
			StartCoroutine(CreateMatchRoutine("TRUE"));
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
		}else Debug.Log("Meh");
            
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

	public IEnumerator sendAttackStatusRoutine(string t)
	{
		Debug.Log ("sendAttackStatusRoutine");
		string r = getResultString (t, ships);
		WWWForm form = new WWWForm();
		form.AddField("matchName",matchName);
		form.AddField("player",playerLogged);
		form.AddField("tile",r);
		WWW webRequest = new WWW(db_url + "SendAttackStatus.php", form);
		yield return webRequest;
		if(webRequest.text.Trim() == "Bum")Debug.Log("deus e pai e criador");
	}


	public IEnumerator checkAttackStatusRoutine()
	{
		Debug.Log ("checkAttackStatusRoutine");
		WWWForm form = new WWWForm();
		form.AddField("matchName",matchName);
		form.AddField("player",playerLogged);

		WWW webRequest = new WWW(db_url + "CheckAttackStatus.php", form);
		yield return webRequest;
		if (webRequest.text.Trim () == "Nada") {
			StartCoroutine (checkAttackStatusRoutine ());
		} else {
			string tiles = webRequest.text.Trim();
			StartCoroutine(sendAttackStatusRoutine(tiles));
			Debug.Log("LOL");
		}
	}

	public IEnumerator getFeedbackRoutine()
	{
		Debug.Log ("getFeedbackRoutine");
		WWWForm form = new WWWForm();
		form.AddField("matchName",matchName);
		form.AddField("player",playerLogged);
		
		WWW webRequest = new WWW(db_url + "CheckAttackStatus.php", form);
		yield return webRequest;
		if (webRequest.text.Trim () == "Nada") {
			StartCoroutine(getFeedbackRoutine());
		} else {
			string feedback = webRequest.text.Trim();
			Debug.Log("recebeu feedback: " + feedback);
		}
	}


	public IEnumerator SendAttackRoutine()
	{
		Debug.Log ("SendAttackRoutine");
		WWWForm form = new WWWForm();
		form.AddField("matchName",matchName);
		form.AddField("player",playerLogged);
		form.AddField("tile", Tiles);

		WWW webRequest = new WWW(db_url + "SendAttack.php", form);
		yield return webRequest;

		if (webRequest.text == "Attack sent") { 
			StartCoroutine (getFeedbackRoutine ());
			Tiles = "";
			StartCoroutine (checkAttackStatusRoutine ());
		} else {
			Debug.Log (webRequest.text);
			StartCoroutine (SendAttackRoutine ());
		}
	}

}
