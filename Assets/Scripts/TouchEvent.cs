using UnityEngine;
using UnityEngine.Events;

public class TouchEvent : MonoBehaviour
{
    [SerializeField] UnityEvent touchEvent;

    void Update()
    {
        
    }

    void OnMouseDown()
    {
        print("OI");
        touchEvent?.Invoke();
    }
    
}
