using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RegisterArea : MonoBehaviour
{
    private ParticleSystem _particles;
    private void Awake()
    {
        _particles = GetComponentInChildren<ParticleSystem>();
        _particles.Stop();
    }
    private void OnTriggerEnter(Collider other)
    {
        // Find client in scene.
        ClientAI client = FindObjectOfType<ClientAI>();

        if (client.Request == other.gameObject)
        {
            _particles.Play();
            Destroy(other);
        }
    }
}
