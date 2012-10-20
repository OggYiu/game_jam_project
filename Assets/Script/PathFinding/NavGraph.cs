using UnityEngine;
using System.Collections.Generic;

public class NavGraph {
  	//a couple more typedefs to save my fingers and to help with the formatting
  //of the code on the printed page
  	protected List<NavGraphNode> nodes_ = null;
  	protected List< List<NavGraphEdge> > edges_ = null;
	protected NavGraphRenderer debugRenderer_ = null;
 
  	//is this a directed graph?
  	//protected bool isDigraph_ = false;
  
  	//the index of the next node to be added
  	protected int nextNodeIndex_ = 0;
    
  	//returns true if an edge is not already present in the graph. Used
  	//when adding edges to make sure no duplicates are created.
	protected bool isUniqueEdge(int fromIdx, int toIdx) {
		NavGraphEdge currEdge = null;
		for ( int i=0; i<edges_[fromIdx].Count; ++i ) {
			currEdge = edges_[fromIdx][i];
    		if (currEdge.To() == toIdx) {
      			return false;
    		}
  		}

  		return true;
	}

  	//iterates through all the edges in the graph and removes any that point
  	//to an invalidated node
  	protected void CullInvalidEdges() {
		List<NavGraphEdge> currEdgeList = null;
		NavGraphEdge currEdge = null;
		
		for ( int i=0; i<edges_.Count; ++i ) {
			currEdgeList = edges_[i];
			
			for ( int j=0; j<currEdgeList.Count; ++j ) {
				currEdge = currEdgeList[j];
      			if (nodes_[currEdge.To()].Index() == NavGraphNode.invalid_node_index || 
          			nodes_[currEdge.From()].Index() == NavGraphNode.invalid_node_index) {
        			currEdgeList.RemoveAt(j);
      			}
    		}
		}
  	}
	
  	//public NavGraph(bool isDigraph) {
  	public NavGraph() {
		//isDigraph_ = isDigraph;
		//isDigraph_ = false;
		
		nodes_ = new List<NavGraphNode>();
		edges_ = new List< List<NavGraphEdge> >();
	}
	
  	//returns the node at the given index
  	public NavGraphNode GetNode(int idx) {
		DebugUtils.Assert( idx < nodes_.Count && idx >= 0, "<NavGraph::GetNode>: invalid index" + idx );
		
		/*
		NavGraphNode targetNode = nodes_[idx];
		
		for ( int i=0; i<nodes_.Count; ++i ) {
			currNode = nodes_[i];
			if( currNode.GetIndex() == idx ) {
			}
		}
		*/
		
	    return nodes_[idx];
	}
	
	public NavGraphNode GetNodeWithPos(Vector2 mapPos) {
		NavGraphNode currNode = null;
		NavGraphNode resultNode = null;
		for ( int i=0; i<nodes_.Count; ++i ) {
			currNode = nodes_[i];
			if (	(int)currNode.Position().x == (int)mapPos.x &&
					(int)currNode.Position().y == (int)mapPos.y ) {
				resultNode = currNode;
			}
		}
		
		//if ( resultNode == null ) {
		//	Debug.LogError ( "NavGraph::GetNodeWIthPos, node not find with position: " + mapPos.x + ", " + mapPos.y );
		//}
		
		return resultNode;
	}
	
  	// obtaining a reference to an edge
  	public NavGraphEdge GetEdge(int fromIdx, int toIdx) {
		DebugUtils.Assert( fromIdx < nodes_.Count && fromIdx >= 0, "<NavGraph::GetNode>: invalid fromIdx" + fromIdx );
		DebugUtils.Assert( nodes_[fromIdx].Index() != NavGraphNode.invalid_node_index, "<NavGraph::GetNode>: invalid fromIdx" + fromIdx );
		DebugUtils.Assert( toIdx < nodes_.Count && toIdx >= 0, "<NavGraph::GetNode>: invalid toIdx" + toIdx );
		DebugUtils.Assert( nodes_[toIdx].Index() != NavGraphNode.invalid_node_index, "<NavGraph::GetNode>: invalid toIdx" + toIdx );
		
		for ( int i=0; i<edges_[fromIdx].Count; ++i ) {
			if ( edges_[fromIdx][i].To() == toIdx ) {
				return edges_[fromIdx][i];
			}
		}

  		DebugUtils.Assert( false, "<NavGraph::GetEdge>: edge does not exist" );
		return null;
	}

  	//retrieves the next free node index
	public int GetNextFreeNodeIndex() { return nextNodeIndex_; }
  
  	//adds a node to the graph and returns its index
  	public int AddNode(NavGraphNode node) {
  		if ( node.Index() < nodes_.Count ) {
    		//make sure the client is not trying to add a node with the same ID as
    		//a currently active node
			//DebugUtils.Assert( nodes_[node.GetIndex()].GetIndex() == NavGraphNode.invalid_node_index, "<NavGraph::AddNode>: Attempting to add a node with a duplicate ID" );
    		nodes_[node.Index()] = node;

    		return nextNodeIndex_;
  		} else {
    		//make sure the new node has been indexed correctly
			DebugUtils.Assert( node.Index() == nextNodeIndex_, "<NavGraph::AddNode>:invalid index" );

    		nodes_.Add(node);
    		edges_.Add(new List<NavGraphEdge>());

    		return nextNodeIndex_++;
  		}
	}

  	//removes a node by setting its index to invalid_node_index
  	public void RemoveNode(int nodeIdx) {
		DebugUtils.Assert( nodeIdx < nodes_.Count && nodeIdx >= 0, "<NavGraph::GetNode>: invalid index" + nodeIdx );

		//set this node's index to invalid_node_index
  		nodes_[nodeIdx].SetIndex(NavGraphNode.invalid_node_index);

  		//if the graph is not directed remove all edges leading to this node and then
  		//clear the edges leading from the node
		/*
  		if (!isDigraph_) {
			NavGraphEdge currEdge1 = null;
			NavGraphEdge currEdge2 = null;
    		//visit each neighbour and erase any edges leading to this node
			for ( int i=0; i<edges_[nodeIdx].Count; ++i ) {
				currEdge1 = edges_[nodeIdx][i];
				
				for ( int j=0; j<edges_[currEdge1.To()].Count; ++j ) {
					currEdge2 = edges_[currEdge1.To()][j];
         			if (currEdge2.To() == nodeIdx) {
         				edges_[currEdge1.To()].RemoveAt(j);
						break;
         			}
      			}
    		}

    		//finally, clear this node's edges
    		edges_[nodeIdx].Clear();
  		}
		//if a digraph remove the edges the slow way
		else {
    		CullInvalidEdges();
  		}
  		*/
	}

  	//Use this to add an edge to the graph. The method will ensure that the
  	//edge passed as a parameter is valid before adding it to the graph. If the
  	//graph is a digraph then a similar edge connecting the nodes in the opposite
  	//direction will be automatically added.
  	public void AddEdge(NavGraphEdge edge) {
  		//first make sure the from and to nodes exist within the graph 
  		DebugUtils.Assert ( ( edge.From() < nextNodeIndex_ ) &&
							( edge.To() < nextNodeIndex_ ),
          					"<NavGraph::AddEdge>: invalid node index, from: " + edge.From() + ", to: " + edge.To() );
          					
  		//make sure both nodes are active before adding the edge
  		if ( (nodes_[edge.To()].Index() != NavGraphNode.invalid_node_index) && 
       		 (nodes_[edge.From()].Index() != NavGraphNode.invalid_node_index)) {
    		//add the edge, first making sure it is unique
    		if ( isUniqueEdge(edge.From(), edge.To()) ) {
      			edges_[edge.From()].Add(edge);
    		}
    		
    		//if the graph is undirected we must add another connection in the opposite
    		//direction
			/*
    		if (!isDigraph_) {
      			//check to make sure the edge is unique before adding
      			if ( isUniqueEdge(edge.To(), edge.From()) ) {
        			NavGraphEdge newEdge = edge;
        			newEdge.SetTo(edge.From());
        			newEdge.SetFrom(edge.To());

        			edges_[edge.To()].Add(newEdge);
				}
    		}
    		*/
  		}
	}

  	//removes the edge connecting from and to from the graph (if present). If
  	//a digraph then the edge connecting the nodes in the opposite direction 
  	//will also be removed.
  	public void RemoveEdge(int fromNodeIdx, int toNodeIdx) {
		DebugUtils.Assert(	( fromNodeIdx < nodes_.Count ) &&
							( toNodeIdx < nodes_.Count ),
   							"<NavGraph::RemoveEdge>:invalid node index");
		
		NavGraphEdge currEdge = null;
		/*
  		if (!isDigraph_) {
			for ( int i=0; i<edges_[toNodeIdx].Count; ++i ) {
				currEdge = edges_[toNodeIdx][i];
      			if ( currEdge.To() == fromNodeIdx ) {
      				edges_[toNodeIdx].RemoveAt(i);
					break;
				}
			}
		}
		*/
		
		for ( int i=0; i<edges_[fromNodeIdx].Count; ++i ) {
			currEdge = edges_[fromNodeIdx][i];
  			if ( currEdge.To() == toNodeIdx ) {
  				edges_[fromNodeIdx].RemoveAt(i);
				break;
			}
		}
	}
	
  	//sets the cost of an edge
  	public void SetEdgeCost(int fromNodeIdx, int toNodeIdx, float cost) {
  		//make sure the nodes given are valid
  		DebugUtils.Assert( ( fromNodeIdx < nodes_.Count ) && ( toNodeIdx < nodes_.Count ),
        					"<NavGraph::SetEdgeCost>: invalid index, fromNodeIdx: " + fromNodeIdx + ", toNodeIdx: " + toNodeIdx);
        				
		NavGraphEdge currEdge = null;
  		//visit each neighbour and erase any edges leading to this node
		for ( int i=0; i<edges_[fromNodeIdx].Count; ++i ) {
			currEdge = edges_[fromNodeIdx][i];
			
    		if ( currEdge.To() == toNodeIdx ) {
      			currEdge.SetCost(cost);
      			break;
    		}
		}
	}

  	//returns the number of active + inactive nodes present in the graph
  	public int NumNodes() { return nodes_.Count; }
  
  	//returns the number of active nodes present in the graph (this method's
  	//performance can be improved greatly by caching the value)
  	public int NumActiveNodes() {
    	int count = 0;
		
		for ( int i=0; i<nodes_.Count; ++i ) {
			if ( nodes_[i].Index() != NavGraphNode.invalid_node_index ) {
				++count;
			}
		}
		
    	return count;
  	}

  	//returns the total number of edges present in the graph
  	public int NumEdges() {
    	int tot = 0;
		
		for ( int i=0; i<edges_.Count; ++i ) {
      		tot += edges_[i].Count;
    	}

    	return tot;
  	}

  	//returns true if the graph is directed
  	//public bool isDigraph() { return isDigraph_; }

  	//returns true if the graph contains no nodes
  	public bool isEmpty() { return nodes_.Count <= 0; }

  	//returns true if a node with the given index is present in the graph
  	public bool isNodePresent(int index) {
    	if ( index >= nodes_.Count || index < 0 )
			return false;
		else if ( nodes_[index].Index() == NavGraphNode.invalid_node_index )
			return false;
		
		return true;
    }
	
  	//returns true if an edge connecting the nodes 'to' and 'from'
  	//is present in the graph
  	public bool isEdgePresent(int fromNodeIdx, int toNodeIdx) {
		if ( isNodePresent(fromNodeIdx) && isNodePresent(toNodeIdx) ) {
			for ( int i=0; i<edges_[fromNodeIdx].Count; ++i ) {
				if ( edges_[fromNodeIdx][i].To() == toNodeIdx ) {
					return true;
				}
			}
		}
        return false;
    }
	
  	//clears the graph ready for new node insertions
  	public void Clear() {
		nextNodeIndex_ = 0;
		nodes_.Clear();
		edges_.Clear();
	}

  	public void RemoveEdges() {
		edges_.Clear();
  	}
  	
	public void Render() {
		if ( debugRenderer_ == null ) {
			debugRenderer_ = new NavGraphRenderer();
		}
		
		debugRenderer_.Render(this);
	}
	
	public List<NavGraphNode> GetNodes() { return nodes_; }
	public List< List<NavGraphEdge> > GetEdgeListList() { return edges_; }
}