using UnityEngine;
using System.Collections;

public class GameApp : MonoBehaviour {
	[SerializeField] Camera main_camera_ = null;
	[SerializeField] Material gl_material_ = null;
	[SerializeField] Vector3 last_mouse_pos_;
	
	// Use this for initialization
	void Start () {
		if ( main_camera_ == null ) {
			Debug.LogError ( "<GameApp::Start>, invalid main_camera_" );
		}
		
	    if ( gl_material_ == null ) {
			Debug.LogError ( "<GameApp::Start>, invalid gl_material_" );
	    }
	}
	
	// Update is called once per frame
	void Update () {
		if ( Input.GetMouseButtonDown ( 0 ) ) {
			last_mouse_pos_ = Input.mousePosition;
			RaycastHit hit;
		    Ray ray = main_camera_.ScreenPointToRay ( Input.mousePosition );
		    if ( Physics.Raycast ( ray, out hit, 100 ) ){
				IEntity target_entity = (IEntity)hit.collider.gameObject.GetComponent ( typeof ( IEntity ) );
				if ( target_entity != null ) {
					target_entity.OnClicked ();
				}
			}
		}
		
//    	Debug.DrawLine ( new Vector3 (100, 50, 1), new Vector3 (100, 100, 1), Color.red );
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
}
