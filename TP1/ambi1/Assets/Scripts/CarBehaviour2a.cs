using UnityEngine;
using System.Collections;

public class CarBehaviour2a : CarBehaviour {

	//subclasse e carBehaviour usada nos calculos dos valores dos motores das rodas, através dos sensores
	
	void Update()
	{
		//Read sensor values

		//sensores de luz
		float leftSensor = LeftLD.getLinearOutput ();//sensor esquerdo ligado a roda esquerda
		float rightSensor = RightLD.getLinearOutput ();//sensor direito ligado a roda direita

		//sensores de proximidade a blocos; seu output irá ser tanto maior quanto menor for a sua distancia a um bloco devido ao 1/dist
        float leftSensorB = LeftB.getLinearOutput(); //sensor esquerdo ligado a roda esquerda
        float rightSensorB = RightB.getLinearOutput(); //sensor direito ligado a roda direita


        //Calculate target motor values  

		//caculo de velocidade de rodas, aplicando soma de outputs de sensores de luz e proximidade (nota: também podia ser aplicada média)
		// roda com maior valor da soma dos sensores irá ganhar mais velocidade fazendo carro prosseguir em direçao oposta a esta

        m_LeftWheelSpeed = (leftSensorB + leftSensor) * MaxSpeed;
        m_RightWheelSpeed = (rightSensorB + rightSensor) * MaxSpeed;
    }
}
