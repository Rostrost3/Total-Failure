using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;

    public void SetVolume()
    {
        float val = musicSlider.value;

        audioMixer.SetFloat("Music", -80 + val > 0 ? 0 : -80 + val);

        PlayerPrefs.SetFloat("MusicVolume", val);
    }

    private void LoadValue()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");

        SetVolume();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            LoadValue();
        }
        else
        {
            SetVolume();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
