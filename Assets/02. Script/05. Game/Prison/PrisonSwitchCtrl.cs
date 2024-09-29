using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PrisonSwitchCtrl : NetworkBehaviour
{
    [SerializeField] GameObject prisonDoor;

    private int playerCount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Human"))
        {
            RPC_ManageDoor(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Human"))
        {
            RPC_ManageDoor(false);
        }
    }

    [Rpc]
    private void RPC_ManageDoor(bool _isPlayerEnter)
    {
        playerCount += _isPlayerEnter ? 1 : -1;

        playerCount = Mathf.Max(0, playerCount);

        if (playerCount >= 1)
        {
            prisonDoor.SetActive(false);
        }
        else
        {
            prisonDoor.SetActive(true);
        }
    }
}