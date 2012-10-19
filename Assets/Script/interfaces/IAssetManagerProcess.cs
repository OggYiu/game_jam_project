using UnityEngine;
using System.Collections;

/**
 * The IAssetManagerProcess interface should be implemented by objects representing an operating IAssetManager.
 * <p>These extra interface requirements are required for internal workings, but are not exposed via the minimal IAssetManager interface.</p>
 * @author	Robert Fell
 */
public interface IAssetManagerProcess : IAssetManager, IProcess
{
}