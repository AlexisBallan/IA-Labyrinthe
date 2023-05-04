using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaController : MonoBehaviour
{
    private Transform m_transf;

    private Vector3 m_movDir = Vector3.zero;
    private float m_rotSpeed = 300f;
    private float m_speed = 0.5f;
    private float m_initialVelocity = 0f;
    private float m_finalVelocity = 3f;
    private float m_currentVelocity = 0f;
    private float m_accelerationRate = 2f;
    private float m_decelerationRate = 0.8f;

    public float maxDistance = 30f;
    public float distForward = 0f;
    public float distLeft = 0f;
    public float distRight = 0f;
    public float distDiagLeft = 0f;
    public float distDiagRight = 0f;
    public bool AfficherRayon = false;

    //Gérer le fitness
    public float fitness = 0f;
    private Vector3 m_lastPosition;
    private float m_distanceTraveled;

    private void Start()
    {
        m_lastPosition = transform.position;
    }

    private void Update()
    {
        CharacterController controller = gameObject.GetComponent<CharacterController>();

        if (Input.GetKey(KeyCode.UpArrow))
            m_currentVelocity += (m_accelerationRate * Time.deltaTime);
        else
            m_currentVelocity -= (m_decelerationRate * Time.deltaTime);
        m_currentVelocity = Mathf.Clamp(m_currentVelocity, m_initialVelocity, m_finalVelocity);

        m_movDir = new Vector3(0, 0, m_currentVelocity);
        m_movDir *= m_speed;
        m_movDir = transform.TransformDirection(m_movDir);

        controller.Move(m_movDir);
        transform.Rotate(0, Input.GetAxis("Horizontal") * m_rotSpeed * Time.deltaTime, 0);

        InteractRaycast();

        m_distanceTraveled += Vector3.Distance(transform.position, m_lastPosition);
        m_lastPosition = transform.position;
        fitness += m_distanceTraveled / 1000;
        fitness -= 0.01f;

        Debug.Log(fitness);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "mur")
            Debug.Log("COLLISION AVEC MUUUUR");
        if (other.gameObject.tag == "checkpoint")
            fitness += 5f;
    }

    private void InteractRaycast()
    {
        m_transf = GetComponent<Transform>();
        Vector3 playerPosition = transform.forward;

        Vector3 forwardDirection = m_transf.forward;
        Vector3 leftDirection = m_transf.right * -1;
        Vector3 rightDirection = m_transf.right;
        Vector3 diagLeft = m_transf.TransformDirection(new Vector3(maxDistance / 5, 0f, maxDistance / 5));
        Vector3 diagRight = m_transf.TransformDirection(new Vector3(-maxDistance / 5, 0f, maxDistance / 5));

        Ray frontRay = new Ray(playerPosition, forwardDirection);
        Ray leftRay = new Ray(playerPosition, leftDirection);
        Ray rightRay = new Ray(playerPosition, rightDirection);
        Ray diagLeftRay = new Ray(playerPosition, diagLeft);
        Ray diagRightRay = new Ray(playerPosition, diagRight);

        RaycastHit hit;
        if (Physics.Raycast(frontRay, out hit, maxDistance) && hit.transform.tag == "mur")
            distForward = hit.distance;
        if (Physics.Raycast(leftRay, out hit, maxDistance) && hit.transform.tag == "mur")
            distLeft = hit.distance;
        if (Physics.Raycast(rightRay, out hit, maxDistance) && hit.transform.tag == "mur")
            distRight = hit.distance;
        if (Physics.Raycast(diagLeftRay, out hit, maxDistance) && hit.transform.tag == "mur")
            distDiagLeft = hit.distance;
        if (Physics.Raycast(diagRightRay, out hit, maxDistance) && hit.transform.tag == "mur")
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
