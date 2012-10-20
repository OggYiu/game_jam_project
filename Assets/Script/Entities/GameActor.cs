using UnityEngine;
using System.Collections;

public enum Directions {
	east,
	sourth,
	west,
	north,
}

public class GameActor : Entity
{
	Vector2 map_position_ = Vector2.zero;
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public Vector2 MapPosition () {
		return map_position_;
	}
	
	public void MoveTo ( Directions direction ) {
	}
	
	override protected void _Updater ( float deltaTime = 0 ) {
	}
	
	public static GameActor Create ( string id ) {
		string target_path = "Prefabs/" + id;
		Object target_prefab = Resources.Load ( target_path );
		if ( target_prefab == null ) {
			Debug.LogError ( "<GameActor::Create>, invalid path :" + target_path );
		}
		GameActor target_actor = ((GameObject)GameObject.Instantiate ( target_prefab )).GetComponent<GameActor>();
		if ( target_actor == null ) {
			Debug.LogError ( "<GameActor::Create>, game actor object not found" );
		}
		target_actor.transform.parent = SceneManager.GetInstance().entities_parent.transform;
		target_actor.transform.localPosition = new Vector3 ( 0, 0, 0 );
		return target_actor;
	}
}

