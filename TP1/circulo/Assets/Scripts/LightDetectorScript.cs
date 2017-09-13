using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class LightDetectorScript : MonoBehaviour {

	public float angle;

	public float output;
	public int numObjects;
    public int funcao; //escolha no unity se a funçao de ativaçao é linear ou gaussiana

    //parametros de escolha nos sensores no unity para calculo de uncoes de ativaçao
    public float mean; //media da gaussiana
    public float desv; //desvio da gaussiana
    public float limiteSup; //limite superior no eixo dos x da funçao de ativaçao
    public float limiteInf; //limite inferior no eixo dos x da funcao de ativaçao
    public float thresholdSup;//limite superior no eixo dos y da funçao de ativaçao
    public float thresholdInf; //limite inferior no eixo dos y da funçao de ativaçao


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
	public float getLinearOutput()
	{
		return output;
	}
	// Get Sensor output value
	public float getOutput(){
		float a; //output de funcoes de ativaçao
        if (funcao == 1) { //funcao==1 é linear
			a=getLinear(); 
		} 
		else{ //funcao==2 é gaussiana 
			a=getGaussiano (); 
		} 
		return a;
	}


	public float getLinear(){
        // nesta funçao o y=x , sendo que x é igual ao output calculado no update, ou seja = 1/distanciaAoBlocoMaisProximoAoSensor
        float res = output;
        //limites no eixo dos x, se x nao estiver compreendido entre os limites entao y =0
        if (output <= limiteInf) {
			res = 0f;
		}
		if (output >= limiteSup) {
			res = 0f;
		}
        //limites no eixo dos y; todo y (que neste caso é igual a output) que passar limiteSup é igual a limiteSup e que for menor que limiteInf é igual a limiteInf
        if (output <= thresholdInf) {
			res = thresholdInf;
		}
		if (output >= thresholdSup) {
			res = thresholdSup;
		} 
		return res;

	}

    //funçao de ativaçao gaussiana
    public float getGaussiano(){
		float res;// resultado y da funçao de ativaçao, ou seja, o output
        //limites no eixo do x, se o output calculado anteriormente (strength) nao estiver compreendido entre os limites entao é igual a zero o output da funçao de ativaçao 
        if (output <= limiteInf)  
		{
			res = 0f; 
		}
		else if (output >= limiteSup)
		{
			res = 0f; 
		}
        else   //x da funçao esta compreendido entre os limites em x
        {
            res = Mathf.Exp(-(Mathf.Pow(output - mean, 2) / 2 * Mathf.Pow(desv, 2))); // aplicar formula de gaussiana pq x esta entre limites 

            //limites de y apos funçao gaussiana; se oy calculado passar de thresholdSup entao passa a ser igual a thresholdSup e se for menor que thresholdInf passa a ser igual a thresholdInf
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
