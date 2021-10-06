using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MostraUltimaPalavraOculta : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = PlayerPrefs.GetString("ultimaPalavraOculta"); //Atualiza componente com o valor da variável ultimaPalavraOculta   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
