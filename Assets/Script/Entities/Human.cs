using UnityEngine;
using System.Collections;

public class Human : GameActor
{
	protected override void _Initer (Hashtable args)
	{
		base._Initer (args);
		type_ = ActorType.human;
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
		if ( false ) {
			processed = true;
		}
		
		// see if he see a monster 
		if ( !processed && NavigationMap.GetInstance().GetNearestMonster ( this, out row, out column ) ) {
			int cur_map_x = (int)this.map_pos.x;
			int cur_map_y = (int)this.map_pos.y;
			int diff_map_x = Mathf.Abs ( cur_map_x - row );
			int diff_map_y = Mathf.Abs ( cur_map_y - column );
			int new_map_x = cur_map_x;
			int new_map_y = cur_map_y;
			int distance = diff_map_x + diff_map_y;
			if ( distance <= GameSettings.GetInstance().HUMAN_AVOID_DISTANCE ) {
				// get away for that area!
				if ( diff_map_x != 0 || diff_map_y != 0 ) {
					if ( diff_map_x > diff_map_y ) {
						if ( row < cur_map_x ) 
							++new_map_x;
						else
							--new_map_x;
					} else {
						if ( column < cur_map_y )
							++new_map_y;
						else
							--new_map_y;
					}
				}
				this.pos = new Vector3 (new_map_x * GameSettings.GetInstance().TILE_SIZE,
										new_map_y * GameSettings.GetInstance().TILE_SIZE,
										0.0f );
				processed = true;
			}
		}
		
		// check if he see food
		if ( !processed && NavigationMap.GetInstance().GetNearestFood ( this, out row, out column ) ) {
			int cur_map_x = (int)this.map_pos.x;
			int cur_map_y = (int)this.map_pos.y;
			int diff_map_x = Mathf.Abs ( cur_map_x - row );
			int diff_map_y = Mathf.Abs ( cur_map_y - column );
			int new_map_x = cur_map_x;
			int new_map_y = cur_map_y;
			if ( diff_map_x != 0 || diff_map_y != 0 ) {
				if ( diff_map_x > diff_map_y ) {
					if ( row > cur_map_x ) 
						++new_map_x;
					else
						--new_map_x;
				} else {
					if ( column > cur_map_y )
						++new_map_y;
					else
						--new_map_y;
				}
			}
			this.pos = new Vector3 (new_map_x * GameSettings.GetInstance().TILE_SIZE,
									new_map_y * GameSettings.GetInstance().TILE_SIZE,
									0.0f );
			processed = true;
		}
	}
}

