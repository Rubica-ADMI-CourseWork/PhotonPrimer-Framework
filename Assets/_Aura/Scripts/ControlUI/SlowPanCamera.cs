using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowPanCamera : MonoBehaviour
{
    [SerializeField] float panSpeed;

    private void Update()
    {
        transform.Translate(transform.forward * panSpeed * Time.deltaTime);
    }
}
