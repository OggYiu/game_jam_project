using UnityEngine;
using System.Collections;

public class GameGLTriangleStrip : GameGLPrimitive
{
	private Vector3[] positions;
	
	public void Init( Vector3[] vPositions ) {
		if ( vPositions.Length < 3  ) {
			Debug.LogError ( "<GameGLLines::ctor>: Invalid position length: " + vPositions.Length + ", it must be larger than 2!" );
			return ;
		}
		positions = vPositions;
	}
	
	override public void Render() {
	    GL.Color( color_ );
		Vector3 targetPos;
		for ( int i=0; i<positions.Length; ++i ) {
			targetPos = positions[i];
			GL.Vertex3( targetPos.x, targetPos.y, targetPos.z );
		}
	}
}

