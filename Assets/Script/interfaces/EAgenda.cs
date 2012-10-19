using UnityEngine;
using System.Collections;

/**
 * Can be used to influence the internal state of an IEntity via IAgendaManager.
 * <p>Can be extended with SubType by using concrete project values.</p> 
 * @author	Robert Fell
 */
public enum EAgenda
{
	/**
	 * The default EAgenda.  Anything assigned to this will be run each update irrespective of what agenda the parent is assigned. 
	 */
	ALWAYS,
	BIRTH,
	DEATH,
	STANDARD,
	ATTACK,
	DEFEND,
}