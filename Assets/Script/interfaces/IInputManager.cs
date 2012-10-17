using UnityEngine;
using System.Collections;

/**
 * The IInputManager interface should be implemented by an object wishing to provide user input states to the kernel.
 * <p>The state machine represents the configuration of the input devices at any specific update frame.</p>
 * <p>State based input is useful for many types of game mechanics, including: momentum, instant replays and special move combos.</p>
 * @author	Robert Fell
 */
public interface IInputManager : IResettable
{
	/**
	 * The virtual keyboard user input: every key on the keyboard.
	 */
	IInputKeyboard GetKeyboard ();
	/**
	 * The virtual mouse user input: 3 button mouse and scroll wheel.
	 */
	IInputMouse GetMouse ();
}