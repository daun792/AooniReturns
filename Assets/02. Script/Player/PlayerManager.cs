using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;

public class PlayerManager : Manager
{
    private readonly List<CharacterCtrl> characterDict = new(4);

    public CharacterCtrl MyCtrl => FindMyChar();

    public IReadOnlyList<CharacterCtrl> AllPlayers => characterDict;

    public void SubmitPlayer(CharacterCtrl _char)
    {
        characterDict.Add(_char);
    }

    public CharacterCtrl FindMyChar()
    {
        for (int i = 0; i < characterDict.Count; ++i)
        {
            if (characterDict[i].Object.StateAuthority.PlayerId == App.Manager.Network.Runner.LocalPlayer.PlayerId)
            {
                return characterDict[i];
            }
        }

        return null;
    }
}