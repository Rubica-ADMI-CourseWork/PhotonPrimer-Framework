using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerShoot : MonoBehaviourPun
{
    [SerializeField] float projectileSpeed;
    [SerializeField] GameObject shellPrefab;
    [SerializeField] Transform firePosition;

    ShootController shootController;

    private void OnEnable()
    {
        shootController = FindObjectOfType<ShootController>();
    }
    private void Start()
    {
        if(shootController == null){ Debug.Log("no shoot controller."); }
        shootController.OnFire += ShootShell;
    }

  
    public void ShootShell()
    {
        AudioManager.Instance.PlayShellFiringFX();
        HandleShoot();
    }

    
    private void HandleShoot()
    {
        Debug.Log("Inside Shoot Shell");
        var shellObj = PhotonNetwork.Instantiate(shellPrefab.name, firePosition.position, firePosition.rotation);
        shellObj.GetComponent<Rigidbody>().velocity = firePosition.forward * projectileSpeed;
    }
    private void OnDestroy()
    {
        shootController.OnFire -= ShootShell;
    }
}
