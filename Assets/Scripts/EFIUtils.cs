
public class EFIUtils
{
    public static readonly float DistanceForEnd = 1.2f;
    public static readonly float CheckGhostDeviceMaxValue = 5f;
    public static readonly float CheckDistance = 3f;

    public static float DeviceValue(float distance)
    {
       return EFIUtils.CheckGhostDeviceMaxValue - ((distance * EFIUtils.CheckGhostDeviceMaxValue) / EFIUtils.CheckDistance);
    }
}
