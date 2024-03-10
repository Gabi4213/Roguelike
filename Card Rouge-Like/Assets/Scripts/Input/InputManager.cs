using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 movement;

    public static bool attack;
    public static bool inventoryOpen;

    public static bool consumeSlotOne;
    public static bool consumeSlotTwo;



    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction attackAction;
    private InputAction openInventoryAction;

    private InputAction consumeSlotOneAction;
    private InputAction consumeSlotTwoAction;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        attackAction = playerInput.actions["Attack"];
        openInventoryAction = playerInput.actions["ToggleInventory"];

        consumeSlotOneAction = playerInput.actions["ConsumeSlot1"];
        consumeSlotTwoAction = playerInput.actions["ConsumeSlot2"];
    }

    private void Update()
    {
        movement = moveAction.ReadValue<Vector2>();
        attack = attackAction.WasPressedThisFrame();

        consumeSlotOne = consumeSlotOneAction.WasPressedThisFrame();
        consumeSlotTwo = consumeSlotTwoAction.WasPressedThisFrame();

        if (openInventoryAction.WasPressedThisFrame())
        {
            inventoryOpen = !inventoryOpen;
        }
    }
}
