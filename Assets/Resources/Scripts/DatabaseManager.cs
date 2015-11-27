using UnityEngine;
using System.Net;
using System.Collections;

public class DatabaseManager : MonoBehaviour {
	public string db_url = "http://battleshipp.pe.hu/";
	// Use this for initialization
	void Start () {
		StartCoroutine (Subscribe ());
	}

	public IEnumerator Subscribe()
	{
		if (CheckConnection (db_url + "default.php")) {
			WWWForm form = new WWWForm ();
			form.AddField ("action", "Subscribe");
			form.AddField ("username", "Victor");
			form.AddField ("hash", "12345");
			form.AddField ("state", "true");
			form.AddField ("status", "nothing");
			WWW webRequest = new WWW (db_url + "default.php", form);
			yield return webRequest;
			Debug.Log(webRequest.text);
		}
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

}
