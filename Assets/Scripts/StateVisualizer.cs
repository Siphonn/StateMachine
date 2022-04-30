using UnityEngine;
using UnityEditor;
using Siphonn.Player;
using Siphonn.Enemy;

namespace Siphonn
{
    public class StateVisualizer : MonoBehaviour
    {

        private PlayerStateMachine[] _playerStateMachine;
        private EnemyStateMachine[] _enemyStateMachine;
        private GUIStyle _style;


        private void Awake()
        {
            _enemyStateMachine = FindObjectsOfType<EnemyStateMachine>();
            _playerStateMachine = FindObjectsOfType<PlayerStateMachine>();

            _style = new GUIStyle();
            _style.normal.textColor = Color.green;
            _style.fontSize = 10;
        }

        void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                if (_playerStateMachine == null) { return; }

                foreach (PlayerStateMachine player in _playerStateMachine)
                {
                    string currentPlayerState = player.currentState.ToString();
                    string[] split = currentPlayerState.Split('.');
                    currentPlayerState = split[split.Length - 1];
                    currentPlayerState = currentPlayerState.Remove(currentPlayerState.Length - 5, 5);
                    Handles.Label(player.transform.position + Vector3.up, currentPlayerState, _style);
                }

                if (_enemyStateMachine == null) { return; }

                foreach (EnemyStateMachine enemy in _enemyStateMachine)
                {
                    string currentPlayerState = enemy.currentState.ToString();
                    string[] split = currentPlayerState.Split('.');
                    currentPlayerState = split[split.Length - 1];
                    currentPlayerState = currentPlayerState.Remove(currentPlayerState.Length - 5, 5);
                    Handles.Label(enemy.transform.position + Vector3.up, currentPlayerState, _style);
                }
            }
        }
    }
}
