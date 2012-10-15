using UnityEngine;
using System.Collections;

/**
 * The IPauseable interface should be implemented by objects intended to be temporarily disabled from the broad phase update traversal.
 * @author	Robert Fell
 */
public interface IPauseable
{
	/**
	 * Determines if the object is updating or not.
	 */
	bool isActive ();
	void SetActive ( bool val );
	
	/**
	 * Sets isActive to false.
	 */
	void Pause();
	
	/**
	 * Sets isActive to true.
	 */
	void Resume();
}
