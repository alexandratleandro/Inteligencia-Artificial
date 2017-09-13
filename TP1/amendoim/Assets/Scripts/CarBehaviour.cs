using UnityEngine;
using System.Collections;

public class CarBehaviour : MonoBehaviour {
	
	public float MaxSpeed; //metemos no unity no objeto


     
    public WheelCollider RR; // é do unity -- colisao de rodas do lado direito
	public WheelCollider RL; //colisao rodas lados esquerdo
	public LightDetectorScript LeftLD; //chama script de luz --> esquerda
	public LightDetectorScript RightLD; //luz--> direita

	public blocoBehav LeftB; //chama script de luz --> esquerda
	public blocoBehav  RightB;

	private Rigidbody m_Rigidbody;
	protected float m_LeftWheelSpeed; //velocidade rodas esquerda
	protected float m_RightWheelSpeed; //velocidade rodas direita
	private float m_axleLength,angVelocity; //


	void Start()
	{
		m_Rigidbody = GetComponent<Rigidbody> (); //buscar componente rigidbody
		m_axleLength = (RR.transform.position - RL.transform.position).magnitude;
	}

	void FixedUpdate ()
    { 
        //andar para a frente
        //Calculate forward movement
        float targetSpeed = Mathf.Min(m_LeftWheelSpeed, m_RightWheelSpeed); //quando se corrige movimento escolhe velocidade minima das rodas
		Vector3 movement = transform.forward * targetSpeed * Time.deltaTime;
        
        float angVelocity = (m_LeftWheelSpeed - m_RightWheelSpeed) / m_axleLength * Mathf.Rad2Deg * Time.deltaTime; // angulo de viragem 
		
		Quaternion turnRotation = Quaternion.Euler (0f, angVelocity, 0f); //rotaçoes
		m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
		m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation); 

    }
}