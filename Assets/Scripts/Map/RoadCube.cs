using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoadCube : MonoBehaviour
{
    [HideInInspector]
    public GameObject CubeItemGameObject;

    [HideInInspector]
    public Guardian CubeItem;

    private Renderer cubeRenderer;

    private Material originMaterial = null;

    public Material hoverMaterial;

    // Start is called before the first frame update
    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        originMaterial = cubeRenderer.material;
    }

    internal void SummonChaosMeteor(GameObject guardianPrefab)
    {
        throw new NotImplementedException();
    }

    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            GuardianType selectedType = BuildManager.Instance.SelectedGuardian.type;

            if (CubeItemGameObject == null && (selectedType == GuardianType.ChaosMeteor || selectedType == GuardianType.Portal))
            {
                cubeRenderer.material = hoverMaterial;
                cubeRenderer.material.color = Color.green;
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
