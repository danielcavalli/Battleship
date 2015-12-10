using UnityEngine;
using System.Collections;

public class SetMap : MonoBehaviour 
{

	int rows = 5;
	int columns = 5;
	GameObject Tile;
	GameObject TileEnemy;
	GameObject prefabShip1;
	GameObject prefabShip2;
	GameObject prefabGhostShip;
	GameObject Ghosts;
	DatabaseManager dbMng;
	int rotation;
	
	float[] gridX;
	float[] gridY;
	int name;
	bool Ghost = false;
	public float playerOffSetX;
	public float playerOffSetY;
	public float enemyOffSetX;
	public float enemyOffSetY;
	public static bool Loaded;
	
	public int DropPhase = 0;
	Vector2 mouse;
	RaycastHit2D hit;

	void SetTiles(){
		gridX = new float[rows];
		gridY = new float[columns];
		for (int i = 0; i < rows; i++){
			for(int j = 0;j < columns;j++){
				gridX[j] = -1.5f * j;
				gridY[i] = 1.5f * i;
				name++;
				GameObject Tiles = Instantiate(Tile,new Vector3(playerOffSetX - gridX[j], playerOffSetY - gridY[i], 0), transform.rotation) as GameObject;
				GameObject eTiles = Instantiate(TileEnemy,new Vector3(playerOffSetX - gridX[j] + (1.6f * columns), playerOffSetY - gridY[i], 0), transform.rotation) as GameObject;
				Tiles.name = "" + name;
				eTiles.name = "Enemy" + name;
				eTiles.AddComponent<TileEnemySelect>();
				if(name == 25)
				{
					Loaded = true;
				}
			}
		}
	}

	void Start () {
		Tile = Resources.Load("Prefabs/Tile") as GameObject;
		TileEnemy = Resources.Load("Prefabs/TileEnemy") as GameObject;
		prefabShip1 = Resources.Load("Prefabs/Ship") as GameObject;
		prefabShip2 = Resources.Load("Prefabs/Ship2") as GameObject;
		prefabGhostShip = Resources.Load("Prefabs/GhostShip") as GameObject;
		SetTiles();
		dbMng = GameObject.Find ("GameManager").GetComponent<DatabaseManager> ();
	}
	
	void Update ()	{
		if(DropPhase<2){
			DropPhaseUpdate();
		}
		//Debug.Log (checkShipCollision (new int[]{1,2,3}));
	}

	void DropPhaseUpdate(){
		mouse = Camera.main.ScreenToWorldPoint( Input.mousePosition );
		hit = Physics2D.Raycast( mouse, Vector2.zero );
		if(!Ghost && hit.collider != null && hit.collider.tag=="Player"){
				if(DropPhase == 0)
				Ghosts = Instantiate(prefabGhostShip,new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, -1), transform.rotation) as GameObject;
				else if(DropPhase == 1)
				Ghosts = Instantiate(prefabShip2,new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, -1), transform.rotation) as GameObject;
				Ghost = true;
			}
		else if(Ghost && hit.collider != null&& hit.collider.tag=="Player"){
				Ghosts.transform.position = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, -1);
				if(Input.GetKeyDown ("r")){
					if(rotation == 0){
						rotation = 270;
					}
					else if(rotation == 270){
						rotation = 0;
					}
					Ghosts.transform.eulerAngles = new Vector3(0,0,rotation);
				}
			}
			//Drop
		int initialPos = 0;
		try{
			initialPos = int.Parse(hit.collider.name);
//			Debug.Log((checkShipCollision(new int[] {initialPos,initialPos + 5,initialPos +10}) && Ghosts.transform.eulerAngles == Vector3.zero) ||
//			          (checkShipCollision(new int[] {initialPos, initialPos -1,initialPos -2})&& Ghosts.transform.eulerAngles == new Vector3(0,0,270f)));
		}catch{}
		if(DropPhase == 0 && hit.collider != null && hit.collider.tag=="Player"){
				if (hit.collider != null && hit.collider.name != "Ship" && 
			    ((int.Parse(hit.collider.name) <= 15 && Ghosts.transform.eulerAngles == Vector3.zero) || (int.Parse(hit.collider.name) <= 25 && Ghosts.transform.eulerAngles == new Vector3(0,0,270f))) && Input.GetKeyDown(KeyCode.Mouse0) ){
					GameObject ship = Instantiate(prefabShip1,new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, -1), Ghosts.transform.rotation) as GameObject;
					if(ship.transform.eulerAngles == new Vector3(0,0,270f)){
						
						ship.GetComponent<ShipBehavior>().points = new int[] {initialPos, initialPos -1,initialPos -2};
						dbMng.ships[0] = initialPos;	
						dbMng.ships[1] = initialPos - 1;
						dbMng.ships[2] = initialPos - 2;
					}
					else{
						ship.GetComponent<ShipBehavior>().points = new int[] {initialPos,initialPos + 5,initialPos +10};
						
						dbMng.ships[0] = initialPos;	
						dbMng.ships[1] = initialPos + 5;
						dbMng.ships[2] = initialPos + 10;
					}
					DropPhase=1;
					Destroy(Ghosts);
					Ghost = false;
				}
				else if(hit.collider == null || int.Parse(hit.collider.name) > 15){
					if(Input.GetKeyDown(KeyCode.Mouse0)){
						Warnings.type = "outside";
					}
				}
			}
		else if(DropPhase == 1 && hit.collider != null&& hit.collider.tag=="Player"){
			if (hit.collider != null && hit.collider.name != "Ship"  &&
			    ((int.Parse(hit.collider.name) <= 20 && Ghosts.transform.eulerAngles == Vector3.zero) || 
			 (int.Parse(hit.collider.name) <= 25 && Ghosts.transform.eulerAngles == new Vector3(0,0,270f)))&&
			    Input.GetKeyDown(KeyCode.Mouse0) && 
			    !((checkShipCollision(new int[] {initialPos,initialPos + 5}) && Ghosts.transform.eulerAngles == Vector3.zero) ||
			    (checkShipCollision(new int[] {initialPos, initialPos -1})&& Ghosts.transform.eulerAngles == new Vector3(0,0,270f))) ){
					GameObject ship =  Instantiate(prefabShip2,new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, -1), Ghosts.transform.rotation) as GameObject;
					if(ship.transform.eulerAngles == new Vector3(0,0,270f)){
						ship.GetComponent<ShipBehavior>().points = new int[] {initialPos, initialPos -1};
						dbMng.ships[3] = initialPos;	
						dbMng.ships[4] = initialPos - 1;
					}
					else{
						ship.GetComponent<ShipBehavior>().points = new int[] {initialPos,initialPos + 5};
						dbMng.ships[3] = initialPos;	
						dbMng.ships[4] = initialPos + 5;
					}
				}
					DropPhase=2;
					Destroy(Ghosts);
					Ghosts = null;
				}
				else if(hit.collider == null || int.Parse( hit.collider.name) > 15){
					if(Input.GetKeyDown(KeyCode.Mouse0)){
						Warnings.type = "outside";
					}
				}
			}
		

		bool checkShipCollision(int[] coordinates){
		ShipBehavior[] sb = FindObjectsOfType<ShipBehavior> ();
		foreach(int i in coordinates){
			foreach(ShipBehavior s in sb )
				foreach(int point in s.points){
				if(point == i) return true;
				}
			}
		return false;
		}

		bool checkShoot(int coordinate) {
			ShipBehavior[] sb = FindObjectsOfType<ShipBehavior> ();
				foreach (ShipBehavior s in sb) {
					foreach(int point in s.points){
					if(point == coordinate) return true;
				}
			}
			return false;
		}

	}

