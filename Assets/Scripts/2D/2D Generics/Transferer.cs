using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transferer : MonoBehaviour
{
    public GameObject Disabletarget;
    public Animator Animator;
    public List<Transform> Bones;

    public void OnEnable()
    {
        Disabletarget.SetActive(false);
    }
}
