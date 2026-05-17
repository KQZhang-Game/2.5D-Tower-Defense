using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LostPanel : BasePanel<LostPanel>
{
    void Start()
    {
        EnemyManager.Instance.OnLost.AddListener(ShowMe);
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
        EnemyManager.Instance.OnLost.RemoveListener(ShowMe);
    }
}
