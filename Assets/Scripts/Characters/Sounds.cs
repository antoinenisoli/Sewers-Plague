using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class Sounds : MonoBehaviour
{
    [Header("Sounds")]
    public List<AudioSource> growlList;
    public List<AudioSource> impactList;
    public List<AudioSource> deathList;
}
