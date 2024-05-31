using UnityEngine;
using UnityEngine.UI; // Добавьте это пространство имен, чтобы работать с UI

public class VolumeValue : MonoBehaviour
{
    public Slider volumeSlider; // Переменная для ползунка громкости
    public bool isOn;
    private AudioSource audioSrc;
    private float musicVolume = 1f;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        LoadVolumeSettings();
    }

    void Update()
    {
        audioSrc.volume = musicVolume;
        AudioListener.volume = musicVolume;
    }

    public void SetVolume(float vol)
    {
        musicVolume = vol;
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();
        UpdateVolumeSlider();
    }

    public void OnOffSound()
    {
        if (!isOn)
        {
            AudioListener.volume = 1f;
            musicVolume = 1f;
            isOn = true;
        }
        else
        {
            AudioListener.volume = 0f;
            musicVolume = 0f;
            isOn = false;
        }
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();
        UpdateVolumeSlider();
    }

    private void LoadVolumeSettings()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        }
        else
        {
            musicVolume = 1f; // Уровень громкости по умолчанию
        }
        UpdateVolumeSlider();
    }

    private void UpdateVolumeSlider()
    {
        if (volumeSlider != null)
        {
            volumeSlider.value = musicVolume; // Устанавливаем ползунок в сохраненное положение
        }
    }
}
