using System.Collections;
using UnityEngine;

public class StateChanger : MonoBehaviour
{
    private bool isDark = false;

    private float timer;
    private float StateChangeTime;

    [SerializeField]
    private int flickerAmount;

    [SerializeField]
    private GameObject[] appearingObj;
    [SerializeField]
    private Material[] shaders;
    [SerializeField]
    private Material GraphWater;
    [SerializeField]
    private GameObject pointLight;
    [SerializeField]
    private TextureCreator painting1;
    [SerializeField]
    private TextureCreator painting2;
    [SerializeField]
    private Light dirLight;

    void Start()
    {
        StateChangeTime = Random.Range(20, 30);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > StateChangeTime || Input.GetKeyDown(KeyCode.Space))
        {
            timer = 0;
            StateChangeTime = Random.Range(20, 30);
            StartCoroutine(FlickerLight());
        }
    }

    private void ChangeState()
    {
        isDark = !isDark;
        foreach (var obj in appearingObj)
        {
            obj.SetActive(!obj.activeSelf);
        }
        foreach (var shader in shaders)
        {
            var shaderNum = shader.GetInteger("_ShaderNums");
            shader.SetInteger("_ShaderNums", shaderNum * -1);
        }
        painting1.isdark = isDark;
        painting1.Draw();

        if (isDark)
        {
            dirLight.intensity = 0.2f;
            GraphWater.SetFloat("_Monster", 1.1f);
            painting2.patternType = TextureCreator.PatternType.experiment;
            painting2.rotateType = TextureCreator.RotateType.middle;
            painting2.degrees = Random.Range(0, 360);
            painting2.Draw();
        }
        else
        {
            dirLight.intensity = 0;
            GraphWater.SetFloat("_Monster", 0);
            painting2.patternType = TextureCreator.PatternType.rainbowfunc;
            painting2.rotateType = TextureCreator.RotateType.none;
            painting2.Draw();
        }
    }

    private IEnumerator FlickerLight()
    {
        for (int i = 0; i < flickerAmount; i++)
        {
            yield return new WaitForSeconds(0.1f);
            pointLight.SetActive(!pointLight.activeSelf);
        }
        if (flickerAmount % 2 == 0)
        {
            yield return new WaitForSeconds(0.1f);
            pointLight.SetActive(!pointLight.activeSelf);
        }
        ChangeState();
    }

    private void OnApplicationQuit()
    {
        foreach (var shader in shaders)
        {
            shader.SetInteger("_ShaderNums", 1);
        }
        GraphWater.SetFloat("_Monster", 0);
    }
}
