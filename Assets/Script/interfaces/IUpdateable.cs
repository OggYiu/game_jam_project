using UnityEngine;
using System.Collections;

/**
 * The IUpdatable interface should be implemented by any object wishing to enter the broad phase update traversal stack.
 * @author	Robert Fell
 */
public interface IUpdateable
{
	/**
	 * The age of the object.
	 * @param	?asTime	If true treats the time as milliseconds, otherwise as frame updates.
	 * @return	The age of the object (as elapsed time, not time since birth).
	 */
	float GetAge ( bool asTime = true );
	
	/**
	 * Used to modify the internal state according to object specific logic and the elapsed time.
	 * <p>This method is called internally by the framework, it will rarely need to be called directly.</p>
	 * @param	?deltaTime	The time elapsed between this update and the previous update.  Can be used to accurately influence rate of change - e.g. speed.
	 */
	void OnUpdate ( float deltaTime );
}