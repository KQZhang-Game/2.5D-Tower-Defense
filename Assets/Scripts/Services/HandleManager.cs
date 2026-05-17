using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class HandleManager : Singleton<HandleManager>
{
    public GameObject _holdingObj;
    public float depth = 5f;
    private Ray _ray;
    private RaycastHit _hit;
    private TowerBase _towerBaseConponent;
    protected override bool IsPersistent => false;
    public void InstantiatePrefab(GameObject towerPrefab)
    {
        if (_holdingObj == null)
        {
            _holdingObj = Instantiate(towerPrefab);
            _towerBaseConponent = _holdingObj.GetComponent<TowerBase>();
            if (_towerBaseConponent == null)
            {
                Destroy(_holdingObj);
                _holdingObj = null;
                return;
            }
            PlatformManager.Instance.ShowDeployableCube(_towerBaseConponent.towerConfig.DeployableType, true);
        }
    }
    private int GetLayer()
    {
        if (_towerBaseConponent == null) return 0;
        string name = "";
        E_Deployable_Type typeName = (E_Deployable_Type)_towerBaseConponent.towerConfig.DeployableType;
        switch (typeName)
        {
            case E_Deployable_Type.PLATFORM:
                name = "DeployablePlatform";
                break;
            case E_Deployable_Type.FLOOR:
                name = "DeployableFloor";
                break;
            default:
                break;
        }
        return 1 << LayerMask.NameToLayer(name);
    }
    private void ObjFollow()
    {
        if (_holdingObj == null) return;
        if (_holdingObj != null)
        {
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, 100, GetLayer()))
            {
                _holdingObj.transform.position = _hit.transform.position;
            }
            else
            {
                _holdingObj.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, depth));
            }
        }
    }
    private void CancelSelect()
    {
        if (_holdingObj != null)
            _holdingObj = null;
        PlatformManager.Instance.ShowDeployableCube(_towerBaseConponent.towerConfig.DeployableType, false);
    }
    private void Update()
    {
        if (_holdingObj == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(_ray, out _hit, 100, 1<<LayerMask.NameToLayer("Player")))
                {
                    _towerBaseConponent = _hit.transform.GetComponent<TowerBase>();
                    if (_towerBaseConponent != null)
                    {
                        _towerBaseConponent.DrawAttackRange();
                    }
                }
                else
                {
                    LineWriter.Instance.HideLine();
                }
            }
        }
        if (_holdingObj != null)
        {
            ObjFollow();
            if (Input.GetMouseButtonDown(0))
            {
                if (_hit.transform != null && _hit.transform.GetComponentInChildren<DeployableCube>().Deploy(_holdingObj))
                {
                    _towerBaseConponent.OnDeploy?.Invoke();
                    GameManager.Instance.SubCost(_towerBaseConponent.towerConfig.DeployCost);
                    CancelSelect();
                }
                else
                {
                    Destroy(_holdingObj);
                    PlatformManager.Instance.ShowDeployableCube(_towerBaseConponent.towerConfig.DeployableType, false);
                }
            }
        }
        if (Input.GetMouseButtonDown(1) && _holdingObj != null)
        {
            Destroy(_holdingObj);
            PlatformManager.Instance.ShowDeployableCube(_towerBaseConponent.towerConfig.DeployableType, false);
        }
    }
}
