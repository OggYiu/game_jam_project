using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Scene : Entity {
	protected List<Entity> entities_ = new List<Entity>();
	
	public void AddEntity ( Entity entity )
	{
		if ( is_disposed_ ) {
			Debug.LogError ( "<Scene::AddEntity>, disposed!" );
			return;
		}
		if ( entity == null ) {
			Debug.LogError ( "<Scene::AddEntity>, invalid entity!" );
			return;
		}
		
		for ( int i = 0; i < entities_.Count; ++i ) {
			if ( entities_[i] == entity ) {
				Debug.LogError ( "<Scene::AddEntity>, entity already added!" );
				return ;
			}
		}
		
		entities_.Add ( entity );
	}
	
	public void RemoveEntity ( Entity entity ) {
		if ( is_disposed_ ) {
			Debug.LogError ( "<Scene::RemoveEntity>, disposed!" );
			return;
		}
		
		for ( int i = 0; i < entities_.Count; ++i ) {
			if ( entities_[i] == entity ) {
				entities_.Remove ( entity );
				break;
			}
		}
	}
	
	public Entity GetEntityById ( string id ) {
		if ( is_disposed_ ) {
			Debug.LogError ( "<Scene::RemoveEntity>, disposed!" );
			return null;
		}
		
		for ( int i = 0; i < entities_.Count; ++i ) {
			if ( entities_[i].GetID() == id ) {
				return entities_[i];
			}
		}
		
		return null;
	}
	
	protected override void _Updater ( float deltaTime = 0 )
	{
		base._Updater (deltaTime);
		
		for ( int i = 0; i < entities_.Count; ++i ) {
			entities_[i].OnUpdate ( deltaTime );
		}
	}
	
	protected override void _Disposer ()
	{
		base._Disposer ();
		
		for ( int i = 0; i < entities_.Count; ++i ) {
			DestroyObject ( entities_[i].gameObject );
		}
	}
	
	virtual public void MouseButtonDownHandler ( int button_index ) {
	}
}