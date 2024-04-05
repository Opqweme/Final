using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemStarter : MonoBehaviour
{
    [SerializeField] private Vector3 desiredStartPosition = new Vector3(0, 0, 0); // Set desired start position here

    private void Start()
    {
        // Set the position of the Particle System to desiredStartPosition
        transform.position = desiredStartPosition;
    }
}
