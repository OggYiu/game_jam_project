//#define LOG_TRACE_INFO
//#define LOG_EXTRA_INFO

using UnityEngine;
using System.Collections;

//------------------------------------------------------------------------------
// class definition
//------------------------------------------------------------------------------
public class MySingletonController : MonoBehaviour
{
	static private MySingletonController controller;

	//------------------------------------------------------------------------------
	// static public methods
	//------------------------------------------------------------------------------
	//------------------------------------------------------------------------------
	// protected mono methods
	//------------------------------------------------------------------------------
	protected void Awake()
	{
		controller = this;
	}
	
	protected void OnDestroy()
	{
		if(controller != null)
		{
			controller = null;
		}
	}
	
	protected void OnDisable()
	{
	}
	
	protected void OnEnable()
	{
	}
	
	protected void Start()
	{
		// called once before first frame
	}
	
	protected void Update()
	{
		// called once each frame
	}

	//------------------------------------------------------------------------------
	// private methods
	//------------------------------------------------------------------------------
}
