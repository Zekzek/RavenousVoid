using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudDisplay : MonoBehaviour
{
    private const int MAX_POWER = 100000;
    private static readonly int[] DAILY_POWER_REQUIREMENTS = { 3000, 5000, 10000, 20000, 50000 };
    private static List<ColorCube> collectedfCubes = new List<ColorCube>();

    public Text powerValueDisplay;

    public GameObject statsPanel;
    public Text statsPowerText;

    public GameObject progressPanel;
    public Text progressDayText;

    public GameObject introPanel;

    private static HudDisplay instance;

    private int powerValue;
    private static int day = 0;

    public static bool Completable { get { return day == DAILY_POWER_REQUIREMENTS.Length - 1 && Progressable; } }
    public static bool Progressable { get { return DAILY_POWER_REQUIREMENTS[day] <= instance.powerValue; } }
    public static bool MovementAllowed { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        AddPower(null, 0);
        ShowIntro();
    }

    public static void AddPower(ColorCube cube, int value)
    {
        if (cube != null && !collectedfCubes.Contains(cube))
            collectedfCubes.Add(cube);

        instance.powerValue = Mathf.Clamp(instance.powerValue + value, 0, MAX_POWER);

        if (instance.powerValue == 0)
        {
            instance.powerValueDisplay.text = "Nothing";
            instance.powerValueDisplay.color = Color.red;
        }
        else if (instance.powerValue == MAX_POWER)
        {
            instance.powerValueDisplay.text = "More than you ever dreamed";
            instance.powerValueDisplay.color = Color.green;
        }
        else
        {
            instance.powerValueDisplay.text = "" + instance.powerValue;
            instance.powerValueDisplay.color = Color.white;
        }

        instance.powerValueDisplay.text += "/" + DAILY_POWER_REQUIREMENTS[day];
    }

    private void ShowIntro()
    {
        MovementAllowed = false;
        instance.introPanel.SetActive(true);
    }

    public void HideIntro()
    {
        MovementAllowed = true;
        instance.introPanel.SetActive(false);
    }

    public static void ShowProgress()
    {
        if (Completable)
            instance.ShowVictory();
        else
        {
            MovementAllowed = false;
            day++;
            instance.progressDayText.text = "Day " + day + " Complete!";
            instance.powerValue = 0;

            foreach (ColorCube cube in collectedfCubes)
                if (cube != null)
                    Destroy(cube.gameObject);
            collectedfCubes.Clear();

            AddPower(null, 0);
            instance.progressPanel.SetActive(true);
        }
    }

    public void HideProgress()
    {
        MovementAllowed = true;
        instance.progressPanel.SetActive(false);
    }

    private void ShowVictory()
    {
        MovementAllowed = false;
        instance.statsPowerText.text = instance.powerValue == MAX_POWER ? "More than you ever dreamed" : "" + instance.powerValue;

        instance.statsPanel.SetActive(true);
    }

    private static string GetCommentOnPower(int value)
    {
        if (value == MAX_POWER)
            return "You have gathered more power than you dreamed possible. You won't have to brave the darkness again for quite some time!";
        else
            return "With the power you've gathered, you should be able to fend off the darkness for another day.";
    }
}
