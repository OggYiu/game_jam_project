using UnityEngine;
using System.Collections;

/**
 * The AAssetManager class provides a minimalist implementation of the IAssetManager interface.
 * <p>It is intended as an abstract class to be extended.</p>
 * <p>For API documentation please review the corresponding Interfaces.</p>
 * @author	Robert Fell
 */
public class AssetManager : Process, IAssetManagerProcess
{
	public AssetManager ( IKernel kernel ) : base ( kernel ) {
	}
	
	override protected void _Init()
	{
		base._Init();
	}

	public object GetAsset ( string p_id )
	{
		return Resources.Load ( p_id );
	}
}