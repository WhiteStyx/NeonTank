using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    [SerializeField] Transform[] SpawnPos;

    public Transform GetPos(int index)
    {
        Debug.Log(index);
        return SpawnPos[index];
    }
}
