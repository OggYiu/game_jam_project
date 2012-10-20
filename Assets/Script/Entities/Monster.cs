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
		
		bool processed = false;
		// see if you can move
		if ( false ) {
			processed = true;
		}
		
		int row = -1;
		int column = -1;
//		if ( !processed && NavigationMap.GetInstance().GetNearestHumanForMonster ( this ) ) {
//		}
	}
}