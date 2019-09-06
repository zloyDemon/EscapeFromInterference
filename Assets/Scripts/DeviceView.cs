using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class DeviceView : MonoBehaviour
{
   [SerializeField] private Image signalImg;
   [SerializeField] private Text signalValueText;

   private float blickDuration = 0.5f;
   private float currentDeviceValue;
   
   private void Awake()
   {
      DeviceManager.Instance.SignalValueChanged += SetSignalValue;
   }

   private void Start()
   {
      StartCoroutine(BlickDevice());
   }

   private void OnDestroy()
   {
      DeviceManager.Instance.SignalValueChanged -= SetSignalValue;
   }

   private void SetSignalValue(float signalValue)
   {
      signalValueText.text = FormatDeviceValue(signalValue);
      if (currentDeviceValue != signalValue)
      {
         currentDeviceValue = signalValue;
         if (currentDeviceValue <= 0.1f)
            blickDuration = 0.5f;
         else
            blickDuration = Mathf.Clamp( (1f - signalValue), 0.1f, 0.5f);
      }
   }

   private string FormatDeviceValue(float value)
   {
      int cel = (int)value;
      int drob = (int)((value - cel) * 10);
      return string.Format("{0}.{1}", cel, drob);
   }

   private IEnumerator BlickDevice()
   {
      while (true)
      {
         signalImg.gameObject.SetActive(true);
         yield return new WaitForSeconds(blickDuration);
         signalImg.gameObject.SetActive(false);
         yield return new WaitForSeconds(blickDuration * 1.5f);
      }
   }
}
