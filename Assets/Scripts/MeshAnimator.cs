using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MeshAnimator : MonoBehaviour
{
    public int startFrame;
    public int endFrame;
    List<List<Mesh>> meshesPerFrame = new List<List<Mesh>>();
    private float timer = 0f;
    private float fps = 30;
    private int currentFrame = 0;
    public MeshFilter bottle;
    public MeshFilter bowl;
    public MeshFilter fluid;

    void Start()
    {

        for (int i = startFrame; i <= endFrame; i++)
        {
            string fileName = $"NewMeshes/anim{i}";
            GameObject fbxObject = Resources.Load<GameObject>(fileName);
            List<Mesh> meshes = new List<Mesh>();
            foreach (MeshFilter currMesh in fbxObject.GetComponentsInChildren<MeshFilter>())
            {
                meshes.Add(currMesh.sharedMesh);
            }

            meshesPerFrame.Add(meshes);
        }
    }


    void Update()
    {
        timer = timer + Time.deltaTime;
        if (timer > 1f / fps)
        {
            bottle.mesh = meshesPerFrame[currentFrame][2];
            bowl.mesh = meshesPerFrame[currentFrame][0];
            fluid.mesh = meshesPerFrame[currentFrame][1];
            currentFrame = (currentFrame + 1) % (endFrame - startFrame + 1);

            timer = 0f;
        }
    }

}