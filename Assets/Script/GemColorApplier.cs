using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemColorApplier : MonoBehaviour
{
    private static List<Material> Materials = new List<Material>();
    private static System.Random random;
    private static bool Loaded = false;

    void Start()
    {
        if (Loaded == false)
        {
            LoadMaterials();
        }
        gameObject.GetComponent<MeshRenderer>().material = Materials[random.Next(0, Materials.Count)];
    }

    private static void LoadMaterials()
    {
        random = new System.Random();
        Object[] loadList = Resources.LoadAll("Materials", typeof(Material));
        foreach (Material loadedObject in loadList)
        {
            if (loadedObject.name.StartsWith("Gem_"))
                Materials.Add(loadedObject);
        }
        Loaded = true;
    }
}
