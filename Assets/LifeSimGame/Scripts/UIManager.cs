using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using Object = UnityEngine.Object;




public class UIManager: MonoBehaviour
{
    public TextMeshProUGUI cubeScoreText;
    public TextMeshProUGUI sphereScoreText;
    public TextMeshProUGUI cylinderScoreText;
    public TextMeshProUGUI winnerText;

    public void updateCubeScoreUI(int cubeScore)
    {
        cubeScoreText.text = cubeScore.ToString();
    }
    public void updateSphereScoreUI(int sphereScore)
    {
        sphereScoreText.text = sphereScore.ToString();
    }
    public void updateCylinderScoreUI(int cylinderScore)
    {
        cylinderScoreText.text = cylinderScore.ToString();
    }

    public void updateWinnerText(string v)
    {
        winnerText.text = v;
    }
}