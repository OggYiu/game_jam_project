using UnityEngine;
using System.Collections;

/**
 * This View class provides nme target overrides.
 * @author	Robert Fell
 */
class View : Process, IView
{
	GameObject _context;
	int _priority;
	object _owner;
	IView _parent;
	bool _isVisible;
	bool _isInViewStack;
	float _x;
	float _y;
	float _globalX;
	float _globalY;
	
	public var context( default, null ):Context;
	public var priority( _get_priority, _set_priority ):Int;
	public var owner( default, null ):Dynamic;
	public var parent( _get_parent, null ):IView;
	public var isVisible( default, _set_isVisible ):Bool;	
	public var isInViewStack( _get_isInViewStack, null ):Bool;
	public var x( default, _set_x ):Float;
	public var y( default, _set_y ):Float;
	public var globalX( default, null ):Float;
	public var globalY( default, null ):Float;
	
	private var _isDirty:Bool;
	private var _children:Array<AView>;
	GameObject _container;
	
	override protected void _Init ()
	{
		if ( context == null )
		{
			context = new Context(); 
		}
		super._init();
	}
	
	override protected void _DriverDisposer()
	{
		if ( ( context != null ) && ( context .parent != null ) )
		{
			try
			{
				context.parent.removeChild( context );
			}
			catch ( l_error:Dynamic ) {}
		}
	}
	
	override private function _driverDraw():Void
	{
		if ( parent != null )
		{
			parent.x = parent.x;
		}
		if ( _container != null && _container.parent != null )
		{
			_container.parent.removeChild( _container );
		}
		_container = new Context();
		_container.mouseEnabled = false;
		context.addChild( _container );
		var l_children:Array<View> = cast _children;
		for ( i in l_children )
		{
			if ( i.isVisible )
			{
				_container.addChild( i.context );
			}
		}
	}
	
	override private function _set_x( p_value:Float ):Float
	{
		context.x = p_value;
		return super._set_x( p_value );
	}
	
	override private function _set_y( p_value:Float ):Float
	{
		context.y = p_value;
		return super._set_y( p_value );
	}
	
}
{
	
	public function new( p_kernel:IKernel, ?p_context:Context, ?p_priority:Int = 0, ?p_owner:Dynamic ) 
	{
		context = p_context;
		priority = p_priority;
		owner = p_owner;
		super( p_kernel );
	}
	
	override private function _init():Void 
	{
		super._init();
		globalX = 0;
		globalY = 0;
		x = 0;
		y = 0;
		isVisible = true;
		_isDirty = true;
		_children = new Array<AView>();
	}
	
	public function addChild( p_child:IView, ?p_priority:Int = 0 ):Void
	{
		if ( isDisposed || ( p_child == null ) )
		{
			return;
		}
		if ( p_child.parent != this )
		{
			p_child.remove();
			if ( Std.is( p_child, AView ) )
			{
				var l_child:AView = cast p_child;
				_children.push( l_child );
				l_child._setParent( this );
			}
		}
		if ( p_priority != 0 )
		{
			p_child.priority = p_priority;
		}
		_isDirty = true;
	}
	
	public function removeChild( p_child:IView ):Void
	{
		if ( isDisposed || ( p_child == null ) )
		{
			return;
		}
		if ( Std.is( p_child, AView ) )
		{
			var l_child:AView = cast p_child;
			if ( l_child.parent != this )
			{
				return;
			}
			_children.remove( l_child );
			l_child._setParent( null );
		}
		_isDirty = true;
	}
	
	public function remove():Void
	{
		if ( parent != null )
		{
			parent.removeChild( this );
		}
	}
	
	public function clear():Void
	{
		for ( i in _children )
		{
			removeChild( i );
		}
	}
		
	override private function _updater( ?p_deltaTime:Int = 0 ):Void 
	{
		super._updater( p_deltaTime );
		for ( i in _children )
		{
			i.update( p_deltaTime );
		}
		if ( _isDirty )
		{
			_draw();
		}
		//TODO: inefficient recalculation of global position - necessary, but needs rethink
		globalX = ( parent == null ) ? x : x + parent.globalX;
		globalY = ( parent == null ) ? y : y + parent.globalY;
	}
	
	override private function _disposer():Void 
	{
		remove();
		_driverDisposer();
		clear();
		super._disposer();	
	}
	
	private function _driverDisposer():Void
	{
		//override me
	}
	
	private function _draw():Void
	{
		if ( isDisposed )
		{
			return;
		}
		_children.sort( _tools.sortByPriority );
		_driverDraw();
		_isDirty = false;
	}
	
	private function _driverDraw():Void
	{
		//override me
	}
	
	private function _setParent( p_parent:IView ):Void
	{
		parent = p_parent;
	}
	
	private function _get_priority():Int
	{
		return priority;
	}
	
	private function _set_priority( p_value:Int ):Int
	{
		if ( p_value == priority )
		{
			return priority;
		}
		priority = p_value;
		if ( Std.is( parent, AView ) )
		{
			var l_parent:AView = cast parent;
			if ( l_parent != null )
			{
				l_parent._isDirty = true;
			}
		}
		return priority;
	}
	
	private function _set_isVisible( p_value:Bool ):Bool
	{
		if ( p_value == isVisible )
		{
			return isVisible;
		}
		isVisible = p_value;
		if ( Std.is( parent, AView ) )
		{
			var l_parent:AView = cast parent;
			if ( l_parent != null )
			{
				l_parent._draw();
			}
		}
		return isVisible;
	}
	
	private function _get_parent():IView
	{
		return parent;
	}
	
	private function _get_isInViewStack():Bool
	{
		if ( !isVisible )
		{
			return false;
		}
		if ( owner == _kernel )
		{
			return true;
		}
		if ( parent == null )
		{
			return false;
		}
		return parent.isInViewStack;
	}
	
	private function _set_x( p_value:Float ):Float
	{
		x = p_value;
		globalX = ( parent == null ) ? x : x + parent.globalX;
		return x;
	}
	
	private function _set_y( p_value:Float ):Float
	{
		y = p_value;
		globalY = ( parent == null ) ? y : y + parent.globalY;
		return y;
	}
	
	public function setPosition( p_x:Float, p_y:Float ):Void
	{
		x = p_x;
		y = p_y;
	}
	
}

