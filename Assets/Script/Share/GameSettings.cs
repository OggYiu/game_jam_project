using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour {
	public int SCREEN_WIDTH = 480;
	public int SCREEN_HEIGHT = 320;
	public int TILE_SIZE = 64;
	public int MAP_WIDTH = 1280;
	public int MAP_HEIGHT = 960;
	public int MAP_TILE_ROW_COUNT = 6;
	public int MAP_TILE_COLUMN_COUNT = 6;
	public int HUMAN_ACTION_POINT = 1;
	public int MONSTER_ACTION_POINT = 2;
	
	static private GameSettings s_instance = null;
	static public GameSettings GetInstance() {
		if ( s_instance == null ) {
			s_instance = (GameSettings)FindObjectOfType(typeof(GameSettings));
        	
			if ( s_instance == null ) {
				Debug.LogError( "<GameSettings::GetInstance>: GameSettings not found" );
			}
		}
		return s_instance;
	}
}

