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
    [SerializeField] float characterRotationSpeedControllerMultiplier = 0.5f;


    bool controller = false;

    private void Start() {
        cachedCamera = Camera.main;
        if (controlScheme.Value == ControlScemes.Gamepad) {
            controller = true;
        }
    }


    public void SetAimProperties(float speed, float rotationSpeed) {
        crosshairSpeed = speed;
        characterRotationSpeed = rotationSpeed;
    }

    public void Look(Vector2 lookDelta) {
        //MoveCrosshairToMousePosition();
        Vector2 lookDir;
        if (controller) { 
            // ensure that the crosshair doesnt exceed 5.0f from the player
            Vector2 _d = (Vector2)crosshair.position - (Vector2)transform.position;
            if (_d.magnitude > 5.0f) { crosshair.position = (Vector2)transform.position + _d.normalized * 5.0f; }

            if (lookDelta.magnitude < 0.1f) { return; }
            lookDir = lookDelta;
            crosshair.position = transform.position + transform.right * 5.0f;
            ClampCrosshairToScreen();

            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                Quaternion.LookRotation(Vector3.forward, lookDir) * Quaternion.Euler(0, 0, 90.0f),
                characterRotationSpeed * characterRotationSpeedControllerMultiplier * Time.deltaTime);
        }
        else { 
            lookDir = (Vector2)crosshair.position - (Vector2)transform.position; 
            crosshair.position = (Vector2)crosshair.position + (Time.deltaTime * mouseSensitivity.Value * lookDelta);
            ClampCrosshairToScreen();

            transform.rotation = Quaternion.RotateTowards(transform.rotation, 
                Quaternion.LookRotation(Vector3.forward, lookDir) * Quaternion.Euler(0, 0, 90.0f), 
                characterRotationSpeed * Time.deltaTime);
        }

        

    }

    public void CustomLook(Vector2 lookDelta, Vector2 customLookDir) {
        //MoveCrosshairToMousePosition();
        crosshair.position = (Vector2)crosshair.position + (Time.deltaTime * mouseSensitivity.Value * lookDelta);

        ClampCrosshairToScreen();



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

    public Vector2 GetMousePositionRaw() {
        return Input.mousePosition;
    }

    public Vector2 GetMousePositionWorld() {
        return crosshair.position;
        //return cachedCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
