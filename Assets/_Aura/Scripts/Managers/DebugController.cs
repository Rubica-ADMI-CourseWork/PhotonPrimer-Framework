using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugController : MonoBehaviour
{
    public TMP_Text debugInfoText;

    public static DebugController Instance;

    private void Awake()
    {
        Instance = this;
    }
}
