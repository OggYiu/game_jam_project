using UnityEngine;
using System.Collections;

public class Base_Tile : MonoBehaviour {
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	public void init(string texture_path)
	{
		Texture2D p = new Texture2D(0, 0);
		//p = Resources.Load("Texture/TileMap/rock_a", typeof(Texture2D)) as Texture2D;
		p = Resources.Load(texture_path, typeof(Texture2D)) as Texture2D;
		this.renderer.material.mainTexture = p;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
