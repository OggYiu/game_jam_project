using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//----------------------- Graph_SearchDijkstra --------------------------------
//
//  Given a graph, source and optional target this class solves for
//  single source shortest paths (without a target being specified) or 
//  shortest path from source to target.
//
//  The algorithm used is a priority queue implementation of Dijkstra's.
//  note how similar this is to the algorithm used in Graph_MinSpanningTree.
//  The main difference is in the calculation of the priority in the line:
//  
//  double NewCost = m_CostToThisNode[best] + pE->Cost;
//------------------------------------------------------------------------
public class Graph_SearchDijkstra {
	private NavGraph navGraph_ = null;

  	//this vector contains the edges that comprise the shortest path tree -
  	//a directed subtree of the graph that encapsulates the best paths from 
  	//every node on the SPT to the source node.
  	private NavGraphEdge[] shortestPathTree_ = null;
  	
  	//this is indexed into by node index and holds the total cost of the best
  	//path found so far to the given node. For example, m_CostToThisNode[5]
  	//will hold the total cost of all the edges that comprise the best path
  	//to node 5, found so far in the search (if node 5 is present and has 
  	//been visited)
  	private float[] costToThisNode_ = null;
	
  	//this is an indexed (by node) vector of 'parent' edges leading to nodes 
  	//connected to the SPT but that have not been added to the SPT yet. This is
  	//a little like the stack or queue used in BST and DST searches.
  	private NavGraphEdge[] searchFrontier_ = null;
	
  	private int sourceNodeID_ = 0;
  	private int targetNodeID_ = 0;
	
	public Graph_SearchDijkstra(NavGraph navGraph,
                       			int sourceNodeID,
                       			int targetNodeID = -1) {
		navGraph_ = navGraph;
		shortestPathTree_ = new NavGraphEdge[navGraph.NumNodes()];
		searchFrontier_ = new NavGraphEdge[navGraph.NumNodes()];
		costToThisNode_ = new float[navGraph.NumNodes()];
		sourceNodeID_ = sourceNodeID;
		targetNodeID_ = targetNodeID;
		
    	Search();
  	}
 
  	protected void Search() {
  		//create an indexed priority queue that sorts smallest to largest
  		//(front to back).Note that the maximum number of elements the iPQ
  		//may contain is N. This is because no node can be represented on the 
  		//queue more than once.
  		IndexedPriorityQLow pq = new IndexedPriorityQLow ( costToThisNode_, navGraph_.NumNodes() );
  		
  		//put the source node on the queue
  		pq.Insert(sourceNodeID_);
  		
  		//while the queue is not empty
  		while ( !pq.Empty() ) {
    		//get lowest cost node from the queue. Don't forget, the return value
    		//is a *node index*, not the node itself. This node is the node not already
    		//on the SPT that is the closest to the source node
    		int nextClosestNode = pq.Pop();

    		//move this edge from the frontier to the shortest path tree
    		shortestPathTree_[nextClosestNode] = searchFrontier_[nextClosestNode];

    		//if the target has been found exit
    		if ( nextClosestNode == targetNodeID_ ) {
				return;
			}
			
    		//now to relax the edges.
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
        			pq.Insert(currEdge.To());
        			searchFrontier_[currEdge.To()] = currEdge;
				}

      			//else test to see if the cost to reach the destination node via the
      			//current node is cheaper than the cheapest cost found so far. If
      			//this path is cheaper, we assign the new cost to the destination
      			//node, update its entry in the PQ to reflect the change and add the
      			//edge to the frontier
      			else if (	(newCost < costToThisNode_[currEdge.To()]) &&
							(shortestPathTree_[currEdge.To()] == null) ) {
        			costToThisNode_[currEdge.To()] = newCost;
        			
        			//because the cost is less than it was previously, the PQ must be
        			//re-sorted to account for this.
        			pq.ChangePriority(currEdge.To());

        			searchFrontier_[currEdge.To()] = currEdge;
				}
			}
		}
	}
	
  	//returns the vector of edges that defines the SPT. If a target was given
  	//in the constructor then this will be an SPT comprising of all the nodes
  	//examined before the target was found, else it will contain all the nodes
  	//in the graph.
  	public NavGraphEdge[] GetSPT() { return shortestPathTree_; }
  	
  	//returns a vector of node indexes that comprise the shortest path
  	//from the source to the target. It calculates the path by working
  	//backwards through the SPT from the target node.
  	public List<int> GetPathToTarget() {
  		//just return an empty path if no target or no path found
  		if (targetNodeID_ < 0) {
			return null;
		}
		
  		List<int> path = new List<int>();
  		int nd = targetNodeID_;
  		path.Add(nd);
		
  		while (	(nd != sourceNodeID_) &&
				(shortestPathTree_[nd] != null)) {
    		nd = shortestPathTree_[nd].From();
    		path.Insert(0, nd);
  		}

  		return path;
	}
	
  	//returns the total cost to the target
  	public float GetCostToTarget() { return costToThisNode_[targetNodeID_]; }
  	
  	//returns the total cost to the given node
  	public float GetCostToNode(int nd) { return costToThisNode_[nd]; }
}