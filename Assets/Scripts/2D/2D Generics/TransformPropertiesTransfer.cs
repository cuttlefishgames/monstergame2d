using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TransformPropertiesTransfer : MonoBehaviour
{
    public class TransferData 
    {
        public Transform main;
        public Transform target;
        public bool transferPosition = true;
        public bool transferRotation = true;
        public bool transferScale = true;
    }

    [Serializable]
    public class BoneTransferData
    {
        public Transform bone;
        public bool transferPosition = true;
        public bool transferRotation = true;
        public bool transferScale = true;
    }

    [SerializeField] private List<BoneTransferData> _bonesTransferData = new List<BoneTransferData>();
    private List<TransferData> transferData = new List<TransferData>();

    public void FillBones(List<Transform> bones)
    {
        int j = -1;
        foreach(var boneData in _bonesTransferData)
        {
            j++;

            if (boneData == null || boneData.bone == null)
                continue;

            var refBone = bones[j];
            var transferData = new TransferData();
            transferData.main = refBone;
            transferData.target = boneData.bone;
            transferData.transferPosition = boneData.transferPosition;
            transferData.transferRotation = boneData.transferRotation;
            transferData.transferScale = boneData.transferScale;
            this.transferData.Add(transferData);
        }
    }

    public void Transfer()
    {
        foreach(var pair in transferData)
        {
            if (pair.transferPosition)
            {
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

            //pair.target.localPosition = pair.main.localPosition;
            //pair.target.localRotation = pair.main.localRotation;
            //pair.target.localScale = pair.main.localScale;
        }
    }
}