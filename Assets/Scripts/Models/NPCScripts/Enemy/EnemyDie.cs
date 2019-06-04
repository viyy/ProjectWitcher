using UnityEngine;
using UnityEditor;
using UnityEngine.Animations;
using System.Collections;
using System.Collections.Generic;

public class EnemyDie : MonoBehaviour
{
    public delegate void DieContainer();
    public static DieContainer DieEvent;

    Animator dieAnim;
    float invisibleSwitch;
    Coroutine invis;

    private void Awake()
    {
        invisibleSwitch = 0f;
        //dieAnim = GetComponent<Animator>();
    }

    IEnumerator Switch()
    {
        WaitForSeconds wait = new WaitForSeconds(1);
        yield return wait;
        invisibleSwitch = invisibleSwitch + 50;
        

    }

    public void Die(MeshRenderer mesh)
    {
        mesh.material.color = Color.red;
        //dieAnim.SetTrigger("Die");
        if (invisibleSwitch < 255)
        {
            mesh.material.color = new Color(255, invisibleSwitch, invisibleSwitch, 255);
            transform.localScale += new Vector3(0.2f, -0.1f, 0.2f);
            invis = StartCoroutine(Switch());
        }
        else
        {
            Debug.Log("invis");
            DieEvent();
        }
    }
}