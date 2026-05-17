using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel : BasePanel<GamePanel>
{
    [System.Serializable]
    public class ButtonInfo
    {
        public GameObject prefab;
        public Button btnTower;
    }
    public TextMeshProUGUI tmpHp;
    public TextMeshProUGUI tmpCoin;
    public Button btnSetting;
    public Button btnClose;
    public List<ButtonInfo> btnList = new List<ButtonInfo>();
    public TextMeshProUGUI tmpClickFail;
    public float tipKeepTime = 0.5f;

    void Start()
    {
        foreach (var btn in btnList)
        {
            var captured = btn;
            TowerType currentType = captured.prefab.GetComponent<TowerBase>().towerType;
            int realCost = 0;
            TowerConfig targetConfig = null;
            foreach (var config in GameDataManager.Instance.TowerConfigList)
            {
                if (config.TowerType == currentType)
                {
                    targetConfig = config;
                    realCost = config.DeployCost;
                    break;
                }
            }
            if (targetConfig == null) continue;
            captured.btnTower.GetComponentInChildren<TextMeshProUGUI>().text = realCost.ToString();

            captured.btnTower.onClick.AddListener(() =>
            {
                if (GameManager.Instance.HasEnoughCost(targetConfig.DeployCost))
                {
                    HandleManager.Instance.InstantiatePrefab(captured.prefab);
                }
                else
                {
                    CancelInvoke(nameof(HideText));
                    tmpClickFail.gameObject.SetActive(true);
                    Invoke(nameof(HideText), tipKeepTime);
                    Vector2 mousePos;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        tmpClickFail.transform.parent as RectTransform,
                        Input.mousePosition, null, out mousePos);
                    (tmpClickFail.transform as RectTransform).anchoredPosition = mousePos;
                }
            });
        }
        btnSetting.onClick.AddListener(() => { SettingPanel.Instance.ShowMe(); });
        btnClose.onClick.AddListener(() =>
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync("BeginScene");
            ao.completed += (ao) =>
            {
                AudioManager.Instance.ChangeBGMByScene();
            };
        });
        tmpClickFail.gameObject.SetActive(false);
    }
    public void UpdateHpText(int value)
    {
        tmpHp.text = Mathf.Clamp(int.Parse(tmpHp.text) + value, 0, 100).ToString();
    }
    public void UpdateCoinText(int value)
    {
        tmpCoin.text = (int.Parse(tmpCoin.text) + value).ToString();
    }
    private void HideText()
    {
        tmpClickFail.gameObject.SetActive(false);
    }
}
