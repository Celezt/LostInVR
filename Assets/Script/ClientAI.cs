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
    public string RequestName = "";

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

        // Give it a random request if no item is requested by default.
        if (RequestName == "")
            RandomRequest();
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

                    // Set the first found game object with the name as request sample.
                    GameObject sample = GameObject.Find(RequestName);

                    // If parent does not contain MeshFilter; Set sample as the first child that has it.
                    if (!sample.GetComponent<MeshFilter>())
                        sample = sample.GetComponentInChildren<MeshFilter>().gameObject;


                    _requestSample = (RequestName != "") ? sample : null;

                    DisplayRequest();
                }

                break;
            case ClientState.Request:
                Vector3 lookPosition = Camera.main.transform.position - transform.position;
                lookPosition.y = 0;
                var rotation = Quaternion.LookRotation(lookPosition);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * DampRotation);

                if (RequestName == "")
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
                    RandomRequest();
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

    private void RandomRequest()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Object");
        int pickedObjectIndex = Random.Range(0, objects.Length);

        RequestName = objects[pickedObjectIndex].name.Split('(')[0].Trim();
    }
}
