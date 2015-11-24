using UnityEngine;
using System.Collections;

public class SetMap : MonoBehaviour {

	int rows = 5;
	int columns = 5;
	GameObject Tile;
	GameObject Ship;
	
	float[] gridX;
	float[] gridY;
	int name;
	
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
		Ship = Resources.Load("Prefabs/Ship") as GameObject;
		SetTiles();
	}
	
	void Update () 
	{
		mouse = Camera.main.ScreenToWorldPoint( Input.mousePosition );
		hit = Physics2D.Raycast( mouse, Vector2.zero );
		if(DropPhase==0)
		{
			if ( hit.collider != null && hit.collider.name != "Ship" && int.Parse(hit.collider.name) <= 15 && Input.GetKeyDown(KeyCode.Mouse0))
			{
				Instantiate(Ship,new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, -1), transform.rotation);
				DropPhase=1;
			}
			else if(hit.collider == null || int.Parse(hit.collider.name) > 15)
			{
				if(Input.GetKeyDown(KeyCode.Mouse0))
				{
					Warnings.type = "Outside";
				}
			}
		}
	}
}
