using UnityEngine;
using System.Collections;

public enum NodeType {
	normal,
	blocked,
}

public class NavigationMap : Entity
{
	NodeType [,] collision_map_;
	protected override void _Resolver (Hashtable args)
	{
		base._Resolver (args);
		
		// testing
		collision_map_ = new NodeType[GameSettings.GetInstance().MAP_TILE_ROW_COUNT, GameSettings.GetInstance().MAP_TILE_ROW_COUNT];
		for ( int i = 0; i < GameSettings.GetInstance().MAP_TILE_ROW_COUNT; ++i ) {
			for ( int j = 0; j < GameSettings.GetInstance().MAP_TILE_ROW_COUNT; ++j ) {
				collision_map_[i,j] = NodeType.normal;
			}
		}
	}
	
	public NodeType GetNodeType ( int row, int column) {
		if ( ( row * GameSettings.GetInstance().MAP_TILE_ROW_COUNT + column ) >= ( GameSettings.GetInstance().MAP_TILE_ROW_COUNT * GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT )  ) {
			Debug.LogError ( "<NavigationMap::GetNodeType>, invalid index, row : " + row + ", column: " + column );
			return NodeType.normal;
		}
		
		return collision_map_[row,column];
	}
	
	public void SetCollisionMapType ( int row, int column, NodeType type ) {
		if ( ( row * GameSettings.GetInstance().MAP_TILE_ROW_COUNT + column ) >= ( GameSettings.GetInstance().MAP_TILE_ROW_COUNT * GameSettings.GetInstance().MAP_TILE_COLUMN_COUNT )  ) {
			Debug.LogError ( "<NavigationMap::SetCollisionMapType>, invalid index, row : " + row + ", column: " + column );
			return ;
		}
		
		collision_map_[row,column] = type;
	}
	
    private static NavigationMap s_instance;
    public static NavigationMap GetInstance() {
        if ( !s_instance ) {
            s_instance = (NavigationMap)GameObject.FindObjectOfType ( typeof ( NavigationMap ) );
            if ( !s_instance )
                Debug.LogError("There needs to be one active GameMap script on a GameObject in your scene.");
        }
        return s_instance;
    }
}

