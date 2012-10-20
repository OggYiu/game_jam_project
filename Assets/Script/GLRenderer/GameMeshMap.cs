using UnityEngine;
using System.Collections;

public class GameMeshMap : MonoBehaviour
{
	public GameMapMesh[] meshes_ = null;
	public GameMapMesh gameMapMeshPrefab = null;
	
	public void GenerateMap() {
		this.transform.position = new Vector3( 0, 0, 0 );
		int meshArraySize = GameSettings.GetInstance().MAP_TILE_ROW_COUNT * GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT;
		meshes_ = new GameMapMesh[meshArraySize];
		
		Transform meshRoot = new GameObject().transform;
		meshRoot.parent = this.transform;
		meshRoot.name = "meshRoot";
		
		Vector3 targetPosition;
		GameMapMesh newMesh = null;
		for ( int i=0; i<GameSettings.GetInstance().MAP_TILE_ROW_COUNT; ++i ) {
			for ( int j=0 ; j<GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT; ++j ) {
				targetPosition = new Vector3(	j * GameSettings.GetInstance().TILE_SIZE - GameSettings.GetInstance().TILE_SIZE/2,
												i * GameSettings.GetInstance().TILE_SIZE - GameSettings.GetInstance().TILE_SIZE/2, 0 );
				newMesh = (GameMapMesh)Instantiate(gameMapMeshPrefab);
				newMesh.Init(targetPosition);
				newMesh.transform.parent = meshRoot.transform;
				newMesh.name = "mesh " + (i*GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT+j);
				meshes_[i*GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT + j] = newMesh;
			}
		}
	}
	
	public GameMapMesh GetMesh( int mapX, int mapY ) {
		int targetPos = GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT * mapY + mapX;
		if ( targetPos < 0 || targetPos >= meshes_.Length ) {
			Debug.LogError ( "<GameMeshMap::SetMeshColor>: invalid targetPos: " + targetPos + ", meshes_.Length: " + meshes_.Length );
			return null;
		}
		
		return meshes_[targetPos];
	}
}

