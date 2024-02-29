using ScriptableObjectDependencyInjection;
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
    [SerializeField] private FloatReference mouseSensitivity;
    [SerializeField] private StringReference controlScheme;

    [ReadOnly][Tooltip("Rotation Speed is in Degrees")][SerializeField] float characterRotationSpeed = 1080f;


    float lowerResolution = 1f;
    bool controller = false;
    private void Start() {
        cachedCamera = Camera.main;
        lowerResolution = Mathf.Max(Screen.width, Screen.height) / 1080f;
        if (controlScheme.Value == ControlScemes.Gamepad) {
            crosshair.gameObject.SetActive(false);
            controller = true;
        }
    }


    public void SetAimProperties(float speed, float rotationSpeed) {
        crosshairSpeed = speed;
        characterRotationSpeed = rotationSpeed;
    }

    public void Look(Vector2 looKDelta) {
        //MoveCrosshairToMousePosition();
        Vector2 lookDir;
        if (controller) { 
            if (looKDelta.magnitude < 0.1f) { return; }
            lookDir = looKDelta;
        }
        else { 
            lookDir = (Vector2)crosshair.position - (Vector2)transform.position; 
        }

        CustomLook(looKDelta, lookDir);

    }

    public void CustomLook(Vector2 lookDelta, Vector2 customLookDir) {
        crosshair.position = (Vector2)crosshair.position + (Time.deltaTime * mouseSensitivity.Value * lookDelta);

        // code to ensure that the crosshair stays within the screen
        ClampCrosshairToScreen();


        //MoveCrosshairToMousePosition();

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.forward, customLookDir) * Quaternion.Euler(0, 0, 90.0f), characterRotationSpeed * Time.deltaTime);
    }

    void ClampCrosshairToScreen() {
        Vector3 bottomleft = cachedCamera.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = cachedCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        bottomleft.x += 0.1f;
        bottomleft.y += 0.1f;
        topRight.x -= 0.1f;
        topRight.y -= 0.1f;

        crosshair.position = new Vector2(Mathf.Clamp(crosshair.position.x, bottomleft.x, topRight.x), Mathf.Clamp(crosshair.position.y, bottomleft.y, topRight.y));
    }

    void MoveCrosshairToMousePosition() {
        Vector2 mousePos = GetMousePositionWorld();
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
        if (controller) { return transform.position + transform.right * 5.0f; }
        return crosshair.position;
        //return cachedCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
