using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 movement;

    public static bool attack;
    public static bool[] abilities;

    public static bool inventoryOpen;
    public static bool consumeSlotOne;
    public static bool consumeSlotTwo;

    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction attackAction;
    private InputAction openInventoryAction;

    private InputAction ability1Action;
    private InputAction ability2Action;
    private InputAction ability3Action;

    private InputAction consumeSlotOneAction;
    private InputAction consumeSlotTwoAction;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        abilities = new bool[3];

        moveAction = playerInput.actions["Move"];
        attackAction = playerInput.actions["Attack"];
        openInventoryAction = playerInput.actions["ToggleInventory"];

        ability1Action = playerInput.actions["Ability1"];
        ability2Action = playerInput.actions["Ability2"];
        ability3Action = playerInput.actions["Ability3"];

        consumeSlotOneAction = playerInput.actions["ConsumeSlot1"];
        consumeSlotTwoAction = playerInput.actions["ConsumeSlot2"];
    }

    private void Update()
    {
        movement = moveAction.ReadValue<Vector2>();
        attack = attackAction.WasPressedThisFrame();

        abilities[0] = ability1Action.WasPressedThisFrame();
        abilities[1] = ability2Action.WasPressedThisFrame();
        abilities[2] = ability3Action.WasPressedThisFrame();

        consumeSlotOne = consumeSlotOneAction.WasPressedThisFrame();
        consumeSlotTwo = consumeSlotTwoAction.WasPressedThisFrame();

        if (openInventoryAction.WasPressedThisFrame())
        {
            inventoryOpen = !inventoryOpen;
        }
    }
}
