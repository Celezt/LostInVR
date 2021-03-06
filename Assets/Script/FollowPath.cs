using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class FollowPath : MonoBehaviour
{
    private Vector3 _offsetPosition;

    public PathCreator Creator;
    public float Speed = 5;
    public float DampRotation = 2;

    public Quaternion LookRotation { get; set; }
    public float DistanceTravelled { get; set; }
    public float PercentTravelled { get; set; }
    /// <summary>
    /// Enable/Disable update for movement.
    /// </summary>
    public bool ToMove { get; set; } = true;
    /// <summary>
    /// Enable/Disable update for rotation.
    /// </summary>
    public bool ToRotate { get; set; } = true;

    void Start()
    {
        _offsetPosition = GetComponentInChildren<FollowPath>().transform.localPosition;
        LookRotation = transform.rotation;

        if (Creator != null)
        {
            // Subscribed to get notified if the path changes during the game.
            Creator.pathUpdated += OnPathChanged;
        }
    }

    void Update()
    {
        if (Creator != null)
        {
            if (ToMove)
            {
                DistanceTravelled += Speed * Time.deltaTime;
                DistanceTravelled %= Creator.path.length;

                PercentTravelled = (DistanceTravelled / Creator.path.length) % 1;
                transform.position = Creator.path.GetPointAtDistance(DistanceTravelled) + _offsetPosition;
            }

            var rotation = Quaternion.LookRotation(Creator.path.GetDirectionAtDistance(DistanceTravelled), Vector3.up);
            LookRotation = Quaternion.Slerp(transform.localRotation, rotation, Time.deltaTime * DampRotation);

            if (ToRotate)
                transform.rotation = LookRotation;
        }
    }

    void OnPathChanged()
    {
        DistanceTravelled = Creator.path.GetClosestDistanceAlongPath(transform.position);
    }
}
