using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float controlSpeed = 20f;
    [SerializeField] InputAction movement;
    [SerializeField] float xRange = 13f;
    [SerializeField] float yRange = 10f;

    [Header("Positional Rotation")]
    [SerializeField] float rotationFactor = 1.1f;
    [SerializeField] float positionPitchFactor = -2f;
    [SerializeField] float controlPitchFactor = -20f;
    [SerializeField] float positionYawFactor = 2f;
    [SerializeField] float controlRollFactor = -20f;

    [Header("Weapons")]
    [SerializeField] InputAction weapons;
    [SerializeField] GameObject[] lasers;

    bool controlsEnabled = true;
    float xThrow;
    float yThrow;

    void Update()
    {
        if (!controlsEnabled) { return; }
        GetControlThrow();
        ProcessFiring();
        ProcessTranslation();
        ProcessRotation();
    }

    void OnDisable()
    {
        movement.Disable();
        weapons.Disable();
    }

    void OnEnable()
    {
        movement.Enable();
        weapons.Enable();
    }

    public void SetControlsEnabled(bool enabled)
    {
        if (enabled)
        {
            controlsEnabled = true;
            OnEnable();
        }
        else
        {
            controlsEnabled = false;
            OnDisable();
        }
    }

    private void GetControlThrow()
    {
        xThrow = movement.ReadValue<Vector2>().x;
        yThrow = movement.ReadValue<Vector2>().y;
    }

    private void ProcessFiring()
    {
        if (weapons.IsPressed())
        {
            SetLasersActive(true);
        }
        else
        {
            SetLasersActive(false);
        }
    }
    
    private void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;
        float pitch = pitchDueToPosition + pitchDueToControlThrow;

        float yawDueToPosition = transform.localPosition.x * positionYawFactor;
        float yaw = yawDueToPosition;

        float rollDueToControlThrow = xThrow * controlRollFactor;
        float roll = rollDueToControlThrow;

        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, roll); // x,y,z == pitch,yaw,roll
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, rotationFactor);
    }
    

    private void ProcessTranslation()
    {
        float newXPos = transform.localPosition.x + (xThrow * controlSpeed * Time.deltaTime);
        float newYPos = transform.localPosition.y + (yThrow * controlSpeed * Time.deltaTime);

        newXPos = Mathf.Clamp(newXPos, -xRange, xRange);
        newYPos = Mathf.Clamp(newYPos, -yRange, yRange);

        transform.localPosition = new Vector3(newXPos, newYPos, transform.localPosition.z);
    }

    private void SetLasersActive(bool isActive)
    {
        foreach(GameObject laser in lasers)
        {
            ParticleSystem.EmissionModule emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }
}
