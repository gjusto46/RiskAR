using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _notificationText;
    [SerializeField] private GameObject _contenedorText;

    public void ShowNotification(string notification)
    {
        CancelInvoke(nameof(ClearText));
        _contenedorText.SetActive(true);
        _notificationText.text = notification;
        Invoke(nameof(ClearText), 3f);
    }

    public void ClearText()
    {
        _contenedorText.SetActive(false);
        _notificationText.text = "";
    }
}