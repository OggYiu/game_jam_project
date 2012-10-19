using UnityEngine;
using System.Collections;

/**
 * The AFactory class provides a minimalist abstract implementation of the IFactory interface.
 * <p>It is intended as an abstract class to be extended by target specific drivers.</p>
 * <p>For API documentation please review the corresponding Interfaces.</p>
 * @author	Robert Fell
 */
class Factory : IFactory, IDisposable
{
	bool _isDisposed;
	GameObject _context;
	Hashtable _config;
	IKernel _kernel;
	Kernel _concreteKernel;
	ITools _tools;
	bool _isDebug;
	string _id;
	string _version;
	string _author;
	EScene _startingSceneType;
	KeyCode _keyPause;
	KeyCode _keyMute;
	KeyCode _keyBack;
	KeyCode _keyNext;
	KeyCode _keySpecial;
		
	public bool IsDisposed () {
		return _isDisposed;
	}
	
	public string ID () {
		return _id;
	}
	
	public string Version () {
		return _version;
	}
	
	public string Author () {
		return _author;
	}
	
	public bool IsDebug () {
		return _isDebug;
	}
	
	public int Width () {
		return Screen.width;
	}
	
	public int Height () {
		return Screen.height;
	}
	
	public Hashtable Config () {
		return _config;
	}
	
	EScene StartingSceneType () {
		return _startingSceneType;
	}
	
	KeyCode KeyPause () {
		return _keyPause;
	}
	
	KeyCode KeyMute () {
		return _keyMute;
	}
	
	KeyCode KeyBack () {
		return _keyBack;
	}
	
	KeyCode KeyNext () {
		return _keyNext;
	}
	
	KeyCode KeySpecial () {
		return _keySpecial;
	}
	
	Factory ( GameObject p_context, bool p_isDebug = false, Hashtable p_config = null )
	{
		Debug.Log ( "<AFactor::new>" );
		_context = p_context;
		_isDebug = p_isDebug;
		_config = p_config;
		_Init();
	}

	void _Init()
	{
		Debug.Log ( "<AFactor::_init>" );
		_config = new Hashtable ();
//		_Configure ( true );
		_DriverInit ();
	}

	void _DriverInit()
	{
//		Debug.Log ( "<AFactor::_DriverInit>" );
//		// override me
//		if ( ( _config != null ) && ( _config.substr( 0, 5 ) == "<?xml" ) )
//		{
//			_traverseElements( Xml.parse( _config ).firstElement().elements(), "" );
//		}
		_LaunchKernel();
	}

//	private function _traverseElements( p_elements:Iterator<Xml>, p_prefix:String ):Void
//	{
//		Debug.Log ( "<AFactor::_traverseElements>" );
//		if ( p_prefix.length != 0 )
//		{
//			p_prefix += ".";
//		}
//		for ( i in p_elements )
//		{
//			var l_name:String = p_prefix + i.nodeName;
//			if ( i.elements().hasNext() )
//			{
//				_traverseElements( i.elements(), l_name );
//			}
//			if ( ( i.firstChild() != null ) && ( i.firstChild().toString().substr( 0, 9 ) == "<![CDATA[" ) )
//			{
//				i.firstChild().nodeValue = i.firstChild().toString().split( "<![CDATA[" ).join( "" ).split( "]]>" ).join( "" );
//			}
//			config.set( l_name, i.firstChild() == null ? "" : i.firstChild().nodeValue );
////			Debug.Log( l_name + " = " + config.get( l_name ) );
//			for ( j in i.attributes() )
//			{
//				var l_aName:String = l_name + "." + j;
//				config.set( l_aName, i.get( j ) );
////				Debug.Log( l_aName + " = " + config.get( l_aName ) );
//			}
//		}
//	}

//	void _Configure ( bool p_isPreconfig = false )
//	{
//		Debug.Log ( "<AFactor::_Configure>" );
//		if ( p_isPreconfig )
//		{
//			id = "awe6";
//			version = "0.0.1";
//			author = "unknown";
//			width = 600;
//			height = 400;
//			bgColor = 0xFF0000;
//			_startingSceneType = EScene.GAME;
//			keyPause = EKey.P;
//			keyMute = EKey.M;
//			keyNext = EKey.SPACE;
//			keyBack = EKey.ESCAPE;
//			keySpecial = EKey.CONTROL;
//		}
//	}
	
	void _LaunchKernel()
	{
		Debug.Log ( "<AFactor::_LaunchKernel>" );
		if ( _concreteKernel != null )
		{
			return;
		}
//		_Configure( false );
		_concreteKernel = new Kernel ( this, _context );
	}
	
//	private function _getAssetUrls():Array<String>
//	{
//		Debug.Log ( "<AFactor::_getAssetUrls>" );
//		var l_result:Array<String> = [];
//		for ( i in 0...1000 )
//		{
//			var l_nodeName:String = _CONFIG_ASSETS_NODE + i;
//			if ( config.exists( l_nodeName ) )
//			{
//				l_result.push( Std.string( config.get( l_nodeName ) ) );
//			}
//		}
//		return l_result;
//	}
	
	void OnInitComplete ( IKernel p_kernel )
	{
		if ( ( _kernel != null ) || ( p_kernel == null ) )
		{
			return;
		}
		_kernel = p_kernel;
		_tools = _kernel.Tools ();
//		_id = ( _tools.toConstCase( StringTools.trim( id ) ) ).substr( 0, 16 );
//		_version = StringTools.trim( version ).substr( 0, 10 );
//		_author = StringTools.trim( author ).substr( 0, 16 );
	}
	
	IAssetManagerProcess CreateAssetManager()
	{
//		if ( Std.is( _kernel.assets, IAssetManagerProcess ) )
//		{
//			return cast( _kernel.assets, IAssetManagerProcess );
//		}
//		else
//		{
			return new AssetManager( _kernel ); // safe downcast
//		}
	}
	
	IEntity CreateEntity ( object p_id )
	{
		return new Entity( _kernel, p_id == null ? null : p_id.ToString() );
	}
	
	IScene CreateScene ( EScene p_type )
	{
		if ( p_type == EScene.START )
		{
			p_type = StartingSceneType ();
		}
		return new Scene ( _kernel, p_type );
	}
	
	EScene GetBackSceneType ( EScene p_type )
	{
		return EScene.EMPTY;
	}
	
	EScene GetNextSceneType ( EScene p_type )
	{
		return EScene.EMPTY;
	}
	
	void Dispose ()
	{
		if ( IsDisposed() || ( _concreteKernel == null ) )
		{
			return;
		}
		_isDisposed = true;
		_DriverDisposer();
		_concreteKernel.Dispose();
		_concreteKernel = null;
		_kernel = null;
		config = null;
	}
	
	virtual protected void _DriverDisposer ()
	{
		// override me
	}
}
