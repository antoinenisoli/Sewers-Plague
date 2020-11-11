using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruction : MonoBehaviour
{
    public void Destruction()
    {
        gameObject.SetActive(false);
    }
}
