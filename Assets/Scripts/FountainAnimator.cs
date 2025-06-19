using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class FountainAnimator : MonoBehaviour
{
    public int startFrame;
    public int endFrame;
    List<List<Mesh>> meshesPerFrame = new List<List<Mesh>>();
    private float timer = 0f;
    private float fps = 30;
    private int currentFrame = 0;
    public MeshFilter fluid;

    void Start()
    {

        for (int i = startFrame; i <= endFrame; i++)
        {
            string fileName = $"NewMeshes3/anim{i}";
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
            fluid.mesh = meshesPerFrame[currentFrame][0];
            if (currentFrame == 499) currentFrame = 420;
            else currentFrame++;
            //currentFrame = (currentFrame + 1) % (endFrame - startFrame + 1);

            timer = 0f;
        }
    }

}