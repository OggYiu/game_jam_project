using UnityEngine;
using System.Collections;

public class Tile_Map_UI : MonoBehaviour {
	
	public GUIText text_era;
	public GUIText text_human_born;
	public GUIText text_human_kill;
	public GUIText text_monster_born;
	public GUIText text_monster_kill;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		text_era.text = GameStatics.game_turn.ToString();
		text_human_born.text = GameStatics.human_spawned.ToString();
		text_human_kill.text = GameStatics.human_died.ToString();
		text_monster_born.text = GameStatics.monster_spawned.ToString();
		text_monster_kill.text = GameStatics.monster_died.ToString();
	}
}
