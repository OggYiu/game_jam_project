using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneManager : MonoBehaviour {
	[SerializeField] string start_page_id_ = "";
	string page_resource_path_ = "ScenePrefab";
	Scene cur_scene_ = null;
	bool is_resolved_ = false;
	bool is_started_ = false;
	string lazy_target_page_id_ = "";
	Hashtable lazy_target_args_ = null;
	public GameObject parent;
	
//	Dictionary<string, int> page_registry_ = new Dictionary<string, int>();
	void Start () {
		is_started_ = true;
	}
	
	public void Init () {
		ChangeScene ( start_page_id_, null );
	}
	
	void Update() {
		if ( !is_started_ ) {
			return;
		}
		
		if ( !is_resolved_ ) {
			is_resolved_ = true;
			Init ();
		}
		
		if ( lazy_target_page_id_.Length > 0 ) {
			ChangeScene_ ( lazy_target_page_id_, lazy_target_args_ );
			lazy_target_page_id_ = "";
			lazy_target_args_ = null;
		}
		
		if ( cur_scene_ != null ) {
			cur_scene_.OnUpdate ( Time.deltaTime );
		}
	}
	
	void ChangeScene_ ( string pageid, Hashtable args ) {
		if ( cur_scene_ != null ) {
			cur_scene_.Dispose();
			DestroyObject ( cur_scene_.gameObject );
			cur_scene_ = null;
		}
		
		string resource_path = page_resource_path_ + "/" + pageid;
		Object page_object = Resources.Load ( resource_path );
		if ( page_object == null ) {
            Debug.LogError("<SceneManager::ChangeScene> scene resource not found at " + resource_path );
		} else {
			cur_scene_ = ( (GameObject)Object.Instantiate ( page_object ) ).GetComponent<Scene>();
			cur_scene_.Init ( args );
		}
	}
	
	public void ChangeScene ( string pageid, Hashtable args = null ) {
		if ( pageid == null || pageid.Length <= 0 ) {
            Debug.LogError("<SceneManager::ChangeScene> invalid game page");
			return ;
		}
		
		lazy_target_page_id_ = pageid;
		lazy_target_args_ = args;
	}
	
    private static SceneManager s_instance;
    public static SceneManager GetInstance() {
        if ( !s_instance ) {
            s_instance = (SceneManager)GameObject.FindObjectOfType ( typeof ( SceneManager ) );
            if ( !s_instance )
                Debug.LogError("There needs to be one active MyClass script on a GameObject in your scene.");
			else 
				s_instance.Init ();
        }
        return s_instance;
    }
	
//	public void RegisterPage_ ( string page_prefab_name ) {
//		if ( page_registry_
//		page_registry_.Add ( page_prefab_name, index );
//	}
}

