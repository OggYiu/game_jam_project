using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneManager : Entity {
	[SerializeField] GameObject entities_parent_ = null;
	[SerializeField] string start_page_id_ = "";
	
	Scene cur_scene_ = null;
	string lazy_target_page_id_ = "";
	Hashtable lazy_target_args_ = null;
	string page_resource_path_ = "Prefabs";
	
	protected override void _Resolver (Hashtable args)
	{
		base._Resolver (args);
		ChangeScene ( start_page_id_, null );
	}
	
	protected override void _Updater (float deltaTime)
	{
		base._Updater (deltaTime);
		
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
        }
        return s_instance;
    }
	
	public GameObject entities_parent {
		get { return entities_parent_; }
	}
	
	public void MouseButtonDownHandler ( int button_index ) {
		if ( cur_scene_ != null ) {
			cur_scene_.MouseButtonDownHandler ( button_index );
		}
	}
	
	public void MousePositionUpdateHandler ( Vector3 mousepos ) {
		if ( cur_scene_ != null ) {
			cur_scene_.MousePositionUpdateHandler ( mousepos );
		}
	}
	
	public Scene cur_scene {
		set {}
		get { return cur_scene_; }
	}
//	public void RegisterPage_ ( string page_prefab_name ) {
//		if ( page_registry_
//		page_registry_.Add ( page_prefab_name, index );
//	}
}

