using UnityEngine;
using System.Collections;

public class PathEdge {
	private Vector2 source_;
	private Vector2 destination_;
    private NavGraphEdge.EdgeType edgeType_;
	
	public PathEdge( Vector2 source, Vector2 destination, NavGraphEdge.EdgeType edgeType=NavGraphEdge.EdgeType.normal ) {
		source_ = source;
		destination_ = destination;
		edgeType_ = edgeType;
	}
	
  	public Vector2 Destination() { return destination_; }
  	public void SetDestination(Vector3 newDest) { destination_ = newDest; }
  
	public Vector2 Source() { return source_; }
  	public void SetSource(Vector2 newSource) { source_ = newSource; }
	
  	public NavGraphEdge.EdgeType GetEdgeType() { return edgeType_; }
	
	override public string ToString() {
		return "source: " + source_.x + ", " + source_.y + " destination: " + destination_.x + ", " + destination_.y;
	}
}