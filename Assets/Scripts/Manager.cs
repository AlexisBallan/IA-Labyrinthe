using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager : MonoBehaviour
{
    public GameObject catPrefab;

    private bool isTraining = false;
    public int populationSize = 40;
    public float timer = 75f;
    private int generationNumber = 0;
    private int nombre_mort = 0;

    private List<NeuralNetwork> nets;
    private List<NeuralNetwork> newNets;
    public List<GameObject> catList = null;

    private int[] layers = new int[] { 6, 5, 4, 2 };

    private float fit = 0;

    public Material matIdentique;
    public Material matExplorateur;
    public Material matTesteur;
    public TMP_Text text;

    private void Timer()
    {
        isTraining = false;
    }

    void Update()
    {
        if(!isTraining)
        {
            if(generationNumber == 0)
            {
                Debug.Log("Début !");
                InitCatNeuralNetwork();
                //CreateCatBodies();
            } else
            {
                for(int i=0; i< populationSize; i++)
                {
                    NNController script = catList[i].GetComponent<NNController>();
                    float fitness = script.fitness;
                    nets[i].SetFitness(fitness);
                }


                nets.Sort();
                //Debug.Log(nets[1].GetFitness());
                nets.Reverse();
                //Debug.Log(nets[1].GetFitness());

                fit = 0;
                for( int i=0; i< populationSize; i++)
                {
                    fit += nets[i].GetFitness();
                    //Debug.Log(fit);
                }
                fit /= populationSize;
                Debug.Log("Fitness = " + fit);

                newNets = new List<NeuralNetwork>();

                for (int i = 0; i < populationSize / 4; i++)
                {
                    NeuralNetwork net = new NeuralNetwork(nets[i]);
                    newNets.Add(net);
                }

                for (int i = 0; i < populationSize / 4; i++)
                {
                    NeuralNetwork net = new NeuralNetwork(nets[i]);
                    net.Mutate(0.05f);
                    newNets.Add(net);
                }
                
                for (int i = 0; i < populationSize / 4; i++)
                {
                    NeuralNetwork net = new NeuralNetwork(nets[i]);
                    net.Mutate(.1f);
                    newNets.Add(net);
                }
                
                for (int i = 0; i < populationSize / 4; i++)
                {
                    NeuralNetwork net = new NeuralNetwork(nets[i]);
                    net.Mutate(.2f);
                    newNets.Add(net);
                }
                
                nets = newNets;
            }

            generationNumber++;
            //Invoke("Timer", timer);
            CreateCatBodies();
            isTraining = true;
            text.text = "gen : " + generationNumber;
        }

        for (int i=0; i<populationSize; i++)
        {
            NNController script = catList[i].GetComponent<NNController>();

            float[] result;
            float vel = script.m_currentVelocity / script.maxDistance;
            float distForward = script.distForward / script.maxDistance;
            float distLeft = script.distLeft / script.maxDistance;
            float distRight = script.distRight / script.maxDistance;
            float distDiagLeft = script.distDiagLeft / script.maxDistance;
            float distDiagRight = script.distDiagRight / script.maxDistance;

            float[] tInput = new float[] { vel, distForward, distLeft, distRight, distDiagLeft, distDiagRight };
            result = nets[i].FeedForward(tInput);
            
            script.results = result;
        }

        for (int i = 0; i < populationSize; i++)
        {
            if (catList[i].GetComponent<NNController>().estMort == true)
            {
                nombre_mort++;
            }
        }

        if (nombre_mort == populationSize)
        {
            nombre_mort = 0;
            
            Timer();
           
        } else
            nombre_mort = 0;
        

        if(Input.GetKeyDown("space"))
        {
            generationNumber++;
            isTraining = true;
            Timer();
            CreateCatBodies();
            text.text = "gen : " + generationNumber;
        }
    }

    private void InitCatNeuralNetwork()
    {
        nets = new List<NeuralNetwork>();

        for(int i=0; i<populationSize; i++)
        {
            NeuralNetwork net = new NeuralNetwork(layers);
            net.Mutate(0.5f);
            nets.Add(net);
        }
    }

    private void CreateCatBodies()
    {
        for (int i=0; i<catList.Count; i++)
        {
            Destroy(catList[i]);
        }

        catList = new List<GameObject>();
        for (int i=0; i<populationSize; i++)
        {
            GameObject cat = Instantiate(catPrefab, new Vector3(-21.5f, 1.1f, -21.8f), catPrefab.transform.rotation);
            catList.Add(cat);
            catList[i] = cat;
        }
        //Debug.Log("generationNumber : " + generationNumber);
        if (generationNumber != 1)
        {
            for (int i = 0; i < populationSize / 4; i++)
            {
                //Debug.Log("matIdentique : " + i);
                Renderer myRend = catList[i].GetComponent<Renderer>();
                myRend.enabled = true;

                myRend.sharedMaterial = matIdentique;
            }

            for (int i = populationSize / 4; i < populationSize / 2; i++)
            {
                //Debug.Log("matExplorateur : " + i);
                Renderer myRend = catList[i].GetComponent<Renderer>();
                myRend.enabled = true;

                myRend.sharedMaterial = matExplorateur;
            }

            for (int i = populationSize / 2; i < (populationSize - (populationSize / 4)); i++)
            {
                //Debug.Log("matTesteur : " + i);
                Renderer myRend = catList[i].GetComponent<Renderer>();
                myRend.enabled = true;

                myRend.sharedMaterial = matTesteur;
            }
        }
    }
}
