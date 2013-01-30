using UnityEngine;
using System.Collections;
using XInputDotNetPure;

/*
 * note to self
 * MAKING THIS FUCKING MODULAR IN THE FUTURE >:(
 */

public class Monster : MonoBehaviour {
	//Character Controller
	private Rigidbody controller;
	private BoxCollider collider;
	
	public float playerSpeed = 5.0f;
	public float maxDistance = 100.0f;	
	public float controllerVib = 0.01f;
	public GameObject target1;
	public GameObject target2;
	public GameObject target3;
	public float playSpaceX = 25.0f;
	public float playSpaceZ = 25.0f;
	
	private float distance1;
	private float distance2;
	private float distance3;
	private Vector3 movement = Vector3.zero;
	private float currentTime;
	
	private Vector3 tempPos = Vector3.zero;
	
	public float rotationSpeed = 300.0f;
	private Vector3 currentDir = Vector3.zero;
	private float targetDir = 0.0f;
	
	//sin wave
	private float sinTime = 0.0f;
	private bool heartBeatVibrate = true;
	
	private GamePadState state;
	
	private int closestTarget = 0;

	private int quadrant;
	
	void Start () {
		/*
		if(!GetComponent("Rigidbody")){
			print("No Rigidbody connected! Creating component...");
			controller = gameObject.AddComponent("Rigidbody") as Rigidbody;
		}else{
			print("Rigidbody already connected! Using current component...");
		}
		
		if(!GetComponent("BoxCollider")){
			print("No BoxCollider connected! Creating component...");
			collider = gameObject.AddComponent("BoxCollider") as BoxCollider;
		}else{
			print("BoxCollider already connected! Using current component...");
		}
		*/		
	}
	
	void Update () {
		currentTime = Time.realtimeSinceStartup;
		
		//Vibration!
		distance1 = Mathf.Abs(target1.transform.position.magnitude - transform.position.magnitude);
		distance2 = Mathf.Abs(target2.transform.position.magnitude - transform.position.magnitude);
		distance3 = Mathf.Abs(target3.transform.position.magnitude - transform.position.magnitude);
		//distance4 = Mathf.Abs(target4.transform.position.magnitude - transform.position.magnitude);
		
		if((distance1 < maxDistance) || (distance2 < maxDistance) || (distance3 < maxDistance)) {
			if((distance1 < distance2) && (distance1 < distance3))
				closestTarget = 1;
			if((distance2 < distance1) && (distance2 < distance3))
				closestTarget = 2;
			if((distance3 < distance1) && (distance3 < distance2))
				closestTarget = 3;
			
			//Debug.Log (distance1 + "\t" + distance2 + "\t" + distance3 + "\t" + distance4 + "\t" + closestTarget);
			
			switch(closestTarget){
			case 1:
				controllerVib = (maxDistance - distance1)/maxDistance;
				break;
			case 2:
				controllerVib = (maxDistance - distance2)/maxDistance;
				break;
			case 3:
				controllerVib = (maxDistance - distance3)/maxDistance;
				break;
			}
			
			/*
			if(heartBeatVibrate){
				controllerVib = Mathf.Sin(sinTime);
			}
			sinTime++;
			if(controllerVib == 0)
				heartBeatVibrate = false;
			*/
            if (controllerVib < 0.5) {
                controllerVib = 0.25f;
                //controllerVib = Mathf.Sin(currentTime * 10);
            } else if (controllerVib >= 0.5) {
                controllerVib = 100;
            }else if(controllerVib <= -0.4)
				controllerVib = 0;
			
			
			
			//Debug.Log (controllerVib);
			
			//GamePad.SetVibration(0,controllerVib,controllerVib);
			GamePad.SetVibration(0,controllerVib, controllerVib);
		}
		
		GamePad.SetVibration(0,0,0);
		
		//Movement!
		movement = Vector3.zero;
		//currentDir = transform.rotation;
		
		state = GamePad.GetState(PlayerIndex.One);
		movement.x = state.ThumbSticks.Left.X;
		movement.z = state.ThumbSticks.Left.Y;
		transform.position += movement*Time.deltaTime*playerSpeed;
		
		targetDir = Mathf.Atan(state.ThumbSticks.Left.Y/state.ThumbSticks.Left.X);
		targetDir *= Mathf.Rad2Deg;
		
		//Determine quadrant
		if(state.ThumbSticks.Left.Y>=0 && state.ThumbSticks.Left.X>= 0)
			quadrant = 1;
		else if(state.ThumbSticks.Left.Y>=0 && state.ThumbSticks.Left.X<0)
			quadrant = 2;
		else if(state.ThumbSticks.Left.Y<0 && state.ThumbSticks.Left.X<0)
			quadrant = 3;
		else if(state.ThumbSticks.Left.Y<0 && state.ThumbSticks.Left.X>=0)
			quadrant = 4;
		
		//Convert to 0-360 degrees
		switch(quadrant){
		case 2:
			targetDir+=180;
			break;
		case 3:
			targetDir+=180;
			break;
		case 4:
			targetDir+=360;
			break;
		default:
			break;
		}
		
		//print (targetDir);
		
		tempPos = transform.localPosition;
		if(tempPos.x > playSpaceX)
			tempPos.x = playSpaceX;
		if(tempPos.x < -playSpaceX)
			tempPos.x = -playSpaceX;
		if(tempPos.z > playSpaceZ)
			tempPos.z = playSpaceZ;
		if(tempPos.z < -playSpaceZ)
			tempPos.z = -playSpaceZ;
		transform.localPosition = tempPos;
		
		/*
		print (targetDir);
		float testLeft = targetDir - transform.rotation.y;
		float testRight = transform.rotation.y + (360-targetDir);
		
		if(testLeft < testRight)
			transform.Rotate(Vector3.up*Time.deltaTime*rotationSpeed);
			//currentDir.y += currentDir.y*Time.deltaTime;
		else if(testRight < testLeft)
			transform.Rotate(-Vector3.up*Time.deltaTime*rotationSpeed);
			//currentDir.y -= currentDir.y*Time.deltaTime;
		*/
		//Quaternion zQuaternion = Quaternion.AngleAxis(targetDir, Vector3.up);
		//transform.rotation = currentDir;	
		
		
		
		//Debug.Log(targetDir + "\t" + testLeft + "\t" + testRight + "\t" + currentDir);
		//transform.Rotate(Vector3.up*Time.deltaTime);
		
		//Debug.Log(targetDir);

		currentDir = transform.eulerAngles;
		currentDir.y = -targetDir;
		
		if(!float.IsNaN(currentDir.y))
			transform.eulerAngles = currentDir;
		//print (transform.eulerAngles.y);
	}
	
	void OnApplicationQuit () {
		GamePad.SetVibration(0,0,0);
	}
}
