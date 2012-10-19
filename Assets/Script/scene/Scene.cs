using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * The Scene class provides a minimalist implementation of the IScene interface.
 * <p>For API documentation please review the corresponding Interfaces.</p>
 * @author	Robert Fell
 */
public class Scene : Process, IScene
{
	public EScene Type () { return _type; }
	public IView View () { return _view; }
	public bool IsDisposable () { return _isDisposable; }
	public bool IsPauseable () { return _isPauseable; }
	public bool IsMuteable () { return _isMuteable; }
	public bool IsSessionSavedOnNext () { return _isSessionSavedOnNext; }

	private IEntity _Entity () { return _entity; }

	public Scene ( IKernel p_kernel, EScene p_type, bool p_isPauseable = false, bool p_isMuteable = true, bool p_isSessionSavedOnNext = false ) 
	{
		//TODO: these defaults aren't working for 2.09 (all set to false)?
		_type = p_type;
		_isPauseable = p_isPauseable;
		_isMuteable = p_isMuteable;
		_isSessionSavedOnNext = p_isSessionSavedOnNext;
		super( p_kernel );
	}
	
	override protected void _Init()
	{
		super._init();
		_isDisposable = true;
		_entity = new Entity( _kernel );
		_view = _entity.view;
	}
	
	override protected void _Updater ( float p_deltaTime )
	{
		super._updater( p_deltaTime );
		_entity.update( p_deltaTime );
	}
	
	override protected void _Disposer()
	{
		_entity.dispose();
		view.dispose();
		super._disposer();		
	}
	
	public void addEntity ( IEntity p_entity, EAgenda p_agenda, bool p_isAddedToView = false, int p_viewPriority = 0 )
	{
		_entity.addEntity( p_entity, p_agenda, p_isAddedToView, p_viewPriority );
	}
	
	public void removeEntity( IEntity p_entity, EAgenda p_agenda, bool p_isRemovedFromView = false )
	{
		_entity.removeEntity( p_entity, p_agenda, p_isRemovedFromView );
	}
	
	public List<IEntity> getEntities( EAgenda p_agenda )
	{
		return _entity.getEntities( p_agenda );
	}
	
	public List<T> getEntitiesByClass<T>( T p_classType, EAgenda p_agenda, bool p_isBubbleDown = false, bool p_isBubbleUp = false, bool p_isBubbleEverywhere = false )
	{
		return _entity.getEntitiesByClass( p_classType, p_agenda, p_isBubbleDown, p_isBubbleUp, false );
	}
	
	public IEntity getEntityById( string p_id, EAgenda p_agenda, bool p_isBubbleDown = false, bool p_isBubbleUp = false, bool p_isBubbleEverywhere = false )
	{
		return _entity.getEntityById( p_id, p_agenda, p_isBubbleDown, p_isBubbleUp, false );
	}
	
	private IView _get_view()
	{
		return view;
	}
}

