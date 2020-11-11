using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSlot : MonoBehaviour
{
    public bool isOccupied;
    [SerializeField] SpawnEnnemies spawner;

    private void Awake()
    {
        isOccupied = false;
        spawner = GetComponentInParent<SpawnEnnemies>();
    }

    private void Update()
    {
        if (isOccupied)
        {
            spawner.slotList.Remove(this);
        }
    }
}
