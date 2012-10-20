using UnityEngine;
using System.Collections.Generic;

public class GraphNodeExtraInfo {
}

//-----------------------------------------------------------------------------
//
//  Graph node for use in creating a navigation graph.This node contains
//  the position of the node and a pointer to a BaseGameEntity... useful
//  if you want your nodes to represent health packs, gold mines and the like
//-----------------------------------------------------------------------------
public class NavGraphNode {
	static public int invalid_node_index = -1;
	
  	//every node has an index. A valid index is >= 0
  	protected int index_ = 0;
	protected Vector2 position_ = Vector2.zero;
//	protected GameEventTrigger trigger_ = null;
//	protected Bot occupiedBot_ = null;

  //often you will require a navgraph node to contain additional information.
  //For example a node might represent a pickup such as armor in which
  //case m_ExtraInfo could be an enumerated value denoting the pickup type,
  //thereby enabling a search algorithm to search a graph for specific items.
  //Going one step further, m_ExtraInfo could be a pointer to the instance of
  //the item type the node is twinned with. This would allow a search algorithm
  //to test the status of the pickup during the search.
	protected object info_ = null;
	
	public NavGraphNode(int idx, Vector2 position) {
		index_ = idx;
		position_ = position;
	}
	
  	public Vector2 Position() { return position_; }
  	public void SetPosition(Vector2 position) { position_ = position; }

  	public object ExtraInfo() { return info_; }
  	public void SetExtraInfo(object info) { info_ = info; }
	
  	public int Index() { return index_; }
  	public void SetIndex(int index) { index_ = index; }
	
//	public bool isOccupied() { return occupiedBot_ != null; }
//	public void SetOccupied(Bot bot) { occupiedBot_s = bot; }
//	public Bot GetOccupiedBot() { return occupiedBot_; }
//	public void FreeOccupied() { occupiedBot_ = null; }
}