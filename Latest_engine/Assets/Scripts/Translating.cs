using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class Translating : MonoBehaviour {

	//Character Controller
	private Rigidbody controller;
	private BoxCollider collider;
	
	//Movement variables
	public float playerSpeed = 6.0f;
	private Vector3 movement = Vector3.zero;

    public bool movementEnabled = true;
	
	//Rotation variables
	//private Quaternion currentDir;
	private float targetDir = 0.0f;
	
	//Physics settings
	public float mass = 1.0f;
	/*
	//Setting up for custom controls
	public KeyCode moveUp = KeyCode.W;
	public KeyCode moveDown = KeyCode.S;
	public KeyCode moveLeft = KeyCode.A;
	public KeyCode moveRight = KeyCode.D;
    */

    public PlayerIndex playerNumber = PlayerIndex.Two;
    private GamePadState state;
    private int quadrant;
    private Vector3 currentDir = Vector3.zero;
	
	private Vector3 moveTester;
	private bool isMoveTesterAssigned = false;
	
	private coffin targetCoffin;
	public int coffinsTagged = 0;
	
	// Use this for initialization
	void Start () {
		if(!GetComponent("Rigidbody")){
			print("No Rigidbody connected! Creating component...");
			controller = gameObject.AddComponent("Rigidbody") as Rigidbody;
		}else{
			controller = rigidbody;
			print("Rigidbody already connected! Using current component...");
		}
		
		if(!GetComponent("BoxCollider")){
			print("No BoxCollider connected! Creating component...");
			collider = gameObject.AddComponent("BoxCollider") as BoxCollider;
		}else{
			//collider = collider;
			print("BoxCollider already connected! Using current component...");
		}		
	} 

	void FixedUpdate () {
        if (movementEnabled) {
            movement = Vector3.zero;
			
			if((moveTester != transform.position)&& (isMoveTesterAssigned)){
				animation.CrossFade ("walk");
			}
			else{
				animation.Play("idle", PlayMode.StopAll);
			}
            state = GamePad.GetState(playerNumber);
            movement.x = state.ThumbSticks.Left.X;
            movement.z = state.ThumbSticks.Left.Y;
            transform.position += movement * Time.deltaTime * playerSpeed;
			moveTester = transform.position;
			isMoveTesterAssigned = true;

            targetDir = Mathf.Atan(state.ThumbSticks.Left.Y / state.ThumbSticks.Left.X);
            targetDir *= Mathf.Rad2Deg;

            //Determine quadrant
            if (state.ThumbSticks.Left.Y >= 0 && state.ThumbSticks.Left.X >= 0)
                quadrant = 1;
            else if (state.ThumbSticks.Left.Y >= 0 && state.ThumbSticks.Left.X < 0)
                quadrant = 2;
            else if (state.ThumbSticks.Left.Y < 0 && state.ThumbSticks.Left.X < 0)
                quadrant = 3;
            else if (state.ThumbSticks.Left.Y < 0 && state.ThumbSticks.Left.X >= 0)
                quadrant = 4;

            //Convert to 0-360 degrees
            switch (quadrant) {
                case 2:
                    targetDir += 180;
                    break;
                case 3:
                    targetDir += 180;
                    break;
                case 4:
                    targetDir += 360;
                    break;
                default:
                    break;
            }

            currentDir = transform.eulerAngles;
			currentDir.x = 0;
            currentDir.y = -targetDir+90;
			currentDir.z = 0;

            if (!float.IsNaN(currentDir.y))
                transform.eulerAngles = currentDir;
        }
	}
	
	void OnTriggerEnter (Collider targetCollider) {
		if(targetCollider != null){
			targetCoffin = targetCollider.GetComponent("coffin") as coffin;
			
			if(!targetCoffin.hasBeenPlayed){
				targetCoffin.animation.Play("open", PlayMode.StopAll);
				coffinsTagged++;
				targetCoffin.hasBeenPlayed = true;
			}
		}
	}
}
