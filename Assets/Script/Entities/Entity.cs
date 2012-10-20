using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity : MonoBehaviour {
	string entity_id_ = "";
	protected bool is_disposed_ = false;
//	bool is_started_ = false;
	bool is_resolved_ = false;
	Hashtable init_args_ = null;
	
//	void Start () {
//		is_started_ = true;
//	}
	
	virtual protected void _Resolver ( Hashtable args ) {
		entity_id_ = System.Guid.NewGuid().ToString();
	}
	
	public string GetID () {
		return entity_id_;
	}
	
	public void Init ( params object[] args ) {
		if ( args == null || args.Length <= 0 || args[0] == null ) {
			init_args_ = null;
		} else {
			init_args_ =  Hash ( args );
//			init_args_ = CleanArgs ( init_args_ );
		}
		
		_Initer ( init_args_ );
	}
	
	virtual protected void _Initer ( Hashtable args ) {
	}
	
	virtual protected void OnClicked () {
		Debug.Log ( "<Entity::OnClicked> entity " + this.gameObject.name + " on clicked!" );
	}
	
	public void Dispose () {
		_Disposer ();
	}
	
	public void OnUpdate ( float deltaTime = 0 ) {
//		if ( !is_started_ ) {
//			return ;
//		}
		
//		if ( is_started_ && !is_resolved_ ) {
		if ( !is_resolved_ ) {
			is_resolved_ = true;
			_Resolver ( init_args_ );
		}
		
		_Updater ( deltaTime );
	}
	
	virtual protected void _Updater ( float deltaTime = 0 ) {
	}
	
	virtual protected void _Disposer () {
	}
	
	public static Hashtable Hash(params object[] args){
		Hashtable hashTable = new Hashtable(args.Length/2);
		if (args.Length %2 != 0) {
			Debug.LogError("Tween Error: Hash requires an even number of arguments!"); 
			return null;
		}else{
			int i = 0;
			while(i < args.Length - 1) {
				hashTable.Add(args[i], args[i+1]);
				i += 2;
			}
			return hashTable;
		}
	}	
	
	static Hashtable CleanArgs(Hashtable args){
		Hashtable argsCopy = new Hashtable(args.Count);
		Hashtable argsCaseUnified = new Hashtable(args.Count);
		
		foreach (DictionaryEntry item in args) {
			argsCopy.Add(item.Key, item.Value);
		}
		
		foreach (DictionaryEntry item in argsCopy) {
			if(item.Value.GetType() == typeof(System.Int32)){
				int original = (int)item.Value;
				float casted = (float)original;
				args[item.Key] = casted;
			}
			if(item.Value.GetType() == typeof(System.Double)){
				double original = (double)item.Value;
				float casted = (float)original;
				args[item.Key] = casted;
			}
		}	
		
		//unify parameter case:
		foreach (DictionaryEntry item in args) {
			argsCaseUnified.Add(item.Key.ToString().ToLower(), item.Value);
		}	
		
		//swap back case unification:
		args = argsCaseUnified;
				
		return args;
	}	
	
}
