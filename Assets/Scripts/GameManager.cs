using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int numTentativas; //Armazena as tentativas válidas da Rodada
    private int maxNumTentativas; //Número máxima de tentativas para Forca ou Vida
    int score = 0;

    public GameObject letra; //prefab da letra no Game
    public GameObject centro; //objeto de texto que indica o centro da tela


    private string palavraOculta = ""; // palavra oculta a ser descoberta
    private int tamanhoPalavraOculta; // tamanho da palavra oculta
    private string[] palavrasOcultas = new string [] {"carro","elefante","futebol"}; // array de palavras ocultas
    char[] letrasOcultas; // letras da palavra oculta
    bool[] letrasDescobertas; // indicador de quais letras foram descobertas

    // Start is called before the first frame update
    void Start()
    {
        centro =  GameObject.Find("centroDaTela");
        InitGame();
        InitLetras();
        numTentativas = 0;
        maxNumTentativas = 10;
        UpdateNumTentativas();
        UpdateScore();
       
    }

    // Update is called once per frame
    void Update()
    {
        checkTeclado();
    }

    // inicia as letras
    void InitLetras(){
        int numLetras = tamanhoPalavraOculta;
        for(int i =0; i<numLetras; i++){
            Vector3 novaPosicao;
            novaPosicao = new Vector3(centro.transform.position.x + ((i-numLetras/2.0f)*80), centro.transform.position.y, centro.transform.position.z);
            GameObject l =(GameObject)Instantiate(letra, novaPosicao, Quaternion.identity);
            l.name ="letra" + (i+1);
            l.transform.SetParent(GameObject.Find("Canvas").transform);
        }
    }
    void InitGame(){
        //int numeroAleatorio = Random.Range(0,palavrasOcultas.Length);
        //palavraOculta =palavrasOcultas[numeroAleatorio]; // palavra a ser descoberta

        palavraOculta = PegaUmaPalavraDoArquivo();
        tamanhoPalavraOculta = palavraOculta.Length; // determina-se o numero de letras da palavra
        palavraOculta = palavraOculta.ToUpper(); // deixar a palavra maiuscula
        letrasOcultas = new char [tamanhoPalavraOculta]; // instancia-se o array char com a quantidade de letras da palavra
        letrasDescobertas = new bool [tamanhoPalavraOculta];// instancia-se o array bool com a quantidade de letras da palavra
        letrasOcultas = palavraOculta.ToCharArray(); // copia a palavra letra por letra no array
    }

    void checkTeclado(){
        if (Input.anyKeyDown)
        {
            char letraTeclada =  Input.inputString.ToCharArray()[0];
            int letraTecladaComoInt = System.Convert.ToInt32(letraTeclada);
            if(letraTecladaComoInt >= 97 &&letraTecladaComoInt<=122){
                numTentativas++;
                UpdateNumTentativas();
                if(numTentativas > maxNumTentativas)
                {
                    SceneManager.LoadScene("lab1_forca");
                }
                for (int i = 0; i < tamanhoPalavraOculta; i++)
                {
                    if (!letrasDescobertas[i])
                    {
                        letraTeclada = System.Char.ToUpper(letraTeclada);
                        if (letrasOcultas[i] == letraTeclada)
                        {
                            letrasDescobertas[i] = true;
                            GameObject.Find("letra"+ (i+1)).GetComponent<Text>().text = letraTeclada.ToString();
                            score = PlayerPrefs.GetInt("score");
                            score++;
                            PlayerPrefs.SetInt("score", score);
                            UpdateScore();
                            VerificaPalavraDescoberta();
                        }
                    }
                }
            }
        }
    }

    void UpdateNumTentativas()
    {
        GameObject.Find("numTentativas").GetComponent<Text>().text = numTentativas + " | " + maxNumTentativas;
    }

    void UpdateScore()
    {
        GameObject.Find("scoreUI").GetComponent<Text>().text = "Score" + score;
    }
    void VerificaPalavraDescoberta()
    {
        bool condicao = true;
        //Verifica se cada uma das letras foram descobertas
        for(int i = 0; i < tamanhoPalavraOculta; i++)
        {
            condicao = condicao && letrasDescobertas[i];
        }
        //Se sim, Carrega a Cena que indica que o jogador ganhou o jogo
        if(condicao)
        {
            PlayerPrefs.SetString("ultimaPalavraOculta", palavraOculta);
            SceneManager.LoadScene("lab1_salvo");
        }
    }
    string PegaUmaPalavraDoArquivo()
    {
        TextAsset t1 = (TextAsset)Resources.Load("palavras", typeof(TextAsset)); //Lê o arquivo em uma variável tipo TextAsset
        string s = t1.text; //Coloca todo o conteúdo em formato de texto em uma string
        string[] palavras = s.Split(' '); //Quebra a string em outras menores a partir do espaço
        int palavraAleatoria = Random.Range(0, palavras.Length + 1); // Escolhe uma das palavras aleatóriamente
        return (palavras[palavraAleatoria]); 
    }
}
