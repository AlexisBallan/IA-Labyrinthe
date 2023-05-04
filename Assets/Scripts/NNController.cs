using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NNController : MonoBehaviour
{
    private Transform m_transf;

    private Vector3 m_movDir = Vector3.zero;
    private float m_rotSpeed = 300f;
    private float m_speed = 0.1f;
    private float m_initialVelocity = 0f;
    private float m_finalVelocity = .5f;
    public float m_currentVelocity = 0f;
    private float m_accelerationRate = 0.1f;
    private float m_decelerationRate = 0.8f;

    public float maxDistance = 30f;
    public float distForward = 30f;
    public float distLeft = 30f;
    public float distRight = 30f;
    public float distDiagLeft = 30f;
    public float distDiagRight = 30f;
    public bool AfficherRayon = false;

    public bool estMort = false;

    //Gérer le fitness
    public float fitness = 0f;
    private Vector3 m_lastPosition;
    private float m_distanceTraveled, m_distanceTraveled2;
    public float[] results;
    public bool active = true;

    private GameObject[] listeCheckPoint = new GameObject[30];
    private int nombreCheckpoint = 1;

    private float timer = 25f;

    private void Start()
    {
        m_lastPosition = transform.position;
        estMort = false;
        Invoke("Suicide", timer);
    }

    private void Suicide()
    {
        estMort = true;
    }

    private void Update()
    {
        if (active)
        {
            CharacterController controller = gameObject.GetComponent<CharacterController>();

            if (results.Length != 0)
            {
                if(results[0] > 0f)
                    m_currentVelocity += (m_accelerationRate * Time.deltaTime);
                else
                    m_currentVelocity -= (m_decelerationRate * Time.deltaTime);

                m_currentVelocity = Mathf.Clamp(m_currentVelocity, m_initialVelocity, m_finalVelocity);

                m_movDir = new Vector3(0, 0, m_currentVelocity);
                m_movDir *= m_speed;
                m_movDir = transform.TransformDirection(m_movDir);

                controller.Move(m_movDir);
                transform.Rotate(0, results[1] * m_rotSpeed * Time.deltaTime, 0);
            }

            InteractRaycast();

            m_distanceTraveled = Vector3.Distance(transform.position, new Vector3(-21.5f, 1.1f, -21.8f));
            m_distanceTraveled2 += Vector3.Distance(transform.position, m_lastPosition);
            m_lastPosition = transform.position;
            fitness += m_distanceTraveled / 1000;
            fitness += m_distanceTraveled2 / 1000;
            fitness -= 0.02f;

        }

        if (fitness < -30f)
        {
            estMort = true;
        }       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "mur")
        {
            active = false;
            fitness -= 30f;
            estMort = true;
        }
        if (other.gameObject.tag == "checkpoint")
        {
            bool estDejaPris = false;
            for(int i = 0; i< listeCheckPoint.Length; i++)
            {
                if(listeCheckPoint[i] == other.gameObject)
                {
                    estDejaPris = true;
                }
            }
            if (!estDejaPris)
            {
                fitness += 10f;
                listeCheckPoint[nombreCheckpoint] = other.gameObject;
                nombreCheckpoint++;
            }
        }

        if (other.gameObject.tag == "sortie")
        {
            bool estDejaPris = false;
            for (int i = 0; i < listeCheckPoint.Length; i++)
            {
                if (listeCheckPoint[i] == other.gameObject)
                {
                    estDejaPris = true;
                }
            }
            if (!estDejaPris)
            {
                fitness += 100000f;
                listeCheckPoint[nombreCheckpoint] = other.gameObject;
                nombreCheckpoint++;
            }
        }
    }

    private void InteractRaycast()
    {
        m_transf = GetComponent<Transform>();
        Vector3 playerPosition = transform.position;

        Vector3 forwardDirection = m_transf.forward;
        Vector3 leftDirection = m_transf.right * -1;
        Vector3 rightDirection = m_transf.right;
        Vector3 diagLeft = m_transf.TransformDirection(new Vector3(maxDistance / 12 , 0f, maxDistance / 12));
        Vector3 diagRight = m_transf.TransformDirection(new Vector3(-maxDistance / 12, 0f, maxDistance / 12));

        Ray frontRay = new Ray(playerPosition, forwardDirection);
        Ray leftRay = new Ray(playerPosition, leftDirection);
        Ray rightRay = new Ray(playerPosition, rightDirection);
        Ray diagLeftRay = new Ray(playerPosition, diagLeft);
        Ray diagRightRay = new Ray(playerPosition, diagRight);

        RaycastHit hit;
        if (Physics.Raycast(frontRay, out hit, maxDistance))
            distForward = hit.distance;
        if (Physics.Raycast(leftRay, out hit, maxDistance))
            distLeft = hit.distance;
        if (Physics.Raycast(rightRay, out hit, maxDistance))
            distRight = hit.distance;
        if (Physics.Raycast(diagLeftRay, out hit, maxDistance))
            distDiagLeft = hit.distance;
        if (Physics.Raycast(diagRightRay, out hit, maxDistance))
            distDiagRight = hit.distance;

        if (AfficherRayon)
        {
            Debug.DrawRay(transform.position, forwardDirection * maxDistance, Color.green);
            Debug.DrawRay(transform.position, leftDirection * maxDistance, Color.green);
            Debug.DrawRay(transform.position, rightDirection * maxDistance, Color.green);
            Debug.DrawRay(transform.position, diagLeft * maxDistance, Color.green);
            Debug.DrawRay(transform.position, diagRight * maxDistance, Color.green);
        }
    }
}
