using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerShoot : MonoBehaviourPun
{
    [SerializeField] float projectileSpeed;
    [SerializeField] WeaponSO weaponData;
    [SerializeField] Transform firePosition;
  
    ShootController shootController;

    private void OnEnable()
    {
        shootController = FindObjectOfType<ShootController>();
    }
    private void Start()
    {
    
        if (shootController == null){ Debug.Log("no shoot controller."); }
        shootController.OnFire += ShootShell;
    }

  
    public void ShootShell()
    {
       AudioManager.Instance.PlayShellFiringFX();
       HandleShoot();
    }


    private void HandleShoot()
    {
        var shell = PhotonNetwork.Instantiate(weaponData.shell.name, firePosition.position, firePosition.rotation);
        shell.GetComponent<Rigidbody>().velocity = transform.forward * projectileSpeed;
    }
    private void OnDestroy()
    {
        shootController.OnFire -= ShootShell;
    }
}
