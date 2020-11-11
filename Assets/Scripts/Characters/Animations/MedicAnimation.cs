using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicAnimation : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] AudioSource throwSound;
    public AudioSource twistSound;

    public void PlayThrow()
    {
        if (!throwSound.isPlaying)
        {
            throwSound.Play();
        }
    }

    public void PlayTwist()
    {
        if (!twistSound.isPlaying)
        {
            twistSound.Play();
        }
    }
}
