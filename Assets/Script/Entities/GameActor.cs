using UnityEngine;
using System.Collections;

public enum Directions {
	east,
	sourth,
	west,
	north,
}

public enum ActorType {
	none,
	human,
	monster,
}

public class GameActor : Entity
{
	[SerializeField] int action_point_gainer_ = 0;
	
	Vector2 map_position_ = Vector2.zero;
	int action_point_ = 0;
	int age_ = 0;
	protected ActorType type_;
	
	// Use this for initialization
	void Start ()
	{
		if ( action_point_gainer_ <= 0 ) {
			Debug.LogError ( "<GameActor::Start>, invalid action_point_gainer_" );
			return ;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public ActorType Type () {
		return type_;
	}
	
	public Vector2 MapPosition () {
		return map_position_;
	}
	
	public void TurnBeginHandler () {
		++age_;
		_TurnBeginHandler ();
	}
	
	virtual protected void _TurnBeginHandler () {
		action_point_ = action_point_gainer_;
	}
	
	public void MoveTo ( Directions direction ) {
	}
	
	override protected void _Updater ( float deltaTime = 0 ) {
	}
	
	public void Think () {
		_Thinker ();
	}
	
	virtual protected void _Thinker () {
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
	
	public int action_point {
		set {}
		get { return action_point_; }
	}
	
	public bool IsActionEnded () {
		return action_point_ <= 0;
	}
	
	public void DoAction () {
		if ( action_point_ <= 0 ) {
			Debug.LogError ( "<GameActor::DoAction>, invalid action_point_" );
			return ;
		}
		
		--action_point_;
		_DoAction ();
	}
	
	virtual protected void _DoAction () {
		Think ();
	}
	
	public Vector3 map_pos {
		set { }
		get { return GameUtils.Real2MapPos ( this.transform.localPosition ); }
	}
	
	public Vector3 pos {
		set { this.transform.localPosition= value; } 
		get { return this.transform.localPosition; }
	}
}

