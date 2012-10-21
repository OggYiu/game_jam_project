using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum GameEndReason {
	human_rulz,
	monster_rulz,
	time_out,
}

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
	GameEndReason game_end_reason_;
	const float game_end_interval_ = 1;
	float game_end_count_down_ = -1;
	bool is_game_ended_ = false;
	GUIText game_end_text_ = null;
	[SerializeField] AudioSource audio_dino_die_;
	[SerializeField] AudioSource audio_human_die_;
	[SerializeField] AudioSource audio_win_;
	[SerializeField] AudioSource audio_background_;
	bool is_dino_die_sound_play_ = false;
	bool is_human_die_sound_play_ = false;
	bool navigation_map_inited_ = false;
	
	protected override void _Resolver (Hashtable args)
	{
		base._Resolver (args);
		
		game_end_text_ = this.GetComponentInChildren<GUIText>();
		if ( game_end_text_ == null ) {
			Debug.LogError ( "<Scene_Game::_Resolver> invalid game_end_text_" );
		}
		game_end_text_.text = "";
		
//		if (args.Contains("Scene_Game")) {
//			scene_game_ =(Scene_Game)args["Scene_Game"];
//		}
		
//		GameDataShare.forest_sand_percentage = 0.2f;
//		GameDataShare.mountain_percentage = 0.1f;
		NavigationMap.GetInstance().Init ( "Scene_Game", this );
		
//		Entity target_entity = null;
		
//		GameActor human = GameActor.Create ( GameSettings.GetInstance().HUMAN_PREFAB_NAME );
//		AddEntity ( human );
//		NavigationMap.GetInstance().RegisterActor ( human );
//		
//		GameActor monster = GameActor.Create ( "Entity_Monster" );
//		AddEntity ( monster );
//		monster.transform.localPosition = new Vector3 ( 64 * 5, 64 * 4 );
//		NavigationMap.GetInstance().RegisterActor ( monster );
		
		tile_map_ = new Tile_Map ();
		tile_map_.Init ();
		
		audio_background_.Play ();
//		monster_ = GameActor.Create ( "BaseEntity" );
//		AddEntity ( monster_ );
//		monster_.transform.localPosition = new Vector3 ( GameSettings.GetInstance().TILE_SIZE, 0, 0 );
	}
	
	void OnBackBtnClicked ( GameObject obj ) {
		SceneManager.GetInstance().ChangeScene ( "Scene_Intro" );
	}
	
	protected override void _Updater (float deltaTime)
	{
		if ( is_game_ended_ ) {
			return ;
		}
		
		NavigationMap.GetInstance().OnUpdate();
		
		if ( !navigation_map_inited_ ) {
			// get all map info to spawn monster and human
			NavigationMap.GetInstance().SpawnCreatures ();
			navigation_map_inited_ = true;
		}
		
		base._Updater (deltaTime);
		
		if ( IsAllActionDone() ) {
			wait_time_ = GameSettings.GetInstance().ACTION_INTERVAL;
//			DamageCharacterInWater ();
			NextTurn ();
		}
		
		if ( game_end_count_down_ > 0 ) {
			game_end_count_down_ -= deltaTime;
			if ( game_end_count_down_ <= 0 ) {
				is_game_ended_ = true;	
				switch ( game_end_reason_ ) {
				case GameEndReason.human_rulz:
					game_end_text_.text = "HUMAN RULZ!";
					break;
				case GameEndReason.monster_rulz:
					game_end_text_.text = "MONSTER RULZ!";
					break;
				case GameEndReason.time_out:
					game_end_text_.text = "TIMES UP!";
					break;
				}
				audio_win_.Play ();
				return ;
			}
		} else {
			CheckIfGameEnd ();
		}
		
		if ( wait_time_ > 0 ) {
			wait_time_ -= Time.deltaTime;
		} else {
			NavigationMap.GetInstance().RefreshAll ();
			// human first
			HumanDoAction ();
			MonsterDoAction ();
			wait_time_ = GameSettings.GetInstance().ACTION_INTERVAL;
			
			if ( is_dino_die_sound_play_ ) {
				if ( !audio_dino_die_.isPlaying ) {
					audio_dino_die_.Play ();
				}
			} else if ( is_human_die_sound_play_ ) {
				if ( !audio_human_die_.isPlaying ) {
					audio_human_die_.Play ();
				}
			}
			is_dino_die_sound_play_ = false;
			is_human_die_sound_play_ = false;
		}
		
		ActorSpawner target_spawner = null;
		for ( int i = 0; i < actor_spawners_.Count; ++i ) {
			target_spawner = actor_spawners_[i];
			
			GameActor actor;
			if ( target_spawner.type == ActorType.human ) {
				actor = GameActor.Create ( GameSettings.GetInstance().HUMAN_PREFAB_NAME );
				++GameStatics.human_spawned;
			} else {
				actor = GameActor.Create ( GameSettings.GetInstance().MONSTER_PREFAB_NAME );
				++GameStatics.monster_spawned;
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
//		NextTurn ();
	}
	
	bool IsAllActionDone () {
		for ( int i = 0; i < entities_.Count; ++i ) {
			if ( !((GameActor)entities_[i]).isAlive() )
				continue;
			if ( !((GameActor)entities_[i]).IsActionEnded () ) {
				return false;
			}
		}
		return true;
	}
	
	void NextTurn () {
		++age_;
		GameStatics.game_turn = age_;
		
		GameActor target_actor = null;
		for ( int i = 0; i < entities_.Count; ++i ) {
			target_actor = ((GameActor)entities_[i]);
			
			if ( !target_actor.isAlive() )
				continue;
			
			target_actor.TurnBeginHandler ();
			
			if ( target_actor.Type() == ActorType.human ) {
				if ( target_actor.age > GameStatics.oldest_human ) {
					GameStatics.oldest_human = target_actor.age;
				}
			} else {
				if ( target_actor.age > GameStatics.oldest_monster ) {
					GameStatics.oldest_monster = target_actor.age;
				}
			}
		}
		
//		oldest_human
//			oldest_monster = 0;
	}
	
	void CheckIfGameEnd () {
		if ( game_end_count_down_ > 0 || is_game_ended_ ) {
			return ;
		}
		
		if ( GameSettings.GetInstance().TURN_LIMIT > 0 && age_ >= GameSettings.GetInstance().TURN_LIMIT ) {
			game_end_count_down_ = game_end_interval_;
			game_end_reason_ = GameEndReason.time_out;
			return ;
		}
		
		bool only_human_existed = true;
		bool only_monster_existed = true;
		GameActor target_actor;
		for ( int i = 0; i < entities_.Count; ++i ) {
			target_actor = (GameActor)entities_[i];
			if ( !target_actor.isAlive() )
				continue;
			if ( target_actor.Type() == ActorType.monster ) {
				only_human_existed = false;
			} else {
				only_monster_existed = false;
			}
		}
		
		if ( only_human_existed ) {
			game_end_count_down_ = game_end_interval_;
			game_end_reason_ = GameEndReason.human_rulz;
		} else if ( only_monster_existed ) {
			game_end_count_down_ = game_end_interval_;
			game_end_reason_ = GameEndReason.monster_rulz;
		}
	}
	
	void HumanDoAction () {
		GameActor target_game_actor = null;
		for ( int i = 0; i < entities_.Count; ++i ) {
			target_game_actor = ((GameActor)entities_[i]);
			if ( !target_game_actor.isAlive() ) {
				continue;
			}
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
			if ( !target_game_actor.isAlive() ) {
				continue;
			}
			if ( target_game_actor.Type() != ActorType.monster ) {
				continue;
			}
			if ( target_game_actor.action_point > 0 ) {
				target_game_actor.DoAction ();
			}
		}
	}
	
//	void DamageCharacterInWater () {
//		int cur_map_x = 0;
//		int cur_map_y = 0;
//		GameActor target_actor;
//		for ( int i = 0; i < entities_.Count; ++i )  {
//			target_actor = (GameActor)entities_[i];
//			cur_map_x = (int)target_actor.map_pos.x;
//			cur_map_y = (int)target_actor.map_pos.y;
//			NodeType type = NavigationMap.GetInstance().GetNodeType ( cur_map_y, cur_map_x );
//			if ( type == NodeType.blocked ) {
//				target_actor.ReduceHealth ( GameSettings.GetInstance().WATER_DAMAGE );
//				
//				if ( !target_actor.isAlive() ) {
//					if ( target_actor.Type() == ActorType.human ) {
//						PlayAudioHumanDie();
//					} else {
//						PlayAudioDinoDie();
//					}
//				}
//			}
//		}
//	}
	
	public void AddActorSpawner ( ActorType type, int row, int column ) {
		actor_spawners_.Add ( new ActorSpawner ( type, row, column ) );
	}
	
	public int age {
		set {}
		get { return age_; }
	}
	
	public void PlayAudioDinoDie () {
		is_dino_die_sound_play_ = true;
	}
	
	public void PlayAudioHumanDie () {
		is_human_die_sound_play_ = true;
	}
}