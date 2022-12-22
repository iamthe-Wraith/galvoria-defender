using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionVFX;

    [SerializeField] Transform parent;

    [SerializeField] TextMeshProUGUI hitPointsText;

    [Tooltip("The number of points the player is awarded for destroying this enemy")]
    [SerializeField]
    int points;

    [SerializeField] int hitPoints = 10;

    ScoreBoard scoreBoard;
    int startingHitPoints;


    void Start()
    {
        scoreBoard = FindObjectOfType<ScoreBoard>();
        startingHitPoints = hitPoints;
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
        vfx.transform.parent = parent;
        Destroy(gameObject);
    }

    private void ProcessHit()
    {
        hitPoints -= 10;

        Debug.Log("hitpoints: " + hitPoints);
        Debug.Log("startingHitPoints: " + startingHitPoints);
        double dec = ((double)hitPoints / (double)startingHitPoints);
        Debug.Log(dec);
        Debug.Log("calc2: " + Mathf.Round((float)(((double)hitPoints / (double)startingHitPoints) * 100.0)));

        hitPointsText.text = $"{Mathf.Round((float)(((double)hitPoints / (double)startingHitPoints) * 100.0))}%";

        if (hitPoints <= 0)
        {
            Kill();
        }
    }
}
