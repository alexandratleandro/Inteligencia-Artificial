using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class LightDetectorScript : MonoBehaviour {

	public float angle;

	public float output;
	public int numObjects;
    public float mean, desv, limiteSup, limiteInf, thresholdSup, thresholdInf;
    public int funcao; // 1= linear, 2=gauss

    void Start () {
		output = 0;
		numObjects = 0;
	}

	void Update () {
		GameObject[] lights = GetVisibleLights (); //vai buscar todos objetos com nome light--> esta mais abaixo

		output = 0;
		numObjects = lights.Length;  
	
		foreach (GameObject light in lights) { 
			float r = light.GetComponent<Light>().range; //raio da luz que aparec no unity
			output += 1f / Mathf.Pow((transform.position - light.transform.position).magnitude / r + 1, 2); //magnitude--> The length of the vector is square root of (x*x+y*y+z*z).
		}

		if(numObjects>0)
			output = output/numObjects;
	}

    // Get Sensor output value
    public float getOutput()
    {
        float res;
        if (funcao == 1)
        { //funcao==1 é linear

            res = output;

            if (output <= limiteInf)
            {
                res = 0f; 
            }
            if (output >= limiteSup)
            {
                res = 0f; 
            }
            if (output <= thresholdInf)
            {
                res = thresholdInf; 
            }
            if (output >= thresholdSup)
            {
                res = thresholdSup; 
            }

            return res;

        }

        else { //funcao==2 é gaussiana

            if (output <= limiteInf)
            {
                res = 0f;
            }
            else if (output >= limiteSup)
            {
                res = 0f;
            }
            else
            {
                res = Mathf.Exp(-(Mathf.Pow(output - mean, 2) / 2 * Mathf.Pow(desv, 2)));

                if (res <= thresholdInf)
                {
                    res = thresholdInf;
                }
                if (res >= thresholdSup)
                {
                    res = thresholdSup;
                }

            }

            return res; 
        }
    }

    // Returns all "Light" tagged objects. The sensor angle is not taken into account.

    GameObject[] GetAllLights()
	{
		return GameObject.FindGameObjectsWithTag ("Light");
	}

	// Returns all "Light" tagged objects that are within the view angle of the Sensor. Only considers the angle over 
	// the y axis. Does not consider objects blocking the view.
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
			float angleToTarget = Vector3.Angle (forward, toVector); //Returns the angle in degrees between from forward and to toVector.

			if (angleToTarget <= halfAngle) {
				visibleLights.Add (light);
			}
		}

		return (GameObject[])visibleLights.ToArray(typeof(GameObject));
	}


}
