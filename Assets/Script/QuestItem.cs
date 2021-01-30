using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    public enum Item
    {
        Test1,
        Test2,
        Test3,
    }

    public Item ItemType;
    public int Score;
    public float Time;
}
