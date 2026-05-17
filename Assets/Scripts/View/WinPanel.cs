using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanel : BasePanel<WinPanel>
{
    void Start()
    {
        EnemyManager.Instance.OnWin .AddListener(ShowMe);
        HideMe();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync("BeginScene");
            ao.completed += (ao) =>
            {
                AudioManager.Instance.ChangeBGMByScene();
            };
        }
    }
    private void OnDestroy()
    {
        EnemyManager.Instance.OnWin.RemoveListener(ShowMe);
    }
}
