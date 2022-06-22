using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
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
        shootController.OnFire += ShootShell;
    }
    public void ShootShell()
    {
       var shellObj = Instantiate(shellPrefab,firePosition.position,firePosition.rotation);
        shellObj.GetComponent<Rigidbody>().velocity = firePosition.forward * projectileSpeed;
    }
}
