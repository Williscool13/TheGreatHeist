using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Vector2 move;
    Vector2 look;
    bool firePressed;
    bool recoverPressed;
    bool flarePressed;
    bool sprintPressed;
    bool dashPressed;

    bool fireHeld;
    bool recoverHeld;
    bool flareHeld;
    bool sprintHeld;
    bool dashHeld;

    [SceneObjectsOnly][SerializeField] private PlayerStateMachine playerCharacter;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private StringReference controlScheme;

    private void Start() {
        playerInput.defaultControlScheme = controlScheme.Value;
        //playerInput.SwitchCurrentControlScheme(controlScheme.Value);
    }

    private void Update() {
        playerCharacter.SetInputs(move, look, firePressed, fireHeld, recoverPressed, recoverHeld, flarePressed, flareHeld, sprintPressed, sprintHeld, dashPressed, dashHeld);


        firePressed = false;
        recoverPressed = false;
        flarePressed = false;
        sprintPressed = false;
        dashPressed = false;
    }

    public void OnMove(InputValue value) {
        move = value.Get<Vector2>();
    }

    public void OnLook(InputValue value) {
        look = value.Get<Vector2>();
    }

    public void OnFire(InputValue value) {
        if (value.isPressed) {
            firePressed = true;
        }

        if (value.Get<float>() > 0.5f) {
            fireHeld = true;
        } else {
            fireHeld = false;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnMenu(InputValue value) {
        if (value.isPressed) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }


    public void OnRecover(InputValue value) {
        if (value.isPressed) {
            recoverPressed = true;
        }

        if (value.Get<float>() > 0.5f) {
            recoverHeld = true;
        } else {
            recoverHeld = false;
        }
    }

    public void OnSpecial(InputValue value) {
        if (value.isPressed) {
            flarePressed = true;
        }

        if (value.Get<float>() > 0.5f) {
            flareHeld = true;
        } else {
            flareHeld = false;
        }
    }

    public void OnSprint(InputValue value) {
        /*if (value.isPressed) {
            sprintPressed = true;
        }

        if (value.Get<float>() > 0.5f) {
            sprintHeld = true;
        }
        else {
            sprintHeld = false;
        }*/
    }

    public void OnDash(InputValue value) {
        if (value.isPressed) {
            dashPressed = true;
        }

        if (value.Get<float>() > 0.5f) {
            dashHeld = true;
        }
        else {
            dashHeld = false;
        }
    }
}


public static class ControlScemes
{
    public static string KeyboardMouse = "Keyboard&Mouse";
    public static string Gamepad = "Gamepad";
}