using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ShellPool : MonoBehaviour
{
    public WeaponSO weapon;
    public static ShellPool Instance;
    private void Awake()
    {
        Instance = this;
    }
    public List<GameObject> shells = new List<GameObject>();

    private void Start()
    {
       for(int i = 0; i < weapon.shellCount; i++)
        {
            GameObject obj = PhotonNetwork.Instantiate(weapon.shell.name, transform.position, Quaternion.identity);
            obj.GetComponent<ShellInfo>().playerNo = PhotonNetwork.LocalPlayer.ActorNumber;
            obj.SetActive(false);
            shells.Add(obj);
        }
    }

    public GameObject GetShell()
    {
        for(int i = 0; i < shells.Count; i++)
        {
            if (!shells[i].activeInHierarchy)
            {
                return shells[i];
            }
        }
        return null;
    }
}
