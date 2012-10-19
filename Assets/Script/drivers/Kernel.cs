using UnityEngine;
using System.Collections;

/**
 * The AKernel class provides a minimalist implementation of the IKernel interface.
 * <p>It is intended as an abstract class to be extended by target specific drivers.</p>
 * <p>For API documentation please review the corresponding Interfaces.</p>
 * @author	Robert Fell
 * @author	Mihail Ivanchev
 */
class Kernel : Process, IKernel
{
	private static string _POWERED_BY = "Powered by awe6";
	private static string _POWERED_BY_URL = "http://awe6.org";
	private static string _RELEASE_CAUTION = "PUBLIC RELEASE NOT ADVISED";
	private static string _RESET_SESSIONS = "Reset All Saved Information";
	private static string _EYE_CANDY_ENABLE = "Enable Eye Candy";
	private static string _EYE_CANDY_DISABLE = "Disable Eye Candy";
	private static string _FULL_SCREEN_ENABLE = "Enter Full Screen Mode";
	private static string _FULL_SCREEN_DISABLE = "Exit Full Screen Mode";
	
//	public IFactory Factory ();
//	public bool IsDebug ();
//	public ITools Tools ();
//	public IAssetManager Assets ();
//	public var audio( default, null ):IAudioManager;
//	public var inputs( default, null ):IInputManager;
//	public var scenes( default, null ):ISceneManager;
//	public var messenger( default, null ):IMessageManager;
	
	GameObject _context;
	View _view;
	IAssetManagerProcess _assetManagerProcess;
	AudioManager _audioManager;
	InputManager _inputManager;
	SceneManager _sceneManager;
	MessageManager _messageManager;
	private List<IProcess> _processes;
	private Factory _factory;

	public Kernel ( IFactory p_factory, Context p_context ) : base ( this )
	{
		_factory = p_factory;
		_context = p_context;
		_tools = new Tools( this ); // created before super constructor because super.tools binds to it
	}

	override protected void _Init()
	{
		super._Init();
		_view = new View( this, _context, 0, this );
		_processes = new List<IProcess>();
		_helperFramerate = new _HelperFramerate( _factory.targetFramerate );
		_isPreloaded = false;

		// Perform driver specific initializations.
		isDebug = _factory.isDebug;
		isLocal = _driverGetIsLocal();
		_driverInit();

		// Initialize managers.
		assets = _assetManagerProcess = new AAssetManager( _kernel );
		audio =	_audioManager = new AudioManager( _kernel );
		inputs = _inputManager = new InputManager( _kernel );
		scenes = _sceneManager = new SceneManager( _kernel );
		messenger = _messageManager = new MessageManager( _kernel );
		_view.addChild( _sceneManager.view, 1 );
		_addProcess( _assetManagerProcess );
		_addProcess( _inputManager );
		_addProcess( _sceneManager );
		_addProcess( _messageManager );
		_addProcess( _audioManager );
		
		// Set defaults for visual switches.
		isEyeCandy = true;
		isFullScreen = false;

		// Signal completion to the factory and initialize factory-dependent components.
		_factory.onInitComplete( this );

		session = _factory.createSession();
		session.reset();
		_preloader = _factory.createPreloader();
		_addProcess( _preloader );
		_view.addChild( _preloader.view, 2 );
	}

	virtual protected bool _DriverGetIsLocal ()
	{
		//override me
		return false;
	}

	virtual protected void _DriverInit ()
	{
		//override me
	}
	
	virtual protected void _DriverDisposer ()
	{
		//override me
	}
	
	public void onPreloaderComplete ()
	{
		var l_assetManagerProcess = _factory.CreateAssetManager();
		if ( l_assetManagerProcess != _assetManagerProcess )
		{
			_removeProcess( _assetManagerProcess );
			assets = _assetManagerProcess = l_assetManagerProcess;
			_addProcess( _assetManagerProcess, false );
		}
		overlay = _overlayProcess = _factory.createOverlay();
		_addProcess( _overlayProcess, true );
		_view.addChild( _overlayProcess.view, 3 );
		if ( isDebug )
		{
			_addProcess( _profiler = new Profiler( this ) );
			_view.addChild( _profiler.view, _tools.BIG_NUMBER );
		}
		scenes.setScene( _factory.startingSceneType );
		overlay.flash();
	}
	
	override protected void _Updater ( float p_deltaTime )
	{
		_helperFramerate.update();
//		float l_deltaTime = _factory.isFixedUpdates ? Std.int( 1000 / _factory.targetFramerate ) : _helperFramerate.timeInterval;
		super._Updater( l_deltaTime );
		for ( int i = 0; i < _processes.Count; ++i ) {
			_processes[i].update( l_deltaTime );
		}
		_view.update( l_deltaTime );
	}
	
	override protected void _Disposer()
	{
		for ( int i = 0; i < _processes.Count; ++i ) {
			_removeProcess( _processes[i] );
		}
		if ( _factory.GetType() == typeof(IDisposable) )
		{
			cast( _factory, IDisposable ).dispose();
		}
		_factory = null;
		_view.dispose();
		_view = null;
		_driverDisposer();
		assets = _assetManagerProcess = null;
		audio =	_audioManager = null;
		inputs = _inputManager = null;
		scenes = _sceneManager = null;
		messenger = _messageManager = null;
		overlay = _overlayProcess = null;
		tools = _tools = null;
		_logger = null;
		_preloader = null;
		session = null;
		super._disposer();
	}
	
	public object GetConfig ( string p_id ) {
		return _factory.config.exists( p_id ) ? _factory.config.get( p_id ) : null;
	}
	
	private void _AddProcess ( IProcess p_process, bool p_isLast = true )
	{
		if ( p_process == null )
		{
			return;
		}
		if ( p_isLast )
		{
			_processes.Add ( p_process );
		}
		else
		{
			_processes.push( p_process );
		}
	}
	
	private bool _RemoveProcess ( IProcess p_process )
	{
		if ( p_process == null )
		{
			return false;
		}
		p_process.dispose();
		return _processes.remove( p_process );
	}
	
	private void _TotalReset ()
	{
		if ( !_isPreloaded )
		{
			return;
		}
		session.deleteAllSessions();
		session = _factory.createSession();
		scenes.setScene( _factory.startingSceneType );
	}
	
	override protected void _Pauser()
	{
		super._pauser();
		if ( scenes.scene != null )
		{
			scenes.scene.pause();
		}
	}
	
	override protected void _Resumer ()
	{
		super._resumer();
		if ( scenes.scene != null )
		{
			scenes.scene.resume();
		}
	}
}