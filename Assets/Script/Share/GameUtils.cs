using UnityEngine;

public class GameUtils {
	//compares two real numbers. Returns true if they are equal
	static public bool isEqual(float a, float b) {
  		return (Mathf.Abs(a-b) < 1E-12);
	}
	
	static public bool isVectorEqual(Vector3 a, Vector3 b) {
		float percentageDifferenceAllowed = 0.01f;
		return (a - b).sqrMagnitude <= (a * percentageDifferenceAllowed).sqrMagnitude;
	}
	
	// as above, but avoiding sqrt
	static public float DistToLineSegmentSq(Vector3 A,
                                 			Vector3 B,
                                 			Vector3 P) {
  		//if the angle is obtuse between PA and AB is obtuse then the closest
  		//vertex must be A
  		float dotA = (P.x - A.x)*(B.x - A.x) + (P.y - A.y)*(B.y - A.y);

  		if (dotA <= 0) {
  			return (A-P).sqrMagnitude;
		}
		
  		//if the angle is obtuse between PB and AB is obtuse then the closest
  		//vertex must be B
  		float dotB = (P.x - B.x)*(A.x - B.x) + (P.y - B.y)*(A.y - B.y);
  		
  		if (dotB <= 0) {
			return (B-P).sqrMagnitude;
		}
		
  		//calculate the point along AB that is the closest to P
  		Vector3 Point = A + ((B - A) * dotA)/(dotA + dotB);
  		
  		//calculate the distance P-Point
  		return (P-Point).sqrMagnitude;
	}
	
  	static public bool isZero(Vector3 v) {
		const float tolerance = 0.1f;
		return v.sqrMagnitude < tolerance;
	}
	
	static public void Clamp(int val, int minVal, int maxVal) {
		if ( maxVal < minVal ) {
			Debug.LogError ( "GameUtils::Clamp: minVal is bigger than maxVal!" );
			return ;
		}
		
  		if (val < minVal) {
			val = minVal;
		}
		
  		if (val > maxVal) {
			val = maxVal;
		}
	}
	
	static public void Clamp(float val, float minVal, float maxVal) {
		if ( maxVal < minVal ) {
			Debug.LogError ( "GameUtils::Clamp: minVal is bigger than maxVal!" );
			return ;
		}
		
  		if (val < minVal) {
			val = minVal;
		}
		
  		if (val > maxVal) {
			val = maxVal;
		}
	}
	
	// given a line segment AB and a point P, this function calculates the 
	// perpendicular distance between them
	static public float DistToLineSegment(	Vector3 A,
                                			Vector3 B,
                                			Vector3 P) {
		//if the angle is obtuse between PA and AB is obtuse then the closest
  		//vertex must be A
  		float dotA = (P.x - A.x)*(B.x - A.x) + (P.y - A.y)*(B.y - A.y);
  		
  		if (dotA <= 0) return (A-P).magnitude;
  		
  		//if the angle is obtuse between PB and AB is obtuse then the closest
  		//vertex must be B
  		float dotB = (P.x - B.x)*(A.x - B.x) + (P.y - B.y)*(A.y - B.y);
  		
  		if (dotB <= 0) return (B-P).magnitude;
  		
  		//calculate the point along AB that is the closest to P
  		Vector3 Point = A + ((B - A) * dotA)/(dotA + dotB);
  		
  		//calculate the distance P-Point
  		return (P-Point).magnitude;
	}
	
	static public bool isInFieldOfView(Vector2 triggerBotMapPos, Vector2 targetBotMapPos, int fov) {
		int fieldOfView = fov;
		Vector2 startPosition = new Vector2(triggerBotMapPos.x - fov, triggerBotMapPos.y - fov);
		Vector2 checkPosition;
		
    	for (int i = -fieldOfView; i <= fieldOfView; i++) {
        	for (int j = -fieldOfView; j <= fieldOfView; j++) {
				checkPosition = new Vector2(startPosition.x + (j+fieldOfView),
											startPosition.y + (i+fieldOfView));
            	if (Mathf.Abs(i) + Mathf.Abs(j) <= fieldOfView ) {
					if((targetBotMapPos-checkPosition).sqrMagnitude < 1) {
						return true;
					}
				}
			}
		}
		return false;
	}
	
	static public bool isSecondInFOVOfFirst(Vector3 posFirst,
                                 			Vector3 facingFirst,
                                 			Vector3 posSecond,
                                 			int fov) {
		bool result = isInFieldOfView(GameUtils.Real2MapPos(posFirst), GameUtils.Real2MapPos(posSecond), fov);
		return result;
	}
	
	static public void Truncate(ref Vector3 v, float max) {
  		if ( v.magnitude > max ) {
    		v = v.normalized * max;
		}
	}
	
	static public Vector2 Real2MapPos( Vector3 position ) {
		return new Vector2( (int) ( Mathf.Round(position.x / GameSettings.GetInstance().TILE_WIDTH) ),
							(int) ( Mathf.Round(position.y / GameSettings.GetInstance().TILE_HEIGHT) ) );
	}
	
	static public Vector3 Map2RealPos( Vector2 position ) {
		return new Vector3( position.x * GameSettings.GetInstance().TILE_WIDTH,
							position.y * GameSettings.GetInstance().TILE_HEIGHT,
							0f );
	}
	
  	//returns true if the bot is close to the given position
  	static public bool isAtPosition(Vector3 pos1, Vector3 pos2) {
  		const float tolerance = 3.0f;
  		return (pos1-pos2).sqrMagnitude < tolerance * tolerance;
	}
	
	static public Vector2 perp(Vector2 v) {
		return new Vector2(v.y, v.x);
	}
}