using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TransformPropertiesTransfer : MonoBehaviour
{
    [Serializable]
    public class TransformData 
    {
        public Transform main;
        public Transform target;
        public bool transferPosition = true;
        public bool transferRotation = true;
        public bool transferScale = true;
    }

    [SerializeField] private List<TransformData> transformData = new List<TransformData>();

    private void Update()
    {
        foreach(var pair in transformData)
        {
            if (pair.transferPosition)
            {
                //pair.target.position = pair.main.position;
                pair.target.localPosition = pair.main.localPosition;
            }
            if (pair.transferRotation)
            {
                pair.target.localRotation = pair.main.localRotation;
            }
            if (pair.transferScale)
            {
                pair.target.localScale = pair.main.localScale;
            }
        }
    }
}
