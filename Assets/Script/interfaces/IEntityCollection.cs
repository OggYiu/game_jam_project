using UnityEngine;
using System.Collections;

/**
 * The IEntityCollection interface should be implemented by objects which compose multiple entities.
 * @author Robert Fell
 */
public interface IEntityCollection
{
	/**
	 * Adds an entity to this object's children.
	 */
	void AddEntity ( Entity entity );
	
	/**
	 * Removes an entity from this object's children.
	 */
	void RemoveEntity ( Entity entity );
	
	/**
	 * Retrieves the child entity with the specified id. 
	 */
	Entity GetEntityById ( string id );
}