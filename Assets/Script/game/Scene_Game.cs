using UnityEngine;
using System.Collections;

public class Scene_Game : Scene {
	protected override void _Resolver (Hashtable args)
	{
		base._Resolver (args);
		
		Entity target_entity = null;
		
		// man1
		target_entity = Entity.Create<Entity> ( "man1" );
		target_entity.Init ( "speed", 100 );
		AddEntity ( target_entity );
		target_entity.transform.parent = SceneManager.GetInstance().parent.transform;
		target_entity.transform.localPosition = new Vector3 ( 0, 0, 0 );
		
		// man2
		target_entity = Entity.Create<Entity> ( "man1" );
		target_entity.Init ( null );
		AddEntity ( target_entity );
		target_entity.transform.parent = SceneManager.GetInstance().parent.transform;
		target_entity.transform.localPosition = new Vector3 ( 32, 0, 0 );
	}
	
	void OnBackBtnClicked ( GameObject obj ) {
		SceneManager.GetInstance().ChangeScene ( "Scene_Intro" );
	}
}