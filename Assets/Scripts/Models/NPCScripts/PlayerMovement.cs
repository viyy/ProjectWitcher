using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PlayerSpace
{
    public class PlayerMovement : MonoBehaviour
    {
        int _groundMask;
        private float _speed = 10;
        private Vector3 _direction;
        float _camRayLength = 100f;
        private Camera _cam;
        public Camera Cam
        {
            get { return _cam; }
            set { _cam = value; }
        }
        private Rigidbody _rb;
        public Rigidbody RB
        {
            get { return _rb; }
            set { _rb = value; }
        }
        private bool _action;
        private GameObject temp;
        public string currentLayer;

        private void Awake()
        {
            currentLayer = "Lobby";
            _groundMask = LayerMask.GetMask("Shootable", "Wall", currentLayer);
            Cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            RB = GetComponent<Rigidbody>();
            _action = false;
        }

        private void FixedUpdate()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            Move(h, v);
            RotatePlayer();

        }

        private void Move(float h, float v)
        {
            _direction.Set(h, 0, v);
            _direction = _direction.normalized * _speed * Time.deltaTime;
            RB.MovePosition(transform.position + _direction);
        }

        private void RotatePlayer()
        {
            Ray CamRay = Cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit groundHit;
            if (Physics.Raycast(CamRay, out groundHit, _camRayLength, _groundMask))
            {
                Vector3 PlayerToMouse = groundHit.point - transform.position;
                PlayerToMouse.y = 0f;
                Quaternion newRotation = Quaternion.LookRotation(PlayerToMouse);
                RB.MoveRotation(newRotation);
            }
        }
    }
}
