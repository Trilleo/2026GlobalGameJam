using UnityEngine;
using UnityEngine.UI;

public class VolumeUI : MonoBehaviour
{
    [Header("主音量控制")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Button masterMuteButton;
    [SerializeField] private Image masterMuteIcon;

    [Header("音乐音量控制")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Button musicMuteButton;
    [SerializeField] private Image musicMuteIcon;

    [Header("音效音量控制")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button sfxMuteButton;
    [SerializeField] private Image sfxMuteIcon;

    [Header("图标")]
    [SerializeField] private Sprite iconVolumeOn;
    [SerializeField] private Sprite iconVolumeOff;

    private VolumeManager volumeManager;

    void Start()
    {
        volumeManager = FindObjectOfType<VolumeManager>();
        if (volumeManager == null)
        {
            GameObject managerObj = new GameObject("VolumeManager");
            volumeManager = managerObj.AddComponent<VolumeManager>();
        }

        InitializeUI();
        LoadCurrentSettings();
    }

    void InitializeUI()
    {
        // 主音量
        if (masterSlider != null)
        {
            masterSlider.minValue = 0f;
            masterSlider.maxValue = 1f;
            masterSlider.onValueChanged.AddListener(OnMasterSliderChanged);
        }

        if (masterMuteButton != null)
        {
            masterMuteButton.onClick.AddListener(() => {
                volumeManager.ToggleMasterMute();
                UpdateMasterUI();
            });
        }

        // 音乐音量
        if (musicSlider != null)
        {
            musicSlider.minValue = 0f;
            musicSlider.maxValue = 1f;
            musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        }

        if (musicMuteButton != null)
        {
            musicMuteButton.onClick.AddListener(() => {
                volumeManager.ToggleMusicMute();
                UpdateMusicUI();
            });
        }

        // 音效音量
        if (sfxSlider != null)
        {
            sfxSlider.minValue = 0f;
            sfxSlider.maxValue = 1f;
            sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);
        }

        if (sfxMuteButton != null)
        {
            sfxMuteButton.onClick.AddListener(() => {
                volumeManager.ToggleSFXMute();
                UpdateSFXUI();
            });
        }
    }

    void LoadCurrentSettings()
    {
        if (volumeManager == null) return;

        UpdateMasterUI();
        UpdateMusicUI();
        UpdateSFXUI();
    }

    void OnMasterSliderChanged(float value)
    {
        if (volumeManager != null)
        {
            if (volumeManager.IsMasterMuted() && value > 0.01f)
            {
                volumeManager.SetMasterVolume(value);
            }
            else if (!volumeManager.IsMasterMuted())
            {
                volumeManager.SetMasterVolume(value);
            }

            UpdateMasterUI();
        }
    }

    void OnMusicSliderChanged(float value)
    {
        if (volumeManager != null)
        {
            if (volumeManager.IsMusicMuted() && value > 0.01f)
            {
                volumeManager.SetMusicVolume(value);
            }
            else if (!volumeManager.IsMusicMuted())
            {
                volumeManager.SetMusicVolume(value);
            }

            UpdateMusicUI();
        }
    }

    void OnSFXSliderChanged(float value)
    {
        if (volumeManager != null)
        {
            if (volumeManager.IsSFXMuted() && value > 0.01f)
            {
                volumeManager.SetSFXVolume(value);
            }
            else if (!volumeManager.IsSFXMuted())
            {
                volumeManager.SetSFXVolume(value);
            }

            UpdateSFXUI();
        }
    }

    void UpdateMasterUI()
    {
        if (volumeManager == null) return;

        if (masterSlider != null)
        {
            masterSlider.interactable = true;
            masterSlider.value = volumeManager.GetMasterVolume();
        }

        if (masterMuteIcon != null && iconVolumeOn != null && iconVolumeOff != null)
        {
            masterMuteIcon.sprite = volumeManager.IsMasterMuted() ? iconVolumeOff : iconVolumeOn;
        }
    }

    void UpdateMusicUI()
    {
        if (volumeManager == null) return;

        if (musicSlider != null)
        {
            musicSlider.interactable = true;
            musicSlider.value = volumeManager.GetMusicVolume();
        }

        if (musicMuteIcon != null && iconVolumeOn != null && iconVolumeOff != null)
        {
            musicMuteIcon.sprite = volumeManager.IsMusicMuted() ? iconVolumeOff : iconVolumeOn;
        }
    }

    void UpdateSFXUI()
    {
        if (volumeManager == null) return;

        if (sfxSlider != null)
        {
            sfxSlider.interactable = true;
            sfxSlider.value = volumeManager.GetSFXVolume();
        }

        if (sfxMuteIcon != null && iconVolumeOn != null && iconVolumeOff != null)
        {
            sfxMuteIcon.sprite = volumeManager.IsSFXMuted() ? iconVolumeOff : iconVolumeOn;
        }
    }

    void OnEnable()
    {
        LoadCurrentSettings();
    }
}