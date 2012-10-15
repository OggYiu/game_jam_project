using UnityEngine;
using System.Collections;

public class Scene_Intro : Scene {
	void OnNextBtnClicked ( GameObject obj ) {
		SceneManager.GetInstance().ChangeScene ( "Scene_Game" );
	}
}

