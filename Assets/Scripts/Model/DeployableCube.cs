using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DeployableCube : MonoBehaviour
{
    public E_Deployable_Type type;
    private GameObject _currentObj;
    private GameObject _eff;
    private void Awake()
    {
        _eff = transform.Find("Aura 1").gameObject;
        _eff.SetActive(false);
    }
    public bool Deploy(GameObject obj)
    {
        if (obj != null && _currentObj == null)
        {
            _currentObj = obj;
            obj.transform.position = transform.position;
            return true;
        }
        return false;
    }
    public void Remove()
    {
        if (_currentObj != null)
        {
            Destroy(_currentObj);
            _currentObj = null;
        }
    }
    public void SetEffActive(bool flag)
    {
        if (_eff == null)
        {
            print("_effÎª¿Õ");
            return;
        }
        _eff.SetActive(flag);
    }
}