using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputManager : MonoBehaviour
{
    [Inject] private GridVisualizer _gridVisualizer;


    void Update()
    {
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            OnClickAction();
        }
    }
    /// <summary>
    /// calling OnClick actions.
    /// </summary>
    void OnClickAction()
    {
        Vector2 wordlPos = Camera.main.ScreenToWorldPoint(Pointer.current.position.ReadValue());
        wordlPos.x -= _gridVisualizer.OffsetPosition.x - (_gridVisualizer.Size.x / 2f) + .5f;
        wordlPos.y -= _gridVisualizer.OffsetPosition.y - (_gridVisualizer.Size.y / 2f) + .5f;

        EventManager.Raise(new OnClick(wordlPos));
    }
}