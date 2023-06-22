using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StudentData", menuName = "ScriptableObjects/StudentData", order = 1)]
public class StudentData : ScriptableObject
{
    [SerializeField] List<MaterialProperties> material;
   
    public float studentSpeed,studentSpeedTowardsDoor;    
    
    public void UpdateStudentSkin(UniversityPhase studentPhase, Student student)
    {
        Material studentMaterial = student.skinMeshRenderer.sharedMaterial; // Get the material of the student
        Material materialInstance = new Material(studentMaterial); // Create a new material instance

        materialInstance.SetFloat("_OutlineSize", 0);
        switch (studentPhase)
        {            
            case (UniversityPhase.Enrollment):
                materialInstance.SetColor("_AlbedoColor", material[0].albedoColor);
                break;
            case (UniversityPhase.Examination):
                materialInstance.SetColor("_AlbedoColor", material[1].albedoColor);
                break;
            case (UniversityPhase.Ceremony):
                materialInstance.SetColor("_AlbedoColor", material[2].albedoColor);
                break;
        }
        student.skinMeshRenderer.material = materialInstance;
    }
    public static void SetOutline(int value, Student student)
    {
        Material studentMaterial = student.skinMeshRenderer.material; // Get the material of the student
        Material materialInstance = new Material(studentMaterial); // Create a new material instance

        // Set the outline size value in the material instance
        materialInstance.SetFloat("_OutlineSize", value);

        // Apply the material instance to the student's mesh renderer
        student.skinMeshRenderer.material = materialInstance;
    }
    [Serializable]
    public struct MaterialProperties
    {
        public Color albedoColor;
    }
}
