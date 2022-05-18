using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class CameraController : NetworkBehaviour
{
    public bool SinglePlayer;

    [Header("Camera Online Solution")]
    public GameObject Camera;
    public GameObject VirtualCameraController;

    [Header("Camera Game Settings")]
    [SerializeField] InputDirector Director;

    public float SensitivityX = 8f;
    public float SensitivityY = 0.5f;
    float mouseX, mouseY;

    float yClamp = 85f; //Lock Rotation
    float yRotation = 0f;

    [Header("Camera Game Controller")]
    [SerializeField] GameObject CamRotator;
    [SerializeField] Transform Player;

    public Vector2 mouseInput;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner && !SinglePlayer)
            EnableCamera();
    }

    public void EnableCamera()
    {
        Camera.SetActive(true);
        VirtualCameraController.SetActive(true);
    }

    private void Update()
    {
        if (Director.enabled)
        {
            MouseLook();
            Player.Rotate(Vector3.up, mouseX * Time.deltaTime); //X INPUT
        }
    }

    void MouseLook()
    {
        RotateViewHorizontal(mouseInput);
        RotateViewVertical();
    }

    public void RotateViewHorizontal(Vector2 MouseInput)
    {
        mouseX = MouseInput.x * SensitivityX;
        mouseY = MouseInput.y * SensitivityY;
    }

    public void RotateViewVertical() //Y AXYS INPUT
    {
        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -yClamp, yClamp);
        Vector3 targetRotation = CamRotator.transform.eulerAngles;
        targetRotation.x = yRotation;
        CamRotator.transform.eulerAngles = targetRotation;
    }

    public void GetMouseInput(Vector2 MouseInput)
    {
        mouseInput = MouseInput;
    }
}
