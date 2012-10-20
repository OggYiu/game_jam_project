using UnityEngine;
using System.Collections;

public class GameGLPrimitive : MonoBehaviour
{
	protected Color color_;
	
	virtual public void Render() {
		Debug.LogError ( "<GameGLPrimitive::Render>: Please implement this method!" );
	}
	
	public void SetColor(Color color) {
		color_ = color;
	}
}

