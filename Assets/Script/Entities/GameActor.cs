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
		if ( 
	}
	
	override protected void _Updater ( float deltaTime = 0 ) {
	}
	
	
}

