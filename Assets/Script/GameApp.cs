using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameApp : MonoBehaviour {
	[SerializeField] Camera main_camera_ = null;
	[SerializeField] Material gl_material_ = null;
	Vector3 last_mouse_pos_ = Vector3.zero;
	bool is_started_ = false;
	bool is_inited_ = false;
//	NavGraph nav_graph_ = null;
	
	// Use this for initialization
	void Start () {
		if ( main_camera_ == null ) {
			Debug.LogError ( "<GameApp::Start>, invalid main_camera_" );
		}
		
	    if ( gl_material_ == null ) {
			Debug.LogError ( "<GameApp::Start>, invalid gl_material_" );
	    }
		
		is_started_ = true;
		
//		GameGLRenderer.GetInstance();
		
//		AudioSource audio_source = null;
//		audio_source.spread = 
	}
	
	void Resolved () {
		//init map
//		nav_graph_ = new NavGraph ();
		
//		Vector2 currentPosition = Vector2.zero;
//		int nextValidNodeIndex = 0;
//		NavGraphNode newNode = null;
//		NavGraphNode westNode = null;
//		NavGraphNode sourthNode = null;
//		
//		for ( int i=0; i<GameSettings.GetInstance().MAP_TILE_ROW_COUNT; ++i ) {
//			for ( int j=0; j<GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT; ++j ) {
//				currentPosition = new Vector2(j, i);
//				newNode = new NavGraphNode(nextValidNodeIndex++, currentPosition );
//				// create the current node
//				nav_graph_.AddNode( newNode );
//				
//				// find the west node, connect to it if it is existed
//				currentPosition.x -= 1;
//				westNode = nav_graph_.GetNodeWithPos(currentPosition);
//				if ( westNode != null ) {
//					nav_graph_.AddEdge( new NavGraphEdge( newNode.Index(), westNode.Index(), 1.0f ) );
//					nav_graph_.AddEdge( new NavGraphEdge( westNode.Index(), newNode.Index(), 1.0f ) );
//				}
//				currentPosition.x += 1;
//				
//				// find the sourth node, connect to it if it is existed
//				currentPosition.y -= 1;
//				sourthNode = nav_graph_.GetNodeWithPos(currentPosition);
//				if ( sourthNode != null ) {
//					nav_graph_.AddEdge( new NavGraphEdge( newNode.Index(), sourthNode.Index(), 1.0f ) );
//					nav_graph_.AddEdge( new NavGraphEdge( sourthNode.Index(), newNode.Index(), 1.0f ) );
//				}
//				currentPosition.y += 1;
//			}
//		}
	}
	
	// Update is called once per frame
	void Update () {
		if ( is_started_ && !is_inited_ ) {
			is_inited_ = true;
			Resolved ();
		}
		
		SceneManager.GetInstance().OnUpdate();
		
		SceneManager.GetInstance().MousePositionUpdateHandler ( Input.mousePosition );
		if ( Input.GetMouseButtonDown ( 0 ) ) {
			SceneManager.GetInstance().MouseButtonDownHandler ( 0 );
		}
		if ( Input.GetMouseButtonDown ( 1 ) ) {
			SceneManager.GetInstance().MouseButtonDownHandler ( 1 );
//			last_mouse_pos_ = Input.mousePosition;
//			RaycastHit hit;
//		    Ray ray = main_camera_.ScreenPointToRay ( Input.mousePosition );
//		    if ( Physics.Raycast ( ray, out hit, 100 ) ){
//				Entity target_entity = (Entity)hit.collider.gameObject.GetComponent ( typeof ( Entity ) );
//				if ( target_entity != null ) {
//					target_entity.OnClicked ();
//				}
//			}
		}
//		NavGraph graph = nav_graph_;
//		if ( graph != null ) {
//			List<NavGraphNode> nodes = graph.GetNodes();
//			NavGraphNode currentNode = null;
//			Vector3 realPosition = Vector3.zero;
//			const int nodeSize = 5;
//			for ( int i=0; i<nodes.Count; ++i ) {
//				currentNode = nodes[i];
//				//Debug.Log ( "node[" + i + "], id: " + currentNode.GetIndex() + ", pos: " + currentNode.Position().x + ", " + currentNode.Position().y );
//				realPosition = GameUtils.Map2RealPos(currentNode.Position());
//				Debug.DrawLine(	new Vector3(realPosition.x - nodeSize, realPosition.y - nodeSize, 0),
//								new Vector3(realPosition.x + nodeSize, realPosition.y + nodeSize, 0),
//								Color.white);
//				Debug.DrawLine(	new Vector3(realPosition.x - nodeSize, realPosition.y + nodeSize, 0),
//								new Vector3(realPosition.x + nodeSize, realPosition.y - nodeSize, 0),
//								Color.white);
//			}
//			
//			List< List<NavGraphEdge> > edges = graph.GetEdgeListList();
//			List<NavGraphEdge> targetEdges = null;
//			NavGraphEdge currentEdge = null;
//			NavGraphNode fromNode = null;
//			NavGraphNode toNode = null;
//			for ( int i=0; i<edges.Count; ++i ) {
//				targetEdges = edges[i];
//				for ( int j=0; j<targetEdges.Count; ++j ) {
//					currentEdge = targetEdges[j];
//					fromNode = graph.GetNode(currentEdge.From());
//					toNode = graph.GetNode(currentEdge.To());
//					
//					Vector3 lineFrom = GameUtils.Map2RealPos(fromNode.Position());
//					Vector3 lineTo = GameUtils.Map2RealPos(toNode.Position());
//					Debug.DrawLine(lineFrom, lineTo, Color.red);
//				}
//			}
//		}
	}
	
	void OnPostRender() {
	    GL.PushMatrix();
	    gl_material_.SetPass(0);
	    GL.LoadOrtho();
	    GL.Begin(GL.LINES);
	    GL.Color(Color.red);
		GL.Vertex3 ( 0, 0, 0 );
		GL.Vertex3 ( last_mouse_pos_.x / Screen.width, last_mouse_pos_.y / Screen.height, 0 );
	    GL.End();
	    GL.PopMatrix();
	}
	
    private static GameApp s_instance;
    public static GameApp GetInstance() {
        if ( !s_instance ) {
            s_instance = (GameApp)GameObject.FindObjectOfType ( typeof ( GameApp ) );
            if ( !s_instance )
                Debug.LogError("There needs to be one active MyClass script on a GameObject in your scene.");
        }
        return s_instance;
    }
	
	public Camera main_camera {
		set {}
		get { return main_camera_; }
	}
}
