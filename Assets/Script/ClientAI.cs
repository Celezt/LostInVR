using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(FollowPath), typeof(Outline))]
public class ClientAI : MonoBehaviour
{
    private FollowPath _follow;
    private Renderer _renderer;

    private int _randomTextureIndex;
    private float _oldPercentTravelled;

    public int DistanceThreshold = 3;
    [Range(0, 1)]
    public float WaitPoint;
    public float DampRotation = 2f;
    public GameObject Request;

    [SerializeField]
    private Texture[] _textures;

    /// <summary>
    /// The current state of the client.
    /// </summary>
    public ClientState State { get; private set; } = ClientState.MoveTo;

    public enum ClientState
    {
        MoveTo,
        MoveFrom,
        Request,
    }

    private void Start()
    {
        _follow = GetComponent<FollowPath>();
        _renderer = GetComponent<Renderer>();

        _oldPercentTravelled = _follow.PercentTravelled;

        ChangeTexture();
    }

    private void Update()
    {
        switch (State)
        {
            case ClientState.MoveTo:
                if (_follow.PercentTravelled > WaitPoint)
                {
                    // Request item.
                    State = ClientState.Request;

                    _follow.ToMove = false;
                    _follow.ToRotate = false;
                }

                break;
            case ClientState.Request:
                Vector3 lookPosition = Camera.main.transform.position - transform.position;
                lookPosition.y = 0;
                var rotation = Quaternion.LookRotation(lookPosition);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * DampRotation);

                if (!Request)
                {
                    _follow.ToMove = true;
                    _follow.ToRotate = true;

                    State = ClientState.MoveFrom;
                }

                break;
            case ClientState.MoveFrom:
                if (_follow.PercentTravelled < _oldPercentTravelled)
                {
                    State = ClientState.MoveTo;
                    ChangeTexture();
                }

                break;
        }

        _oldPercentTravelled = _follow.PercentTravelled;
    }

    private void ChangeTexture()
    {
        // Random skin
        _randomTextureIndex = Random.Range(0, _textures.Length);
        _renderer.material.mainTexture = _textures[_randomTextureIndex];
    }
}
