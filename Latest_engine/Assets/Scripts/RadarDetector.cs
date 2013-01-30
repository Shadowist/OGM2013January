/* 
 * Radar Detector.cs
 * Author: Matt Doucette!
 * 
 * Purpose: attach script to the "Radar Cone" object,
 * which is attached to the "Monster." The script will
 * notify the monster whether or not a "runner" is within
 * the scope of the radar.
 * 
 * 
 * Edited: Steven Burrichter :D
 * Added RequireComponent to save any confusion temporarily.
 * 
 * In order for players not to fall through, we need child colliders.
 * Adjusted name search to check the parent.
 * */



using UnityEngine;
using System.Collections;


[RequireComponent(typeof(MeshCollider))]
//[RequireComponent(typeof(Rigidbody))]
public class RadarDetector : MonoBehaviour
{
	public bool playerOneDetect;
	public bool playerTwoDetect;
	public bool playerThreeDetect;
	public bool playerFourDetect;
	
	public AudioClip closest;
	public AudioClip secondClosest;
	public AudioClip thirdClosest;
	public AudioClip fourthClosest;
	//public AudioClip fifthClosest;
	
	/*
	public GameObject target1;
	public GameObject target2;
	public GameObject target3;
	public GameObject target4;
	*/
	
	public GameObject[] target = new GameObject[3];
	private GameObject currentTarget;
	private int numberPlayersToCheck;
	private int maxPlayers = 4;
	
	public int closestTarget = 1;
	public float maxDistance = 10.0f;
	public float distance = 0.0f;
	
	private float[] playerDistances = new float[3];
	
	private float distance1;
	private float distance2;
	private float distance3;
	
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
	void Start()
	{
		//rigidbody.isKinematic = true;
		playerOneDetect = false;
		playerTwoDetect = false;
		playerThreeDetect = false;
		playerFourDetect = false;
		
		targetAudioSource1 = target[0].GetComponent("AudioSource") as AudioSource;
		targetAudioSource2 = target[1].GetComponent("AudioSource") as AudioSource;
		targetAudioSource3 = target[2].GetComponent("AudioSource") as AudioSource;
	}
	
	void Update ()
	{
		//Audio stuff!
		if(playerOneDetect || playerTwoDetect || playerThreeDetect){
			currentTarget = null;
			
			numberPlayersToCheck = 0;
			//playerDistances[0] = playerDistances[1] = playerDistances[2] = playerDistances[3] = 0f;
			playerDistances[0] = 0.0f;
			
			allPlayers = players123 = false;
			players12 = players13 = players23 = false;
			
			if(playerOneDetect)
				playerDistances[0] = Mathf.Abs(target[0].transform.position.magnitude - transform.position.magnitude);
			if(playerTwoDetect)
				playerDistances[1] = Mathf.Abs(target[1].transform.position.magnitude - transform.position.magnitude);
			if(playerThreeDetect)
				playerDistances[2] = Mathf.Abs(target[2].transform.position.magnitude - transform.position.magnitude);
			
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
			
			if(allPlayers){
				for(int i=0; i<=2; i++){
					for(int j=0; j<=2; j++){
						if(playerDistances[i] < playerDistances[j]){
							closestTarget = i;
							currentTarget = target[i];
						}
					}
				}
			} else if (players123) {
				for(int i=0; i<=2; i++){
					for(int j=0; j<=2; j++){
						if(playerDistances[i] < playerDistances[j]){
							closestTarget = i;
							currentTarget = target[i];
						}
					}
				}
			
			} else if (players12) {
				if(playerDistances[0] < playerDistances[1]){
					closestTarget = 0;
					currentTarget = target[0];
				} else {
					closestTarget = 1;
					currentTarget = target[1];
				}		
			} else if (players13) {
				if(playerDistances[0] < playerDistances[2]){
					closestTarget = 0;
					currentTarget = target[0];
				} else {
					closestTarget = 1;
					currentTarget = target[1];
				}
			} else if(players23) {
				if(playerDistances[1] < playerDistances[2]){
					closestTarget = 1;
					currentTarget = target[1];
				} else {
					closestTarget = 2;
					currentTarget = target[2];
				} 	
			} else if(players1) {
				closestTarget = 0;
				currentTarget = target[0];
			} else if(players2) {
				closestTarget = 1;
				currentTarget = target[1];
            } else if (players3) {
                closestTarget = 2;
                currentTarget = target[2];
            }
			
			//Debug.Log("Closest target = " + currentTarget);

			switch(closestTarget){
			case 0:
				distance = Mathf.Abs(maxDistance - playerDistances[0])/maxDistance;
				//if(distance >= 0.75)
				//	targetAudioSource1.clip = closest;
				if(distance < 0.75 && distance >= 0.5)
					targetAudioSource1.clip = secondClosest;
				else if(distance < 0.5 && distance >= 0.25)
					targetAudioSource1.clip = thirdClosest;
				else if(distance < 0.25 )
					targetAudioSource1.clip = fourthClosest;
			
				if(!targetAudioSource1.isPlaying)
					targetAudioSource1.Play();
				break;
			case 1:
				distance = Mathf.Abs(maxDistance - playerDistances[1])/maxDistance;
				//if(distance >= 0.75)
				//	targetAudioSource2.clip = closest;
				if(distance < 0.75 && distance >= 0.5)
					targetAudioSource2.clip = secondClosest;
				else if(distance < 0.5 && distance >= 0.25)
					targetAudioSource2.clip = thirdClosest;
				else if(distance < 0.25 )
					targetAudioSource2.clip = fourthClosest;
				
				if(!targetAudioSource2.isPlaying)
					targetAudioSource2.Play();
				break;
			case 2:
				distance = Mathf.Abs(maxDistance - playerDistances[2])/maxDistance;
				//if(distance >= 0.75)
				//	targetAudioSource3.clip = closest;
				if(distance < 0.75 && distance >= 0.5)
					targetAudioSource3.clip = secondClosest;
				else if(distance < 0.5 && distance >= 0.25)
					targetAudioSource3.clip = thirdClosest;
				else if(distance < 0.25 )
					targetAudioSource3.clip = fourthClosest;
			
				if(!targetAudioSource3.isPlaying)
					targetAudioSource3.Play();
				break;
			}
		}
	}
	void OnTriggerStay(Collider collision)
	{
		
		//Debug.Log ("Trigger detected");
			
		if(collision.gameObject.name == "Player 1"){
			Debug.Log("Player 1_Stay");
			playerOneDetect = true;
		}
		if(collision.gameObject.name == "Player 2"){
			Debug.Log("Player 2_Stay");
			playerTwoDetect = true;
		}
		if(collision.gameObject.name == "Player 3"){
			Debug.Log("Player 3_Stay");
			playerThreeDetect = true;
		}	
	}
		
	void OnTriggerExit(Collider uncollision)
	{
		if(uncollision.gameObject.name != null){
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
}
