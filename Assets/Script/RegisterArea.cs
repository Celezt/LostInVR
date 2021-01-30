using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RegisterArea : MonoBehaviour
{
    private ParticleSystem _particles;
    private GameLoopManager _manager;
    private void Awake()
    {
        _particles = GetComponentInChildren<ParticleSystem>();
        _manager = FindObjectOfType<GameLoopManager>();
        _particles.Stop();
    }
    private void OnTriggerEnter(Collider other)
    {
        // Find client in scene.
        ClientAI client = FindObjectOfType<ClientAI>();

        if (client.Request == other.gameObject)
        {
            QuestItem item = other.GetComponent<QuestItem>();

            if (item && _manager)
            {
                _manager.IncreaseTimeRemaining(item.Time);
                _manager.IncreaseScore(item.Score);
            }

            _particles.Play();
            Destroy(other);
        }
    }
}
