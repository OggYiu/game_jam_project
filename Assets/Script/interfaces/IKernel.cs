using UnityEngine;
using System.Collections;

/**
 * Handles main updates and provides global locators for all managers
 * @author	Robert Fell
 */
public interface IKernel : IPauseable
{
	/**
	 * Assets manager.
	 */
	IAssetManager Assets ();
	/**
	 * Audio manager.
	 */
	IAudioManager Audio ();
	/**
	 * Inputs manager.
	 */
	IInputManager Inputs ();
	/**
	 * Scene manager.  State machine containing IEntities.
	 */
	ISceneManager Scenes ();
	/**
	 * Messenger manager.  Arbitrator for observer pattern across IEntityCollections.
	 */
	IMessageManager Messenger ();
	/**
	 * Helper methods.
	 */
	ITools Tools ();
	/**
	 * Build properties and factory methods to create the application.
	 */
	IFactory Factory ();
	/**
	 * Used for read only application settings and localisation text.
	 * @param	id	The unique identifier for the config setting (e.g. XML node name).
	 * @return	Value of the corresponding config setting.
	 */
	object GetConfig ( string id );
	/**
	 * Request the framerate of the application.
	 * @param	?asActual	Use actual framerate (potentially laggy), or the desired framerate (from IFactory).
	 * @return	Frames per second.
	 */
	float GetFramerate ( bool asActual = true );
}
