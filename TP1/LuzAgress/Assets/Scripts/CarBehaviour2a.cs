using UnityEngine;
using System.Collections;

public class CarBehaviour2a : CarBehaviour {

	// calsse filha de carbehaviour 
	
	void Update()
	{

         float leftSensor = LeftLD.getOutput();
        float rightSensor = RightLD.getOutput();
        float leftSensorB = LeftB.getOutput();
        float rightSensorB = RightB.getOutput();

        // outputs dos sensor de luminosidade dão energia à roda contraria para carro ir na direção da luz em vez de se desviar
        m_LeftWheelSpeed = (leftSensorB + rightSensor) * MaxSpeed;  
        m_RightWheelSpeed = (rightSensorB + leftSensor) * MaxSpeed; 



    }
}
