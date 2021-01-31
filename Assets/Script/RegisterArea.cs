﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Prefabs.Interactions.Interactables;

[RequireComponent(typeof(Collider))]
public class RegisterArea : MonoBehaviour
{
    private ParticleSystem _particles;
    private GameLoopManager _manager;
    private ClientAI _client;
    private void Awake()
    {
        _particles = GetComponentInChildren<ParticleSystem>();
        _manager = FindObjectOfType<GameLoopManager>();
        _client = FindObjectOfType<ClientAI>();
        _particles.Stop();
    }
    private void OnTriggerEnter(Collider other)
    {
        InteractableFacade interactable = other.GetComponent<InteractableFacade>();

        // Skip if interactable facade is currently grabbing.
        if (interactable != null && interactable.IsGrabbed == true)
            return;

        if (_client.RequestName == other.name.Split('(')[0].Trim())
        {
            _client.RequestName = "";

            QuestItem item = other.GetComponent<QuestItem>();

            if (item && _manager)
            {
                _manager.IncreaseTimeRemaining(item.Time);
                _manager.IncreaseScore(item.Score);
            }

            _particles.Play();
            Destroy(other.gameObject);
        }
    }
}
