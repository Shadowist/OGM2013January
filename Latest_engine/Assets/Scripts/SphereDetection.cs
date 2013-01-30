using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshCollider))]
public class SphereDetection : MonoBehaviour {
	
	private Translating targetCollision;
	
	public bool playerOneDetect;
	public bool playerTwoDetect;
	public bool playerThreeDetect;
	
	public GameObject[] target = new GameObject[3];
	
	public AudioClip defaultAudioClip;
	
	private AudioSource targetAudioSource1;
	private AudioSource targetAudioSource2;
	private AudioSource targetAudioSource3;
	
	private bool allPlayers;
	private bool players123;
	private bool players12;
	private bool players13;
	private bool players23;
	private bool players1;
	private bool players2;
	private bool players3;
	
	// Use this for initialization
	void Start () {
		//rigidbody.isKinematic = true;
		playerOneDetect = false;
		playerTwoDetect = false;
		playerThreeDetect = false;
		
		targetAudioSource1 = target[0].GetComponent("AudioSource") as AudioSource;
		targetAudioSource2 = target[1].GetComponent("AudioSource") as AudioSource;
		targetAudioSource3 = target[2].GetComponent("AudioSource") as AudioSource;
	}
	
	// Update is called once per frame
	void Update () {
		if(playerOneDetect || playerTwoDetect || playerThreeDetect){
			if(playerOneDetect && playerTwoDetect && playerThreeDetect)
				allPlayers = true;
			else if(playerOneDetect && playerTwoDetect && playerThreeDetect)
				players123 = true;
			else if(playerOneDetect && playerTwoDetect)
				players12 = true;
			else if(playerOneDetect && playerThreeDetect)
				players13 = true;
			else if(playerTwoDetect && playerThreeDetect)
				players23 = true;
			else if(playerOneDetect)
				players1 = true;
			else if(playerTwoDetect)
				players2 = true;
			else if(playerThreeDetect)
				players3 = true;
			
			Debug.Log (players3);
			
			if(allPlayers){
				if(!(targetAudioSource1.isPlaying && targetAudioSource2.isPlaying && targetAudioSource3.isPlaying)){
					targetAudioSource1.clip = defaultAudioClip;
					targetAudioSource2.clip = defaultAudioClip;
					targetAudioSource3.clip = defaultAudioClip;
					
					if(targetAudioSource1.enabled && targetAudioSource2.enabled && targetAudioSource3.enabled){
					targetAudioSource1.Play();
					targetAudioSource2.Play();
					targetAudioSource3.Play();
					}
				}
			} else if (players12) {
				if(!(targetAudioSource1.isPlaying && targetAudioSource2.isPlaying)){
					targetAudioSource1.clip = defaultAudioClip;
					targetAudioSource2.clip = defaultAudioClip;
					
					if(targetAudioSource1.enabled && targetAudioSource2.enabled){
					targetAudioSource1.Play();
					targetAudioSource2.Play();
					}
				}
			} else if (players13) {
				if(!(targetAudioSource1.isPlaying && targetAudioSource3.isPlaying)){
					targetAudioSource1.clip = defaultAudioClip;
					targetAudioSource3.clip = defaultAudioClip;
					
					if(targetAudioSource1.enabled && targetAudioSource3.enabled){
					targetAudioSource1.Play();
					targetAudioSource3.Play();
					}
				}
			} else if(players23) {
				if(!(targetAudioSource2.isPlaying && targetAudioSource3.isPlaying)){
					targetAudioSource2.clip = defaultAudioClip;
					targetAudioSource3.clip = defaultAudioClip;
					
					if(targetAudioSource2.enabled && targetAudioSource3.enabled){
					targetAudioSource2.Play();
					targetAudioSource3.Play();
					}
				}
			} else if(players1) {
				if(!targetAudioSource1.isPlaying){
					targetAudioSource1.clip = defaultAudioClip;
					
					if(targetAudioSource1.enabled)
						targetAudioSource1.Play();
				}
			} else if(players2) {
				if(!targetAudioSource2.isPlaying){
					targetAudioSource2.clip = defaultAudioClip;
					
					if(targetAudioSource2.enabled)
					targetAudioSource2.Play();
				}
			} else if(players3) {
				if(!targetAudioSource3.isPlaying){
					targetAudioSource3.clip = defaultAudioClip;
					
					if(targetAudioSource3.enabled)
					targetAudioSource3.Play();
				}
			}
			
			//targetAudioSource3.clip = defaultAudioClip;
			
			if(targetAudioSource1.isPlaying)
				Debug.Log("Player 1 - On");
			if(targetAudioSource2.isPlaying)
				Debug.Log("Player 2 - On");
			if(targetAudioSource3.isPlaying)
				Debug.Log("Player 3 - On");
		}
	}
	
	void OnTriggerStay (Collider collision) {
		//Debug.Log ("Trigger detected");
		
		//print (collision);
		targetCollision = collision.gameObject.GetComponent("Translating") as Translating;
		
		if(collision.gameObject.name == "Player 1"){
			Debug.Log("Player 1_Stay");
			playerOneDetect = true;
			
			if(targetCollision.movementEnabled)
				collision.gameObject.animation.Play("startle", PlayMode.StopAll);
		}
		if(collision.gameObject.name == "Player 2"){
			Debug.Log("Player 2_Stay");
			playerTwoDetect = true;
			
			if(targetCollision.movementEnabled)
				collision.gameObject.animation.Play("startle", PlayMode.StopAll);
		}
		if(collision.gameObject.name == "Player 3"){
			Debug.Log("Player 3_Stay");
			playerThreeDetect = true;
			
			if(targetCollision.movementEnabled)
				collision.gameObject.animation.Play("startle", PlayMode.StopAll);
		}			
	}
	
	void OnTriggerExit(Collider uncollision)
	{
		
		if(uncollision.gameObject.name == "Player 1"){
			Debug.Log("Player 1_Unhit");
			playerOneDetect = false;
		}
		if(uncollision.gameObject.name == "Player 2"){
			Debug.Log("Player 2_Unhit");
			playerTwoDetect = false;
		}
		if(uncollision.gameObject.name == "Player 3"){
			Debug.Log("Player 3_Unhit");
			playerThreeDetect = false;
		}
	}
}
