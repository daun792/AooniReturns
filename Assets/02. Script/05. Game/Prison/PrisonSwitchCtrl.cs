using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PrisonSwitchCtrl : NetworkBehaviour
{
    [SerializeField] GameObject prisonDoor;

    private List<PlayerRef> playerList = new(8);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Human"))
        {
            if (collision.GetComponentInParent<CharacterCtrl>().Object.HasStateAuthority)
            {
                var player = collision.GetComponentInParent<CharacterCtrl>().Object.StateAuthority;

                RPC_ManageDoor(true, player);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Human"))
        {
            if (collision.GetComponentInParent<CharacterCtrl>().Object.HasStateAuthority)
            {
                var player = collision.GetComponentInParent<CharacterCtrl>().Object.StateAuthority;

                RPC_ManageDoor(false, player);
            }
        }
    }

    private void RPC_ManageDoor(bool _isPlayerEnter, PlayerRef _player)
    {
        if (_isPlayerEnter)
        {
            if (!playerList.Contains(_player))
            {
                playerList.Add(_player);
            }

            prisonDoor.SetActive(false);

            var playerObj = App.Manager.Network.Runner.GetPlayerObject(_player);
            var charCtrl = playerObj.GetComponent<CharacterCtrl>();

            App.Manager.UI.Chat.SendNotice($"<color=#FFFF00>{charCtrl.NickName}´ÔÀÌ ¼ö°¨ÀÚµéÀ» Å»¿Á½ÃÄ×½À´Ï´Ù.</color>");
        }
        else
        {
            if (playerList.Contains(_player))
            {
                playerList.Remove(_player);
            }

            if (playerList.Count <= 0)
            {
                prisonDoor.SetActive(true);
            }
        }
    }
}