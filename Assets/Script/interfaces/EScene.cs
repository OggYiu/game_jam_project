using UnityEngine;
using System.Collections;

/**
 * Default Scene types.  A basic game can be made using these defaults.
 * <p>Can be extended with SubType by using concrete project values.</p> 
 * @author	Robert Fell
 */

public enum EScene
{
	START,
	EMPTY,
	INTRO,
	MENU,
	GAME,
	/**
	 * Recommended to be used as a testing sandbox to test new entities etc.
	 */
	TEST,
}