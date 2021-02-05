using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(FollowPath))]
public class ClientAI : MonoBehaviour
{
    [Range(0, 1)]
    public float WaitPoint;
    public float DampAvatarRotation = 2f;
    public float SampleRotateSpeed = 20f;
    public float BobSpeed = 0.1f;
    public float BobAmount = 0.1f;
    public string RequestName = "";

    [SerializeField]
    private Texture[] _textures;

    public bool IsRequesting { get; private set; }
    public bool IsRequestAvailable { get; private set; }
    public bool IsMoving { get; private set; } = true;

    /// <summary>
    /// The current state of the client.
    /// </summary>
    public ClientState State { get; private set; } = ClientState.MoveTo;

    private FollowPath _follow;
    private Renderer _renderer;
    private GameObject _requestSample;
    private GameObject[] _displayItems;

    private Vector3 _startlocalScale;
    private int _randomTextureIndex;
    private float _oldPercentTravelled;
    private float _scaleOffset;

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

        _startlocalScale = transform.localScale;
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
                OnMoveTo();
                break;
            case ClientState.Request:
                OnRequest();
                break;
            case ClientState.MoveFrom:
                OnMoveFrom();
                break;
        }

        _oldPercentTravelled = _follow.PercentTravelled;
    }

    // Move to wait point.
    private void OnMoveTo()
    {
        // If reaching the wait point.
        if (_follow.PercentTravelled > WaitPoint)
        {
            IsMoving = false;
            IsRequesting = true;

            State = ClientState.Request;

            _follow.ToMove = false;
            _follow.ToRotate = false;

            AkSoundEngine.PostEvent("Char15", gameObject);
            DisplayRequest();
        }
    }

    // Waiting on request.
    private void OnRequest()
    {
        // Rotate to look at the main camera.
        Vector3 lookPosition = Camera.main.transform.position - transform.position;
        lookPosition.y = 0;
        var rotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * DampAvatarRotation);

        _scaleOffset += BobSpeed;
        transform.localScale = new Vector3(_startlocalScale.x, _startlocalScale.y + Mathf.Cos(_scaleOffset) * BobAmount, _startlocalScale.z);

        // If empty it means it was never assigned a request or the request was a success.
        if (RequestName == "")
        {
            _scaleOffset = 0;
            transform.localScale = _startlocalScale;

            IsMoving = true;
            IsRequesting = false;

            _follow.ToMove = true;
            _follow.ToRotate = true;

            State = ClientState.MoveFrom;

            OnDestroyDisplay();
        }

        // Rotate samples if it exist.
        if (_displayItems != null)
            for (int i = 0; i < _displayItems.Length; i++)
                _displayItems[i].transform.Rotate(Vector3.up * (SampleRotateSpeed * Time.deltaTime));
    }

    // Moving out and reseting itself for next client.
    private void OnMoveFrom()
    {
        // If reached the end of the path.
        if (_follow.PercentTravelled < _oldPercentTravelled)
        {
            State = ClientState.MoveTo;
            ChangeTexture();
            RandomRequest();
        }
    }

    // Create a new game object for display. It has no collision.
    private void DisplayRequest()
    {
        // Set the first found game object with the name as request sample.
        GameObject sample = GameObject.Find(RequestName);

        // If not found, try using (Clone) after.
        if (!sample)
            sample = GameObject.Find(RequestName + "(Clone)");

        _requestSample = (RequestName != "") ? sample : null;

        OnDestroyDisplay();

        if (_requestSample)
        {
            MeshFilter meshFilter = _requestSample.GetComponent<MeshFilter>();
            GameObject parentObject = (meshFilter) ? meshFilter.gameObject : null;

            MeshFilter[] meshFilters = _requestSample.GetComponentsInChildren<MeshFilter>();
            // Length of all game objects that has the component MeshFilter including the parent.
            GameObject[] gameObjects = new GameObject[meshFilters.Length + ((parentObject) ? 1 : 0)];
            _displayItems = new GameObject[gameObjects.Length];

            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (parentObject)
                    gameObjects[i] = parentObject;
                else
                    gameObjects[i] = meshFilters[i].gameObject;

                _displayItems[i] = new GameObject("DisplayItem" + i);
                _displayItems[i].transform.position = transform.position + new Vector3(1, 2, 0);
                _displayItems[i].transform.rotation = transform.rotation;
                _displayItems[i].transform.localScale = _requestSample.transform.localScale;
                _displayItems[i].transform.parent = transform;

                MeshRenderer renderer = _displayItems[i].AddComponent<MeshRenderer>();
                MeshFilter filter = _displayItems[i].AddComponent<MeshFilter>();

                renderer.sharedMaterials = gameObjects[i].GetComponent<MeshRenderer>().sharedMaterials;
                filter.sharedMesh = gameObjects[i].GetComponent<MeshFilter>().sharedMesh;

                Outline outline = _displayItems[i].AddComponent<Outline>();
                outline.OutlineMode = Outline.Mode.OutlineVisible;
                outline.OutlineColor = Color.yellow;
                outline.OutlineWidth = 3f;
            }
        }
    }

    // Change randomly the texture based on input.
    private void ChangeTexture()
    {
        // Random skin
        _randomTextureIndex = Random.Range(0, _textures.Length);
        _renderer.material.mainTexture = _textures[_randomTextureIndex];
    }

    // Create random request based on all game objects containing the tag "Object".
    private void RandomRequest()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Object");

        if (objects.Length > 0)
        {
            IsRequestAvailable = true;

            int pickedObjectIndex = Random.Range(0, objects.Length);

            RequestName = objects[pickedObjectIndex].name.Split('(')[0].Trim();
        }
        else
            IsRequestAvailable = false;
    }

    // Destroy display item.
    private void OnDestroyDisplay()
    {
        if (_displayItems != null)
        {
            for (int i = 0; i < _displayItems.Length; i++)
                Destroy(_displayItems[i]);

            _displayItems = null;
        }
    }
}
