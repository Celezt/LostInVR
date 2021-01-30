using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshController : MonoBehaviour
{
    public List<GameObject> Prefabs;
    public List<GameObject> Objects;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject go in Objects)
        {
            int i = Random.Range(0, Prefabs.Count);
            Instantiate(Prefabs[i], go.transform);
        }
    }
}