using UnityEngine;
using System.Collections;

public class Scene_Game : Scene {
	bool next_turn_ = false;
	bool next_action_ = false;
	bool turn_ended_ = false;
	GameActor human_ = null;
//	GameActor monster_ = null;
	int age_ = 0;
	
	protected override void _Resolver (Hashtable args)
	{
		base._Resolver (args);
		
//		Entity target_entity = null;
		
		human_ = GameActor.Create ( "BaseEntity" );
		AddEntity ( human_ );
		human_.transform.localPosition = NavigationMap.GetInstance().GetRandomPos ();
		
//		monster_ = GameActor.Create ( "BaseEntity" );
//		AddEntity ( monster_ );
//		monster_.transform.localPosition = new Vector3 ( GameSettings.GetInstance().TILE_SIZE, 0, 0 );
	}
	
	void OnBackBtnClicked ( GameObject obj ) {
		SceneManager.GetInstance().ChangeScene ( "Scene_Intro" );
	}
	
	protected override void _Updater (float deltaTime)
	{
		if ( next_turn_ ) {
			next_turn_ = false;
			++age_;
		
			for ( int i = 0; i < entities_.Count; ++i ) {
				((GameActor)entities_[i]).TurnBeginHandler ();
			}
			
			turn_ended_ = false;
		}
		
		if ( next_action_ && !turn_ended_ ) {
			// human first
			HumanDoAction ();
			MonsterDoAction ();
			
			turn_ended_ = true;
			for ( int i = 0; i < entities_.Count; ++i ) {
				if ( ((GameActor)entities_[i]).IsActionEnded () ) {
					turn_ended_ = false;
					break;
				}
			}
		}
		
		base._Updater (deltaTime);
		
		NavigationMap.GetInstance().OnUpdate();
	}
	
	override public void MouseButtonDownHandler ( int button_index ) {
		if ( button_index == 0 ) {
			next_action_ = true;
		} else {
			next_turn_ = true;
			next_action_ = true;
		}
	}
	
	void HumanDoAction () {
		GameActor target_game_actor = null;
		for ( int i = 0; i < entities_.Count; ++i ) {
			target_game_actor = ((GameActor)entities_[i]);
			if ( target_game_actor.GetType() != typeof(Human) ) {
				continue;
			}
		}
	}
	
	void MonsterDoAction () {
		GameActor target_game_actor = null;
		for ( int i = 0; i < entities_.Count; ++i ) {
			target_game_actor = ((GameActor)entities_[i]);
			if ( target_game_actor.GetType() != typeof(Monster) ) {
				continue;
			}
		}
	}
}