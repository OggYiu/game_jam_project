using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {
	
	private float last_update_time = 0;
	private int current_frame = 0;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{	
		last_update_time += Time.deltaTime;
		
		if (last_update_time > 0.08)
		{
			Texture2D p = new Texture2D(0, 0);
			p = Resources.Load("Texture/star_0" + (current_frame+1).ToString(), typeof(Texture2D)) as Texture2D;
			this.renderer.material.mainTexture = p;
			
			last_update_time = 0;
			current_frame ++;
			
			if (current_frame >= 6)
			{
				current_frame = 0;
			}
		}
	}
}
