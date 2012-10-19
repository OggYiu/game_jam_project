using UnityEngine;
using System.Collections;

/**
 * The IPositionable interface should be implemented by objects intended to have 2D spatial position.
 * @author	Robert Fell
 */
public interface IPositionable
{
	/**
	 * The horizontal position.
	 */
	float GetX ();
	void SetX ( float x );
	/**
	 * The vertical position.
	 */
	float GetY ();
	void SetY ( float y );
	/**
	 * Sets both the horizontal and vertical position;
	 * @param	x	The horizontal position.
	 * @param	y	The vertical position.
	 */
	void SetPosition ( float x, float y );
}