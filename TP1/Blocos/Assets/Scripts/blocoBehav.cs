using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class blocoBehav : MonoBehaviour {

	public float angle;

	public float output;
	public int numObjects;
    public int funcao; //escolha no unity se a funçao de ativaçao é linear ou gaussiana

    //parametros de escolha nos sensores no unity para calculo de funcoes de ativaçao
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
		GameObject[] blocos = GetBlocosVisiveis(); 
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
            //limites no eixo do x, se o output calculado anteriormente (strength) nao estiver compreendido entre os limites entao é igual a zero o output da funçao de ativaçao 
            if (output <= limiteInf)
            {
                res = 0f;
            }
            else if (output >= limiteSup)
            {
                res = 0f;
            }

            //x da funçao esta compreendido entre os limites em x
            else
            { 
                res = Mathf.Exp(-(Mathf.Pow(output - mean, 2) / 2 * Mathf.Pow(desv, 2)));  // aplicar formula de gaussiana pq x esta entre limites 

                //limites de y apos funçao gaussiana; se o y calculado passar de thresholdSup entao passa a ser igual a thresholdSup e se for menor que thresholdInf passa a ser igual a thresholdInf
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
