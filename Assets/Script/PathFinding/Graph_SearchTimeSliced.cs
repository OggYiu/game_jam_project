using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// base class to define a common interface for graph search algorithms
public class Graph_SearchTimeSliced {
	//these enums are used as return values from each search update method
	public enum SearchResult { target_found, target_not_found, search_incomplete };
	
	public enum SearchType { Unknown, AStar, Dijkstra };

  	private SearchType searchType_ = SearchType.Unknown;

  	public Graph_SearchTimeSliced(SearchType type) {
  		searchType_ = type;
	}
	
  	//When called, this method runs the algorithm through one search cycle. The
  	//method returns an enumerated value (target_found, target_not_found,
  	//search_incomplete) indicating the status of the search
  	virtual public SearchResult CycleOnce() {
		Debug.LogError( "Graph_SearchTimeSliced::CycleOnce: please implement this method" );
		return SearchResult.target_not_found;
	}

  	//returns the vector of edges that the algorithm has examined
  	virtual public NavGraphEdge[] GetSPT() {
		Debug.LogError( "Graph_SearchTimeSliced::GetSPT: please implement this method" );
		return null;
	}
	
  	//returns the total cost to the target
  	virtual public float GetCostToTarget() {
		Debug.LogError( "Graph_SearchTimeSliced::GetCostToTarget: please implement this method" );
		return float.MaxValue;
	}

  	//returns a list of node indexes that comprise the shortest path
  	//from the source to the target
  	virtual public List<int> GetPathToTarget() {
		Debug.LogError( "Graph_SearchTimeSliced::GetPathToTarget: please implement this method" );
		return null;
	}

  	//returns the path as a list of PathEdges
  	virtual public List<PathEdge> GetPathAsPathEdges() {
		Debug.LogError( "Graph_SearchTimeSliced::GetPathAsPathEdges: please implement this method" );
		return null;
	}

  	public SearchType GetSearchType() { return searchType_; }
};