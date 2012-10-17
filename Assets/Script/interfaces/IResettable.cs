using UnityEngine;
using System.Collections;

/**
 * The IResettable interface should be implemented by objects intended to be reset (returned to initial state).
 * @author	Robert Fell
 */
public interface IResettable {
	/**
	 * Call method to return object to it's initial state.
	 * @return	True if reset was successful, false otherwise.
	 */
	bool Reset();
}