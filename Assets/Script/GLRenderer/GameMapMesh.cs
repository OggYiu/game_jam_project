using UnityEngine;
using System.Collections;

public class GameMapMesh : MonoBehaviour
{
	private Mesh mesh_ = null;
	[SerializeField] private MeshRenderer meshRenderer_ = null;
	[SerializeField] private MeshFilter meshFilter_ = null;
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public void Init(Vector3 position) {
		this.transform.position = position;
		
		// create mesh
		mesh_ = CreatePlaneMesh();
		
		// create mesh renderer
		meshFilter_.mesh = mesh_;
	}
	
	private Mesh CreatePlaneMesh()
    {
		if ( mesh_ != null ) {
			Debug.LogError ( "<GameMapMesh::CreatePlaneMesh>: mesh already created!" );
			return null;
		}
		
        mesh_ = new Mesh();
		
		float meshSize = GameSettings.GetInstance().TILE_WIDTH;
        Vector3[] vertices = new Vector3[]
        {
            new Vector3( meshSize, meshSize, 0 ),
            new Vector3( meshSize, 0, 0 ),
            new Vector3( 0, meshSize, 0 ),
            new Vector3( 0, 0, 0),
        };

        Vector2[] uv = new Vector2[]
        {
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(0, 0),
        };
		
        Vector3[] normals = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 0),
        };

        int[] triangles = new int[]
        {
            0, 1, 2,
            2, 1, 3,
        };

        mesh_.vertices = vertices;
        mesh_.uv = uv;
        mesh_.triangles = triangles;
        mesh_.normals = normals;
		//mesh_.normals = 

        return mesh_;
    }
}

