using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Move : MonoBehaviour
{
    private List<Transform> path = new List<Transform>();
    private Vector3 currentTarget;
    private Vector3 originPos;
    private float rotateSpeed;
    private float currentDistance;
    private int currentIndex = -1;
    private float moveSpeed;
    private float timer = 0;
    private float duration;
    private EnemyBase tower;
    private bool isPause = false;
    void Start()
    {
        tower = GetComponent<EnemyBase>();
        if (tower == null) return;
        path = tower.GetMovePath();
        moveSpeed = tower.enemyConfig.MoveSpeed;
        rotateSpeed = tower.enemyConfig.RotateSpeed;
        ResetState();
    }
    void Update()
    {
        if (path == null) return;
        if (!isPause)
        {
            if (Mathf.Abs(Vector3.Distance(transform.position, currentTarget)) < 0.1f)
            {
                ResetState();
            }
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(originPos, currentTarget, Mathf.Clamp(timer / duration, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(currentTarget - transform.position), Time.deltaTime * rotateSpeed);
        }
    }
    private void ResetState()
    {
        currentIndex += 1;
        if (currentIndex >= path.Count)
        {
            EnemyManager.Instance.OnEnterBlueDoor?.Invoke();
            Destroy(gameObject);
            return;
        }
        timer = 0;
        originPos = transform.position;
        currentTarget = path[currentIndex].position;
        currentDistance = Vector3.Distance(originPos, currentTarget);
        duration = currentDistance / moveSpeed;
    }
    public void PauseMove()
    {
        isPause = true;
    }
    public void ContinueMove()
    {
        isPause = false;
    }
}
