using UnityEngine;
using System.Collections;

public class Factory : MonoBehaviour
{
	public static Entity CreateEntity ( string id ) {
		Resources.Load ( id );
		return null;
	}
}