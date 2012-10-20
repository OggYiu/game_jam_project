using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity : MonoBehaviour {
	string entity_id_ = "";
	protected bool is_disposed_ = false;
	
	virtual protected void _Resolver ( Hashtable args ) {
		entity_id_ = System.Guid.NewGuid().ToString();
	}
	
	public string GetID () {
		return entity_id_;
	}
	
	public void Init ( Hashtable args ) {
		_Initer ( args );
	}
	
	virtual protected void OnClicked () {
		Debug.Log ( "<Entity::OnClicked> entity " + this.gameObject.name + " on clicked!" );
	}
	
	public void Dispose () {
		_Disposer ();
	}
	
	public void OnUpdate ( float deltaTime = 0 ) {
		_Updater ( deltaTime );
	}
	
	virtual protected void _Initer ( Hashtable args ) {
	}
	
	virtual protected void _Updater ( float deltaTime = 0 ) {
	}
	
	virtual protected void _Disposer () {
	}
}
