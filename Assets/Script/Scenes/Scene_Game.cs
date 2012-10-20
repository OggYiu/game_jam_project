using UnityEngine;
using System.Collections;

public class Scene_Game : Scene {
//	GameActor human_ = null;
//	GameActor human_ = null;
	float wait_time_ = 0;
//	GameActor monster_ = null;
	int age_ = 0;
	
	protected override void _Resolver (Hashtable args)
	{
		base._Resolver (args);
		
//		if (args.Contains("Scene_Game")) {
//			scene_game_ =(Scene_Game)args["Scene_Game"];
//		}
		
		NavigationMap.GetInstance().Init ( "Scene_Game", this );
		
//		Entity target_entity = null;
		
		GameActor human = GameActor.Create ( "Entity_Human" );
		AddEntity ( human );
		NavigationMap.GetInstance().RegisterActor ( human );
		
		GameActor monster = GameActor.Create ( "Entity_Monster" );
		AddEntity ( monster );
		monster.transform.localPosition = new Vector3 ( 64 * 3, 64 * 2 );
		NavigationMap.GetInstance().RegisterActor ( monster );
		
//		monster_ = GameActor.Create ( "BaseEntity" );
//		AddEntity ( monster_ );
//		monster_.transform.localPosition = new Vector3 ( GameSettings.GetInstance().TILE_SIZE, 0, 0 );
	}
	
	void OnBackBtnClicked ( GameObject obj ) {
		SceneManager.GetInstance().ChangeScene ( "Scene_Intro" );
	}
	
	protected override void _Updater (float deltaTime)
	{
		NavigationMap.GetInstance().OnUpdate();
		base._Updater (deltaTime);
		
		if ( IsAllActionDone() ) {
			wait_time_ = 1;
			NextTurn ();
		}
		
		if ( wait_time_ > 0 ) {
			wait_time_ -= Time.deltaTime;
		} else {
			// human first
			HumanDoAction ();
			MonsterDoAction ();
		}
	}
	
	override public void MouseButtonDownHandler ( int button_index ) {
//		if ( button_index == 0 ) {
//			next_action_ = true;
//		} else {
//			next_turn_ = true;
//			next_action_ = true;
//		}
//		turn_ended_ = false;
		
		NextTurn ();
	}
	
	bool IsAllActionDone () {
		for ( int i = 0; i < entities_.Count; ++i ) {
			if ( !((GameActor)entities_[i]).IsActionEnded () ) {
				return false;
			}
		}
		return true;
	}
	
	void NextTurn () {
		++age_;
		
		for ( int i = 0; i < entities_.Count; ++i ) {
			((GameActor)entities_[i]).TurnBeginHandler ();
		}
	}
	
	void HumanDoAction () {
		GameActor target_game_actor = null;
		for ( int i = 0; i < entities_.Count; ++i ) {
			target_game_actor = ((GameActor)entities_[i]);
			if ( target_game_actor.Type() != ActorType.human ) {
				continue;
			}
			target_game_actor.DoAction ();
		}
	}
	
	void MonsterDoAction () {
		GameActor target_game_actor = null;
		for ( int i = 0; i < entities_.Count; ++i ) {
			target_game_actor = ((GameActor)entities_[i]);
			if ( target_game_actor.Type() != ActorType.monster ) {
				continue;
			}
			target_game_actor.DoAction ();
		}
	}
}