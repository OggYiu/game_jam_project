using UnityEngine;
using System.Collections;

public class Monster : GameActor
{
	protected override void _Resolver (Hashtable args)
	{
		type_ = ActorType.monster;
		base._Resolver (args);
	}
	
	protected override void _Thinker ()
	{
		base._Thinker ();
		
		// check if he see 
	}
}