using System;
using System.Collections.Generic;
using UnityEngine;

public class CreditBar : MonoBehaviour
{
    public enum ScreenMode
    {
        SixteenToNine,
        SixteenToTen,
    }

    public Texture2D Img;
    public GUIStyle NumberStyle;

    public float NumberChangeTime = 0.8f;
    public int BetNum = 50;
    public int CreditNum = 1500;
    public int PaidNum { get; set; }
    public int FontSize = 50;
    private int meterIncrement;
    private int currentMeterNum;

    private GUIStyle blankStyle = new GUIStyle(); //an "empty" style to avoid any of Unity's default padding, margin and background defaults
    private Rect container;
    private Rect betRect;
    private Rect creditRect;
    private Rect paidRect;

    private readonly Dictionary<ScreenMode, Dictionary<string, List<float>>> screenTuneList =
        new Dictionary<ScreenMode, Dictionary<string, List<float>>>();

    void OnGUI()
    {
        DynamicTune();

        var screenMode = GetScreenMode(Screen.width * 1f / Screen.height);
        AdjustScreen(screenMode);

        GUI.BeginGroup(container, Img, blankStyle);
        GUI.Label(betRect, "" + BetNum, NumberStyle);
        GUI.Label(creditRect, "" + (CreditNum + meterIncrement), NumberStyle);
        GUI.Label(paidRect, "" + (PaidNum + meterIncrement), NumberStyle);
        GUI.EndGroup();
    }

    private ScreenMode GetScreenMode(float ratio)
    {
        const float theta = 0.1f;
        if(Mathf.Abs(ratio - 16 * 1f / 9) < theta)
        {
            return ScreenMode.SixteenToNine;
        }
        if(Mathf.Abs(ratio - 16 * 1f / 10) < theta)
        {
            return ScreenMode.SixteenToTen;
        }
        throw new NotSupportedException("Not support ratio: " + ratio);
    }

    private void AdjustScreen(ScreenMode screenMode)
    {
        var tune = screenTuneList[screenMode];
        container.Set(Screen.width * tune["container"][0], Screen.height * tune["container"][1], Screen.width * tune["container"][2], Screen.height * tune["container"][3]);
        betRect.Set(Screen.width * tune["bet"][0], Screen.height * tune["bet"][1], Screen.width * tune["bet"][2], Screen.height * tune["bet"][3]);
        creditRect.Set(Screen.width * tune["credit"][0], Screen.height * tune["credit"][1], Screen.width * tune["credit"][2], Screen.height * tune["credit"][3]);
        paidRect.Set(Screen.width * tune["paid"][0], Screen.height * tune["paid"][1], Screen.width * tune["paid"][2], Screen.height * tune["paid"][3]);
        NumberStyle.fontSize = (int)(FontSize * Screen.width / Screen.currentResolution.width * 1f);
    }

    public void Bet()
    {
        if(CreditNum >= BetNum)
        {
            CreditNum -= BetNum;
        }
        PaidNum = 0;
    }

    public void UpdateMeter(int finalNum)
    {
        iTween.StopByName("MeterIncrement");
        iTween.ValueTo(gameObject,
                       iTween.Hash("name", "MeterIncrement", "time", NumberChangeTime, "from", 0, "to",
                                   finalNum, "onupdate", "OnMeterIncrement", "oncomplete", "OnUpdateComplete"));
    }

    void OnMeterIncrement(int i)
    {
        meterIncrement = i;
    }

    void OnUpdateComplete()
    {
        CreditNum += meterIncrement;
        PaidNum += meterIncrement;
        meterIncrement = 0;
    }

    void DynamicTune()
    {
        screenTuneList[ScreenMode.SixteenToNine] = new Dictionary<string, List<float>>
                                                       {
                                                           {"container", new List<float> {0.05f, 0.9f, 0.9f, 0.1f}},
                                                           {"bet", new List<float> {0.171f, 0.03f, 0.06f, 0.035f}},
                                                           {"credit", new List<float> {0.45f, 0.03f, 0.12f, 0.035f}},
                                                           {"paid", new List<float> {0.614f, 0.03f, 0.12f, 0.035f}}
                                                       };
        screenTuneList[ScreenMode.SixteenToTen] = new Dictionary<string, List<float>>
                                                      {
                                                          {"container", new List<float> {0.05f, 0.9f, 0.9f, 0.1f}},
                                                          {"bet", new List<float> {0.187f, 0.03f, 0.06f, 0.035f}},
                                                          {"credit", new List<float> {0.495f, 0.03f, 0.12f, 0.035f}},
                                                          {"paid", new List<float> {0.67f, 0.03f, 0.12f, 0.035f}}
                                                      };
    }
}
