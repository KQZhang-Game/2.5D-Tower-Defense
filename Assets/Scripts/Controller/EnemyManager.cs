using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : Singleton<EnemyManager>
{
    protected override bool IsPersistent => false;
    private List<EnemyBase> enemyList = new List<EnemyBase>();
    [SerializeField] private int createCount = 15;
    public int CreateCount=>createCount;
    [HideInInspector]public UnityEvent OnWin;
    [HideInInspector] public UnityEvent OnLost;
    [HideInInspector] public UnityEvent OnEnterBlueDoor;
    [SerializeField] private float minInterval = 1f;
    [SerializeField] private float maxInterval = 2f;
    private bool isCreateFinsh = false;
    [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();
    [SerializeField] private List<SpawnPoint> redDoorList = new List<SpawnPoint>();
    private Coroutine createEnemyCor;
    private GameObject obj;
    void Start()
    {
        createEnemyCor = StartCoroutine(CreateEnemy());
    }
    void Update()
    {
        if (isCreateFinsh && enemyList.Count == 0 && GameManager.Instance.Hp>0)
        {
            OnWin?.Invoke();
            StopCoroutine(createEnemyCor);
        }
        if (GameManager.Instance.Hp == 0)
        {
            OnLost?.Invoke();
            StopCoroutine(createEnemyCor);
        }
    }
    IEnumerator CreateEnemy()
    {
        for (int i = 0; i < createCount; i++)
        {
            obj = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)]);
            int index = Random.Range(0, redDoorList.Count);
            obj.transform.position = redDoorList[index].transform.position;
            obj.transform.rotation = Quaternion.identity;
            EnemyBase ebComp = obj.GetComponent<EnemyBase>();
            ebComp.SetMovePath(redDoorList[index].GetPath());
            enemyList.Add(ebComp);
            ebComp.OnDestroyAction.AddListener(() => enemyList.Remove(ebComp));
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
        }
        isCreateFinsh = true;
    }
}
