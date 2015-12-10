using UnityEngine;
using System.Collections;

public class TileEnemySelect : MonoBehaviour {

	private GameObject manager;
	void Start()
	{
		manager = GameObject.Find ("GameManager");
	}

	void OnMouseDown()
	{
		if (manager.GetComponent<SetMap> ().DropPhase >= 2) {

			string tile = this.gameObject.name.Remove (0, 5);
			
			manager.GetComponent<DatabaseManager>().attacks ++;
			if (manager.GetComponent<DatabaseManager>().attacks <=3) {

				if(manager.GetComponent<DatabaseManager>().attacks == 3)
					manager.GetComponent<DatabaseManager> ().Tiles = manager.GetComponent<DatabaseManager> ().Tiles + tile;

				else if(manager.GetComponent<DatabaseManager>().attacks <= 2)
					manager.GetComponent<DatabaseManager> ().Tiles = manager.GetComponent<DatabaseManager> ().Tiles + tile + ",";
			} 
			else {
				Warnings.type = "attacked";
				Debug.Log ("You have attacked 3 times");
			}

		}
	}
}
