using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour {
	public int SCREEN_WIDTH = 480;
	public int SCREEN_HEIGHT = 320;
	public int TILE_WIDTH = 32;
	public int TILE_HEIGHT = 32;
	public int MAP_WIDTH = 1280;
	public int MAP_HEIGHT = 960;
	public int MAP_TILE_ROW_COUNT = 6;
	public int MAP_TILE_COLUMN_COUNT = 6;
	public int TERRAIN_Z_ORDER = 2;
	public int BUILDING_Z_ORDER = 0;
	public int OBJECT_Z_ORDER = -2;
	public int MESH_MAP_Z_ORDER = -3;
	public float HUMAN_VELOCITY = 100.0f;
	public float MONSTER_VELOCITY = 100.0f;
	
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

