using UnityEngine;
using System.Collections;

//classe usada nos sensores esquerdo e direito de proximidade a blocos

public class BlockDetectorScript : MonoBehaviour {

    public float angle; // angulo do sensor

    public float output; //output do sensor que irá ser calculado 
    public int numObjects; //número de blocos 
    

    void Start()
    {
        output = 0;
        numObjects = 0;
    }

    void Update()
    {
        GameObject[] blocos = GetBlocosVisiveis(); //get dos blocos visiveis ao sensor 

        numObjects = blocos.Length;

        if (numObjects > 0) //se existirem blocos visiveis, ou seja, nas proximidades
        {
            output = 0;
            //distancia do sensor ao primeiro bloco encontrado 
            float distProx = Vector3.Distance(transform.position, blocos[0].transform.position); //distancia ao primeiro bloco encontrado 

            foreach (GameObject bloco in blocos)
            {
                float distObj = Vector3.Distance(transform.position, bloco.transform.position); //distancia próximo bloco no array de blocos visiveis
                // calcula output consoante bloco mais proximo 
                if (distObj < distProx)
                {
					distProx = distObj;//se a distancia do proximo bloco for menor , atualizamos distancia mais próxima
                }
            }
			output = 1f / distProx; // 1/distanciaAoBlocoMaisProximo para output estar entre 0 e 1(nao ter velocidades muito grandes);

        }
    }

    public float getLinearOutput() //retornar output calculado
    {
        return output;
    }
    // Get Sensor output value
    
    GameObject[] GetBlocos() //devolve todos blocos do ambiente porque todos têm tag "Cube"
    {
        return GameObject.FindGameObjectsWithTag("Cube");
    }

	GameObject[] GetBlocosVisiveis() //retorna apenas os blocos visiveis ao sensor
    {
        ArrayList blocosVisiveis = new ArrayList();
        float halfAngle = angle / 2.0f;

        GameObject[] blocos = GameObject.FindGameObjectsWithTag("Cube");

        foreach (GameObject bloco in blocos)
        {
            Vector3 toVector = (bloco.transform.position - transform.position);
            Vector3 forward = transform.forward;

            toVector.y = 0;
            forward.y = 0;
            float angleToTarget = Vector3.Angle(forward, toVector);

            if (angleToTarget <= halfAngle)
            {
                blocosVisiveis.Add(bloco);
            }
        }

        return (GameObject[])blocosVisiveis.ToArray(typeof(GameObject));
    }
}
