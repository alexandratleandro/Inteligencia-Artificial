using UnityEngine;
using System.Collections;

public class CarBehaviour2a : CarBehaviour {

	// classe filha de carbehaviour 
	
	void Update()
	{

        float leftSensor = LeftLD.getOutput();
        float rightSensor = RightLD.getOutput();
        float leftSensorB = LeftB.getOutput();
        float rightSensorB = RightB.getOutput();
        float leftSensorB2 = LeftB.getOutput();
        float rightSensorB2 = RightB.getOutput();
        m_LeftWheelSpeed = ((leftSensorB + leftSensorB2)/2) * MaxSpeed;
        m_RightWheelSpeed = ((rightSensorB  + rightSensorB2)/2 ) * MaxSpeed;

        
		
	}
}
