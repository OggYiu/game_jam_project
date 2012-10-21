using UnityEngine;
using System.Collections;

public class Boom : MonoBehaviour {
	
	private float last_update_time = 0;
	private int current_frame = 1;
	private bool start_animation = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	public void StartAnimation()
	{
		start_animation = true;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (start_animation)
		{
			last_update_time += Time.deltaTime;
			
			if (last_update_time > 0.1)
			{
				Texture2D p = new Texture2D(0, 0);
				p = Resources.Load("Texture/bow_0" + (current_frame+1).ToString(), typeof(Texture2D)) as Texture2D;
				this.renderer.material.mainTexture = p;
				
				last_update_time = 0;
				current_frame ++;
				
				if (current_frame >= 11)
				{
					start_animation = false;
					Application.LoadLevel("Yiu");
				}
			}
		}
	}
}
