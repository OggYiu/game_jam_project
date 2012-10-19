using UnityEngine;
using System.Collections;

/**
 * The IProgress interface should be implemented by objects intended to progress from start to finish ( 0...1 ).
 * @author	Robert Fell
 */
public interface IProgress
{
	/**
	 * Range: 0...1.  0 represents just starting, 1 represents complete.
	 */
	float Progress ();
}