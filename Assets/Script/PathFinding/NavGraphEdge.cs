using UnityEngine;
using System.Collections.Generic;

//-----------------------------------------------------------------------------
//
//  Graph node for use in creating a navigation graph.This node contains
//  the position of the node and a pointer to a BaseGameEntity... useful
//  if you want your nodes to represent health packs, gold mines and the like
//-----------------------------------------------------------------------------
public class NavGraphEdge {
  	//examples of typical flags
  	public enum EdgeType { normal };
	
	protected int fromIdx_ = NavGraphNode.invalid_node_index;
	protected int toIdx_ = NavGraphNode.invalid_node_index;
	protected float cost_ = 0.0f;
  	protected EdgeType edgeType_ = 0;
	
	public NavGraphEdge( int fromIdx, int toIdx, float cost, EdgeType edgeType=EdgeType.normal ) {
		fromIdx_ = fromIdx;
		toIdx_ = toIdx;
		cost_ = cost;
		edgeType_ = edgeType;
	}
	
	public int From() { return fromIdx_; }
  	public void SetFrom(int index) { fromIdx_ = index; }

  	public int To() { return toIdx_; }
  	public void SetTo(int index) { toIdx_ = index; }

  	public float Cost() { return cost_; }
  	public void SetCost(float cost) { cost_ = cost; }
	
  	public EdgeType GetEdgeType() { return edgeType_; }
  	void SetEdgeType(EdgeType type) { edgeType_ = type; }
}
