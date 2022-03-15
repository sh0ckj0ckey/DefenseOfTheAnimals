using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapCube : MonoBehaviour
{
    [HideInInspector]
    public GameObject CubeGuardian;             // 当前格子上放置的守卫

    [HideInInspector]
    public bool bGuardianUpgraded = false;  // 当前格子上的守卫是否已经升级

    public GameObject GuardianBuildEffect;
    public GameObject GuardianUpgradedEffect;   // 升级守卫光环特效

    private Renderer render;

    private Material originMaterial = null;

    public Material hoverMaterial;

    private Color upgradeColor = new Color(0, 1, 1);    // 格子升级显示蓝色
    private Color deleteColor = new Color(1, 0, 0);     // 格子删除显示红色

    private void Start()
    {
        render = GetComponent<Renderer>();
        originMaterial = render.material;
    }

    public void SummonGuardian(GameObject guardianPrefab)
    {
        CubeGuardian = GameObject.Instantiate(guardianPrefab, transform.position, Quaternion.identity);    //角度 4个0

        var pos = transform.position;
        pos.y += 2f;
        GameObject effect = GameObject.Instantiate(GuardianBuildEffect, pos, GuardianBuildEffect.transform.rotation);    //保持物体原来调过的角度
        Destroy(effect, 2);

        // 触发一下鼠标Hover，修改格子颜色
        OnMouseEnter();
    }

    public void UpgradeGuardian()
    {
        if (CubeGuardian != null && bGuardianUpgraded == false)
        {
            GuardianUpgradedEffect = GameObject.Instantiate(GuardianUpgradedEffect, transform.position, GuardianUpgradedEffect.transform.rotation);
            // GuardianUpgradedEffect.transform.SetParent(CubeGuardian.transform);
            var pos = GuardianUpgradedEffect.transform.position;
            pos.y += 2f;
            GuardianUpgradedEffect.transform.position = pos;
            bGuardianUpgraded = true;

            CubeGuardian.GetComponent<Guardian>().UpgradeGuardian();

            // 触发一下鼠标Hover，修改格子颜色
            OnMouseEnter();
        }
    }

    // 放了一个守卫之后，鼠标在其碰撞范围内hover方块就不能变色了，去Project Setting里面的Physic把Queries Trigger Hit关掉
    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            GuardianType selectedType = BuildManager.Instance.SelectedGuardian.type;

            if (CubeGuardian == null && selectedType != GuardianType.Delete && selectedType != GuardianType.Upgrade)
            {
                render.material = hoverMaterial;
                render.material.color = Color.green;
            }
            else if (CubeGuardian != null && selectedType == GuardianType.Delete)
            {
                render.material = hoverMaterial;
                render.material.color = deleteColor;
            }
            else if (CubeGuardian != null && selectedType == GuardianType.Upgrade && bGuardianUpgraded == false)
            {
                render.material = hoverMaterial;
                render.material.color = upgradeColor;
            }
        }
    }

    private void OnMouseExit()
    {
        render.material = originMaterial;
        render.material.color = Color.white;
    }
}
