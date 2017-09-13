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

        m_LeftWheelSpeed = (leftSensorB + leftSensor) * MaxSpeed;
        m_RightWheelSpeed = (rightSensorB + rightSensor) * MaxSpeed;

        
		
	}
}
