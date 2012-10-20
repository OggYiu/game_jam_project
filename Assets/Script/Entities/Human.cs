using UnityEngine;
using System.Collections;

public class Human : GameActor
{
	protected override void _Initer (Hashtable args)
	{
		base._Initer (args);
		type_ = ActorType.human;
		action_point_gainer_ = GameSettings.GetInstance().HUMAN_ACTION_POINT;
		health_ = GameSettings.GetInstance().HUMAN_HEALTH;
		damage_ = GameSettings.GetInstance().HUMAN_DAMAGE;
	}
	
	protected override void _Resolver (Hashtable args)
	{
		base._Resolver (args);
	}
	
	protected override void _Thinker ()
	{
		base._Thinker ();
		
		int row = 0;
		int column = 0;
		
		bool processed = false;
		// see if you can move
//		if ( false ) {
//			processed = true;
//		}
		
		// see if he see a monster 
		if ( !processed && NavigationMap.GetInstance().GetNearestMonster ( this, out row, out column ) ) {
			int cur_map_x = (int)this.map_pos.x;
			int cur_map_y = (int)this.map_pos.y;
			int diff_map_x = Mathf.Abs ( cur_map_x - row );
			int diff_map_y = Mathf.Abs ( cur_map_y - column );
			int distance = diff_map_x + diff_map_y;
			if ( distance <= GameSettings.GetInstance().HUMAN_AVOID_DISTANCE ) {
				int new_map_x = 0;
				int new_map_y = 0;
				if ( NavigationMap.GetInstance().MoveAway ( this, row, column, out new_map_x, out new_map_y ) ) {
					NavigationMap.GetInstance().UnRegisterActor ( this );
					// get away for that area!
					this.pos = new Vector3 (new_map_x * GameSettings.GetInstance().TILE_SIZE,
											new_map_y * GameSettings.GetInstance().TILE_SIZE,
											0.0f );
					NavigationMap.GetInstance().RegisterActor ( this );
					processed = true;
				}
			}
		}
		
		// check if he see food
		if ( !processed && NavigationMap.GetInstance().GetNearestFood ( this, out row, out column ) ) {
			int cur_map_x = (int)this.map_pos.x;
			int cur_map_y = (int)this.map_pos.y;
			int new_map_x = cur_map_x;
			int new_map_y = cur_map_y;
			
			if ( NavigationMap.GetInstance().MoveForward ( this, row, column, out new_map_x, out new_map_y ) ) {
				NavigationMap.GetInstance().UnRegisterActor ( this );
				this.pos = new Vector3 (new_map_x * GameSettings.GetInstance().TILE_SIZE,
										new_map_y * GameSettings.GetInstance().TILE_SIZE,
										0.0f );
				NavigationMap.GetInstance().RegisterActor ( this );
			}
			
			processed = true;
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

