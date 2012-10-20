//#define LOG_TRACE_INFO
//#define LOG_EXTRA_INFO

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//------------------------------------------------------------------------------
// class definition
//------------------------------------------------------------------------------
public class MyPoolController : MonoBehaviour
{
	static private List<MyPoolController> controllers;

	//------------------------------------------------------------------------------
	// static public methods
	//------------------------------------------------------------------------------
	static public MyPoolController Spawn()
	{
		// search for the first free controller
		foreach(MyPoolController controller in controllers)
		{
			// if disabled, then it's available
			if(controller.gameObject.active == false)
			{
				// switch it back on
				controller.gameObject.SetActiveRecursively(true);
				// return a reference to the caller
				return controller;
			}
		}
		return null;
	}

	//------------------------------------------------------------------------------
	// public methods
	//------------------------------------------------------------------------------
	//------------------------------------------------------------------------------
	// protected mono methods
	//------------------------------------------------------------------------------
	protected void Awake()
	{
		// does the pool exist yet?
		if(controllers == null)
		{
			// lazy initialize it
			controllers = new List<MyPoolController>();
		}
		// add myself
		controllers.Add(this);
		// disable everything
		gameObject.SetActiveRecursively(false);
	}
	
	protected void OnDestroy()
	{
		// remove myself from the pool
		controllers.Remove(this);
		// was I the last one?
		if(controllers.Count == 0)
		{
			// remove the pool itself
			controllers = null;
		}
	}
	
	protected void OnDisable()
	{
		// returning to the pool, perform any cleanup needed
	}
	
	protected void OnEnable()
	{
		// activated from the pool, perform any runtime initialization needed
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
