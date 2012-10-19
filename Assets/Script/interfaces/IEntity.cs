using UnityEngine;
using System.Collections;

//public interface IEntity {
//	void OnClicked ();
//}

/**
 * The IEntity interface should be implemented by all objects in the entity broad phase traversal stack.
 * <p>The IEntity represents the fundamental awe6 building block and provides sufficient functionality to build most game elements.</p>
 * <p>Project specific entities can be created as custom classes, or by injecting functionality through the IEntity interface.</p>
 * @author Robert Fell
 */
public interface IEntity : IProcess, IViewable, IEntityCollection
{
	/**
	 * The unique identifier of this entity.
	 * <p>This value is very useful for retrieving a specific entity.</p> 
	 */
	string ID ();
	/**
	 * The parent of this entity
	 * <p>The reference is null if this entity has no parent (for example an entity not in the entity traversal stack).</p>
	 * <p>Consider this a runtime only property, rather than calling it during constructor or initialization phases.</p>
	 */
	IEntity Parent ();
	/**
	 * Used to easily remove this entity from its parent.
	 * @param	?isRemovedFromView	Determines whether this object's view is removed from the view stack at the same time.
	 */
	void Remove ( bool isRemovedFromView = false );
}