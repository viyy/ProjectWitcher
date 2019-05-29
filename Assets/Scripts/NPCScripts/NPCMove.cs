using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMove : MonoBehaviour
{
    private float speed;
    private Rigidbody rb;

    private void Awake()
    {
        speed = 5f;
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 direction)
    {
        direction = direction.normalized * speed * Time.deltaTime;
        rb.MovePosition(transform.position + direction);
    }

    private void Rotate(Vector3 dir)
    {
        dir.y = 0f;
        dir = dir.normalized * speed;
        Quaternion newRotation = Quaternion.LookRotation(dir);
        rb.MoveRotation(newRotation);
    }
}
