using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MyClanBack : MonoBehaviour
{
    [SerializeField] GameObject hasClanBack;
    [SerializeField] GameObject noClanBack;

    [Header("HasClanBack")]
    [SerializeField] TextMeshProUGUI clanNameTMP;
    [SerializeField] Button outBtn;
    [SerializeField] Transform clanMemberParent;
    [SerializeField] GameObject clanMemberPrefab;

    [Header("NoClanBack")]
    [SerializeField] TMP_InputField createClanInput;
    [SerializeField] Button createBtn;

    private bool isClanOwner;

    private void Awake()
    {
        createBtn.onClick.AddListener(OnClickCreate);
    }

    private void OnClickCreate()
    {
        if (string.IsNullOrEmpty(createClanInput.text)) return;

        App.Data.Clan.CreateClan(createClanInput.text,
        (result) =>
        {
            App.Manager.UI.GetPanel<ClanPanel>().CheckHasClan();
        }, null);
    }

    public void SetActiveToHasClan(bool _hasClan)
    {
        hasClanBack.SetActive(_hasClan);
        noClanBack.SetActive(!_hasClan);

        if (_hasClan)
        {
            App.Data.Clan.GetClanName(
            (clanName) =>
            {
                clanNameTMP.text = string.Format("Å¬·£ <color=#00FF00>{0}</color>", clanName);
            },
            (error) =>
            {
                Debug.LogError("Failed to get clan name: " + error);
            });
        }
    }

    public void SetActiveToIsClanOwner(bool _isOwner)
    {
        outBtn.gameObject.SetActive(!_isOwner);
        isClanOwner = _isOwner;
    }
}
