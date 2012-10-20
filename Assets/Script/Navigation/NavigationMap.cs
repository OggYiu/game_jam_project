using UnityEngine;
using System.Collections;

public enum NodeType {
	normal,
	blocked,
}

public class NavigationMap : Entity
{
	NodeType [,] collision_map_;
	protected override void _Resolver (Hashtable args)
	{
		base._Resolver (args);
		
		// testing
		collision_map_ = new NodeType[GameSettings.GetInstance().MAP_TILE_ROW_COUNT, GameSettings.GetInstance().MAP_TILE_ROW_COUNT];
		for ( int i = 0; i < GameSettings.GetInstance().MAP_TILE_ROW_COUNT; ++i ) {
			for ( int j = 0; j < GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT; ++j ) {
				collision_map_[i,j] = NodeType.normal;
			}
		}
	}
	
	public NodeType GetNodeType ( int row, int column) {
		if ( ( row * GameSettings.GetInstance().MAP_TILE_ROW_COUNT + column ) >= ( GameSettings.GetInstance().MAP_TILE_ROW_COUNT * GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT )  ) {
			Debug.LogError ( "<NavigationMap::GetNodeType>, invalid index, row : " + row + ", column: " + column );
			return NodeType.normal;
		}
		
		return collision_map_[row,column];
	}
	
	public void SetCollisionMapType ( int row, int column, NodeType type ) {
		if ( ( row * GameSettings.GetInstance().MAP_TILE_ROW_COUNT + column ) >= ( GameSettings.GetInstance().MAP_TILE_ROW_COUNT * GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT )  ) {
			Debug.LogError ( "<NavigationMap::SetCollisionMapType>, invalid index, row : " + row + ", column: " + column );
			return ;
		}
		
		collision_map_[row,column] = type;
	}
	
	public Vector3 GetRandomPos () {
		int row = 0;
		int column = 0;
		while ( true ) {
			row = Random.Range ( 0, GameSettings.GetInstance().MAP_TILE_ROW_COUNT - 1);
			column = Random.Range ( 0, GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT- 1);
			if ( collision_map_ [row, column] != NodeType.blocked ) {
				break;
			}
		}
		return new Vector3 ( column * GameSettings.GetInstance().TILE_SIZE, row * GameSettings.GetInstance().TILE_SIZE, 0 );
	}
	
	public void GetClosestNodeType ( GameActor actor, NodeType type, out int row, out int column ) {
		row = -1;
		column = -1;
		
		Vector3 actor_pos = actor.transform.localPosition;
		Vector3 target_map_pos = Vector3.zero;
		int closest_distance = int.MaxValue;
		
		for ( int i = 0; i < GameSettings.GetInstance().MAP_TILE_ROW_COUNT; ++i ) {
			for ( int j = 0; j < GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT; ++j ) {
				if ( collision_map_[i,j] == type ) {
					target_map_pos = new Vector3 (	j * GameSettings.GetInstance().TILE_SIZE,
													i * GameSettings.GetInstance().TILE_SIZE, 
													0 );
					int distance = i * GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT + j;
					if ( distance <= closest_distance ) {
						closest_distance = distance;
						row = i;
						column = j;
					}
				}
			}
		}
	}
	
	public bool CanWalkTo ( GameActor actor, int row, int column ) {
		return true;
	}
	
	public bool CanSeeInDiagonal ( GameActor actor, int row, int column ) {
		return false;
	}
	
    private static NavigationMap s_instance;
    public static NavigationMap GetInstance() {
        if ( !s_instance ) {
            s_instance = (NavigationMap)GameObject.FindObjectOfType ( typeof ( NavigationMap ) );
            if ( !s_instance )
                Debug.LogError("There needs to be one active GameMap script on a GameObject in your scene.");
        }
        return s_instance;
    }
}

