using UnityEngine;
using System.Collections;

public class GameGLQuads : GameGLPrimitive
{
	private Vector3[] positions;
	
	public void Init( Vector3[] vPositions ) {
		if ( vPositions.Length <= 0 || (vPositions.Length % 4) != 0 ) {
			Debug.LogError ( "<GameGLLines::ctor>: Invalid position length: " + vPositions.Length + ", it must be not zero and it must be %4 == 0!" );
			return ;
		}
		positions = vPositions;
	}
	
	override public void Render() {
	    GL.Color( color_ );
		Vector3 targetPos;
		for ( int i=0; i<positions.Length; ++i ) {
			targetPos = positions[i];
			GL.Vertex3(	this.transform.position.x + targetPos.x,
						this.transform.position.y + targetPos.y,
						targetPos.z );
		}
	}
}

