using UnityEngine;
using System.Collections;

/**
 * The ISceneManager should be implemented by objects intended to manage the IScene state machine.
 * <p>Only a single scene is active at any given update.  Which scene is configured by this manager.</p>
 * @author	Robert Fell
 */
public interface ISceneManager
{
	/**
	 * The currently active scene.
	 * <p>Use as a runtime property and not as an initialization property.</p>
	 */
	IScene Scene ();
	/**
	 * Sets the current scene to a new scene.
	 * @param	type	The new scene.
	 */
	void SetScene ( EScene type );
	/**
	 * Sets the current scene to the scene returned by IFactory.getBackSceneType().
	 * <p>The new scene should be representative of retreat.</p> 
	 * @see awe6.interfaces.IFactory.getBackSceneType
	 */
	void Back ();
	/**
	 * Sets the current scene to the scene returned by IFactory.getNextSceneType().
	 * <p>The new scene should be representative of progress.</p> 
	 * @see awe6.interfaces.IFactory.getNextSceneType
	 */
	void Next ();
	/**
	 * Restarts the current scene.
	 * <p>Equivalent of disposing current scene and then setScene to current scene again.</p>
	 */
	void Restart ();
}