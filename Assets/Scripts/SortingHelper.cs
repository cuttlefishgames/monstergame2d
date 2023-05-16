using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SortingHelper : MonoBehaviour
{
    [SerializeField] private Transform _sortingTransform;
    [SerializeField] private SortingGroup _sortingGroup;
    [SerializeField] private bool _static = true;
    [SerializeField] private int _offset = 0;
    public int _zOrder;

    static int _layersShift = 20;
    static float _scaler = 5f;

    private void OnValidate()
    {
        if (_sortingTransform == null)
            _sortingTransform = transform;

        Sort();
    }

    private void Awake()
    {
        if (_static)
        {
            Sort();
            enabled = false;
        }
    }

    private void Update()
    {
        Sort();
    }

    private void Sort()
    {
        if (_sortingGroup == null)
        {
            _sortingGroup = gameObject.AddComponent<SortingGroup>();
        }
        _sortingGroup.sortingOrder = Mathf.FloorToInt(_sortingTransform.position.y * _scaler) * -_layersShift + _offset;
        _zOrder = _sortingGroup.sortingOrder;
    }
}
