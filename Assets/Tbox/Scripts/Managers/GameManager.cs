using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameState { Menu, Playing, Paused, GameOver }
    public GameState currentState;

    [Header("Wheels")]
    public Wheels[] wheels;
    public int minWheelsDes = 1;
    public int maxWheelsDes = 3;

    private void Awake()
    {
        // Asegurar que solo haya un GameManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // El GameManager no se destruirá al cambiar de escena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(GameState.Playing); // Empezar en el menú principal
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        // Lógica para pausar o reanudar el juego
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (currentState == GameState.Playing)
                ChangeState(GameState.Paused);
            else if (currentState == GameState.Paused)
                ChangeState(GameState.Playing);
        }

        // Lógica para manejar estados adicionales del juego
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case GameState.Menu:
                // Mostrar menú principal
                break;
            case GameState.Playing:
                // Empezar o reanudar el juego
                DesinflarWheels();
                break;
            case GameState.Paused:
                // Pausar el juego
                break;
            case GameState.GameOver:
                // Lógica de game over
                break;
        }
    }

    public void StartGame()
    {
        ChangeState(GameState.Playing);
    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver);
    }

    public void ReturnToMenu()
    {
        ChangeState(GameState.Menu);
    }

    private void DesinflarWheels()
    {
        maxWheelsDes = Mathf.Min(maxWheelsDes, wheels.Length);
        
        int numWheelsDes = Random.Range(minWheelsDes, maxWheelsDes + 1);

        // Convert the array to a list and shuffle it
        List<Wheels> shuffledWheels = new List<Wheels>(wheels);
        ShuffleList(shuffledWheels);

        // Activate the method on the selected number of wheels
        for (int i = 0; i < numWheelsDes; i++)
        {
            shuffledWheels[i].StartDesinflando();
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
