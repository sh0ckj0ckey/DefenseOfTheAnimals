using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapCube : MonoBehaviour
{
    [HideInInspector]
    public GameObject GuardianGameObject;               // 当前格子上放置的守卫

    [HideInInspector]
    public Guardian CubeGuardian;                       // 当前格子上放置的守卫

    private GameObject upgradedGuardianEffect;
    public GameObject GuardianBuildEffect;              // 放置守卫特效
    public GameObject GuardianUpgradedEffect;           // 升级守卫光环特效

    private Renderer cubeRenderer;

    private Material originMaterial = null;

    public Material hoverMaterial;

    private Color upgradeColor = new Color(0, 1, 1);    // 格子升级显示蓝色
    private Color deleteColor = new Color(1, 0, 0);     // 格子删除显示红色

    private void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        originMaterial = cubeRenderer.material;
    }

    public void SummonGuardian(GameObject guardianPrefab)
    {
        GuardianGameObject = GameObject.Instantiate(guardianPrefab, transform.position, Quaternion.identity);    //角度 4个0
        CubeGuardian = GuardianGameObject.GetComponent<Guardian>();

        var pos = transform.position;
        pos.y += 2f;
        GameObject effect = GameObject.Instantiate(GuardianBuildEffect, pos, GuardianBuildEffect.transform.rotation);    //保持物体原来调过的角度
        Destroy(effect, 2);

        // 触发一下鼠标Hover，修改格子颜色
        OnMouseEnter();
    }

    public void UpgradeGuardian()
    {
        if (GuardianGameObject != null && CubeGuardian.bGuardianUpgraded == false)
        {
            upgradedGuardianEffect = GameObject.Instantiate(GuardianUpgradedEffect, transform.position, GuardianUpgradedEffect.transform.rotation);
            // GuardianUpgradedEffect.transform.SetParent(CubeGuardian.transform);
            var pos = upgradedGuardianEffect.transform.position;
            pos.y += 2f;
            upgradedGuardianEffect.transform.position = pos;

            GuardianGameObject.GetComponent<Guardian>().UpgradeGuardian();

            // 触发一下鼠标Hover，修改格子颜色
            OnMouseEnter();
        }
    }

    public void DeleteGuardian()
    {
        if (GuardianGameObject != null)
        {
            Destroy(GuardianGameObject);
            GuardianGameObject = null;
            CubeGuardian = null;
        }

        if (upgradedGuardianEffect != null)
        {
            Destroy(upgradedGuardianEffect);
        }

        // 触发一下鼠标Hover，修改格子颜色
        OnMouseEnter();
    }

    // 放了一个守卫之后，鼠标在其碰撞范围内hover方块就不能变色了，去Project Setting里面的Physic把Queries Trigger Hit关掉
    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            GuardianType selectedType = BuildManager.Instance.SelectedGuardian.type;

            if (GuardianGameObject == null && selectedType != GuardianType.Delete && selectedType != GuardianType.Upgrade)
            {
                cubeRenderer.material = hoverMaterial;
                cubeRenderer.material.color = Color.green;
            }
            else if (GuardianGameObject != null && selectedType == GuardianType.Delete)
            {
                cubeRenderer.material = hoverMaterial;
                cubeRenderer.material.color = deleteColor;
            }
            else if (GuardianGameObject != null && selectedType == GuardianType.Upgrade && CubeGuardian.bGuardianUpgraded == false)
            {
                cubeRenderer.material = hoverMaterial;
                cubeRenderer.material.color = upgradeColor;
            }
            else
            {
                cubeRenderer.material = originMaterial;
                cubeRenderer.material.color = Color.white;
            }
        }
    }

    private void OnMouseExit()
    {
        cubeRenderer.material = originMaterial;
        cubeRenderer.material.color = Color.white;
    }
}
