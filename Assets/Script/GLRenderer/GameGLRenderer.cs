using UnityEngine;
using System.Collections.Generic;

public class GameGLRenderer : MonoBehaviour
{
	private Material rendererMaterial_ = null;
	private List<GameGLLines> lines_ = null;
	private List<GameGLTriangleStrip> triangleStrips_ = null;
	private List<GameGLQuads> quads_ = null;
	private GameObject gameGLPrimitiveRoot_ = null;
	[SerializeField] private GameGLLines gameGLLinesPrefab_ = null;
	[SerializeField] private GameGLTriangleStrip gameGLTriangleStripPrefab_ = null;
	[SerializeField] private GameGLQuads gameGLQuadsPrefab_ = null;
	
	static private GameGLRenderer s_instance = null;
	static public GameGLRenderer GetInstance() {
		if ( s_instance == null ) {
			s_instance = (GameGLRenderer)FindObjectOfType(typeof(GameGLRenderer));
        	
			if ( s_instance == null ) {
				Debug.LogError( "<GameGLRenderer::GetInstance>: GameGLRenderer not found" );
			} else {
				s_instance.Init();
			}
		}
		return s_instance;
	}
	
	// Use this for initialization
	//void Start ()
	private void Init()
	{
		gameGLPrimitiveRoot_ = new GameObject();
		gameGLPrimitiveRoot_.name = "gameGLPrimitiveRoot";
		
		lines_ = new List<GameGLLines>();
		triangleStrips_ = new List<GameGLTriangleStrip>();
		quads_ = new List<GameGLQuads>();
		
    	CreateLineMaterial();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	public GameGLLines AddLines(Vector3[] positions) {
		GameGLLines lines = (GameGLLines)Instantiate(gameGLLinesPrefab_);
		lines.transform.parent = gameGLPrimitiveRoot_.transform;
		lines.Init(positions);
		lines_.Add(lines);
		return lines_[lines_.Count - 1];
	}
	
	public GameGLTriangleStrip AddTriangleStrips(Vector3[] positions) {
		GameGLTriangleStrip triangles = (GameGLTriangleStrip)Instantiate(gameGLTriangleStripPrefab_);
		triangles.transform.parent = gameGLPrimitiveRoot_.transform;
		triangles.Init(positions);
		triangleStrips_.Add(triangles);
		return triangleStrips_[triangleStrips_.Count - 1];
	}
	
	public GameGLQuads AddQuads(Vector3[] positions) {
		GameGLQuads quads = (GameGLQuads)Instantiate(gameGLQuadsPrefab_);
		quads.transform.parent = gameGLPrimitiveRoot_.transform;
		quads.Init(positions);
		quads_.Add(quads);
		return quads_[quads_.Count - 1];
	}
	
	/*
    static private MeshMap s_instance = null;
	
    static public MeshMap GetInstance() { 
        if ( s_instance == null ) {
            GameObject go = new GameObject();
            s_instance = go.AddComponent<MeshMap>();
            go.name = "MeshMapSingleton";
        }

        return s_instance; 
    }
    */
    private void CreateLineMaterial() {
	    if( rendererMaterial_ == null ) {
	        rendererMaterial_ = new Material( "Shader \"Lines/Colored Blended\" {" +
	            "SubShader { Pass { " +
	            "    Blend SrcAlpha OneMinusSrcAlpha " +
	            "    ZWrite Off Cull Off Fog { Mode Off } " +
	            "    BindChannels {" +
	            "      Bind \"vertex\", vertex Bind \"color\", color }" +
	            "} } }" );
	        rendererMaterial_.hideFlags = HideFlags.HideAndDontSave;
	        rendererMaterial_.shader.hideFlags = HideFlags.HideAndDontSave;
	    }
	}
	
	public void OnPostRender() {
    	GL.PushMatrix();
    	// set the current material
    	rendererMaterial_.SetPass( 0 );
		
		if( lines_.Count > 0 ) {
			GL.Begin(GL.LINES);
			for ( int i=0; i<lines_.Count; ++i ) {
				lines_[i].Render();
			}
			GL.End();
		}
		
		if( triangleStrips_.Count > 0 ) {
			GL.Begin(GL.TRIANGLE_STRIP);
			for ( int i=0; i<triangleStrips_.Count; ++i ) {
				triangleStrips_[i].Render();
			}
			GL.End();
		}
		
		if( quads_.Count > 0 ) {
			GL.Begin(GL.QUADS);
			for ( int i=0; i<quads_.Count; ++i ) {
				quads_[i].Render();
			}
			GL.End();
		}
    	GL.PopMatrix();
	}
}

