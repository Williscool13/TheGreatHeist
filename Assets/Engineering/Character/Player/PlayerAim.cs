using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{


    Camera cachedCamera;

    public bool Stunned { get; set; } = false;

    [SerializeField] RectTransform crosshair;
    [ReadOnly][SerializeField] float crosshairSpeed = 0.35f;
    [ReadOnly][Tooltip("Rotation Speed is in Degrees")][SerializeField] float characterRotationSpeed = 1080f;


    float lowerResolution = 1f;
    private void Start() {
        cachedCamera = Camera.main;
        lowerResolution = Mathf.Max(Screen.width, Screen.height) / 1080f;
    }


    public void SetAimProperties(float speed, float rotationSpeed) {
        crosshairSpeed = speed;
        characterRotationSpeed = rotationSpeed;
    }

    public void Look() {
        MoveCrosshairToMousePosition();

        Vector3 crossPos = cachedCamera.ScreenToWorldPoint(crosshair.position);
        Vector2 look = (Vector2)crossPos - (Vector2)transform.position;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.forward, look) * Quaternion.Euler(0, 0, 90.0f), characterRotationSpeed * Time.deltaTime);
    }

    public void CustomLook(Vector2 lookDir, bool crosshairMove) {
        MoveCrosshairToMousePosition();

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.forward, lookDir) * Quaternion.Euler(0, 0, 90.0f), characterRotationSpeed * Time.deltaTime);
    }

    void MoveCrosshairToMousePosition() {
        Vector2 mousePos = GetMousePositionRaw();
        Vector2 distance = mousePos - (Vector2)crosshair.position;
        distance *= crosshairSpeed * lowerResolution * Time.deltaTime;


        if (Vector2.Distance(mousePos, (Vector2)crosshair.position) < distance.magnitude) {
            crosshair.position = mousePos;
        }
        else {
            crosshair.position += (Vector3)distance;
        }
    }

    public Vector2 GetMousePositionRaw() {
        return Input.mousePosition;
    }

    public Vector2 GetMousePositionWorld() {
        return cachedCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
