using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DVDBouncePartyUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform playfield;   // jouw 1600x900 background
    [SerializeField] private RectTransform logo;        // DVD logo image
    [SerializeField] private Image logoImage;           // Image component om kleur te zetten
    [SerializeField] private TMP_Text debugText;        // optioneel

    [Header("Movement")]
    [SerializeField] private float speed = 350f;
    [SerializeField] private float cornerTolerance = 2.0f; // UI-units; kleiner = "perfecter"

    [Header("Start Direction Tuning")]
    [Tooltip("Hoe diagonaal de start richting moet zijn. Hoger = minder verticale/horizontale start.")]
    [Range(0.0f, 0.95f)]
    [SerializeField] private float minAxisMagnitude = 0.45f; // 0.35-0.6 is meestal nice

    [Header("Colors")]
    [Tooltip("Hoe vaak de kleur wisselt tijdens normaal bouncen (seconden).")]
    [SerializeField] private float normalColorTick = 999f; // 999 = alleen bij bounce wisselen
    [Tooltip("Hoe vaak de kleur wisselt in party mode (seconden).")]
    [SerializeField] private float partyColorTick = 0.08f;

    [Header("Win / Restart")]
    [SerializeField] private float partyDuration = 4f;

    [Header("Debug")]
    [SerializeField] private bool enableDebug = true;

    private Vector2 dir;
    private bool inParty;
    private float nextColorTime;
    private int colorIndex;

    private Coroutine restartRoutine;

    // heldere "rainbow" kleuren (geen donker)
    private static readonly Color[] BrightRainbow =
    {
        new Color(1f, 0.2f, 0.2f),  // red-ish
        new Color(1f, 0.6f, 0.2f),  // orange
        new Color(1f, 1f, 0.2f),    // yellow
        new Color(0.2f, 1f, 0.35f), // green
        new Color(0.2f, 0.9f, 1f),  // cyan
        new Color(0.25f, 0.45f, 1f),// blue
        new Color(0.75f, 0.3f, 1f), // purple
        new Color(1f, 0.25f, 0.7f), // pink
    };

    void Reset()
    {
        logo = transform as RectTransform;
        logoImage = GetComponent<Image>();
    }

    void Awake()
    {
        if (logo == null) logo = transform as RectTransform;
        if (logoImage == null && logo != null) logoImage = logo.GetComponent<Image>();
    }

    void OnEnable()
    {
        HardReset();
    }

    public void HardReset()
    {
        if (restartRoutine != null)
        {
            StopCoroutine(restartRoutine);
            restartRoutine = null;
        }

        inParty = false;

        // start midden
        logo.anchoredPosition = Vector2.zero;

        // random richting maar niet bijna verticaal/horizontaal (meer "DVD" feel)
        dir = GetRandomClampedDirection(minAxisMagnitude);

        // kleur reset
        colorIndex = Random.Range(0, BrightRainbow.Length);
        ApplyNextColor();

        // kleur tick timing
        nextColorTime = Time.unscaledTime + normalColorTick;

        UpdateDebugText(false, false, GetBounds());
    }

    private Vector2 GetRandomClampedDirection(float minAxis)
    {
        Vector2 d = Random.insideUnitCircle.normalized;
        if (d == Vector2.zero) d = new Vector2(1f, 1f).normalized;

        // Clamp per-as zodat beide axes "genoeg" bijdrage hebben
        float sx = Mathf.Sign(d.x == 0 ? 1 : d.x);
        float sy = Mathf.Sign(d.y == 0 ? 1 : d.y);

        d.x = sx * Mathf.Max(Mathf.Abs(d.x), minAxis);
        d.y = sy * Mathf.Max(Mathf.Abs(d.y), minAxis);

        return d.normalized;
    }

    void Update()
    {
        if (playfield == null || logo == null || logoImage == null) return;

        BoundsData b = GetBounds();

        if (!inParty)
        {
            // bewegen
            Vector2 pos = logo.anchoredPosition + dir * speed * Time.unscaledDeltaTime;

            bool hitX = false;
            bool hitY = false;

            // bounce X
            if (pos.x <= b.minX)
            {
                pos.x = b.minX;
                dir.x *= -1;
                hitX = true;
            }
            else if (pos.x >= b.maxX)
            {
                pos.x = b.maxX;
                dir.x *= -1;
                hitX = true;
            }

            // bounce Y
            if (pos.y <= b.minY)
            {
                pos.y = b.minY;
                dir.y *= -1;
                hitY = true;
            }
            else if (pos.y >= b.maxY)
            {
                pos.y = b.maxY;
                dir.y *= -1;
                hitY = true;
            }

            logo.anchoredPosition = pos;

            // kleur wisselen bij bounce
            if (hitX || hitY)
                ApplyNextColor();

            // (optioneel) extra kleur tick, als je normalColorTick niet 999 zet
            if (normalColorTick < 998f && Time.unscaledTime >= nextColorTime)
            {
                nextColorTime = Time.unscaledTime + normalColorTick;
                ApplyNextColor();
            }

            // corner check (met tolerance)
            bool nearLeft = Mathf.Abs(pos.x - b.minX) <= cornerTolerance;
            bool nearRight = Mathf.Abs(pos.x - b.maxX) <= cornerTolerance;
            bool nearBottom = Mathf.Abs(pos.y - b.minY) <= cornerTolerance;
            bool nearTop = Mathf.Abs(pos.y - b.maxY) <= cornerTolerance;

            bool inCornerNow = (nearLeft || nearRight) && (nearBottom || nearTop);

            // Echte corner hit voelt het meest "legit" als je net een bounce had
            if (inCornerNow && (hitX || hitY))
            {
                // snap exact naar hoek + freeze
                float cornerX = nearLeft ? b.minX : b.maxX;
                float cornerY = nearBottom ? b.minY : b.maxY;

                logo.anchoredPosition = new Vector2(cornerX, cornerY);
                EnterPartyMode();
            }

            UpdateDebugText(hitX, hitY, b);
        }
        else
        {
            // party mode: alleen kleuren knallen
            if (Time.unscaledTime >= nextColorTime)
            {
                nextColorTime = Time.unscaledTime + partyColorTick;
                ApplyNextColor();
            }

            UpdateDebugText(false, false, b, party: true);
        }
    }

    private void EnterPartyMode()
    {
        if (inParty) return;

        inParty = true;
        nextColorTime = Time.unscaledTime + partyColorTick;

        // start restart timer
        restartRoutine = StartCoroutine(RestartAfterSeconds(partyDuration));
    }

    private IEnumerator RestartAfterSeconds(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        HardReset();
    }

    private void ApplyNextColor()
    {
        colorIndex = (colorIndex + 1) % BrightRainbow.Length;
        logoImage.color = BrightRainbow[colorIndex];
    }

    private struct BoundsData
    {
        public float minX, maxX, minY, maxY;
        public Vector2 halfField;
        public Vector2 halfLogo;
    }

    private BoundsData GetBounds()
    {
        // playfield en logo moeten beide centered anchors/pivot hebben voor deze math
        Vector2 halfField = playfield.rect.size * 0.5f;
        Vector2 halfLogo = logo.rect.size * 0.5f;

        BoundsData b;
        b.halfField = halfField;
        b.halfLogo = halfLogo;

        b.minX = -halfField.x + halfLogo.x;
        b.maxX = halfField.x - halfLogo.x;
        b.minY = -halfField.y + halfLogo.y;
        b.maxY = halfField.y - halfLogo.y;

        return b;
    }

    private void UpdateDebugText(bool hitX, bool hitY, BoundsData b, bool party = false)
    {
        if (!enableDebug || debugText == null) return;

        Vector2 pos = logo.anchoredPosition;

        debugText.text =
            $"DVD DEBUG\n" +
            $"pos: {pos.x:0.0}, {pos.y:0.0}\n" +
            $"dir: {dir.x:0.00}, {dir.y:0.00}\n" +
            $"bounds X[{b.minX:0.0}..{b.maxX:0.0}] Y[{b.minY:0.0}..{b.maxY:0.0}]\n" +
            $"hitX: {hitX}  hitY: {hitY}\n" +
            $"cornerTolerance: {cornerTolerance:0.0}\n" +
            $"minAxisMagnitude: {minAxisMagnitude:0.00}\n" +
            $"party: {party}";
    }

    // Handig voor een button: "Restart"
    public void RestartNow()
    {
        HardReset();
    }
}
