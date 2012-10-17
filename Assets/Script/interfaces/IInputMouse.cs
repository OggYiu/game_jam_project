using UnityEngine;
using System.Collections;

/**
 * The IInputMouse interface should be implemented by objects wishing to act as virtual mouse controllers.
 * <p>Screen bounds are based on IFactory.width & IFactory.height.</p>
 * @author	Robert Fell
 */
public interface IInputMouse 
{
	/**
	 * The horizontal component of the mouse position.
	 */
	float GetX ();
	/**
	 * The vertical component of the mouse position.
	 */
	float GetY ();
	/**
	 * The horizontal position of the mouse relative to screen width.  Range 0...1.
	 */
	float GetRelativeX ();
	/**
	 * The vertical position of the mouse relative to screen height.  Range 0...1.
	 */
	float GetRelativeY ();
	/**
	 * The horizontal position of the mouse relative to screen width and offset to screen centre.  Range -1...1.
	 */
	float GelativeCentralisedX ();
	/**
	 * The vertical position of the mouse relative to screen height and offset to screen centre.  Range -1...1.
	 */
	float GelativeCentralisedY ();
	/**
	 * Returns true if the mouse position is within the bounding rectangle (factory width x factory height).
	 */
	bool IsWithinBounds ();
	/**
	 * Returns true if the mouse position is different to the previous update's position.
	 */
	bool IsMoving ();
	/**
	 * Specify the visibility of the mouse cursor.
	 * <p>If true the cursor will be displayed, if false the cursor is hidden.</p>
	 */
	bool IsVisible ();
	/**
	 * The current cursor type.
	 */
	EMouseCursor CursorType ();
	/**
	 * The horizontal velocity of the mouse position.
	 * @param	?asTime	If true then returns the velocity as pixels per second (extrapolated from the previous update), else returns velocity as pixels moved in previous update.
	 * @return	The horizontal velocity of the mouse.
	 */
	float GetDeltaX ( bool asTime = true );
	/**
	 * The vertical velocity of the mouse position.
	 * @param	?asTime	If true then returns the velocity as pixels per second (extrapolated from the previous update), else returns velocity as pixels moved in previous update.
	 * @return	The vertical velocity of the mouse.
	 */
	float GetDeltaY ( bool asTime = true );
	/**
	 * The velocity of the mouse.
	 * @param	?asTime	If true then returns the velocity as pixels per second (extrapolated from the previous update), else returns velocity as pixels moved in previous update.
	 * @return	The velocity of the mouse.
	 */
	float GetSpeed ();
	/**
	 * Determine how long the mouse has been still.
	 * @param	?asTime	If true then returns duration as milliseconds, else returns duration as frame updates.
	 * @return	Returns the duration the mouse has been still.
	 */
	float GetStillDuration ( bool asTime = true );
	/**
	 * Determine if a specific mouse button was clicked twice (within the defined time).
	 * @param	?type	The mouse button.
	 * @param	?delay	The time within which the mouse button must be clicked twice.
	 * @return	Returns true if the mouse button was clicked twice (within the defined time).
	 */
	bool GetIsButtonDoubleClick ( EMouseButton type, float delay = 0.1f );
	/**
	 * Determine if the mouse is being dragged with a specific mouse button down (for at least the defined delay).
	 * @param	?type	The mouse button.
	 * @param	?delay	The time which, if exceeded, assumes the mouse is being dragged. 
	 * @return	Returns true if the mouse button was down for a duration exceeding delay.
	 */
	bool GetIsButtonDrag ( EMouseButton type, float delay = 0.1f );
	/**
	 * Determine if a specific mouse button is currently down.
	 * @param	?type	The mouse button.
	 * @return	Returns true is the mouse button is currently down, false otherwise.
	 */
	bool GetIsButtonDown ( EMouseButton type );
	/**
	 * Determine if a specific mouse button was pressed in the current update frame.
	 * <p>A press is defined as a new down - i.e. was up previous frame, and is down this frame.</p>
	 * @param	type	The mouse button.
	 * @return	Returns true is the mouse button was pressed in the current update, false otherwise.
	 */
	bool GetIsButtonPress ( EMouseButton type );
	/**
	 * Determine if a specific mouse button was released in the current update.
	 * <p>A release is defined as a new up - i.e. was down previous frame, and is up this frame.</p>
	 * @param	type	The mouse button.
	 * @return	Returns true is the mouse button was released in the current update, false otherwise.
	 */
	bool GetIsButtonRelease ( EMouseButton type );
	/**
	 * Determine the duration a specific mouse button is down.
	 * @param	?type	The mouse button.
	 * @param	?asTime	If true then returns duration as milliseconds, else returns duration as frame updates.
	 * @param	?isPrevious	If true then returns the previous duration down (the time held prior to the most recent release).
	 * @return	The duration a specific mouse button is down.
	 */
	float GetButtonDownDuration ( EMouseButton type, bool asTime = true, bool isPrevious = false );
	/**
	 * Determine the duration a specific mouse button is up.
	 * @param	?type	The mouse button.
	 * @param	?asTime	If true then returns duration as milliseconds, else returns duration as frame updates.
	 * @param	?isPrevious	If true then returns the previous duration up (the time unused prior to the most recent press).
	 * @return	The duration a specific mouse button is up.
	 */
	float GetButtonUpDuration ( EMouseButton type, bool asTime = true, bool isPrevious = false );
	/**
	 * Determine the horizontal movement of the mouse since a specific mouse button was pressed.
	 * @param	?type	The mouse button.
	 * @return	The horizontal movement of the mouse.
	 */
	float GetButtonDragWidth ( EMouseButton type );
	/**
	 * Determine the vertical movement of the mouse since a specific mouse button was pressed.
	 * @param	?type	The mouse button.
	 * @return	The vertical movement of the mouse.
	 */
	float GetButtonDragHeight ( EMouseButton type );
	/**
	 * Determine the horizontal position of the mouse when a specific mouse button was last clicked.
	 * @param	?type	The mouse button.
	 * @return	The horizontal position of the mouse.
	 */
	float GetButtonLastClickedX ( EMouseButton type );
	/**
	 * Determine the vertical position of the mouse when a specific mouse button was last clicked.
	 * @param	?type	The mouse button.
	 * @return	The vertical position of the mouse.
	 */
	float GetButtonLastClickedY ( EMouseButton type );
}