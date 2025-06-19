using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject model1;
    public GameObject model2;
    public GameObject model3;
    
    public ObjectPlacement placementManager;
    public GameObject canvas;

    public void SelectModel1()
    {
        /*GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(0, 0, 1); // in front of the camera

        // Optional: change color so you notice it
        cube.GetComponent<Renderer>().material.color = Color.red;*/

        placementManager.EnablePlacementMode(model1);

        canvas.SetActive(false);

    }
    public void SelectModel2()
    {
        //GameState.SelectedModelPrefab = model2;
        placementManager.EnablePlacementMode(model2);

        canvas.SetActive(false); 

    }
    public void SelectModel3()
    {
        //GameState.SelectedModelPrefab = model3;
        FindObjectOfType<ObjectPlacement>().EnablePlacementMode(model3);

        canvas.SetActive(false); 

    }
}
