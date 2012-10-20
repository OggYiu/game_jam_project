using UnityEngine;
using System.Collections;

public class Monster : GameActor
{
	protected override void _Initer (Hashtable args)
	{
		base._Initer (args);
		type_ = ActorType.monster;
		action_point_gainer_ = GameSettings.GetInstance().MONSTER_ACTION_POINT;
		health_ = GameSettings.GetInstance().MONSTER_HEALTH;
		damage_ = GameSettings.GetInstance().MONSTER_DAMAGE;
	}
	
	protected override void _Resolver (Hashtable args)
	{
		base._Resolver (args);
	}
	
	protected override void _Thinker ()
	{
		base._Thinker ();
		
		bool processed = false;
		// check if he see
//		if ( false ) {
//			processed = true;
//		}
		
		int row = -1;
		int column = -1;
		if ( !processed && NavigationMap.GetInstance().GetNearestHumanForMonster( this, out row, out column ) ) {
			int new_map_x = 0;
			int new_map_y = 0;
			
			if ( NavigationMap.GetInstance().MoveForward ( this, row, column, out new_map_x, out new_map_y ) ) {
				NavigationMap.GetInstance().UnRegisterActor ( this );
				this.pos = new Vector3 (new_map_x * GameSettings.GetInstance().TILE_SIZE,
										new_map_y * GameSettings.GetInstance().TILE_SIZE,
										0.0f );
				NavigationMap.GetInstance().RegisterActor ( this );
				NavigationMap.GetInstance().EatHuman ( this, new_map_x, new_map_y );
				processed = true;
			}
		}
		
		if ( moving_to_target_ ) {
			moving_to_target_ = !processed;
		}
		
		if ( !processed && !moving_to_target_ ) {
			NavigationMap.GetInstance().GetRandomPos ( out target_row_, out target_column_ );
			moving_to_target_ = true;
			processed = true;
		}
		
		if ( moving_to_target_ ) {
			int cur_map_x = (int)this.map_pos.x;
			int cur_map_y = (int)this.map_pos.y;
			
			if ( cur_map_x == target_row_ && cur_map_x == target_row_ ) {
				moving_to_target_ = false;
			} else {
				int new_map_x = 0;
				int new_map_y = 0;
				if ( NavigationMap.GetInstance().MoveForward ( this, target_row_, target_column_, out new_map_x, out new_map_y ) ) {
					NavigationMap.GetInstance().UnRegisterActor ( this );
					this.pos = new Vector3 (new_map_x * GameSettings.GetInstance().TILE_SIZE,
											new_map_y * GameSettings.GetInstance().TILE_SIZE,
											0.0f );
					NavigationMap.GetInstance().RegisterActor ( this );
				}
				
				processed = true;
			}
		}
	}
}