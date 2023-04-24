using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovingBlob : MonoBehaviour
{
    public Transform _target;
    private NavMeshAgent _agent;
    //[SerializeField] private List<SpriteRenderer> _renderers;
    private int _direction = 1;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    private void Update()
    {
        _agent.isStopped = false;
        _agent.SetDestination(_target.transform.position);

        var lookDirection = transform.position.x < _target.position.x ? 1 : -1;
        if(lookDirection != _direction)
        {
            _direction *= -1;

            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            //if (_direction == -1)
            //    _renderers.ForEach(r => r.flipX = true);
            //else
            //    _renderers.ForEach(r => r.flipX = false);
        }
    }
}
