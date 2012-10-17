using UnityEngine;
using System.Collections;

/**
 * The IInputKeyboard interface should be implemented by an object wishing to be used as a virtual keyboard input device.
 * @author	Robert Fell
 */
public interface IInputKeyboard {
	/**
	 * Determine if a specific key is currently down.
	 * @param	type	They key.
	 * @return	Returns true is the key is currently down, false otherwise.
	 */
	bool GetIsKeyDown ( KeyCode type );
	/**
	 * Determine if a specific key was pressed in the current update frame.
	 * <p>A press is defined as a new down - i.e. was up previous frame, and is down this frame.</p>
	 * @param	type	The key.
	 * @return	Returns true is the key was pressed in the current update, false otherwise.
	 */
	bool GetIsKeyPress ( KeyCode type );
	/**
	 * Determine if a specific key was released in the current update.
	 * <p>A release is defined as a new up - i.e. was down previous frame, and is up this frame.</p>
	 * @param	type	The key.
	 * @return	Returns true is the key was released in the current update, false otherwise.
	 */
	bool GetIsKeyRelease ( KeyCode type );
	/**
	 * Determine how long a specific key has been down.
	 * @param	type	The key.
	 * @param	?asTime	If true then returns duration as milliseconds, else returns duration as frame updates.
	 * @param	?isPrevious	If true then returns the previous duration down (the time held prior to the most recent release).
	 * @return	Returns the duration the key has been down.
	 */
	float GetKeyDownDuration ( KeyCode type, bool asTime = true, bool isPrevious = false );
	/**
	 * Determine how long a specific key has been up.
	 * @param	type	The key.
	 * @param	?asTime	If true then returns duration as milliseconds, else returns duration as frame updates.
	 * @param	?isPrevious	If true then returns the previous duration up (the time unused prior to the most recent press).
	 * @return	Returns the duration the key has been up.
	 */
	float GetKeyUpDuration( KeyCode type, bool asTime = true, bool isPrevious = false );
	/**
	 * Translate a specific key to a keyboard keyCode.
	 * @param	type	The key.
	 * @return	Returns the keyboard keyCode of the corresponding key.
	 */
	string GetKeyCode ( KeyCode type );
	/**
	 * Translate a keyCode to a specific key.
	 * @param	type	The keyCode.
	 * @return	Returns the key of the corresponding keyboard keyCode.
	 */
	KeyCode GetKey( string keyCode );
}

