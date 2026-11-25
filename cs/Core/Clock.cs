static class Clock
{
    public static int Day => store.GetDay();
    public static int Hour => store.GetHour();
    public static int Minute => store.GetMinute();
    public static int GameMinutes => Hour * 60 + Minute;
    public static int TotalMinutes => (Day - 1) * 1440 + GameMinutes;

    // One day is 600 real seconds. The extra minute covers clock quantization.
    public const int RetryMinutes = 13;

    // Alcohol opens at 06:00; stop at 21:00, one hour before the native 22:00 ban.
    public static bool CanSell(Product product)
    {
        if (!product.IsAlcohol())
        {
            return true;
        }
        int hour = Hour;
        return hour >= 6 && hour < 21;
    }

    public static void WaitSeconds(int seconds)
    {
        if (seconds <= 0)
        {
            return;
        }
        int waitMinutes = (seconds * 12 + 4) / 5 + 1;
        int started = TotalMinutes;
        while (TotalMinutes - started < waitMinutes)
        {
            robot.Turn(robot.GetFacing());
        }
    }
}
