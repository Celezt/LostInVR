using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshController : MonoBehaviour
{
    public GameObject BasePrefab;
    public System.Random random;
    public List<GameObject> Prefabs;
    public List<GameObject> Objects;

    // Start is called before the first frame update
    void Start()
    {
        Object[] loadList = Resources.LoadAll("Objects", typeof(GameObject));
        foreach (GameObject loadedObject in loadList)
        {
            Prefabs.Add(loadedObject);
        }

        random = new System.Random();
        foreach (GameObject go in Objects)
        {
            int i = random.Next(0, Prefabs.Count);
            Instantiate(Prefabs[i], go.transform);
        }
    }

    public GameObject NewObject(Transform parent)
    {
        int i = random.Next(0, Prefabs.Count);
        GameObject baseObject = Instantiate(BasePrefab, parent);
        Instantiate(Prefabs[i], baseObject.transform);
        return baseObject;
    }
}