using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class blocoBehav : MonoBehaviour {

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
		GameObject[] blocos = GetBlocosVisiveis();
       // GameObject[] paredes = GetParedes();
        numObjects = blocos.Length;

        if (numObjects > 0)
        {
            output = 0;  
            //distancia ao primeiro bloco encontrado 
            float distProx = Vector3.Distance(transform.position, blocos[0].transform.position); //distancia ao primeiro bloco encontrado 
             
            foreach (GameObject bloco in blocos) 
            {
                float distObj = Vector3.Distance(transform.position, bloco.transform.position);
                // calcula output consoante bloco mais proximo 
                if (distObj < distProx)
                {
                    distProx = distObj;
                } 
            }             
            output = 1f/distProx; 

        }
    }

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


	GameObject[] GetBlocos()
	{
		return GameObject.FindGameObjectsWithTag("Cube");
	}
  
    GameObject[] GetBlocosVisiveis()
	{
		ArrayList blocosVisiveis = new ArrayList();
		float halfAngle = angle / 2.0f;

		GameObject[] blocos= GameObject.FindGameObjectsWithTag ("Cube");

		foreach (GameObject bloco in blocos) {
			Vector3 toVector = (bloco.transform.position - transform.position);  
			Vector3 forward = transform.forward;

			toVector.y = 0;
			forward.y = 0;
			float angleToTarget = Vector3.Angle (forward, toVector);  

			if (angleToTarget <= halfAngle) {
				blocosVisiveis.Add (bloco);
			}
		}

		return (GameObject[])blocosVisiveis.ToArray(typeof(GameObject));
	}
}
