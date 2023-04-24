using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster
{
    public class PlayerMovementController : MonoBehaviour
    {
        public static PlayerMovementController Instance;

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Animator _animator;
        [SerializeField] [Range(0.1f, 100f)] private float _speed = 1;
        [SerializeField] [Range(0.1f, 100f)] private float _angles = 2;

        private Vector3 _inputDirection;

        private void Awake()
        {
            if(Instance != null)
            {
                Debug.LogError("There is a player controller instance already");
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
            }

            if(_rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody>();
            }

            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
        }

        void FixedUpdate()
        {
            var xAxys = Input.GetAxis("Horizontal");
            var yAxys = Input.GetAxis("Vertical");
            _inputDirection = Vector3.Normalize(new Vector3(xAxys, 0, yAxys)) * _speed;
            //Debug.Log("_inputDirection " + _inputDirection.ToString("0.00"));
            _inputDirection = new Vector3(_inputDirection.x, _rigidbody.velocity.y, _inputDirection.z);

            var cameraDir = Camera.main.transform.TransformDirection(_inputDirection);
            cameraDir = new Vector3(cameraDir.x, 0, cameraDir.z);
            cameraDir.Normalize();


            //_rigidbody.velocity = cameraDir * _speed;
            //_rigidbody.velocity = _inputDirection;
            //_rigidbody.velocity = _inputDirection;

            //transform.LookAt(transform.position + _inputDirection);
            //transform.LookAt(transform.position + cameraDir);

            Debug.Log(cameraDir.ToString("00.00"));


            if (_inputDirection.x != 0 || _inputDirection.z != 0)
            {
                _animator.SetInteger("State", 1);
                var originalLook = transform.rotation;
                transform.LookAt(transform.position + new Vector3(cameraDir.x, 0, cameraDir.z));
                var newLook = transform.rotation;
                transform.rotation = Quaternion.RotateTowards(originalLook, newLook, _angles);
                _rigidbody.velocity = new Vector3(cameraDir.x * _speed, _rigidbody.velocity.y, cameraDir.z * _speed);
            }
            else
            {
                _animator.SetInteger("State", 0);
                _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
            }
        }
    }
}