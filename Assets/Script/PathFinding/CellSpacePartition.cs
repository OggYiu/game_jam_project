using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//-----------------------------------------------------------------------------
//
//  Name:   InvertedAABBox2D.h
//
//  Author: Mat Buckland (www.ai-junkie.com)
//
//  Desc:   v simple inverted (y increases down screen) axis aligned bounding
//          box class
//-----------------------------------------------------------------------------
public class InvertedAABBox2D {
	private Vector2 topLeft_;
	private Vector2 bottomRight_;
	private Vector2 center_;
	
	public InvertedAABBox2D(Vector2 topLeft, Vector2 bottomRight) {
		topLeft_ = topLeft;
		bottomRight_ = bottomRight;
		center_ = ( ( topLeft_ + bottomRight_ ) / 2.0f );
	}
  	
	//returns true if the bbox described by other intersects with this one
  	public bool isOverlappedWith( InvertedAABBox2D other) {
    	return !(	(other.Top() > Bottom()) ||
					(other.Bottom() < Top()) ||
					(other.Left() > Right()) ||
					(other.Right() < Left()));
  	}
  	
	public Vector2 TopLeft() { return topLeft_; }
	public Vector2 BottomRight() { return bottomRight_; }
  	public float Top() { return topLeft_.y; }
  	public float Left() { return topLeft_.x; }
  	public float Bottom() { return bottomRight_.y; }
  	public float Right() { return bottomRight_.x; }
  	public Vector2 Center() { return center_; }
};

//------------------------------------------------------------------------
//
//  defines a cell containing a list of pointers to entities
//------------------------------------------------------------------------
public class Cell {
	//all the entities inhabiting this cell
	public List<NavGraphNode> members = null;

  	//the cell's bounding box (it's inverted because the Window's default
  	//co-ordinate system has a y axis that increases as it descends)
  	public InvertedAABBox2D bbox = null;

  	public Cell(Vector2 topleft, Vector2 botright) {
		bbox = new InvertedAABBox2D(topleft, botright);
		members = new List<NavGraphNode>();
	}
}

//  Desc:   class to divide a 2D space into a grid of cells each of which
//          may contain a number of entities. Once created and initialized 
//          with entities, fast proximity querys can be made by calling the
//          CalculateNeighbors method with a position and proximity radius.
//
//          If an NavGraphNode is capable of moving, and therefore capable of moving
//          between cells, the Update method should be called each update-cycle
//          to sychronize the NavGraphNode and the cell space it occupies

public class CellSpacePartition
{
	//the required amount of cells in the space
	private List<Cell> cells_ = null;
	
  	//this is used to store any valid neighbors when an agent searches
  	//its neighboring space
	private NavGraphNode[] neighbors_ = null;
	
  	//the width and height of the world space the entities inhabit
  	private float spaceWidth_ = 0;
  	private float spaceHeight_ = 0;
	
	//the number of cells the space is going to be divided up into
  	private int numCellsX_ = 0;
  	private int numCellsY_ = 0;
	
  	private float cellSizeX_ = 0f;
  	private float cellSizeY_ = 0f;
	
	//given a position in the game space this method determines the           
  	//relevant cell's index
  	private int PositionToIndex(Vector2 pos) {
  		int idx =	(int)( numCellsX_ * pos.x / spaceWidth_ ) + 
  					( (int)( numCellsY_ * pos.y / spaceHeight_ ) * numCellsX_ );

  		//if the NavGraphNode's position is equal to vector2d(m_dSpaceWidth, m_dSpaceHeight)
  		//then the index will overshoot. We need to check for this and adjust
  		if (idx > (int)cells_.Count - 1) {
			idx = (int)cells_.Count - 1;
		}

  		return idx;
	}
	
	public CellSpacePartition(	float width,	//width of the environment
                     			float height,	//height ...
                     			int cellsX,		//number of cells horizontally
                     			int cellsY,		//number of cells vertically
                     			int maxEntitys)	//maximum number of entities to add
	{
		spaceWidth_ = width;
		spaceHeight_ = height;
		numCellsX_ = cellsX;
		numCellsY_ = cellsY;
		neighbors_ = new NavGraphNode[maxEntitys];
		for ( int i=0; i<neighbors_.Length; ++i ) {
			neighbors_[i] = null;
		}
		cells_ = new List<Cell>();
		
  		//calculate bounds of each cell
  		cellSizeX_ = width  / cellsX;
  		cellSizeY_ = height / cellsY;
		
  		//create the cells
  		for (int y=0; y<numCellsY_; ++y) {
    		for (int x=0; x<numCellsX_; ++x) {
      			float left = x * cellSizeX_;
      			float right = left + cellSizeX_;
      			float top = y * cellSizeY_;
      			float bot = top + cellSizeY_;
				
     	 		cells_.Add( new Cell( new Vector2(left, top), new Vector2(right, bot) ) );
    		}
  		}
	}
	
  	//adds entities to the class by allocating them to the appropriate cell
	public void AddEntity(NavGraphNode node) {
		if ( node == null ) {
			Debug.LogError( "CellSpacePartition::AddEntity: invalid node" );
			return ;
		}
  		//int sz = cells_.Count;
  		int idx = PositionToIndex(node.Position());
  		
		if ( idx >= cells_.Count ) {
			Debug.LogError ( "CellSpacePartition::AddEntity: idx out of range! idx: " + idx + ", cells_.Count: " + cells_.Count );
		}
		cells_[idx].members.Add(node);
	}
	
	//update an NavGraphNode's cell by calling this from your NavGraphNode's Update method 
  	public void UpdateEntity(NavGraphNode node, Vector2 oldPos) {
  		//if the index for the old pos and the new pos are not equal then
  		//the NavGraphNode has moved to another cell.
  		int oldIdx = PositionToIndex(oldPos);
  		int newIdx = PositionToIndex(node.Position());

  		if (newIdx == oldIdx) return;

  		//the NavGraphNode has moved into another cell so delete from current cell
  		//and add to new one
  		cells_[oldIdx].members.Remove(node);
  		cells_[newIdx].members.Add(node);
}


  	//this method calculates all a target's neighbors and stores them in
  	//the neighbor vector. After you have called this method use the begin, 
  	//next and end methods to iterate through the vector.
  	public void CalculateNeighbors(Vector2 targetPos, float queryRadius) {
		if ( neighbors_.Length <= 0 ) {
			Debug.LogError ( "CellSpacePartition::CalculateNeighbors: invalid neighbors since it is empty" );
			return ;
		}
		
  		//create an iterator and set it to the beginning of the neighbor vector
		int currNeighborIndex = 0;
  		//NavGraphNode curNbor = neighbors_.begin();
  
		//create the query box that is the bounding box of the target's query
		//area
		InvertedAABBox2D queryBox = new InvertedAABBox2D(	targetPos - new Vector2(queryRadius, queryRadius),
                            								targetPos + new Vector2(queryRadius, queryRadius));
                            		
  		//iterate through each cell and test to see if its bounding box overlaps
  		//with the query box. If it does and it also contains entities then
  		//make further proximity tests.
		Cell currCell = null;
		NavGraphNode currNode = null;
		for ( int i=0; i<cells_.Count; ++i ) {
			currCell = cells_[i];
			
    		//test to see if this cell contains members and if it overlaps the
    		//query box
    		if (currCell.bbox.isOverlappedWith(queryBox) &&
				currCell.members.Count > 0) {
      			//add any entities found within query radius to the neighbor list
      			//std::list<NavGraphNode>::iterator it = currCell->memebers.begin();
				for ( int j=0; j<currCell.members.Count; ++j ) {
					currNode = currCell.members[j];
					
					if ( ( currNode.Position() - targetPos ).sqrMagnitude < queryRadius * queryRadius ) {
						neighbors_[currNeighborIndex++] = currNode;
          				//*curNbor++ = *it;
        			}
      			}    
    		}
  		} //next cell

  		//mark the end of the list with a zero.
  		neighbors_[currNeighborIndex] = null;
	}
	
  	//empties the cells of entities
  	public void EmptyCells() {
		for ( int i=0; i<cells_.Count; ++i ) {
			cells_[i].members.Clear();
		}
	}
	
	public NavGraphNode[] GetNeighbors() { return neighbors_; }
}