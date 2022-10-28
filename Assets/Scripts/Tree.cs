using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    // static akan membuat variable ini shared pada semua Tree
    public static List<Vector3> AllPosition = new List<Vector3>();

    private void OnEnable()
    {
        AllPosition.Add(this.transform.position);
    }
    private void OnDisable()
    {
        AllPosition.Remove(this.transform.position);
    }
}
