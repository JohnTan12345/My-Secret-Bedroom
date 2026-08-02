using UnityEngine;

public class EndGameHandler : MonoBehaviour
{
    public void EndGame()
    {
        GameManager.instance.EndGame();
    }
}
