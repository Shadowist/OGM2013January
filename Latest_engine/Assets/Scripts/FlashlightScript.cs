using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class FlashlightScript : MonoBehaviour
{
	public Light flashLight;
	public Light areaLight;
	public float duration = 3.0f;
	public int maxSpotRange = 40;
	public int minSpotRange = 17;
	public int maxAreaRange = 20;
	public int minAreaRange = 10;
	public float minSpotIntensity = 1.5f;
	public float maxSpotIntensity = 1.5f;
	public float minAreaIntensity = 1.5f;
	public float maxAreaIntensity = 1.5f;
	public float flickerSpeed = 2.0f;
	public float rechargeTime = 2.0f;
	public float flickerLength = 0.25f;
    public PlayerIndex playerNumber = PlayerIndex.Two;
	//public KeyCode activateKey = KeyCode.E;
	
	private bool isLightCharged = true;
	private bool isLightOn = false;
	private float tempTime;
	private bool isTimeAssigned = false;

    public bool isActivated = true;

	void Start()
	{
		// minimum
			flashLight.range = minSpotRange;
			flashLight.intensity = minSpotIntensity;
			flashLight.spotAngle = 40;
			flashLight.color = new Color32(242,211,128,255);
			areaLight.range = minAreaRange;
			areaLight.intensity = minAreaIntensity;
			areaLight.color = new Color32(242,211,128,255);
	}
	void Update ()
	{
        if (isActivated) {

            if (!flashLight.enabled)
                flashLight.enabled = true;
            if (!areaLight.enabled)
                areaLight.enabled = true;

            if (GamePad.GetState(playerNumber).Buttons.A == ButtonState.Pressed) {
                if (isLightCharged) {
                    Debug.Log("Flashlight On!");
                    isLightOn = true;
                    flashLight.range = maxSpotRange;
                    flashLight.intensity = maxSpotIntensity;
                    flashLight.spotAngle = 45;
                    flashLight.color = (Color.white * new Color32(242, 211, 128, 255));
                    areaLight.range = maxAreaRange;
                    areaLight.intensity = maxAreaIntensity;
                    areaLight.color = new Color32(242, 211, 128, 255);
                } else {
                    Debug.Log("You're flashlight is not charged!");
                }
            }
            if (isLightOn) {
                ChargeFlashlight();
            }
        } else {
            if (flashLight.enabled)
                flashLight.enabled = false;
            //if (areaLight.enabled)
            //    areaLight.enabled = false;
        }
	}
	
	void ChargeFlashlight()
	{
		isLightCharged = false;
		bool isLightReady = false;
		if(!isTimeAssigned)
		{
			tempTime = Time.timeSinceLevelLoad;
			// Debug.Log (tempTime);
			isTimeAssigned = true;
		}
		if((Time.timeSinceLevelLoad >= (tempTime + duration)) && Time.timeSinceLevelLoad < (tempTime + (duration + 2.0f)))
		{	
			// Debug.Log("Light Flicker");
			Color a = (Color.white*new Color32(242,211,128,255));
			Color b = Color.clear;
			float t = (Mathf.PingPong(Time.time, (flickerSpeed*0.1f))/(flickerSpeed*flickerLength)) * (flickerLength*10f);
			flashLight.color = Color.Lerp(b,a,t);
			flashLight.range = Mathf.Lerp( maxSpotRange,minSpotRange,t);
			flashLight.intensity = Mathf.Lerp(maxSpotIntensity,minSpotIntensity,t);
			areaLight.intensity = minAreaIntensity;
			areaLight.range = Mathf.Lerp(maxAreaRange,minAreaRange,(t*2));
			areaLight.color = new Color32(242,211,128,255);
		}

		if((Time.timeSinceLevelLoad >= (tempTime + (duration + 2.0f)))
			&& (Time.timeSinceLevelLoad < (tempTime + (duration + rechargeTime))))
		{
			// Debug.Log ("Light off");
			flashLight.range = minSpotRange;
			flashLight.intensity = minSpotIntensity;
			flashLight.spotAngle = 40;
			flashLight.color = new Color32(242,211,128,255);
			areaLight.range = minAreaRange;
			areaLight.intensity = minAreaIntensity;
			areaLight.color = new Color32(242,211,128,255);
		}
		if((Time.timeSinceLevelLoad >= (tempTime +(duration + rechargeTime)))
			&& (Time.timeSinceLevelLoad < (tempTime +(duration + (rechargeTime+2f)))))
		{
			// Debug.Log ("areaLight flicker");
			// Debug.Log (tempTime +(duration + (rechargeTime+2f)));
			Color a = Color.white;
			Color b = Color.green;
			float t = (Mathf.PingPong(Time.time, (flickerSpeed*0.1f))/(flickerSpeed*flickerLength)) * (flickerLength*10f);
			areaLight.color = Color.Lerp(a,b,t);
		}
		if(Time.timeSinceLevelLoad >= (tempTime +(duration + (rechargeTime+2f))))
		{
			// Debug.Log ("Light Charged");
			areaLight.color = new Color32(242,211,128,255);
			isLightCharged = true;
			isLightOn = false;
			isTimeAssigned = false;
		}
	}
}
