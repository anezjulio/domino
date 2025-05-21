using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isGameEnded = false;
    private GameObject boardManager;
    private BoardGenerator boardGenerator;

    public GameObject boardPrefab;
    public GameObject tilePrefab;

    public List<Player> players = new List<Player>();

    private Camera currentCamera;
    private int currentPlayerIndex = 0;

    void Start()
    {
        CreatePlayersAndCameras();
        SetActiveCamera(currentPlayerIndex);
        CreateScoreBoard(players);
        boardManager = CreateBoard();
        boardGenerator = boardManager.GetComponent<BoardGenerator>();
        boardGenerator.GenerateTiles();
        //startNewMatch();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ChangeCamera(-1);  // Cambiar a la cámara de la izquierda
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            ChangeCamera(1);  // Cambiar a la cámara de la derecha
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isGameEnded = true;
        }

        if (isGameEnded)
        {
            if (Application.isPlaying)  // Verifica si el juego está en ejecución
            {
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;  // Detener la ejecución del editor
                #else
                Application.Quit();  // Cerrar la aplicación si no estamos en el editor
                #endif
            }
        }

    }


    void CreateScoreBoard(List<Player> players)
    {
        // create ScoreBoard and set 2 teams on 0 point
    }

    void ChangeCamera(int direction)
    {
        currentCamera.enabled = false;
        currentPlayerIndex += direction;
        if (currentPlayerIndex < 0) currentPlayerIndex = players.Count - 1;
        if (currentPlayerIndex >= players.Count) currentPlayerIndex = 0;
        SetActiveCamera(currentPlayerIndex);
    }

    void SetActiveCamera(int index)
    {
        currentCamera = players[index].playerCamera;
        currentCamera.enabled = true;
    }

    private void CreatePlayersAndCameras()
    {
        float distanceFromCenter = 4f;
        float HeighFromCenter = 4.5f;

        Vector3 center = Vector3.zero;
        Vector3[] positions = new Vector3[]
        {
            new Vector3(0, HeighFromCenter, -distanceFromCenter),
            new Vector3(-distanceFromCenter, HeighFromCenter, 0),
            new Vector3(0, HeighFromCenter, distanceFromCenter),
            new Vector3(distanceFromCenter, HeighFromCenter, 0)
        };

        for (int i = 0; i < 4; i++)
        {
            GameObject playerObj = new GameObject($"Player_{i + 1}");
            Player player = playerObj.AddComponent<Player>();
            GameObject cameraObj = new GameObject($"Camera_{i + 1}");
            cameraObj.transform.parent = playerObj.transform; // Hacerlo hijo
            Camera cam = cameraObj.AddComponent<Camera>();
            cameraObj.transform.position = positions[i];
            cameraObj.transform.LookAt(center);
            player.playerCamera = cam;
            players.Add(player);
        }
    }

    GameObject CreateBoard()
    {
        int boardScale = 300;
        Vector3 center = Vector3.zero;
        GameObject board = Instantiate(boardPrefab);
        board.transform.SetPositionAndRotation(center, Quaternion.Euler(-90, 0, 0));
        board.transform.localScale = new Vector3(boardScale, boardScale, boardScale);
        board.name = $"Board";
        BoardGenerator boardGenerator = board.AddComponent<BoardGenerator>();
        boardGenerator.tilePrefab = tilePrefab;
        boardGenerator.Initialize(players);
        return board;
    }

}
