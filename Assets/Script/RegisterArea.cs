using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Prefabs.Interactions.Interactors;

[RequireComponent(typeof(Collider))]
public class RegisterArea : MonoBehaviour
{
    public GameObject LeftHand;
    public GameObject RightHand;
    private ParticleSystem _particles;
    private GameLoopManager _manager;
    private ClientAI _client;
    private void Awake()
    {
        _particles = GetComponentInChildren<ParticleSystem>();
        _manager = GameLoopManager.Instance;
        _client = FindObjectOfType<ClientAI>();
        _particles.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_client.RequestName == other.name.Split('(')[0].Trim())
        {
            _client.RequestName = "";

            QuestItem item = other.GetComponent<QuestItem>();

            if (item && _manager)
            {
                int _randomTextureIndex = _client._randomTextureIndex;
                if (_randomTextureIndex <= 1)
                {
                    AkSoundEngine.PostEvent("Male1_Thanks", gameObject);
                }
                if (_randomTextureIndex == 2 || _randomTextureIndex == 3)
                {
                    AkSoundEngine.PostEvent("Male2_Thanks", gameObject);
                }
                if (_randomTextureIndex == 4 || _randomTextureIndex == 5)
                {
                    AkSoundEngine.PostEvent("Female_Thanks", gameObject);
                }
                {
                    AkSoundEngine.PostEvent("Success", gameObject);
                }

                _client.IsWaiting = false;
                _manager.IncreaseTimeRemaining(item.Time);
                _manager.IncreaseScore((int)(item.Score - ((1 - 1 / (1 + 0.15 * _manager.secondsSinceLastItemDelivered)) * 100)));
            }

            _particles.Play();
            Destroy(other.gameObject);
            _manager.secondsSinceLastItemDelivered = 0f;
        }
    }
}
