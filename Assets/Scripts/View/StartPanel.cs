using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartPanel : BasePanel<StartPanel>
{
    public Button btnStart;
    public Button btnSetting;
    public Button btnClose;
    private void Start()
    {
        if (btnStart != null) btnStart.onClick.AddListener(() =>
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync("GameScene");
            ao.completed += (ao) =>
            {
                AudioManager.Instance.ChangeBGMByScene();
            };
            });
        if (btnSetting != null) btnSetting.onClick.AddListener(() => SettingPanel.Instance.ShowMe());
        if (btnClose != null) btnClose.onClick.AddListener(() => Application.Quit());
    }
    private void OnDestroy()
    {
        btnStart.onClick.RemoveAllListeners();
        btnSetting.onClick.RemoveAllListeners();
        btnClose.onClick.RemoveAllListeners();
    }
}
