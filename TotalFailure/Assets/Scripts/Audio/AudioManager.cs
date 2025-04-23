
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource Music;

    [Header("Audio Clip")]
    public AudioClip background;

    private void Start()
    {
        Music.clip = background;
        Music.Play();
    }
}
