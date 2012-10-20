using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity : MonoBehaviour {
	string entity_id_ = "";
	protected bool is_disposed_ = false;
	bool is_started_ = false;
	bool is_resolved_ = false;
	Hashtable init_args_ = null;
	
	void Start () {
		is_started_ = true;
	}
	
	virtual protected void _Resolver ( Hashtable args ) {
		entity_id_ = System.Guid.NewGuid().ToString();
	}
	
	public string GetID () {
		return entity_id_;
	}
	
	public void Init ( Hashtable args ) {
		init_args_ = args;
	}
	
	virtual protected void OnClicked () {
		Debug.Log ( "<Entity::OnClicked> entity " + this.gameObject.name + " on clicked!" );
	}
	
	public void Dispose () {
		_Disposer ();
	}
	
	public void OnUpdate ( float deltaTime = 0 ) {
		if ( is_started_ && !is_resolved_ ) {
			is_resolved_ = true;
			_Resolver ( init_args_ );
		}
		
		_Updater ( deltaTime );
	}
	
	virtual protected void _Updater ( float deltaTime = 0 ) {
	}
	
	virtual protected void _Disposer () {
	}
}
