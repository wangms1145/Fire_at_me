// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerUI : MonoBehaviour
// {
//     // Start is called before the first frame update
//     private PlayerScript ply;
//     void Start()
//     {
//         ply = GetComponentInParent<PlayerScript>();
//         gameObject.SetActive(ply.IsOwner);
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private PlayerScript ply;
    public GameObject deathPanel;  // Assign your Death Panel UI here
    private bool deathPanelActivated = false; // Ensure it only activates once

    void Start()
    {
        ply = GetComponentInParent<PlayerScript>();
        gameObject.SetActive(ply.IsOwner);
    }

    void Update()
    {
        // Check if player is dead and the panel hasn't been activated yet
        if (!deathPanelActivated)
        {
            if (!ply.isAlive)
            {
                ActivateDeathPanel();
            }
        }
        else
        {
            if (ply.isAlive)
            {
                // If player is alive and the death panel is active, deactivate it
                deathPanel.SetActive(false);
                deathPanelActivated = false;
            }
        }
    }
    void ActivateDeathPanel()
    {
        if (deathPanel != null)
        {
            deathPanel.SetActive(true);
            deathPanelActivated = true;

            // Optionally pause the game
        }
        else
        {
            Debug.LogWarning("Death panel not assigned in PlayerUI!");
        }
    }
}
