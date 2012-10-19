using UnityEngine;
using System.Collections;

/**
 * The IVewable interface should be implemented by all objects that compose a view.
 * @author	Robert Fell
 */
public interface IViewable
{
	/**
	 * The view bound to this object.
	 */
	IView view ();
}