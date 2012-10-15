using UnityEngine;
using System.Collections;
/**
 * The IProcess represents the smallest atom of the awe6 framework.
 * <p>Many managers will implement this interface.</p>
 * @author	Robert Fell
 */
public interface IProcess : IUpdateable, IDisposable, IPauseable
{
}