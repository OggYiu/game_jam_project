using UnityEngine;
using System.Collections;

//----------------------- IndexedPriorityQLow ---------------------------
//
//  Priority queue based on an index into a set of keys. The queue is
//  maintained as a 2-way heap.
//
//  The priority in this implementation is the lowest valued key
//------------------------------------------------------------------------
public class IndexedPriorityQLow {
	private float[] _vecKeys = null;
  	private int[] heap_ = null;
  	private int[] invHeap_ = null;
	private int size_ = 0;
	private int maxSize_ = 0;
	
  	void Swap(int a, int b) {
    	int temp = heap_[a]; heap_[a] = heap_[b]; heap_[b] = temp;
		
    	//change the handles too
    	invHeap_[heap_[a]] = a; invHeap_[heap_[b]] = b;
	}
	
	void ReorderUpwards(int nd) {
    	//move up the heap swapping the elements until the heap is ordered
    	while ( (nd>1) &&
				(_vecKeys[heap_[nd/2]] > _vecKeys[heap_[nd]]) ) {
      		Swap(nd/2, nd);
      		nd /= 2;
    	}
  	}
  	
  	void ReorderDownwards(int nd, int HeapSize) {
    	//move down the heap from node nd swapping the elements until
    	//the heap is reordered
    	while (2*nd <= HeapSize) {
      		int child = 2 * nd;

      		//set child to smaller of nd's two children
      		if (	(child < HeapSize) &&
					(_vecKeys[heap_[child]] > _vecKeys[heap_[child+1]])) {
        		++child;
      		}
			
      		//if this nd is larger than its child, swap
      		if (_vecKeys[heap_[nd]] > _vecKeys[heap_[child]]) {
	        	Swap(child, nd);

        		//move the current node down the tree
        		nd = child;
      		} else {
        		break;
      		}
    	}
  	}
  	
  	//you must pass the constructor a reference to the std::vector the PQ
  	//will be indexing into and the maximum size of the queue.
  	public IndexedPriorityQLow(	float[] keys,
                      			int maxSize) {
		_vecKeys = keys;
		maxSize_ = maxSize;
		size_ = 0;
		
		heap_ = new int[maxSize_+1];
		invHeap_ = new int[maxSize_+1];
  	}
	
  	public bool Empty() { return ( size_ == 0 ); }

  	//to insert an item into the queue it gets added to the end of the heap
  	//and then the heap is reordered from the bottom up.
  	public void Insert(int idx) {
		if ( (size_+1) > maxSize_) {
			Debug.LogError ( "IndexedPriorityQLow::insert, heap is full!" );
			return ;
		}
		
    	++size_;
    	heap_[size_] = idx;
    	invHeap_[idx] = size_;
    	ReorderUpwards(size_);
  	}

  	//to get the min item the first element is exchanged with the lowest
  	//in the heap and then the heap is reordered from the top down. 
  	public int Pop() {
    	Swap(1, size_);
    	ReorderDownwards(1, size_-1);
    	return heap_[size_--];
  	}

  	//if the value of one of the client key's changes then call this with 
  	//the key's index to adjust the queue accordingly
  	public void ChangePriority(int idx) {
    	ReorderUpwards(invHeap_[idx]);
  	}
};