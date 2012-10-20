using UnityEngine;
using System.Collections;
using System.Collections.Generic;

	/*
public class WalkPath
{
	protected List<Vector2> paths_ = null;
	
	public WalkPath( int[] collisionMap, Vector2 botPos, Vector2 targetPos ) {
		int botMapIndex = Mathf.FloorToInt(botPos.x) * Mathf.FloorToInt(botPos.y);
		int targetMapIndex = Mathf.FloorToInt(targetPos.x) * Mathf.FloorToInt(targetPos.y);
		
		if ( botMapIndex < 0 || botMapIndex >= collisionMap.Length ) {
			Debug.LogError ( "WalkPath, invalid botMapIndex: " + botMapIndex );
			return ;
		}
		
		if ( targetMapIndex < 0 || targetMapIndex >= collisionMap.Length ) {
			Debug.LogError ( "WalkPath, invalid targetMapIndex: " + targetMapIndex );
			return ;
		}
		
		if ( collisionMap[botMapIndex] != 0 ) {
			Debug.LogError ( "WalkPath, bot is stucking!" );
			return ;
		}
		
		if ( collisionMap[targetMapIndex] != 0 ) {
			Debug.LogError ( "WalkPath, target is not reachable!" );
			return ;
		}
	}
	
	public List<Vector2> GetPath() {
		return paths_;
	}
	
	*/
	


//-------------------------- Graph_SearchAStar_TS -----------------------------
//
//  a A* class that enables a search to be completed over multiple update-steps
//-----------------------------------------------------------------------------
public class Graph_SearchAStar_TS : Graph_SearchTimeSliced {
	protected NavGraph graph_ = null;

  	//indexed into my node. Contains the 'real' accumulative cost to that node
  	float[] gCosts_;
  	
  	//indexed into by node. Contains the cost from adding gCosts_[n] to
  	//the heuristic cost from n to the target node. This is the vector the
  	//iPQ indexes into.
  	protected float[] fCosts_;
	
  	protected NavGraphEdge[] shortestPathTree_;
  	protected NavGraphEdge[] searchFrontier_;
	
	protected int sourceIdx_ = NavGraphNode.invalid_node_index;
	protected int targetIdx_ = NavGraphNode.invalid_node_index;
	
  	//create an indexed priority queue of nodes. The nodes with the
  	//lowest overall F cost (G+H) are positioned at the front.
  	protected IndexedPriorityQLow pq_;

 
	public Graph_SearchAStar_TS(NavGraph graph, int source, int target) : base(SearchType.AStar) {
		graph_ = graph;
		shortestPathTree_ = new NavGraphEdge[graph.NumNodes()];
		searchFrontier_ = new NavGraphEdge[graph.NumNodes()];
		gCosts_ = new float[graph.NumNodes()];
		fCosts_ = new float[graph.NumNodes()];
		sourceIdx_ = source;
		targetIdx_ = target;
		
     	//create the PQ
     	pq_ = new IndexedPriorityQLow(fCosts_, graph_.NumNodes());
    
		//put the source node on the queue
		pq_.Insert(sourceIdx_);
  	}

  	//When called, this method pops the next node off the PQ and examines all
  	//its edges. The method returns an enumerated value (target_found,
  	//target_not_found, search_incomplete) indicating the status of the search
	
  	override public SearchResult CycleOnce() {
  		//if the PQ is empty the target has not been found
  		if (pq_.Empty()) {
    		return SearchResult.target_not_found;
  		}
		
		//get lowest cost node from the queue
		int NextClosestNode = pq_.Pop();
	
		//put the node on the SPT
  		shortestPathTree_[NextClosestNode] = searchFrontier_[NextClosestNode];

  		//if the target has been found exit
  		if (NextClosestNode == targetIdx_) {
    		return SearchResult.target_found;
  		}
  		
  		//now to test all the edges attached to this node
  		List<NavGraphEdge> edgeList = graph_.GetEdgeListList()[NextClosestNode];
		NavGraphEdge edge = null;
		for ( int i=0; i<edgeList.Count; ++i ) {
			edge = edgeList[i];
			
    		//calculate the heuristic cost from this node to the target (H)                       
    		float HCost = Calculate(graph_, targetIdx_, edge.To()); 

    		//calculate the 'real' cost to this node from the source (G)
    		float GCost = gCosts_[NextClosestNode] + edge.Cost();

    		//if the node has not been added to the frontier, add it and update
    		//the G and F costs
    		if ( searchFrontier_[edge.To()] == null ) {
      			fCosts_[edge.To()] = GCost + HCost;
      			gCosts_[edge.To()] = GCost;
      			
      			pq_.Insert(edge.To());
      			
      			searchFrontier_[edge.To()] = edge;
    		}
    		
    		//if this node is already on the frontier but the cost to get here
    		//is cheaper than has been found previously, update the node
    		//costs and frontier accordingly.
    		else if (	(GCost < gCosts_[edge.To()]) &&
						(shortestPathTree_[edge.To()] == null)) {
      			fCosts_[edge.To()] = GCost + HCost;
      			gCosts_[edge.To()] = GCost;
				
      			pq_.ChangePriority(edge.To());
      			
      			searchFrontier_[edge.To()] = edge;
    		}
  		}
  
  		//there are still nodes to explore
  		return SearchResult.search_incomplete;
	}
	
	private float Calculate(NavGraph graph, int nd1, int nd2) {
		float distance = new Vector2(	graph.GetNode(nd1).Position().x - graph.GetNode(nd2).Position().x,
										graph.GetNode(nd1).Position().y - graph.GetNode(nd2).Position().y).magnitude;
		return distance;
	}

  	//returns the vector of edges that the algorithm has examined
  	override public NavGraphEdge[] GetSPT() { return shortestPathTree_; }
  	
  	//returns a vector of node indexes that comprise the shortest path
  	//from the source to the target
  	override public List<int> GetPathToTarget() {
  		List<int> path = new List<int>();

  		//just return an empty path if no target or no path found
  		if (targetIdx_ < 0)  return path;

  		int nd = targetIdx_;

  		path.Add(nd);
    
  		while ( (nd != sourceIdx_) && (shortestPathTree_[nd] != null) ) {
    		nd = shortestPathTree_[nd].From();
    		path.Add(nd);
  		}
		
		return path;
	}
	
  	//returns the path as a list of PathEdges
	override public List<PathEdge> GetPathAsPathEdges() {
		List<PathEdge> path = new List<PathEdge>();
		
  		//just return an empty path if no target or no path found
  		if (targetIdx_ < 0)  return path;    
  		
  		int nd = targetIdx_;
  		
		PathEdge newPathEdge = null;
			
		Vector2 source;
		Vector2 destination;
  		while ((nd != sourceIdx_) && (shortestPathTree_[nd] != null)) {
			source = graph_.GetNode(shortestPathTree_[nd].From()).Position();
			destination = graph_.GetNode(shortestPathTree_[nd].To()).Position();
			
			// change map position to real position
			source.x *= GameSettings.GetInstance().TILE_WIDTH;
			source.y *= GameSettings.GetInstance().TILE_HEIGHT;
			destination.x *= GameSettings.GetInstance().TILE_WIDTH;
			destination.y *= GameSettings.GetInstance().TILE_HEIGHT;
			
			newPathEdge = new PathEdge ( source, destination );
    		path.Insert(0, newPathEdge);

    		nd = shortestPathTree_[nd].From();
  		}

  		return path;
	}

  	//returns the total cost to the target
	override public float GetCostToTarget() { return gCosts_[targetIdx_]; }
}
