using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Rendering;

public class BoardGenerator : MonoBehaviour
{
    public List<Player> players = new();
    public List<Tile> tiles = new();
    public List<GameObject> gameObjectsTiles = new();
    public GameObject tilePrefab;
    public int limitOfTiles = 28;
    private List<Vector3> availableSpawnPositionsForTiles = new List<Vector3>();
    public void Initialize(List<Player> players)
    {
        this.players = players;

        // Podrías aquí llamar CreateTiles() u otra lógica inicial
    }

    public void GenerateTiles()
    {
        SetAvailableSpawnPositionsForTiles();

        CreateTileOnPositions();

        ShuffleTilePositions();

        // Aquí colocamos las fichas de los jugadores
        for (int playerIndex = 0; playerIndex < players.Count; playerIndex++)
        {
            PlacePlayerTiles(playerIndex);  // Coloca las 7 fichas para cada jugador
        }

    }

    public void PlacePlayerTiles(int playerIndex)
    {

        float spacing = .25f;  // Distancia entre las fichas
        float distanceFromCenter = 1.5f;  // Distancia desde el centro del tablero
        Vector3 center = transform.position;  // El centro del tablero
        center.y += 1f;

        for (int i = 0; i < 7; i++) // 7 fichas por jugador
        {
            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;

            // Aquí se calculan las posiciones y rotaciones dependiendo del jugador
            switch (playerIndex)
            {
                case 0: // Jugador en la parte inferior (mirando hacia arriba)
                    position = new Vector3(center.x + (i - 3) * spacing, center.y, center.z - distanceFromCenter);
                    rotation = Quaternion.Euler(0, 0, 0);  // Rotación 0 grados, mirando hacia arriba
                    break;

                case 1: // Jugador en la parte derecha (mirando hacia la izquierda)
                    position = new Vector3(center.x + distanceFromCenter, center.y, center.z + (i - 3) * spacing);
                    rotation = Quaternion.Euler(0, -90, 0);  // Rotación 90 grados, mirando hacia la izquierda
                    break;

                case 2: // Jugador en la parte superior (mirando hacia abajo)
                    position = new Vector3(center.x + (i - 3) * spacing, center.y, center.z + distanceFromCenter);
                    rotation = Quaternion.Euler(0, 180, 0);  // Rotación 180 grados, mirando hacia abajo
                    break;

                case 3: // Jugador en la parte izquierda (mirando hacia la derecha)
                    position = new Vector3(center.x - distanceFromCenter, center.y, center.z + (i - 3) * spacing);
                    rotation = Quaternion.Euler(0, 90, 0);  // Rotación -90 grados, mirando hacia la derecha
                    break;
            }

            // Coloca la ficha en la posición y rotación calculada
            MoveTile(tiles[playerIndex * 7 + i].gameObject, position, rotation);

        }
    }

    public void MoveTile(GameObject tile, Vector3 targetPosition, Quaternion targetRotation)
    {
        tile.transform.position = targetPosition;  // Mueve la ficha a la nueva posición
        tile.transform.rotation = targetRotation;  // Aplica la rotación calculada
    }

    public List<Vector3> GetPlayerPositions(int playerIndex, int totalPlayers = 4)
    {
        List<Vector3> playerPositions = new List<Vector3>();
        List<Quaternion> playerRotations = new List<Quaternion>();

        float spacing = .25f;  // Distancia entre las fichas
        float distanceFromCenter = 1.25f;  // Distancia desde el centro del tablero a las fichas
        Vector3 center = Vector3.zero;  // El centro del tablero
        center.y = center.y + 1f;

        // Dependiendo del índice del jugador, la orientación cambiará
        for (int i = 0; i < 7; i++)  // 7 fichas por jugador
        {
            Vector3 position = Vector3.zero;
            position.y = position.y + 1f;

            Quaternion rotation = Quaternion.identity;  // Por defecto no tiene rotación

            // Aquí se calculan las posiciones dependiendo del jugador
            switch (playerIndex)
            {
                case 0: // Jugador en la parte inferior (mirando hacia arriba)
                    position = new Vector3(center.x + (i - 3) * spacing, center.y, center.z - distanceFromCenter);
                    rotation = Quaternion.Euler(0, 0, 0);  // No rotar, orientado a lo largo del eje Y
                    break;

                case 1: // Jugador en la parte derecha (mirando hacia la izquierda)
                    position = new Vector3(center.x + distanceFromCenter, center.y, center.z + (i - 3) * spacing);
                    rotation = Quaternion.Euler(0, 90, 0);  // Rotar 90 grados, mirando hacia la izquierda
                    break;

                case 2: // Jugador en la parte superior (mirando hacia abajo)
                    position = new Vector3(center.x + (i - 3) * spacing, center.y, center.z + distanceFromCenter);
                    rotation = Quaternion.Euler(0, 180, 0);  // Rotar 180 grados, mirando hacia abajo
                    break;

                case 3: // Jugador en la parte izquierda (mirando hacia la derecha)
                    position = new Vector3(center.x - distanceFromCenter, center.y, center.z + (i - 3) * spacing);
                    rotation = Quaternion.Euler(0, -90, 0);  // Rotar -90 grados, mirando hacia la derecha
                    break;
            }

            // Añadir la posición calculada a la lista
            playerPositions.Add(position);
            playerRotations.Add(rotation);
        }

        return playerPositions;
    }

    private void SetAvailableSpawnPositionsForTiles()
    {
        availableSpawnPositionsForTiles.Clear();

        int filas = 4;
        int columnas = 7;
        float separacionX = .5f;  // Espacio entre fichas en el eje X
        float separacionY = .5f;  // Espacio entre fichas en el eje Y

        // Centrar las fichas en el tablero
        float offsetX = (columnas - 1) * separacionX * 0.25f;  // Centrado en X
        float offsetY = (filas - 1) * separacionY * 0.25f;    // Centrado en Y

        // Crear las posiciones para el arreglo 4x7
        for (int y = 0; y < filas; y++)
        {
            for (int x = 0; x < columnas; x++)
            {
                Vector3 position = new Vector3(
                    transform.position.x + (x * separacionX) - offsetX,
                    transform.position.y + 1f,
                    transform.position.z + (y * separacionY) - offsetY
                );

                availableSpawnPositionsForTiles.Add(position);
            }
        }

    }

    public void CreateTileOnPositions()
    {
        tiles.Clear();

        int tileIndex = 0;
        for (int i = 0; i <= 6; i++)
        {
            for (int j = i; j <= 6; j++)
            {
                if (tiles.Count >= limitOfTiles) return;

                // Instanciar el GameObject de la ficha
                GameObject tileObject = Instantiate(tilePrefab, availableSpawnPositionsForTiles[tileIndex], Quaternion.identity);
                tileObject.name = $"Tile_{i}_{j}";

                // Añadir el componente Tile al GameObject
                Tile tileComponent = tileObject.AddComponent<Tile>();

                // Inicializar el Tile con los valores i y j
                tileComponent.SetValues(i, j);

                // Añadir a la lista de tiles
                tiles.Add(tileComponent);
                gameObjectsTiles.Add(tileObject);

                tileIndex++;
            }
        }

    }

    public void ShuffleTilePositions()
    {
        // Crear una lista con las posiciones actuales
        List<Vector3> originalPositions = new List<Vector3>();
        foreach (var tile in gameObjectsTiles)
        {
            originalPositions.Add(tile.transform.position);
        }

        // Barajar la lista de posiciones
        for (int i = 0; i < originalPositions.Count; i++)
        {
            int randomIndex = Random.Range(i, originalPositions.Count);
            Vector3 temp = originalPositions[i];
            originalPositions[i] = originalPositions[randomIndex];
            originalPositions[randomIndex] = temp;
        }

        // Asignar las nuevas posiciones a los GameObjects
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].transform.position = originalPositions[i];
        }
    }


}
