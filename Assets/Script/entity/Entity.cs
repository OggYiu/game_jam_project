using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//public class Entity : Process, IEntity {
//	string entity_id_ = "";
//		
//	override protected void _Resolver ( Hashtable args ) {
//		entity_id_ = System.Guid.NewGuid().ToString();
//	}
//	
//	public string GetID () {
//		return entity_id_;
//	}
//	
//	public static T Create<T> ( string entity_id ) where T : Entity {
//		Object entity_obj = null;
//		GameObject target_gameobj = null;
//		T target_entity = default(T);
//		
//		entity_obj = Resources.Load ( "Entities/" + entity_id );
//		if ( entity_obj == null ) {
//			Debug.LogError ( "<Entity::Create>, resource not found at path : " + entity_id );
//			return default(T);
//		}
//		
//		target_gameobj = (GameObject)(GameObject.Instantiate ( entity_obj ));
//		target_entity = target_gameobj.GetComponent<T>();
//		return target_entity;
//	}
//	
//	public void OnClicked () {
//		Debug.Log ( "<Entity::OnClicked> entity " + this.gameObject.name + " on clicked!" );
//	}
//}

/**
 * The Entity class provides a minimalist implementation of the IEntity interface.
 * <p>For API documentation please review the corresponding Interfaces.</p>
 * @author	Robert Fell
 */
public class Entity : Process, IEntity
{
	string _id;
	EAgenda _agenda;
	IEntity _parent;
	IView _view;
	
	public string ID () { return _id; }
	public EAgenda Agenda () { return _agenda; }
	public IEntity Parent () { return _parent; }
	public IView View () { return _view; }
	
	private List<_HelperEntityAgendaPair> _entityAgendaPairs;
	private bool _isAgendaDirty;
	private List<IEntity> _cachedEntities;

	public Entity ( IKernel p_kernel, string p_id = "", GameObject p_context = null ) 
	{
		if ( view == null )
		{
			view = new View( p_kernel, p_context, 0, this );
		}
		id = ( p_id == null ) ? p_kernel.tools.createGuid() : p_id;
		super( p_kernel );
	}
	
	override protected void _Init()
	{
		super._init();
		agenda = EAgenda.ALWAYS;
		_entityAgendaPairs = new FastList<_HelperEntityAgendaPair>();
		_isAgendaDirty = true;
		_cachedEntities = new List<IEntity>();
	}
	
	override protected void _Updater( float p_deltaTime = 0 )
	{
		super._updater( p_deltaTime );
		if ( _isAgendaDirty )
		{
			_cachedEntities = _getEntities( agenda );
			if ( !Type.enumEq( agenda, EAgenda.ALWAYS ) )
			{
				_cachedEntities = _cachedEntities.concat( _getEntities( EAgenda.ALWAYS ) );
			}
			_isAgendaDirty = false;
		}
		
		for ( int i = 0; i < _cachedEntities.Count; ++i ) {
			_cachedEntities[i].update( p_deltaTime );
		}
	}
	
	override protected void _Disposer()
	{
		Remove();
		_kernel.messenger.removeSubscribers( this );
		_kernel.messenger.removeSubscribers( null, null, null, this );
		List<IEntity> l_entities = _getEntities();
		l_entities.reverse();
		for ( int i = 0; i < l_entities.Count; ++i ) {
			l_entities[i].dispose();
		}
			
		for ( int i = 0; i < _entityAgendaPairs.Count; ++i ) {
			_entityAgendaPairs.Remove( _entityAgendaPairs[i] );
		}
		view.dispose();
		super._disposer();
	}
			
	public void addEntity ( IEntity p_entity, EAgenda p_agenda = EAgenda.ALWAYS, bool p_isAddedToView = false, int p_viewPriority = 0 )
	{
		if ( isDisposed )
		{
			return;
		}
		if ( p_entity == null )
		{
			return;
		}
		if ( p_agenda == null )
		{
			p_agenda = EAgenda.ALWAYS;
		}
		
		for ( int i = 0; i < _entityAgendaPairs.Count; ++i ) {
			if ( ( _entityAgendaPairs[i].entity == p_entity ) && ( Type.enumEq( i.agenda, p_agenda ) ) )
			{
				return; // already exists
			}
		}
		_isAgendaDirty = true;
		if ( p_entity.parent != this )
		{
			p_entity.remove( p_isAddedToView );
//			if ( Std.is( p_entity, Entity ) )
			if (p_entity.GetType() == Entity )
			{
				Entity l_child = (Entity)p_entity;
				l_child._setParent( this );
			}
		}
		_HelperEntityAgendaPair l_helperEntityAgendaPair = new _HelperEntityAgendaPair ( p_entity, p_agenda );
		_entityAgendaPairs.add( l_helperEntityAgendaPair );
		if ( p_isAddedToView )
		{
			if ( Type.enumEq( p_agenda, agenda ) || ( p_agenda == EAgenda.ALWAYS ) )
			{
				view.addChild( p_entity.view, p_viewPriority );
			}
			else
			{
				l_helperEntityAgendaPair.isAddedToView = true;
			}
		}
	}
	
	public void removeEntity( IEntity p_entity, EAgenda p_agenda = EAgenda.ALWAYS, bool p_isRemovedFromView = false )
	{
		if ( isDisposed )
		{
			return;
		}
		bool l_isRemoved = false;
		for ( int i = 0; i < _entityAgendaPairs.Count; ++i ) {
			if ( ( _entityAgendaPairs[i].entity == p_entity ) && ( ( p_agenda == null ) || ( Type.enumEq( i.agenda, p_agenda ) ) ) )
			{
				_entityAgendaPairs.remove( _entityAgendaPairs[i] );
				l_isRemoved = true;
			}
		}
		if ( l_isRemoved )
		{
			_isAgendaDirty = true;
//			if ( Std.is( p_entity, Entity ) )
			if ( p_entity.GetType() == Entity )
			{
				Entity l_child = (Entity)p_entity;
				l_child._setParent( null );
			}
			if ( p_isRemovedFromView )
			{
				p_entity.view.remove();
			}
		}
	}
	
	public void Remove( bool p_isRemovedFromView = false )
	{
		if ( parent != null )
		{
			parent.removeEntity( this, p_isRemovedFromView );
		}
	}	
	
	public List<IEntity> getEntities( EAgenda p_agenda = EAgenda.ALWAYS )
	{
		return _getEntities( p_agenda );
	}
	
	List<IEntity> _getEntities( EAgenda p_agenda = EAgenda.ALWAYS )
	{
		List<IEntity> l_result = new List<IEntity>();
		for ( int i = 0; i < _entityAgendaPairs.Count; ++i )
		{
			if ( ( p_agenda == null ) || Type.enumEq( p_agenda, i.agenda ) )
			{
				l_result.push( _entityAgendaPairs[i].entity );
			}
		}
		l_result.reverse();
		return l_result;
	}
	
	public List<T> getEntitiesByClass<T>( T p_classType, EAgenda p_agenda = EAgenda.ALWAYS, bool p_isBubbleDown = false, bool p_isBubbleUp = false, bool p_isBubbleEverywhere = false )
	{
		if ( p_isBubbleEverywhere && ( _kernel.scenes.scene != null ) )
		{
			return _kernel.scenes.scene.getEntitiesByClass( p_classType, p_agenda, true );
		}
		List<T> l_result = new List<T>();
		List<IEntity> l_entities = _getEntities( p_agenda );
		
		for ( int i = 0; i < l_entities.Count; ++i ) {
//			if ( Std.is( i, p_classType ) )
			if ( l_entities[i].GetType() == p_classType )
			{
				l_result.Add ( l_entities[i] );
			}
			if ( p_isBubbleDown )
			{
				l_result.concat( i.getEntitiesByClass( p_classType, p_agenda, true ) );
			}
		}
		if ( p_isBubbleUp && ( parent != null ) )
		{
			l_result.concat( parent.getEntitiesByClass( p_classType, p_agenda, false, true ) );
		}
		return l_result;
	}
	
	public IEntity getEntityById ( string p_id, EAgenda p_agenda = EAgenda.ALWAYS, bool p_isBubbleDown = false, bool p_isBubbleUp = false, bool p_isBubbleEverywhere = false )
	{
		if ( id == p_id )
		{
			return this;
		}
		if ( p_isBubbleEverywhere && ( _kernel.scenes.scene != null ) )
		{
			return _kernel.scenes.scene.getEntityById( p_id, p_agenda, true );
		}
		IEntity l_result = null;
		List<IEntity> l_entities = _getEntities( p_agenda );
		for ( int i = 0; i < l_entities.Count; ++i ) {
			if ( l_entities[i].id == p_id )
			{
				return l_entities[i];
			}
			if ( p_isBubbleDown )
			{
				l_result = l_entities[i].getEntityById( p_id, p_agenda, true );
			}
			if ( l_result != null )
			{
				return l_result;
			}
		}
		if ( p_isBubbleUp && ( parent != null ) )
		{
			l_result = parent.getEntityById( p_id, p_agenda, false, true );
		}
		return l_result;
	}
	
	public bool setAgenda( EAgenda p_type )
	{
		if ( p_type == null )
		{
			p_type = EAgenda.ALWAYS;
		}
		if ( Type.enumEq( agenda, p_type ) )
		{
			return false;
		}
		_isAgendaDirty = true;
		for ( int i = 0; i < _entityAgendaPairs.Count; ++i ) {
			bool l_isAddedToView = ( Type.enumEq( agenda, _entityAgendaPairs[i].agenda ) && ( _entityAgendaPairs[i].entity.view.parent == view ) );
			if ( l_isAddedToView )
			{
				i.entity.view.remove();
			}
			i.isAddedToView = i.isAddedToView || l_isAddedToView;			
		}
		agenda = p_type;
		for ( int i = 0; i < _entityAgendaPairs.Count; ++i ) {
			if ( _entityAgendaPairs[i].isAddedToView && ( Type.enumEq( EAgenda.ALWAYS, _entityAgendaPairs[i].agenda ) || Type.enumEq( agenda, _entityAgendaPairs[i].agenda ) ) )
			{
				view.addChild( i.entity.view ); 
			}
		}
		return true;
	}
	
	private void _setParent( IEntity p_parent )
	{
		parent = p_parent;
	}
	
	private string _set_id ( string p_value )
	{
		id = p_value;
		return id;
	}
	
	private EAgenda _get_agenda()
	{
		return agenda;
	}
	
	private IEntity _get_parent()
	{
		return parent;
	}
	
	private IView _get_view()
	{
		return view;
	}	
}

class _HelperEntityAgendaPair
{
	IEntity _entity;
	EAgenda _agenda;
	bool _isAddedToView;
	
	public _HelperEntityAgendaPair ( IEntity p_entity, EAgenda p_agenda )
	{
		_entity = p_entity;
		_agenda = p_agenda;
		_isAddedToView = false;
	}
	
	public IEntity Entity () { return _entity; }
	public EAgenda Agenda () { return _agenda; }
	public bool isAddedToView () { return _isAddedToView; }
}