using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    [SerializeField] float duration = 2;

    public IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
