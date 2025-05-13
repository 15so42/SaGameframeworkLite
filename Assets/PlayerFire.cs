using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFire : MonoBehaviour
{
    private ChaState chaState;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
        playerInputActions.Player.Fire.performed += Fire;
    }

    private void OnDisable()
    {
        playerInputActions.Enable();
        playerInputActions.Player.Fire.performed += Fire;
    }

    void Fire(InputAction.CallbackContext ctx)
    {
        chaState.CastSkill("Fire");
    }

    // Start is called before the first frame update
    void Start()
    {
        chaState = GetComponent<ChaState>();
        chaState.LearnSkill(DesingerTables.Skill.data["Fire"]);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
