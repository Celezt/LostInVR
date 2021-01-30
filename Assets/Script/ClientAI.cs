using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(FollowPath), typeof(Outline))]
public class ClientAI : MonoBehaviour
{
    private FollowPath _follow;
    private Outline _outline;

    [SerializeField]
    private GameObject _player;

    public int DistanceThreshold = 3;
    [Range(0, 1)]
    public float WaitPoint;
    public float DampRotation = 2f;

    /// <summary>
    /// The current state of the client.
    /// </summary>
    public ClientState State { get; private set; } = ClientState.Move;

    public enum ClientState
    {
        Move,
        Request,
        Register,
    }

    private void Start()
    {
        _follow = GetComponent<FollowPath>();
        _outline = GetComponent<Outline>();
    }

    private void Update()
    {
        switch (State)
        {
            case ClientState.Move:
                if (_follow.PercentTravelled > WaitPoint)
                {
                    // Request item.
                    State = ClientState.Request;

                    _follow.ToMove = false;
                    _follow.ToRotate = false;
                }

                break;
            case ClientState.Request:
                Vector3 lookPosition = _player.transform.position - transform.position;
                lookPosition.y = 0;
                var rotation = Quaternion.LookRotation(lookPosition);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * DampRotation);
                break;
            case ClientState.Register:
                break;
        }

        if (Vector3.Distance(_player.transform.position, transform.position) < DistanceThreshold)
            _outline.enabled = true;
        else
            _outline.enabled = false;

    }

    private void OnMouseDown()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) < DistanceThreshold)
            print("hej");
    }
}
