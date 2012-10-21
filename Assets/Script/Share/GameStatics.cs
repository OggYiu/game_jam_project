using UnityEngine;
using System.Collections;

public class GameStatics // : MonoBehaviour
{
	public static int human_spawned = 0;
	public static int monster_spawned = 0;
	public static int human_died = 0;
	public static int monster_died = 0;
	public static int game_turn = 0;
	public static int oldest_human = 0;
	public static int oldest_monster = 0;
	

//	static private GameStatics s_instance = null;
//	static public GameStatics GetInstance() {
//		if ( s_instance == null ) {
//			s_instance = (GameStatics)FindObjectOfType(typeof(GameStatics));
//        	
//			if ( s_instance == null ) {
//				Debug.LogError( "<GameStatics::GetInstance>: GameStatics not found" );
//			}
//		}
//		return s_instance;
//	}
}

