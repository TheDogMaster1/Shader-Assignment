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
    private GameObject eyes;
    [SerializeField]
    private Material wave;
    [SerializeField]
    private GameObject pointLight;

    void Start()
    {
        StateChangeTime = Random.Range(20, 30);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > StateChangeTime || Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FlickerLight());
        }
    }

    private void ChangeState()
    {
        isDark = !isDark;
        eyes.SetActive(!eyes.activeSelf);
        var shaderNum = wave.GetInteger("_ShaderNums");
        wave.SetInteger("_ShaderNums", shaderNum * -1);
        StateChangeTime = Random.Range(20, 30);
        timer = 0;
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
        wave.SetInteger("_ShaderNums", 1);
    }
}
