using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformManager : Singleton<PlatformManager> 
{
    protected override bool IsPersistent => false;
    [SerializeField]private GameObject floorObjsFather;
    [SerializeField] private GameObject platformObjsFather;
    private List<DeployableCube> _platforms;
    void Start()
    {
        _platforms = new List<DeployableCube>();
        if (floorObjsFather != null && platformObjsFather != null)
        {
            _platforms = floorObjsFather.GetComponentsInChildren<DeployableCube>().ToList();
            _platforms.AddRange(platformObjsFather.GetComponentsInChildren<DeployableCube>().ToList());
        }
    }

    public void ShowDeployableCube(E_Deployable_Type type,bool flag)
    {
        for(int i = 0; i < _platforms.Count; i++)
        {
            if (_platforms[i].type == type)
            {
                _platforms[i].SetEffActive(flag);
            }
        }
    }
}
