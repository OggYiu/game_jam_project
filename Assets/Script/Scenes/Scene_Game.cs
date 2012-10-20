using UnityEngine;
using System.Collections;

public class Scene_Game : Scene {
	bool next_turn_ = false;
	GameActor human_ = null;
	GameActor monster_ = null;
	
	protected override void _Resolver (Hashtable args)
	{
		base._Resolver (args);
		
		Entity target_entity = null;
		
		human_ = GameActor.Create ( "BaseEntity" );
		AddEntity ( human_ );
		
		// man1
//		target_entity = Entity.Create<Entity> ( "man1" );
//		target_entity.Init ( "speed", 100 );
//		AddEntity ( target_entity );
//		target_entity.transform.parent = SceneManager.GetInstance().entities_parent.transform;
//		target_entity.transform.localPosition = new Vector3 ( 0, 32, 0 );
		
		// man2
//		target_entity = Entity.Create<Entity> ( "man1" );
//		target_entity.Init ( null );
//		AddEntity ( target_entity );
//		target_entity.transform.parent = SceneManager.GetInstance().entities_parent.transform;
//		target_entity.transform.localPosition = new Vector3 ( 32, 32, 0 );
	}
	
	void OnBackBtnClicked ( GameObject obj ) {
		SceneManager.GetInstance().ChangeScene ( "Scene_Intro" );
	}
	
	protected override void _Updater (float deltaTime)
	{
		base._Updater (deltaTime);
		
		if ( next_turn_ ) {
			next_turn_ = false;
		}
		
		NavigationMap.GetInstance().OnUpdate();
	}
	
	virtual protected void MouseButtonDownHandler ( int button_index ) {
		next_turn_ = true;
	}
}