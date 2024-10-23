// Node UI 관련 Script.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    public GameObject ui;            // 활성화 / 비활성화 될 UI.
    //private Node target;             // UI가 활성화될 Node.

    public Text upgradeCost;         // Upgrade 비용.
    public Text sellAmount;          // Sell 비용.
    public AudioSource upgradeSound; // Upgrade 효과음
    public AudioSource sellSound;    // Sell 효과음
    public Button upgradeButton;     // Upgrade Button 클릭 가능 여부.

    public Description description;  // Description 객체.

    // UI를 활성화할 Node 지정.
    //public void SetTarget(Node _target)
    //{
    //    // target 지정.
    //    target = _target;

    //    // target의 위치를 Build될 포탑의 위치로 설정.
    //    transform.position = target.GetBuildPosition();
        
    //    // Upgrade 단계가 0인 경우.
    //    if (target.isUpgradeed == 0)
    //    {
    //        // Upgrade 비용 지정.
    //        upgradeCost.text = "$ " + target.turretBluePrint.cost_2;
    //        // Upgrade Button 클릭 가능.
    //        upgradeButton.interactable = true;
    //    }
    //    // Upgrade 단계가 1인 경우.
    //    else if (target.isUpgradeed == 1)
    //    {
    //        // Upgrade 비용 지정.
    //        upgradeCost.text = "$ " + target.turretBluePrint.cost_3;
    //        // Upgrade Button 클릭 가능.
    //        upgradeButton.interactable = true;
    //    }
    //    // Upgrade 이후인 경우.
    //    else
    //    {
    //        // Upgrade 금지.
    //        upgradeCost.text = "DONE";
    //        // Upgrade Button 클릭 불가능.
    //        upgradeButton.interactable = false;
    //    }

    //    // Sell 비용 지정.
    //    sellAmount.text = "$ " + target.turretBluePrint.GetSellAmount(target.isUpgradeed);

    //    // UI 활성화.
    //    ui.SetActive(true);

    //    // 타워 설명창 정보 갱신.
    //    description.Tower_Description_Button_Toggle(target.turret);
    //}

    // UI 비활성화.
    public void Hide()
    {
        // UI 비활성화.
        ui.SetActive(false);
    }

    // 타워 Upgrade.
    public void Upgrade()
    {
        // Upgrade 효과음 발생.
        upgradeSound.Play();

        // Upgrade 함수 호출.
        //target.TurretUpgrade();

        // Node UI 비활성화.
        //BuildManager.instance.DeselectNode();
    }

    // 타워 Sell.
    public void Sell()
    {
        // Sell 효과음 발생.
        sellSound.Play();

        // Sell 함수 호출.
        //target.TurretSell();

        // Node UI 비활성화.
        //BuildManager.instance.DeselectNode();
    }

}
