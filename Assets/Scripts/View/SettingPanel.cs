using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel<SettingPanel>
{
    public Button btnClose;
    public Toggle togMusicOpen;
    public Toggle togSoundOpen;
    public Slider sliderMusicVolume;
    public Slider sliderSoundVolume;
    protected override bool IsPersistent => true;
    private void Start()
    {
        if (btnClose != null) btnClose.onClick.AddListener(() => HideMe());
        if (togMusicOpen != null)
        {
            togMusicOpen.onValueChanged.AddListener((value) =>
            {
                GameDataManager.Instance.ChangMusicState(value);
                AudioManager.Instance.ChangMusicState(value);
            });
        }
        if (togSoundOpen != null)
        {
            togSoundOpen.onValueChanged.AddListener((value) =>
            {
                GameDataManager.Instance.ChangSoundState(value);
            });
        }
        if (sliderMusicVolume != null)
        {
            sliderMusicVolume.onValueChanged.AddListener((value) =>
            {
                GameDataManager.Instance.ChangMusicVolume(value);
                AudioManager.Instance.ChangeMusicVolume(value);
            });
        }
        if (sliderSoundVolume != null)
        {
            sliderSoundVolume.onValueChanged.AddListener((value) =>
            {
                GameDataManager.Instance.ChangSoundVolume(value);
            });
        }
        HideMe();
    }
    public override void ShowMe()
    {
        base.ShowMe();
        Time.timeScale = 0;
        UpdatePanelInfo();
    }
    public override void HideMe()
    {
        base.HideMe();
        Time.timeScale = 1;
    }
    private void UpdatePanelInfo()
    {
        MusicData data = GameDataManager.Instance.MusicData;
        togMusicOpen.isOn = data.isMusicOpen;
        togSoundOpen.isOn = data.isSoundOpen;
        sliderMusicVolume.value = Mathf.Clamp01(data.musicVolume);
        sliderSoundVolume.value = Mathf.Clamp01(data.soundVolume);
    }
    private void OnDestroy()
    {
        btnClose.onClick.RemoveAllListeners();
        togMusicOpen.onValueChanged.RemoveAllListeners();
        togSoundOpen.onValueChanged.RemoveAllListeners();
        sliderMusicVolume.onValueChanged.RemoveAllListeners();
        sliderSoundVolume.onValueChanged.RemoveAllListeners();
    }
}