using UnityEngine;
using System.Collections;

public class GameGLMap : MonoBehaviour
{
	protected GameGLRenderer glRenderer_ = null;
	const float QUAD_Z = -2;
	
	// Use this for initialization
	public void Init (GameGLRenderer glRenderer)
	{
		/*
		GameObject go = new GameObject();
		go.transform.parent = this.transform;
		glRenderer_ = go.AddComponent<GameGLRenderer>();
		glRenderer_.Init();
		*/
		
		//glRenderer_ = this.transform.GetComponent<GameGLRenderer>();
		glRenderer_ = glRenderer;
		if ( glRenderer_ == null ) {
			Debug.LogError ( "<GameGLMap::Start>: you should put GameGLMap with a GameGLRenderer!" );
			return ;
		}
		
		// init the quad map
		Vector3 targetPosition;
		
		for ( int i=0 ; i<GameSettings.GetInstance().MAP_TILE_ROW_COUNT; ++i ) {
			for ( int j=0 ; j<GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT; ++j ) {
				targetPosition = new Vector3( j*GameSettings.GetInstance().TILE_SIZE, i*GameSettings.GetInstance().TILE_SIZE, QUAD_Z );
				Vector3[] quadPositions = {	new Vector3(targetPosition.x - GameSettings.GetInstance().TILE_SIZE / 2, targetPosition.y - GameSettings.GetInstance().TILE_SIZE / 2, targetPosition.z),
											new Vector3(targetPosition.x + GameSettings.GetInstance().TILE_SIZE / 2, targetPosition.y - GameSettings.GetInstance().TILE_SIZE / 2, targetPosition.z),
											new Vector3(targetPosition.x + GameSettings.GetInstance().TILE_SIZE / 2, targetPosition.y + GameSettings.GetInstance().TILE_SIZE / 2, targetPosition.z),
											new Vector3(targetPosition.x - GameSettings.GetInstance().TILE_SIZE / 2, targetPosition.y + GameSettings.GetInstance().TILE_SIZE / 2, targetPosition.z) };
				glRenderer_.AddQuads(quadPositions);
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
}

