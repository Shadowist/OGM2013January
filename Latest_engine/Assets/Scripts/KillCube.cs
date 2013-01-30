using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class KillCube : MonoBehaviour {

    private Translating target;
    private FlashlightScript targetFlashlight;
	private AudioSource targetAudioSource;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider targetCollider) {
		if(targetCollider != null){
	        //Debug.Log(targetCollider.name + " killed");
	        target = targetCollider.gameObject.GetComponent("Translating") as Translating;
	        targetFlashlight = targetCollider.gameObject.GetComponent("FlashlightScript") as FlashlightScript;
			targetAudioSource = targetCollider.gameObject.GetComponent("AudioSource") as AudioSource;
			
			//if(target == null)
			//	Debug.Log ("Broken =(");
			
			if(target != null && targetFlashlight != null && targetAudioSource != null){
		        if(target.movementEnabled){ //FUUUUUUUUUU
					target.animation.Play("faint",PlayMode.StopAll);
				}
				
				target.movementEnabled = false;
		        targetFlashlight.isActivated = false;
				targetAudioSource.enabled = false;
				
		        GamePad.SetVibration(0, 1, 1);
			}
		}
    }

    void OnTriggerExit() {
        GamePad.SetVibration(0, 0, 0);
    }
}
