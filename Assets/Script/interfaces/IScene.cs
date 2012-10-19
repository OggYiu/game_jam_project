using UnityEngine;
using System.Collections;

/**
 * The IScene interface should be implemented by objects intending to represent scene states in the ISceneManager.
 * <p>Scenes represent the larger building blocks of the awe6 concept, and contain Entities which do the work.</p> 
 * @author	Robert Fell
 */
public interface IScene : IProcess, IEntityCollection, IViewable
{
	/**
	 * The type of this scene.
	 */
	EScene Type ();
	/**
	 * Sets whether the scene is disposed when no longer the active scene.  In most cases this should be true.
	 */
	bool IsDisposable ();
	/**
	 * Sets whether the pause button is displayed / active in the overlay.
	 */
	bool IsPauseable ();
}