using UnityEngine;
using System.Collections;

public class Tile_Map : MonoBehaviour {

	[SerializeField] int tile_height;
	[SerializeField] int tile_width;
	
	[SerializeField] int tile_col;
	[SerializeField] int tile_row;
	
	// Use this for initialization
	void Start () {
		
		for (int i = 0; i < tile_col; ++i) 
		{
			for (int j = 0; j < tile_row; ++j)
			{
				string texture_path;
				
				int result = Random.Range(0, 100);
				if (result <= 20)
				{
					texture_path = "Texture/water";
				}
				else if (result <= 40)
				{
					texture_path = "Texture/tree_a";
				}
				else if (result <= 60)
				{
					texture_path = "Texture/grass";
				}
				else if (result <= 80)
				{
					texture_path = "Texture/sand";
				}
				else
				{
					texture_path = "Texture/hills_a";
				}
				
				GameObject prefab = Resources.Load("Prefabs/BaseTile", typeof(GameObject)) as GameObject;
				Base_Tile basetile = ((GameObject)GameObject.Instantiate(prefab)).gameObject.GetComponent<Base_Tile>();
				basetile.init(texture_path);
				basetile.transform.position = new Vector3(j * tile_height, i * tile_width, 0);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
