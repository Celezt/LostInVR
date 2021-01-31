using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(FollowPath), typeof(Outline))]
public class ClientAI : MonoBehaviour
{
    private FollowPath _follow;
    private Renderer _renderer;
    private GameObject _requestSample;
    private GameObject _displayItem;


    private int _randomTextureIndex;
    private float _oldPercentTravelled;

    public int DistanceThreshold = 3;
    [Range(0, 1)]
    public float WaitPoint;
    public float DampRotation = 2f;
    public string RequestTag = "Untagged";

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

                    // Set the first found game object with the tag as request sample.
                    _requestSample = (RequestTag != "Untagged") ? GameObject.FindWithTag(RequestTag) : null;

                    DisplayRequest();
                }

                break;
            case ClientState.Request:
                Vector3 lookPosition = Camera.main.transform.position - transform.position;
                lookPosition.y = 0;
                var rotation = Quaternion.LookRotation(lookPosition);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * DampRotation);

                if (RequestTag == "Untagged")
                {
                    _follow.ToMove = true;
                    _follow.ToRotate = true;

                    State = ClientState.MoveFrom;

                    Destroy(_displayItem);
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
    private void DisplayRequest()
    {
        if (_displayItem)
            Destroy(_displayItem);

        if (_requestSample)
        {
            _displayItem = new GameObject("DisplayItem");
            _displayItem.transform.position = transform.position + new Vector3(0, 1, -1);
            _displayItem.transform.rotation = transform.rotation;
            _displayItem.transform.localScale = _requestSample.transform.localScale;
            _displayItem.transform.parent = transform;

            MeshRenderer renderer = _displayItem.AddComponent<MeshRenderer>();
            MeshFilter filter = _displayItem.AddComponent<MeshFilter>();

            renderer.sharedMaterials = _requestSample.GetComponent<MeshRenderer>().sharedMaterials;
            filter.sharedMesh = _requestSample.GetComponent<MeshFilter>().sharedMesh;

            Outline outline = _displayItem.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 10f;
        }
    }

    private void ChangeTexture()
    {
        // Random skin
        _randomTextureIndex = Random.Range(0, _textures.Length);
        _renderer.material.mainTexture = _textures[_randomTextureIndex];
    }
}
