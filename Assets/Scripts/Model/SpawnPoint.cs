using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private List<Transform> path = new List<Transform>();
    public List<Transform> GetPath()
    {
        return path;
    }
}
