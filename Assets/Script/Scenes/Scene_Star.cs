using UnityEngine;
using System.Collections;

public class Scene_Star : Scene {
	
	public GameObject star;
	public GameObject star_background;
	public GameObject game_camera;
	
	float upper_part_sum = 0;
	float lower_part_sum = 0;
	float total_sum = 0;
	
	float acceleration = 1.01f;
	float speed = 1.0f;
	
	float end_position = 1800;
	
	private Vector3 mouse_last_position = Vector3.zero;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		total_sum += Mathf.Abs(768 / 2);
		if (star.transform.position.y > 0)
		{
			upper_part_sum += Mathf.Abs(star.transform.position.y);
		}
		else
		{
			lower_part_sum += Mathf.Abs(star.transform.position.y);
		}
		
		if (star != null && star.transform.position.x < end_position)
		{
			speed *= acceleration;
			star.transform.position = new Vector3(star.transform.position.x + speed, star.transform.position.y, star.transform.position.z);
			
			
			//-----------------------------------------------
			GameObject prefab = Resources.Load("Prefabs/StarTail", typeof(GameObject)) as GameObject;
			GameObject startail = ((GameObject)GameObject.Instantiate(prefab));
			startail.transform.position = star.transform.position;
			//-----------------------------------------------
			
			
			if (game_camera != null && game_camera.transform.position.x < end_position - 450)
			{
				game_camera.transform.position = new Vector3(game_camera.transform.position.x + (speed * 0.65f), game_camera.transform.position.y, game_camera.transform.position.z);
			}
		}
	
	}
	
	override public void MousePositionUpdateHandler ( Vector3 mousepos ) 
	{	
		if (mouse_last_position != Vector3.zero)
		{
			float y_diff = acceleration * (mouse_last_position.y - mousepos.y) / 1.5f * -1;
			
			if (star != null && star.transform.position.x < end_position)
			{
				star.transform.position = new Vector3(star.transform.position.x, star.transform.position.y + y_diff, star.transform.position.z);
			}
			else
			{
				
				float mountain_percentage = upper_part_sum / total_sum;
				float forest_sand_percentage = lower_part_sum / total_sum;
				int a = 0;
				
			}
		}
		
		mouse_last_position = mousepos;
	}
	
	override public void MouseButtonDownHandler ( int button_index ) 
	{
		
	}
}
