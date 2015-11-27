using UnityEngine;
using System.Collections;

public class SetMap : MonoBehaviour 
{

	int rows = 5;
	int columns = 5;
	GameObject Tile;
	GameObject Ship3;
	GameObject Ship2;
	GameObject GhostShip;
	GameObject Ghosts;
	int rotation;
	
	float[] gridX;
	float[] gridY;
	int name;
	bool Ghost = false;
	
	public static bool Loaded;
	
	private int DropPhase = 0;
	Vector2 mouse;
	RaycastHit2D hit;

	void SetTiles()
	{
		gridX = new float[rows];
		gridY = new float[columns];
		for (int i = 0; i < rows; i++)
		{
			for(int j = 0;j < columns;j++)
			{
				gridX[j] = -1.5f * j;
				gridY[i] = 1.5f * i;
				name++;
				GameObject Tiles = Instantiate(Tile,new Vector3(0 - gridX[j], 0 - gridY[i], 0), transform.rotation) as GameObject;
				Tiles.name = "" + name;
				if(name == 25)
				{
					Loaded = true;
				}
			}
		}
	}

	void Start () 
	{
		Tile = Resources.Load("Prefabs/Tile") as GameObject;
		Ship3 = Resources.Load("Prefabs/Ship") as GameObject;
		Ship2 = Resources.Load("Prefabs/Ship2") as GameObject;
		GhostShip = Resources.Load("Prefabs/GhostShip") as GameObject;
		SetTiles();
	}
	
	void Update () 
	{
		mouse = Camera.main.ScreenToWorldPoint( Input.mousePosition );
		hit = Physics2D.Raycast( mouse, Vector2.zero );
		if(DropPhase<2)
		{	
			//Ghost
			if(!Ghost)
			{
				Ghosts = Instantiate(GhostShip,new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, -1), transform.rotation) as GameObject;
				Ghost = true;
			}
			else if(Ghost)
			{
				Ghosts.transform.position = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, -1);
				if(Input.GetKeyDown ("r"))
				{
					if(rotation == 0)
					{
						rotation = 270;
					}
					else if(rotation == 270)
					{
						rotation = 0;
					}
					Ghosts.transform.eulerAngles = new Vector3(0,0,rotation);
				}
			}
			//Drop
			if(DropPhase == 0)
			{
				if (hit.collider != null && hit.collider.name != "Ship" && int.Parse(hit.collider.name) <= 15 && Input.GetKeyDown(KeyCode.Mouse0))
				{
					Instantiate(Ship3,new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, -1), Ghosts.transform.rotation);
					DropPhase=1;
				}
				else if(hit.collider == null || int.Parse(hit.collider.name) > 15)
				{
					if(Input.GetKeyDown(KeyCode.Mouse0))
					{
						Warnings.type = "outside";
					}
				}
			}
			else if(DropPhase == 1)
			{
				if (hit.collider != null && hit.collider.name != "Ship" && int.Parse(hit.collider.name) <= 15 && Input.GetKeyDown(KeyCode.Mouse0))
				{
					Instantiate(Ship2,new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, -1), Ghosts.transform.rotation);
					DropPhase=2;
					Destroy(Ghosts);
				}
				else if(hit.collider == null || int.Parse(hit.collider.name) > 15)
				{
					if(Input.GetKeyDown(KeyCode.Mouse0))
					{
						Warnings.type = "outside";
					}
				}
			}
		}
	}
}
