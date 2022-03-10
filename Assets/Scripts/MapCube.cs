using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapCube : MonoBehaviour
{
    [HideInInspector]
    public GameObject turretGo; //当前cube上放置的炮台

    public GameObject buildEffect;

    private Renderer render;

    private Material originMaterial = null;

    public Material hoverMaterial;

    private void Start()
    {
        render = GetComponent<Renderer>();
        originMaterial = render.material;
    }

    public void BuildTurret(GameObject turretPrefab)
    {
        turretGo = GameObject.Instantiate(turretPrefab, transform.position, Quaternion.identity);           //角度 4个0

        var pos = transform.position;
        pos.y += 2f;
        GameObject effect = GameObject.Instantiate(buildEffect, pos, buildEffect.transform.rotation);    //保持物体原来调过的角度
        Destroy(effect, 2);
    }

    // 放了一个守卫之后，鼠标在其碰撞范围内hover方块就不能变色了，去Project Setting里面的Physic把Queries Trigger Hit关掉
    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (turretGo == null)
            {
                render.material = hoverMaterial;
                render.material.color = Color.green;
            }
            //else
            //{
            //    renderer.material = hoverMaterial;
            //    renderer.material.color = Color.red;
            //}
        }
    }

    private void OnMouseExit()
    {
        render.material = originMaterial;
        render.material.color = Color.white;
    }
}
