using UnityEngine;
using System.Collections;

/**
 * The IFactory interface should be implemented by objects designed to populate an awe6 implementation.
 * <p>The IFactory represents the blueprint and builder for all project specific classes.</p>
 * @author Robert Fell
 */
public interface IFactory
{
	/**
	 * The unique identifier for this specific project.  <=16 chars, no spaces.
	 */
	string ID ();
	/**
	 * The current version of this specific project.  Suggestion: major.minor.revision - e.g. 1.2.345
	 */
	string Version ();
	/**
	 * The author or this specific project.
	 */
	string Author ();
	/**
	 * A convenient switch to allow debug modes or verbose output in your code.  Adjust as needed.
	 */
	bool IsDebug ();
	/**
	 * The horizontal width of this application's bounding rectangle.
	 */
	int Width ();
	/**
	 * The vertical height of this application's bounding rectangle.
	 */
	int Height ();
	/**
	 * Dictionary of values.  Can be used to load initial configuration settings or store global variables.
	 */
	Hashtable Config ();
	/**
	 * The scene which is displayed first.  The application starts here.
	 */
	EScene StartingSceneType ();
	/**
	 * The default key used in this application to pause updates.
	 */
	KeyCode KeyPause ();
	/**
	 * The default key used in this application to mute the audio.
	 */
	KeyCode KeyMute ();
	/**
	 * The default key used in this application to back out of the current scene.
	 */
	KeyCode KeyBack ();
	/**
	 * The default key used in this application to advance to the next scene.
	 */
	KeyCode KeyNext ();
	/**
	 * The default key used in this application to do a special action (determined by the specific application).
	 */
	KeyCode KeySpecial ();
	/**
	 * Called by the kernel to complete initialization (due to both requiring an initialized instance of each).
	 * @param	kernel	An intialized kernel offering services to the factory.
	 */
	void OnInitComplete ( IKernel kernel );
	/**
	 * Builds the application's asset manager which store images, sounds etc.
	 * @return	Asset manager.
	 */
	IAssetManagerProcess CreateAssetManager();
	/**
	 * Builds an empty Entity for injection.
	 * @param	?id	The unique identifier of this entity.
	 * @return	An empty Entity.
	 */
	IEntity CreateEntity (object id );
	/**
	 * Builds the application's scenes which contain specific functionality.
	 * @param	type	The type of scene.
	 * @return	Scene which contain specific functionality.
	 */
	IScene CreateScene ( EScene type );
	/**
	 * When a scene is backed out of it will be replaced by the scene returned here.
	 * @param	type	Type of scene to back out from.
	 * @return	Scene type to back out to.
	 */
	EScene GetBackSceneType ( EScene type );
	/**
	 * When a scene requests the next scene it will be replaced by the scene returned here.
	 * @param	type	Type of scene to advance from.
	 * @return	Scene type to advance to next.
	 */
	EScene GetNextSceneType ( EScene type );
}
