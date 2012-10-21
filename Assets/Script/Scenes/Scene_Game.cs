using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class ActorSpawner {
	public ActorSpawner ( ActorType p_type, int p_row, int p_column ) {
		type = p_type;
		row = p_row;
		column = p_column;
	}
	
	public ActorType type;
	public int row;
	public int column;
}

public class Scene_Game : Scene {
//	GameActor human_ = null;
//	GameActor human_ = null;
	float wait_time_ = 0;
//	GameActor monster_ = null;
	int age_ = 0;
	List<ActorSpawner> actor_spawners_ = new List<ActorSpawner>();
	Tile_Map tile_map_;
	
	protected override void _Resolver (Hashtable args)
	{
		base._Resolver (args);
		
//		if (args.Contains("Scene_Game")) {
//			scene_game_ =(Scene_Game)args["Scene_Game"];
//		}
		
		NavigationMap.GetInstance().Init ( "Scene_Game", this );
		
//		Entity target_entity = null;
		
		GameActor human = GameActor.Create ( GameSettings.GetInstance().HUMAN_PREFAB_NAME );
		AddEntity ( human );
		NavigationMap.GetInstance().RegisterActor ( human );
		
		GameActor monster = GameActor.Create ( "Entity_Monster" );
		AddEntity ( monster );
		monster.transform.localPosition = new Vector3 ( 64 * 5, 64 * 4 );
		NavigationMap.GetInstance().RegisterActor ( monster );
		
		tile_map_ = new Tile_Map ();
		tile_map_.Init ();
		
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
			wait_time_ = GameSettings.GetInstance().ACTION_INTERVAL;
			NextTurn ();
		}
		
		if ( wait_time_ > 0 ) {
			wait_time_ -= Time.deltaTime;
		} else {
			// human first
			HumanDoAction ();
			MonsterDoAction ();
			wait_time_ = GameSettings.GetInstance().ACTION_INTERVAL;
		}
		
		ActorSpawner target_spawner = null;
		for ( int i = 0; i < actor_spawners_.Count; ++i ) {
			target_spawner = actor_spawners_[i];
			
			GameActor actor;
			if ( target_spawner.type == ActorType.human ) {
				actor = GameActor.Create ( GameSettings.GetInstance().HUMAN_PREFAB_NAME );
			} else {
				actor = GameActor.Create ( GameSettings.GetInstance().MONSTER_PREFAB_NAME );
			}
			AddEntity ( actor );
			actor.transform.localPosition = new Vector3 (	target_spawner.column * GameSettings.GetInstance().TILE_SIZE,
															target_spawner.row * GameSettings.GetInstance().TILE_SIZE,
															0 );
			NavigationMap.GetInstance().RegisterActor ( actor );
		}
		actor_spawners_.Clear();
	}
	
	override public void MouseButtonDownHandler ( int button_index ) {
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
			if ( target_game_actor.action_point > 0 ) {
				target_game_actor.DoAction ();
			}
		}
	}
	
	void MonsterDoAction () {
		GameActor target_game_actor = null;
		for ( int i = 0; i < entities_.Count; ++i ) {
			target_game_actor = ((GameActor)entities_[i]);
			if ( target_game_actor.Type() != ActorType.monster ) {
				continue;
			}
			if ( target_game_actor.action_point > 0 ) {
				target_game_actor.DoAction ();
			}
		}
	}
	
	public void AddActorSpawner ( ActorType type, int row, int column ) {
		actor_spawners_.Add ( new ActorSpawner ( type, row, column ) );
	}
}