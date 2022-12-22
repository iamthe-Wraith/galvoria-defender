using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] ParticleSystem explosion;

    private ScoreBoard scoreBoard;
    private PlayerControls playerControls;
    private float resetLevelDelay = 1f;

    private void Start()
    {
        scoreBoard = FindObjectOfType<ScoreBoard>();
        playerControls = GetComponent<PlayerControls>();
    }

    private void OnTriggerEnter(Collider other)
    {
        ProcessCollision(other.gameObject);
    }

    private void ExecuteCrashSequence()
    {
        if (!explosion.isPlaying)
        {
            explosion.Play();
        }

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
    }

    private void ProcessCollision(GameObject other)
    {
        playerControls.SetControlsEnabled(false);
        ExecuteCrashSequence();
        StartCoroutine(WaitAndLoadScene(SceneManager.GetActiveScene().buildIndex, resetLevelDelay));
    }

    private IEnumerator WaitAndLoadScene(int scene, float delay)
    {
        if (scoreBoard != null)
        {
            scoreBoard.ResetScore();
        }

        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scene);        
    }

    private IEnumerator WaitAndLoadScene(string scene, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scene);        
    }
}
