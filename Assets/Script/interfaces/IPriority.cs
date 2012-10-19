using UnityEngine;
using System.Collections;

/**
 * The IPriority interface should be implemented by objects intended to be ranked or sorted.
 * @author	Robert Fell
 */
public interface IPriority 
{
	/**
	 * The rank score of this item.
	 * <p>Higher numbers should be considered on top of the list, therefore of higher priority.</p>
	 */
	int Priority ();
}