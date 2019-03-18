using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    //This is the Transform of the second GameObject
    public Transform m_NextPoint;
    Quaternion m_MyQuaternion;
    float m_Speed = 0.1f;

    void Start()
    {
        m_MyQuaternion = new Quaternion();
    }

    void Update()
    {
        //Set the Quaternion rotation from the GameObject's position to the next GameObject's position
        m_MyQuaternion.SetFromToRotation(transform.position, m_NextPoint.position);
        //Move the GameObject towards the second GameObject
        transform.position = Vector3.Lerp(transform.position, m_NextPoint.position, m_Speed * Time.deltaTime);
        //Rotate the GameObject towards the second GameObject
        transform.rotation = m_MyQuaternion * transform.rotation;
    }
}