using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Graph_SearchDijkstras_TS : Graph_SearchTimeSliced {
	private NavGraph navGraph_ = null;
	
  	//indexed into my node. Contains the accumulative cost to that node
  	private float[] costToThisNode_ = null;

  	private NavGraphEdge[] shortestPathTree_ = null;
  	private NavGraphEdge[] searchFrontier_ = null;
	
	private int sourceNodeIndex_ = NavGraphNode.invalid_node_index;
	private int targetNodeIndex_ = NavGraphNode.invalid_node_index;
	
  	//create an indexed priority queue of nodes. The nodes with the
  	//lowest overall F cost (G+H) are positioned at the front.
  	private IndexedPriorityQLow pq_ = null;
	
	public Graph_SearchDijkstras_TS(	NavGraph navGraph,
                          				int sourceNodeIndex,
                          				int targetNodeIndex) : base(Graph_SearchTimeSliced.SearchType.Dijkstra) {
		navGraph_ = navGraph;
		shortestPathTree_ = new NavGraphEdge[navGraph_.NumNodes()];
		searchFrontier_ = new NavGraphEdge[navGraph_.NumNodes()];
		costToThisNode_ = new float[navGraph_.NumNodes()];
		sourceNodeIndex_ = sourceNodeIndex;
		targetNodeIndex_ = targetNodeIndex;
		
     	//create the PQ         ,
     	pq_ = new IndexedPriorityQLow( costToThisNode_, navGraph_.NumNodes() );

    	//put the source node on the queue
    	pq_.Insert( sourceNodeIndex_ );
	}

  	//let the search class take care of tidying up memory (the wary amongst
  	//you may prefer to use std::auto_ptr or similar to replace the pointer
  	//to the termination condition)
	~Graph_SearchDijkstras_TS() {
		pq_ = null;
	}
	
  	//When called, this method pops the next node off the PQ and examines all
  	//its edges. The method returns an enumerated value (target_found,
  	//target_not_found, search_incomplete) indicating the status of the search
  	override public SearchResult CycleOnce() {
  		//if the PQ is empty the target has not been found
  		if ( pq_.Empty()) {
    		return Graph_SearchTimeSliced.SearchResult.target_not_found;
  		}

  		//get lowest cost node from the queue
  		int nextClosestNode = pq_.Pop();

  		//move this node from the frontier to the spanning tree
  		shortestPathTree_[nextClosestNode] = searchFrontier_[nextClosestNode];

  		//if the target has been found exit
  		if ( isSatisfied(navGraph_, targetNodeIndex_, nextClosestNode) ) {
    		//make a note of the node index that has satisfied the condition. This
    		//is so we can work backwards from the index to extract the path from
    		//the shortest path tree.
    		targetNodeIndex_ = nextClosestNode;

    		return Graph_SearchTimeSliced.SearchResult.target_found;
  		}
		
  		//now to test all the edges attached to this node
		List<NavGraphEdge> edges = navGraph_.GetEdgeListList()[nextClosestNode];
		NavGraphEdge currEdge = null;
		for ( int i=0; i<edges.Count; ++i ) {
			currEdge = edges[i];
			
			//the total cost to the node this edge points to is the cost to the
    		//current node plus the cost of the edge connecting them.
    		float newCost = costToThisNode_[nextClosestNode] + currEdge.Cost();

    		//if this edge has never been on the frontier make a note of the cost
    		//to get to the node it points to, then add the edge to the frontier
    		//and the destination node to the PQ.
    		if ( searchFrontier_[currEdge.To()] == null ) {
				costToThisNode_[currEdge.To()] = newCost;
      			pq_.Insert(currEdge.To());
      			searchFrontier_[currEdge.To()] = currEdge;
			}

    		//else test to see if the cost to reach the destination node via the
    		//current node is cheaper than the cheapest cost found so far. If
    		//this path is cheaper, we assign the new cost to the destination
    		//node, update its entry in the PQ to reflect the change and add the
    		//edge to the frontier
    		else if (	( newCost < costToThisNode_[currEdge.To()] ) &&
              			( shortestPathTree_[currEdge.To()] == null ) ) {
      			costToThisNode_[currEdge.To()] = newCost;

      			//because the cost is less than it was previously, the PQ must be
      			//re-sorted to account for this.
      			pq_.ChangePriority(currEdge.To());
      			searchFrontier_[currEdge.To()] = currEdge;
			}
		}
  
  		//there are still nodes to explore
		return Graph_SearchTimeSliced.SearchResult.search_incomplete;
	}

  	//returns the vector of edges that the algorithm has examined
  	override public NavGraphEdge[] GetSPT() { return shortestPathTree_; }

  	//returns a vector of node indexes that comprise the shortest path
  	//from the source to the target
  	override public List<int> GetPathToTarget() {
  		//just return an empty path if no target or no path found
  		if ( targetNodeIndex_ < 0 ) {
			return null;
		}
		
  		List<int> pathIndices = null;
  		int nd = targetNodeIndex_;
  		pathIndices.Add(nd);
  		
  		while (	(nd != sourceNodeIndex_) &&
				(shortestPathTree_[nd] != null)) {
    		nd = shortestPathTree_[nd].From();
    		pathIndices.Insert(0, nd);
  		}
  		return pathIndices;
	}

  	//returns the path as a list of PathEdges
  	override public List<PathEdge> GetPathAsPathEdges() {
  		//just return an empty path if no target or no path found
  		if ( targetNodeIndex_ < 0) {
			return null;
		}
		
  		List<PathEdge> pathEdges = new List<PathEdge>();
		
  		int nd = targetNodeIndex_;
  		
		while ( ( nd != sourceNodeIndex_ ) &&
				(shortestPathTree_[nd] != null) ) {
			pathEdges.Insert(0, new PathEdge(	navGraph_.GetNode(shortestPathTree_[nd].From()).Position(),
                             					navGraph_.GetNode(shortestPathTree_[nd].To()).Position(),
                             					shortestPathTree_[nd].GetEdgeType() ) );
                             					
    		nd = shortestPathTree_[nd].From();
		}
  		return pathEdges;
	}

  	//returns the total cost to the target
	override public float GetCostToTarget() { return costToThisNode_[targetNodeIndex_]; }
	
  	bool isSatisfied(NavGraph navGraph, int target, int currentNodeIdx) {
		return target == currentNodeIdx;
  	}
}