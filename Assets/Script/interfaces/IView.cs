using UnityEngine;
using System.Collections;

/**
 * The IView interface should be implemented by all objects in the view broad phase traversal stack.
 * @author	Robert Fell
 */
public interface IView : IPriority, IPositionable, IDisposable, IUpdateable
{
	/**
	 * Optional: the object who this view represents.
	 */
	object Owner ();
	/**
	 * The parent view of this view.
	 * <p>The reference is null if this view has no parent (for exemple a view not in the view traversal stack).</p>
	 */
	IView parent ();
	/**
	 * Specify the visibility of this view.
	 * <p>If true the view will be displayed, if false the view is hidden.</p>
	 */
	bool IsVisible ();
	/**
	 * Determined by whether this view is visible and included in a visible branch of the view stack (i.e. actually has the potential to be drawn within the overlay).
	 * <p>If true the view is potentially visible, if false the view is impossible to be seen.</p>
	 */
	bool IsInViewStack ();
	/**
	 * The horizontal position considering all parent's positions / scene graph.
	 */
	float GlobalX ();
	/**
	 * The vertical position considering all parent's positions / scene graph.
	 */
	float GlobalY ();
	/**
	 * Adds a new view child to this view. 
	 * <p>A view can have multiple children, and when you add a child to a view, it is automatically connected to the parent node through its parent property.</p>
	 * @param	child	The child view to add.
	 * @param	?priority	The sorting priority of the child view to add.  Higher numbers will appear towards the top of the view stack.  Default value is 0.
	 */
	void AddChild ( IView child, int priority = 0 );
	/**
	 * Remove the specified view.
	 * <p>The removed view will no longer be included in the view traversal stack so will no longer be visible.</p>
	 * <p>The view itself is still in memory, if you want to free them completely call child.dispose().</p>
	 * @param	child	The view to remove.
	 */
	void RemoveChild ();
	/**
	 * Removes all child views.
	 * <p>The children are still in memory, if you want to free them completely call view.dispose() from their owner object.</p>
	 */
	void Clear();
	/**
	 * Removes this view from the view traversal stack and subsequently all of its child views.
	 * <p>The view itself is still in memory, if you want to free it completely call dispose().</p>
	 */
	void Remove ();
}