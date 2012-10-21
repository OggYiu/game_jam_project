using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile_Map {
	int tile_height;
	int tile_width;
	
	int tile_col;
	int tile_row;
	
	public void Init () {
		tile_width = tile_height = GameSettings.GetInstance().TILE_SIZE;
		tile_col = GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT;
		tile_row = GameSettings.GetInstance().MAP_TILE_ROW_COUNT;
		
		NodeType[,] map = new NodeType[tile_col,tile_row];
		for (int i = 0; i < tile_col; ++i)
		{
			for (int j = 0; j < tile_row; ++j)
			{
				map[i,j] = NodeType.blocked;
//				map[i,j] = NodeType.;
			}
		}
		
		double mountaion_percentage = GameDataShare.mountain_percentage;
		double sand_forest_percentage = GameDataShare.forest_sand_percentage;;
		
		int no_of_mountain = Mathf.RoundToInt((float)(tile_col * tile_row * mountaion_percentage));
		
		for (int i = 0; i < no_of_mountain; ++i)
		{
			int col = Random.Range(0, tile_col);
			int row = Random.Range(0, tile_row);
			
			if (map[col,row] != NodeType.mountain)
			{
				map[col,row] = NodeType.mountain;
			}
			else
			{
				i--;
			}
		}
		
		for (int i = 0; i < tile_col; ++i)
		{
			for (int j = 0; j < tile_row; ++j)
			{
				if(map[i,j] == NodeType.mountain)
				{
					int col = i == 0 ? 0 : i - 1;
					int row = j == 0 ? 0 : j - 1;
					
					if (map[col,row] != NodeType.mountain)
					{
						map[col,row] = NodeType.grass;
					}
					
					col = i == 0 ? 0 : i - 1;
					row = j;
					
					if (map[col,row] != NodeType.mountain)
					{
						map[col,row] = NodeType.grass;
					}
					
					col = i;
					row = j == 0 ? 0 : j - 1;
					
					if (map[col,row] != NodeType.mountain)
					{
						map[col,row] = NodeType.grass;
					}
					
					col = i >= tile_col - 1 ? tile_col - 1 : i + 1;
					row = j >= tile_row - 1 ? tile_row - 1 : j + 1;
					
					if (map[col,row] != NodeType.mountain)
					{
						map[col,row] = NodeType.grass;
					}
					
					col = i >= tile_col - 1 ? tile_col - 1 : i + 1;
					row = j;
					
					if (map[col,row] != NodeType.mountain)
					{
						map[col,row] = NodeType.grass;
					}
					
					col = i;
					row = j >= tile_row - 1 ? tile_row - 1 : j + 1;
					
					if (map[col,row] != NodeType.mountain)
					{
						map[col,row] = NodeType.grass;
					}
					
					col = i >= tile_col - 1 ? tile_col - 1 : i + 1;
					row = j == 0 ? 0 : j - 1;
					
					if (map[col,row] != NodeType.mountain)
					{
						map[col,row] = NodeType.grass;
					}
					
					col = i == 0 ? 0 : i - 1;
					row = j >= tile_row - 1 ? tile_row - 1 : j + 1;
					
					if (map[col,row] != NodeType.mountain)
					{
						map[col,row] = NodeType.grass;
					}
				}
			}
		}
		
		for (int i = 0; i < tile_col; ++i)
		{
			for (int j = 0; j < tile_row; ++j)
			{
				if (map[i,j] == NodeType.grass)
				{
					int rand = Random.Range(0, 100);
					if (rand <= sand_forest_percentage * 100)
					{
						int rand2 = Random.Range(0, 100);
						if (rand2 < 50)
						{
							map[i, j] = NodeType.food;
						}
						else
						{
							map[i, j] = NodeType.normal;
						}
					}
				}
				
				GenerateBaseTile(i, j, map[i,j]);
			}
		}
	}
	
	private void GenerateBaseTile(int col, int row, NodeType nt)
	{
		string texture_path;
		
		if (nt == NodeType.blocked)
		{
			texture_path = "Texture/water";
		}
		else if (nt == NodeType.food)
		{
			texture_path = "Texture/tree_a";
		}
		else if (nt == NodeType.grass)
		{
			texture_path = "Texture/grass";
		}
		else if (nt == NodeType.normal)
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
		basetile.transform.parent = SceneManager.GetInstance().entities_parent.transform;
		basetile.transform.localPosition = new Vector3(col * tile_height, row * tile_width, 2);
		basetile.transform.localScale = new Vector3 ( 64, 1, 64 );
		
		NavigationMap.GetInstance().SetCollisionMapType(row, col, nt);
	}
}
