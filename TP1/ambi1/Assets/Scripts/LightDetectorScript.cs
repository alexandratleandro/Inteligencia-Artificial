using UnityEngine;
using System.Collections;
using System.Linq;
using System;

//classe sensores de intensidade de luz

public class LightDetectorScript : MonoBehaviour {

	public float angle; //angulo sensor

	public float output; //valor calculado sensor
	public int numObjects; //numero de luzes
  

    void Start () {
		output = 0;
		numObjects = 0;
	}

	void Update () {
		GameObject[] lights = GetVisibleLights(); //get de luz visiveis pelo sensor

		output = 0;
		numObjects = lights.Length;
	
		foreach (GameObject light in lights) { //calculo de intensidade de luz de cada luz e soma linear ao output
			float r = light.GetComponent<Light>().range;
			output += 1f / Mathf.Pow((transform.position - light.transform.position).magnitude / r + 1, 2);
		}

		if(numObjects>0)
			output = output/numObjects; //dividir soma das intensidades pelo numero de luzes
	}

	// Get Sensor output value
	public float getLinearOutput(){ //get do output calculado

		return output;
	} 
   
    // Returns all "Light" tagged objects. The sensor angle is not taken into account.
	//todas as luzes do ambiente
    GameObject[] GetAllLights()
	{
		return GameObject.FindGameObjectsWithTag ("Light");
	}

	// Returns all "Light" tagged objects that are within the view angle of the Sensor. Only considers the angle over 
	// the y axis. Does not consider objects blocking the view.

	//luzes visiveis pelo sensor
	GameObject[] GetVisibleLights()
	{
		ArrayList visibleLights = new ArrayList();
		float halfAngle = angle / 2.0f;

		GameObject[] lights = GameObject.FindGameObjectsWithTag ("Light");

		foreach (GameObject light in lights) {
			Vector3 toVector = (light.transform.position - transform.position);
			Vector3 forward = transform.forward;
			toVector.y = 0;
			forward.y = 0;
			float angleToTarget = Vector3.Angle (forward, toVector);

			if (angleToTarget <= halfAngle) {
				visibleLights.Add (light);
			}
		}

		return (GameObject[])visibleLights.ToArray(typeof(GameObject));
	}


}
