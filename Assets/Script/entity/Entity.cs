using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity : Process, IEntity {
	string entity_id_ = "";
	
	override protected void _Resolver ( Hashtable args ) {
		entity_id_ = System.Guid.NewGuid().ToString();
	}
	
	public string GetID () {
		return entity_id_;
	}
	
	public static T Create<T> ( string entity_id ) where T : Entity {
		Object entity_obj = null;
		GameObject target_gameobj = null;
		T target_entity = default(T);
		
		entity_obj = Resources.Load ( "Entities/" + entity_id );
		if ( entity_obj == null ) {
			Debug.LogError ( "<Entity::Create>, resource not found at path : " + entity_id );
			return default(T);
		}
		
		target_gameobj = (GameObject)(GameObject.Instantiate ( entity_obj ));
		target_entity = target_gameobj.GetComponent<T>();
		return target_entity;
	}
	
	public void OnClicked () {
		Debug.Log ( "<Entity::OnClicked> entity " + this.gameObject.name + " on clicked!" );
	}
}
