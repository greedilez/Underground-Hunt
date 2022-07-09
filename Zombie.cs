using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] private float Health = 100;

    private Animator animator;

    private NavMeshAgent agent;

    private bool isZombieDead = false;
    
    public bool IsZombieDead{ get => isZombieDead; }

    private BoxCollider boxCollider;

    private GameObject player;

    private void Awake(){
        InitializeFields();
    }

    private protected void InitializeFields(){
        player = GameObject.FindGameObjectWithTag("Player");
        boxCollider = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate(){
        DeathOnZeroHealth();
        RunForPlayer();
    }

    public void SetDamage(float damage){
        if(Health > 0){
            Health -= damage;
        }
    }

    private protected void DeathOnZeroHealth(){
        if(!isZombieDead){
            if(Health <= 0){
                animator.SetBool("Killed", true);
                boxCollider.enabled = false;
                agent.enabled = false;
                isZombieDead = true;
            }
        }
    }

    private protected void RunForPlayer(){
        if(!isZombieDead){
            agent.SetDestination(player.transform.position);
        }
    }
}
