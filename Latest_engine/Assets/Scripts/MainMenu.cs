using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed ||
            GamePad.GetState(PlayerIndex.Two).Buttons.A == ButtonState.Pressed ||
            GamePad.GetState(PlayerIndex.Three).Buttons.A == ButtonState.Pressed ||
            GamePad.GetState(PlayerIndex.Four).Buttons.A == ButtonState.Pressed) {

            Destroy(gameObject);
        }
	}

    void OnGUI() {
        if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height), "START")) {
            Destroy(gameObject);
        }
    }
}
