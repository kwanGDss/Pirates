using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 다른 객체의 태그가 "Player"인 경우
        if (other.CompareTag("Player"))
        {
            animator.SetBool("IsCollision", true);
            Debug.Log("Collision!");
        }
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
