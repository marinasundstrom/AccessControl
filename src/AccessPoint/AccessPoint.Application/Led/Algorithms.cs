namespace AccessPoint.Application.Led
{
    public static class Algorithms
    {
        // 0 <= stepNumber <= lastStepNumber
        public static int Interpolate(int startValue, int endValue, int stepNumber, int lastStepNumber)
        {
            return (endValue - startValue) * stepNumber / lastStepNumber + startValue;
        }
    }
}
