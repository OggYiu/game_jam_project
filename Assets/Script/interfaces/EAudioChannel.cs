using UnityEngine;
using System.Collections;

/**
 * Can be used to separate audio space to allow different transforms to apply to different groups of playing sounds.
 * <p>Can be extended with SubType by using concrete project values.</p> 
 * @author	Robert Fell
 */

public enum EAudioChannel
{
	DEFAULT,
	EFFECTS,
	INTERFACE,
	MUSIC,
}