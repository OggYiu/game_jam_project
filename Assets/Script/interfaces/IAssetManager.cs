using UnityEngine;
using System.Collections;

/**
 * Provides functions to interact with media assets embedded or loaded in the application.
 * <p>Use with caution, there are usually more type safe ways to utilise assets.</p>
 * @author	Robert Fell
 */
public interface IAssetManager
{
	/**
	 * Request an embedded or loaded media asset.  E.g. bitmap or sound.
	 * @param	id	The uniqie reference of the requested asset.  E.g. className.
	 * @param	?packageId	The package of the requested asset.  Will default to "assets" if not provided.
	 * @param	?args	Some assets may require additional arguments, provide them here.
	 * @return	The asset - can be of any type for type inference (or cast as appropriate).
	 */
	object GetAsset ( string id );
}