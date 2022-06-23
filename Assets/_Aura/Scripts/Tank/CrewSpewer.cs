using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CrewSpewer : MonoBehaviourPun
{
    [SerializeField] float xPlosionForce;
    [SerializeField] float xPlosionRadius;
    [SerializeField] float xPlosionUpForce;
    [SerializeField] int crewAmount;
    [SerializeField] GameObject crewPrefab;

    [PunRPC]
    public void SpewCrew()
    {
        for(int i = 0; i < crewAmount; i++)
        {
            var crewObj = PhotonNetwork.Instantiate(crewPrefab.name,transform.position,Quaternion.identity);
            crewObj.GetComponent<Rigidbody>().AddExplosionForce(xPlosionForce, transform.position, xPlosionRadius,xPlosionUpForce);
        }
    }
}
