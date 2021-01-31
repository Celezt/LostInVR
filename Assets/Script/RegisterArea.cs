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
        _manager = FindObjectOfType<GameLoopManager>();
        _client = FindObjectOfType<ClientAI>();
        _particles.Stop();
    }
    private void OnTriggerEnter(Collider other)
    {
        bool found = false;
        if (_client.RequestName == other.name.Split('(')[0].Trim())
        {
            foreach (Transform child in LeftHand.transform)
            {
                if (child == other.gameObject)
                {
                    found = true;
                    break;
                }
            }
            if (found == false)
            {
                foreach (Transform child in RightHand.transform)
                {
                    if (child == other.gameObject)
                    {
                        found = true;
                        break;
                    }
                }
            }
            if (found == false)
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
}
