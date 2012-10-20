using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavGraphRenderer
{
	private bool renderDone_ = false;
	private NavGraph navGraph_ = null;
	
	public void Render(NavGraph navGraph) {
		navGraph_ = navGraph;
//		Camera gameCamera = GameApp.GetInstance().main_camera;
//		tk2dCamera gameCamera = GameDirector.GetInstance().ma
		
		// display occupied bot
//		{
//			List<NavGraphNode> nodes = navGraph_.GetNodes();
//			Bot occupiedBot = null;
//			NavGraphNode targetNode = null;
//			Vector3 nodeRealPos;
//			for ( int i=0; i<nodes.Count; ++i ) {
//				targetNode = nodes[i];
//				occupiedBot = targetNode.GetOccupiedBot();
//				
//				if ( occupiedBot ) {
//					nodeRealPos = GameUtils.Map2RealPos( targetNode.Position() );
//					GUI.Label( new Rect(nodeRealPos.x - gameCamera.transform.position.x - GameSettings.GetInstance().TILE_SIZE / 2,
//										GameSettings.GetInstance().MAP_HEIGHT - nodeRealPos.y + gameCamera.transform.position.y - GameSettings.GetInstance().TILE_SIZE / 2,
//										300, 300), occupiedBot.gameObject.name );
//				}
//			}
//		}
		
		if ( renderDone_ ) {
			return ;
		}
		
		// render all nodes position
		{
			List<NavGraphNode> nodes = navGraph_.GetNodes();
			Vector2 nodePos;
			NavGraphNode targetNode = null;
			for ( int i=0; i<nodes.Count; ++i ) {
				targetNode = nodes[i];
				nodePos = targetNode.Position();
				
				Vector3[] targetPositions = {	new Vector3(nodePos.x * GameSettings.GetInstance().TILE_SIZE + GameSettings.GetInstance().TILE_SIZE / 2,
															nodePos.y * GameSettings.GetInstance().TILE_SIZE + GameSettings.GetInstance().TILE_SIZE / 2,
															0.0f ),
												new Vector3(nodePos.x * GameSettings.GetInstance().TILE_SIZE - GameSettings.GetInstance().TILE_SIZE / 2,
															nodePos.y * GameSettings.GetInstance().TILE_SIZE + GameSettings.GetInstance().TILE_SIZE / 2,
															0.0f ),
												new Vector3(nodePos.x * GameSettings.GetInstance().TILE_SIZE - GameSettings.GetInstance().TILE_SIZE / 2,
															nodePos.y * GameSettings.GetInstance().TILE_SIZE - GameSettings.GetInstance().TILE_SIZE / 2,
															0.0f ),
												new Vector3(nodePos.x * GameSettings.GetInstance().TILE_SIZE + GameSettings.GetInstance().TILE_SIZE / 2,
															nodePos.y * GameSettings.GetInstance().TILE_SIZE - GameSettings.GetInstance().TILE_SIZE / 2,
															0.0f ) };
				GameGLQuads quads = GameGLRenderer.GetInstance().AddQuads(targetPositions);
				quads.SetColor( new Color(1.0f, 0.0f, 0.0f, 0.5f) );
			}
		}
		
		// render all edges position
		{
			List<NavGraphEdge> targetGraphList = null;
	  		List< List<NavGraphEdge> > edgeListList = navGraph_.GetEdgeListList();
			NavGraphEdge targetEdge = null;
			for ( int i=0; i<edgeListList.Count; ++i ) {
				targetGraphList = edgeListList[i];
				for ( int j=0; j<targetGraphList.Count; ++j ) {
					targetEdge = targetGraphList[j];
					
					NavGraphNode nodeFrom = navGraph_.GetNode(targetEdge.From());
					NavGraphNode nodeTo = navGraph_.GetNode(targetEdge.To());
					Vector2 nodeFromPos = nodeFrom.Position();
					Vector2 nodeToPos = nodeTo.Position();
					Vector3[] edgePositions = { new Vector3(nodeFromPos.x * GameSettings.GetInstance().TILE_SIZE, nodeFromPos.y * GameSettings.GetInstance().TILE_SIZE, 0.0f),
												new Vector3(nodeToPos.x * GameSettings.GetInstance().TILE_SIZE, nodeToPos.y * GameSettings.GetInstance().TILE_SIZE, 0.0f) };
					GameGLLines lines = GameGLRenderer.GetInstance().AddLines(edgePositions);
					//lines.transform.position = new Vector3(nodeFromPos.x * GameSettings.GetInstance().TILE_SIZE, nodeFromPos.y * GameSettings.GetInstance().TILE_SIZE, -3.0f);
					lines.SetColor( new Color(0.0f, 1.0f, 0.0f, 0.5f) );
				}
			}
		}
		
		renderDone_ = true;
	}
}

