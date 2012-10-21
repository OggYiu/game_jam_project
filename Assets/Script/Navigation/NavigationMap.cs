using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum NodeType {
	normal,
	food,
	blocked,
	mountain,
	grass,
}

public class NavigationMap : Entity
{
	NodeType [,] collision_map_;
	List<GameActor> [,] actor_map_;
	
	Scene_Game scene_game_ = null;
	
	protected override void _Initer ( Hashtable args ) {
		base._Initer ( args );
		collision_map_ = new NodeType[GameSettings.GetInstance().MAP_TILE_ROW_COUNT, GameSettings.GetInstance().MAP_TILE_ROW_COUNT];
		
		
		for ( int i = 0; i < GameSettings.GetInstance().MAP_TILE_ROW_COUNT; ++i ) {
			for ( int j = 0; j < GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT; ++j ) {
				collision_map_[i,j] = NodeType.normal;
			}
		}
		
		// testing
		collision_map_[2, 2] = NodeType.food;
		
		actor_map_ = new List<GameActor> [GameSettings.GetInstance().MAP_TILE_ROW_COUNT,GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT];
		for ( int i = 0; i < GameSettings.GetInstance().MAP_TILE_ROW_COUNT; ++i ) {
			for ( int j = 0; j < GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT; ++j ) {
				actor_map_[i,j] = new List<GameActor>();
			}
		}
	}
	
	public bool IsExistedIn ( GameActor actor, int row, int column ) {
		for ( int i = 0; i < GameSettings.GetInstance().MAP_TILE_ROW_COUNT; ++i ) {
			for ( int j = 0; j < GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT; ++j ) {
				for ( int k = 0; k < actor_map_[i, j].Count; ++k ) {
					if ( actor_map_[i, j][k] == actor ) {
						return true;
					}
				}
			}
		}
		
		return false;
	}
	
	public void RegisterActor ( GameActor actor ) {
		int row = (int)actor.map_pos.y;
		int column = (int)actor.map_pos.x;
		
		if ( !IsPosValid ( row, column ) ) {
			Debug.LogError ( "<NavigationMap::RegisterActor>, invalid index, row : " + row + ", column: " + column );
			return ;
		}
		
		// check if it is already existed
		if ( !IsExistedIn ( actor, row, column ) ) {
			actor_map_[row, column].Add ( actor );
		}
	}
	
	public void UnRegisterActor ( GameActor actor ) {
		int row = (int)actor.map_pos.y;
		int column = (int)actor.map_pos.x;
		
		if ( !IsPosValid ( row, column ) ) {
			Debug.LogError ( "<NavigationMap::UnRegisterActor>, invalid index, row : " + row + ", column: " + column );
			return ;
		}
		
//		actor_map_[row, column] = ActorType.none;
		
		// check if it is already existed
		if ( IsExistedIn ( actor, row, column ) ) {
			actor_map_[row, column].Remove ( actor );
		}
	}
	
	protected override void _Resolver ( Hashtable args)
	{
		base._Resolver (args);
	
		if (args.Contains("Scene_Game")) {
			scene_game_ =(Scene_Game)args["Scene_Game"];
		}
	}
	
	public NodeType GetNodeType ( int row, int column) {
		if ( !IsPosValid ( row, column ) ) {
			Debug.LogError ( "<NavigationMap::GetNodeType>, invalid index, row : " + row + ", column: " + column );
			return NodeType.blocked;
		}
		
		return collision_map_[row,column];
	}
	
	public void SetCollisionMapType ( int row, int column, NodeType type ) {
		if ( !IsPosValid ( row, column ) ) {
			Debug.LogError ( "<NavigationMap::GetNodeType>, invalid index, row : " + row + ", column: " + column );
			return ;
		}
		
		collision_map_[row,column] = type;
	}
	
	public void GetRandomPos ( out int row, out int column ) {
		while ( true ) {
			row = Random.Range ( 0, GameSettings.GetInstance().MAP_TILE_ROW_COUNT - 1);
			column = Random.Range ( 0, GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT- 1);
			if ( collision_map_ [row, column] != NodeType.blocked ) {
				break;
			}
		}
	}
	
	public bool GetNearestMonster ( GameActor actor, out int row, out int column ) {
//		for ( int i = 0; i < GameSettings.GetInstance().MAP_TILE_ROW_COUNT; ++i ) {
//			for ( int j = 0; j < GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT; ++j ) {
//				Debug.Log ( "actor_map_ i: " + i + ", j: " + j + ": " + actor_map_[i,j] );
//			}
//		}
		int closest_distance = int.MaxValue;
		int temp_distance = int.MaxValue;
		row = -1;
		column = -1;
		bool monster_found = false;
		for ( int i = 0; i < GameSettings.GetInstance().MAP_TILE_ROW_COUNT; ++i ) {
			for ( int j = 0; j < GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT; ++j ) {
				for ( int k = 0; k < actor_map_[i,j].Count; ++k ) {
					if ( actor_map_[i,j][k].Type() == ActorType.monster ) {
//						temp_distance = ( i * GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT + i );
						temp_distance = CalculateDistance ( actor, i, j );
						if ( closest_distance > temp_distance ) {
							monster_found = true;
							row = i;
							column = j;
						}
					}
				}
			}
		}
		return monster_found;
	}
	
	public bool GetNearestHumanForMonster ( GameActor actor, out int row, out int column ) {
		row = -1;
		column = -1;
		
		int closest_distance = int.MaxValue;
		int temp_distance = 0;
		bool human_found = false;
		for ( int i = 0; i < GameSettings.GetInstance().MAP_TILE_ROW_COUNT; ++i ) {
			for ( int j = 0; j < GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT; ++j ) {
				for ( int k = 0; k < actor_map_[i,j].Count; ++k ) {
					if ( actor_map_[i,j][k].Type() == ActorType.human ) {
						temp_distance = CalculateDistance ( actor, i, j );
						if ( temp_distance < closest_distance ) {
							closest_distance = temp_distance;
							row = i;
							column = j;
							human_found = true;
						}
					}
				}
			}
		}
		
		return human_found;
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
					for ( int k = 0; k < actor_map_[i,j].Count; ++k ) {
						if ( actor_map_[i,j][k].Type() == ActorType.human ) {
							temp_distance = ( i * GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT + i );
							if ( closest_distance > temp_distance ) {
								food_found = true;
								row = i;
								column = j;
							}
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
		
//		Vector3 actor_pos = actor.pos;
//		Vector3 target_map_pos = Vector3.zero;
		int closest_distance = int.MaxValue;
		
		for ( int i = 0; i < GameSettings.GetInstance().MAP_TILE_ROW_COUNT; ++i ) {
			for ( int j = 0; j < GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT; ++j ) {
				if ( collision_map_[i,j] == type ) {
//					target_map_pos = new Vector3 (	j * GameSettings.GetInstance().TILE_SIZE,
//													i * GameSettings.GetInstance().TILE_SIZE, 
//													0 );
					int distance = CalculateDistance ( actor, i, j );
					if ( distance <= closest_distance ) {
						closest_distance = distance;
						row = i;
						column = j;
					}
				}
			}
		}
		
		return IsPosValid ( row, column );
	}
	
	public bool CanWalkTo ( GameActor actor, int row, int column ) {
		return IsPosValid ( row, column );
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
	
	public int CalculateDistance ( GameActor actor, int row, int column ) {
		int cur_map_x = (int)actor.map_pos.x;
		int cur_map_y = (int)actor.map_pos.y;
		int diff_map_x = Mathf.Abs ( cur_map_x - column );
		int diff_map_y = Mathf.Abs ( cur_map_y - row );
		return diff_map_x + diff_map_y;
	}
	
	public bool MoveForward ( GameActor actor, int row, int column, out int new_map_x, out int new_map_y ) {
		int cur_map_x = (int)actor.map_pos.x;
		int cur_map_y = (int)actor.map_pos.y;
		int diff_map_x = Mathf.Abs ( cur_map_x - column );
		int diff_map_y = Mathf.Abs ( cur_map_y - row );
		new_map_x = cur_map_x;
		new_map_y = cur_map_y;
		
		if ( diff_map_x != 0 || diff_map_y != 0 ) {
			if ( diff_map_x > diff_map_y ) {
				if ( column > cur_map_x ) 
					++new_map_x;
				else
					--new_map_x;
			} else {
				if ( row > cur_map_y )
					++new_map_y;
				else
					--new_map_y;
			}
			
			return IsPosValid ( new_map_y, new_map_x );
		}
		
		return false;
	}
	
	public bool MoveAway ( GameActor actor, int row, int column, out int new_map_x, out int new_map_y ) {
		int cur_map_x = (int)actor.map_pos.x;
		int cur_map_y = (int)actor.map_pos.y;
		int diff_map_x = Mathf.Abs ( cur_map_x - column );
		int diff_map_y = Mathf.Abs ( cur_map_y - row );
		new_map_x = cur_map_x;
		new_map_y = cur_map_y;
		
		// get away for that area!
		if ( diff_map_x != 0 || diff_map_y != 0 ) {
			if ( diff_map_x > diff_map_y ) {
				if ( column < cur_map_x ) 
					++new_map_x;
				else
					--new_map_x;
			} else {
				if ( row < cur_map_y )
					++new_map_y;
				else
					--new_map_y;
			}
			
			return IsPosValid ( new_map_y, new_map_x );
		}
		return false;
	}
	
	public void EatFood ( GameActor actor, int row, int column ) {
		if ( !IsPosValid ( row, column ) ) {
			Debug.LogError ( "<NavigationMap::EatFood>, invalid index, row : " + row + ", column: " + column );
		}
		
		if ( collision_map_[row, column] != NodeType.food ) {
			Debug.LogError ( "<NavigationMap::EatFood>, man, it's not food" );
		}
		
		if ( GameSettings.GetInstance().FOOD_DISAPEAR_AFTER_EATING ) {
			collision_map_[row, column] = NodeType.normal;
		}
	}
	
	public int EatHuman ( GameActor actor, int row, int column ) {
		if ( !IsPosValid ( row, column ) ) {
			Debug.LogError ( "<NavigationMap::EatHuman>, invalid index, row : " + row + ", column: " + column );
		}
		
		int kill_counter = 0;
		// just eat the first human
		GameActor target_actor = null;
		for ( int i = 0; i < actor_map_[row,column].Count; ++i ) {
			target_actor = actor_map_[row,column][i];
			if ( target_actor.Type() == ActorType.human ) {
				if ( actor.isAlive() && target_actor.isAlive() ) {
					actor.Combat ( target_actor );
					if ( !target_actor.isAlive() ) {
						++kill_counter;
					}
				}
			}
		}
		return kill_counter;
	}
	
	public bool IsPosValid ( int row, int column ) {
		return 	( row >= 0 && row <= GameSettings.GetInstance().MAP_TILE_ROW_COUNT ) &&
				( column >= 0 && column <= GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT );
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

