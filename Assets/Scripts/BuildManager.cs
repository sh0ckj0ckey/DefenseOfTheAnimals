using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    public GuardianData CactusGuardian;
    public GuardianData SoulStreamGuardian;
    public GuardianData ChainLightningGuardian;

    public GuardianData ChaosMeteor;
    public GuardianData DeleteGuardian;
    public GuardianData UpgradeGuardian;

    // 当前选中的守卫
    public GuardianData SelectedGuardian = null;

    public Text MoneyText;

    public Animator MoneyAnimator;

    public int Money = 50;

    public void AddMoney(int change = 0)
    {
        Money += change;
        MoneyText.text = Money.ToString();
    }

    private void Awake()
    {
        Instance = this;    //把当前的对象赋给Instance便于外部访问
    }

    private void Start()
    {
        SelectedGuardian = CactusGuardian;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                if (SelectedGuardian.type == GuardianType.Cactus ||
                    SelectedGuardian.type == GuardianType.SoulStream ||
                    SelectedGuardian.type == GuardianType.ChainLightning)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    var map = LayerMask.GetMask("MapCube");
                    bool isCollider = Physics.Raycast(ray, out hit, 1000, map);
                    if (isCollider)
                    {
                        MapCube mapCube = hit.collider.GetComponent<MapCube>();   // 得到点击的MapCube
                        if (mapCube.CubeGuardian == null)
                        {
                            if (Money >= SelectedGuardian.Cost)
                            {
                                AddMoney(-SelectedGuardian.Cost);
                                mapCube.SummonGuardian(SelectedGuardian.GuardianPrefab);
                            }
                            else
                            {
                                MoneyAnimator.SetTrigger("Flick");
                            }
                        }
                        else
                        {
                            //升级
                        }
                    }
                }
            }
        }
    }

    public void OnCactusGuardianSelected(bool isOn)
    {
        if (isOn)
        {
            SelectedGuardian = CactusGuardian;
        }
    }
    public void OnSoulStreamGuardianSelected(bool isOn)
    {
        if (isOn)
        {
            SelectedGuardian = SoulStreamGuardian;
        }
    }
    public void OnChainLightningGuardianSelected(bool isOn)
    {
        if (isOn)
        {
            SelectedGuardian = ChainLightningGuardian;
        }
    }
    public void OnChaosMeteorSelected(bool isOn)
    {
        if (isOn)
        {
            SelectedGuardian = ChaosMeteor;
        }
    }
    public void OnDeleteGuardianSelected(bool isOn)
    {
        if (isOn)
        {
            SelectedGuardian = DeleteGuardian;
        }
    }
    public void OnUpgradeGuardianSelected(bool isOn)
    {
        if (isOn)
        {
            SelectedGuardian = UpgradeGuardian;
        }
    }
}
