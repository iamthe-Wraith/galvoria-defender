using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionVFX;

    [Tooltip("The number of points the player is awarded for destroying this enemy")]
    [SerializeField]
    int points;

    [SerializeField] int hitPoints = 10;

    ScoreBoard scoreBoard;
    GameObject parentGameObject;
    int startingHitPoints;


    void Start()
  {
    scoreBoard = FindObjectOfType<ScoreBoard>();
    startingHitPoints = hitPoints;

    AddRigidBody();
    AddParent();
  }

  private void AddRigidBody()
  {
    Rigidbody rb = gameObject.AddComponent<Rigidbody>();
    rb.useGravity = false;
  }

  private void AddParent()
  {
    parentGameObject = GameObject.FindWithTag("SpawnAtRuntime");
  }
  

  private void OnParticleCollision(GameObject other)
    {
        Debug.Log($"{name} hit by {other.gameObject.name}");
        ProcessHit();
    }

    private void Kill()
    {
        if (scoreBoard != null)
        {
            scoreBoard.UpdateScore(points);
        }
        
        ParticleSystem vfx = Instantiate(explosionVFX, transform.position, Quaternion.identity);
        vfx.transform.parent = parentGameObject.transform;
        Destroy(gameObject);
    }

    private void ProcessHit()
    {
        hitPoints -= 10;
        // float hitPointsPercent = Mathf.Round((float)(((float)hitPoints / (float)startingHitPoints) * 100.0))

        if (hitPoints <= 0)
        {
            Kill();
        }
    }
}
