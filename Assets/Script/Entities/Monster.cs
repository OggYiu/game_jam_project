using UnityEngine;
using System.Collections;

public class Monster : GameActor
{
	protected override void _Initer (Hashtable args)
	{
		base._Initer (args);
		type_ = ActorType.monster;
	}
	
	protected override void _Resolver (Hashtable args)
	{
		base._Resolver (args);
	}
	
	protected override void _Thinker ()
	{
		base._Thinker ();
		
		// check if he see 
	}
}