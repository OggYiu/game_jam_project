using UnityEngine;
using System.Collections;

	
/**
 * The Process class provides a minimalist implementation of the IProcess interface.
 * <p>For API documentation please review the corresponding Interfaces.</p>
 * @author	Robert Fell
 */
public class Process : MonoBehaviour, IProcess {
	float age_ = 0;
	float updates_ = 0;
	protected bool is_active_ = true;
	protected bool is_disposed_ = false;
	bool is_started_ = false;
	bool is_resolved_ = false;
	Hashtable init_args_ = null;
	
	static Hashtable Hash(params object[] args) {
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
	
	void Start () {
		is_started_ = true;
	}
	
	public void Init ( params object[] args ) {
		if ( args == null || args[0] == null ) {
			init_args_ = null;
		} else {
			init_args_ = Process.Hash ( args );
			init_args_ = Process.CleanArgs ( init_args_ );
		}
	}
	
	virtual protected void _Resolver ( Hashtable args ) {
	}
	
	public bool isActive () {
		return is_active_;
	}
	
	public bool isDisposed () {
		return is_disposed_;
	}
	
	public void Dispose () {
		if ( isDisposed () ) {
			Debug.LogError ( "<Entity::Dispose> entity already disposed!" );
			return ;
		}
		
		is_disposed_ = true;
		is_active_ = false;
		
		_Disposer();
	}
	
	virtual protected void _Disposer () {
		// override me
	}
	
	public float GetAge ( bool asTime = true ) {
		if ( asTime ) {
			return age_;
		} else {
			return updates_;
		}
	}
	
	public void OnUpdate( float deltaTime = 0 )
	{
		if ( !is_active_ || is_disposed_ || !is_started_ )
		{
			return;
		}
		else
		{
			if ( !is_resolved_ ) {
				is_resolved_ = true;
				_Resolver ( init_args_ );
			}
			
			age_ += deltaTime;
			updates_++;
			_Updater ( deltaTime );
			return;
		}
	}
	
	virtual protected void _Updater( float deltaTime = 0 )
	{
		// override me
	}
	
	public void SetActive( bool val )
	{
		if ( is_disposed_ )
		{
			is_active_ = false;
			return;
		}
		if ( val != is_active_ )
		{
			if ( val ) {
				Resume();
			} else {
				Pause();
			}
		}
	}
	
	public void Pause()
	{
		if ( !is_active_ || is_disposed_ )
		{
			return;
		}
		else
		{
			_Pauser();
			
			is_active_ = false;
//			if ( _isEntity )
//			{
//				_kernel.messenger.sendMessage( EMessage.PAUSE, cast this, true );
//			}
		}
	}
	
	virtual protected void _Pauser()
	{
		// override me
	}
	
	public void Resume()
	{
		if ( is_active_ || is_disposed_ )
		{
			return;
		}
		else
		{
			_Resumer();
	//		Reflect.setField( this, "isActive", true ); // avoids the setter
			is_active_ = true;
//			if ( _isEntity )
//			{
//				_kernel.messenger.sendMessage( EMessage.RESUME, cast this, true );
//			}
			return;
		}
	}
	
	virtual protected void _Resumer()
	{
		// override me
	}
	
}

