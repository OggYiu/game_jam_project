using UnityEngine;
using System.Collections;

public enum NodeType {
	normal,
	food,
	blocked,
}

public class NavigationMap : Entity
{
	NodeType [,] collision_map_;
	ActorType [,] actor_map_;
	
	Scene_Game scene_game_ = null;
	
	protected override void _Initer ( Hashtable args ) {
		base._Initer ( args );
		collision_map_ = new NodeType[GameSettings.GetInstance().MAP_TILE_ROW_COUNT, GameSettings.GetInstance().MAP_TILE_ROW_COUNT];
		actor_map_ = new ActorType[GameSettings.GetInstance().MAP_TILE_ROW_COUNT, GameSettings.GetInstance().MAP_TILE_ROW_COUNT];
		
		for ( int i = 0; i < GameSettings.GetInstance().MAP_TILE_ROW_COUNT; ++i ) {
			for ( int j = 0; j < GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT; ++j ) {
				collision_map_[i,j] = NodeType.normal;
			}
		}
		
		// testing
		collision_map_[2, 3] = NodeType.food;
			
		for ( int i = 0; i < GameSettings.GetInstance().MAP_TILE_ROW_COUNT; ++i ) {
			for ( int j = 0; j < GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT; ++j ) {
				actor_map_[i,j] = ActorType.none;
			}
		}
	}
	
	public void RegisterActor ( GameActor actor ) {
		int row = (int)actor.map_pos.x;
		int column = (int)actor.map_pos.y;
		
		if ( ( row * GameSettings.GetInstance().MAP_TILE_ROW_COUNT + column ) >= ( GameSettings.GetInstance().MAP_TILE_ROW_COUNT * GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT )  ) {
			Debug.LogError ( "<NavigationMap::GetNodeType>, invalid index, row : " + row + ", column: " + column );
			return ;
		}
		
		actor_map_[row, column] = actor.Type();
	}
	
	public void UnRegisterActor ( GameActor actor ) {
		int row = (int)actor.map_pos.x;
		int column = (int)actor.map_pos.y;
		
		if ( ( row * GameSettings.GetInstance().MAP_TILE_ROW_COUNT + column ) >= ( GameSettings.GetInstance().MAP_TILE_ROW_COUNT * GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT )  ) {
			Debug.LogError ( "<NavigationMap::GetNodeType>, invalid index, row : " + row + ", column: " + column );
			return ;
		}
		
		actor_map_[row, column] = ActorType.none;
	}
	
	protected override void _Resolver ( Hashtable args)
	{
		base._Resolver (args);
	
		if (args.Contains("Scene_Game")) {
			scene_game_ =(Scene_Game)args["Scene_Game"];
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
	
	public bool GetNearestMonster ( GameActor actor, out int row, out int column ) {
		for ( int i = 0; i < GameSettings.GetInstance().MAP_TILE_ROW_COUNT; ++i ) {
			for ( int j = 0; j < GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT; ++j ) {
				Debug.Log ( "actor_map_ i: " + i + ", j: " + j + ": " + actor_map_[i,j] );
			}
		}
		int closest_distance = int.MaxValue;
		int temp_distance = int.MaxValue;
		row = -1;
		column = -1;
		bool monster_found = false;
		for ( int i = 0; i < GameSettings.GetInstance().MAP_TILE_ROW_COUNT; ++i ) {
			for ( int j = 0; j < GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT; ++j ) {
				if ( actor_map_[i,j] == ActorType.monster ) {
					temp_distance = ( i * GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT + i );
					if ( closest_distance > temp_distance ) {
						monster_found = true;
						row = i;
						column = j;
					}
				}
			}
		}
		return monster_found;
	}
	
	public bool GetNearestFood ( GameActor actor, out int row, out int column ) {
		row = -1;
		column = -1;
		
		if ( actor.Type() == ActorType.human ) {
			return GetClosestNodeType ( actor, NodeType.food, out row, out column );
		} else {
			int temp_distance = int.MaxValue;
			int closest_distance = int.MaxValue;
			bool food_found = false;
			for ( int i = 0; i < GameSettings.GetInstance().MAP_TILE_ROW_COUNT; ++i ) {
				for ( int j = 0; j < GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT; ++j ) {
					if ( actor_map_[i,j] == ActorType.human ) {
						temp_distance = ( i * GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT + i );
						if ( closest_distance > temp_distance ) {
							food_found = true;
							row = i;
							column = j;
						}
					}
				}
			}
			
			if ( food_found ) {
				return food_found;
			}
		}
		
		return false;
	}
	
	public bool GetClosestNodeType ( GameActor actor, NodeType type, out int row, out int column ) {
		row = -1;
		column = -1;
		
		Vector3 actor_pos = actor.pos;
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
		
		return !(row < 0 || column < 0 );
	}
	
	public bool CanWalkTo ( GameActor actor, int row, int column ) {
		return true;
	}
	
	public bool CanSeeInDiagonal ( GameActor actor, int row, int column ) {
		Vector3 actor_map_pos = actor.map_pos;
		int actor_column = (int)( actor_map_pos.x );
		int actor_row = (int)( actor_map_pos.y );
		return ( Mathf.Abs ( actor_column - column ) == Mathf.Abs ( actor_row - row ) );
	}
	
	public bool CanSeeInStraight ( GameActor actor, int row, int column ) {
		Vector3 actor_map_pos = actor.map_pos;
		int actor_column = (int)( actor_map_pos.x );
		int actor_row = (int)( actor_map_pos.y );
		return row == actor_row || column == actor_column;
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

