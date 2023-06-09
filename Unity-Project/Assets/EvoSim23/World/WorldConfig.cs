using UnityEngine;

public class WorldConfig : MonoBehaviour
{
    [SerializeField] float deathBelowSize;
    public static float DeathBelowSize;

    [SerializeField] float shrinkSpeed;
    public static float ShrinkSpeed;

    [SerializeField] float spawnMargin;
    public static float SpawnMargin;

    [SerializeField] float absorbSpeed;
    public static float AbsorbSpeed;

    [SerializeField] float absorbEfficiency;
    public static float AbsorbEfficiency;

    private void Awake()
    {
        DeathBelowSize = deathBelowSize;
        ShrinkSpeed = shrinkSpeed;
        SpawnMargin = spawnMargin;
        AbsorbSpeed = absorbSpeed;
        AbsorbEfficiency = absorbEfficiency;
    }

    public static float FixedSatVal = 0.9f;
    public static float FPS;

    System.Collections.IEnumerator Start()
    {
        while (true)
        {
            FPS = 1 / Time.unscaledDeltaTime;
            /*
            if (!toggleSpeed.isOn) yield return new WaitForSeconds(1);
            if (WorldController.CellCount + FPS > 65 + targetCellCount) speedSlider.value++;
            if (WorldController.CellCount + FPS < 40 + targetCellCount && speedSlider.value > 20) speedSlider.value--;
            */
            yield return new WaitForSeconds(1);
        }
    }

    //[serializefield] private float cellspawnsize;
    //public float cellspawnsize
    //{
    //    get { return cellspawnsize; }
    //    set { cellspawnsize = value; }
    //}

    //[serializefield] private int autosaveminutes;

    //public int autosaveminutes
    //{
    //    get { return autosaveminutes; }
    //    set { autosaveminutes = value; }
    //}
}