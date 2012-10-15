using UnityEngine;
using System.Collections;

public class CharacterEntity : Entity
{
	float speed_ = 10;
	protected override void _Resolver (Hashtable args)
	{
		base._Resolver (args);
		
		if ( args != null ) {
			if ( args.ContainsKey ( "speed" ) ) {
				speed_ = (float)args["speed"];
			}
		}
	}
	
	protected override void _Updater ( float deltaTime = 0 )
	{
		base._Updater (deltaTime);
		
		this.transform.localPosition = new Vector3 ( this.transform.localPosition.x + deltaTime * speed_, this.transform.localPosition.y, this.transform.localPosition.z );
	}
}

