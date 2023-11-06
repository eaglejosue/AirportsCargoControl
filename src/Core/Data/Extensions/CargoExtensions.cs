namespace Core.Data.Extensions;

public static class CargoExtensions
{
    public static void SetPerformance(ref List<Cargo> cargos)
    {
        foreach (var c in cargos)
            c.Performance = c.GetPerformance();
    }

    public static PerformanceEnum GetPerformance(this Cargo c)
    {
        if (c.StepFirstMile() && c.EstimatedDeliveryDate.Date == DateTime.Today)
            return PerformanceEnum.AtRisk;

        if (c.DeliveryDateTime.HasValue)//c Entregue
        {
            if (c.DeliveryDateTime?.Date > c.EstimatedDeliveryDate.Date)
            {
                if (c.OccurrenceDate.HasValue && c.OccurrenceDate?.Date <= c.EstimatedDeliveryDate.Date)
                    return PerformanceEnum.Normal;//Ocorrencia

                return PerformanceEnum.Late;
            }

            return PerformanceEnum.Normal;
        }
        else//Não Entregue
        {
            if (DateTime.Today > c.EstimatedDeliveryDate.Date)
            {
                if (c.OccurrenceDate.HasValue && c.OccurrenceDate?.Date <= c.EstimatedDeliveryDate.Date)
                    return PerformanceEnum.Normal;//Ocorrencia

                return PerformanceEnum.Late;
            }
        }

        return PerformanceEnum.Normal;
    }

    #region Steps
    public static bool StepFirstMile(this Cargo c) => c.Step == CargoStepContants.FirstMile;
    public static bool StepMiddleMile(this Cargo c) => c.Step == CargoStepContants.MiddleMile;
    public static bool StepLastMile(this Cargo c) => c.Step == CargoStepContants.LastMile;
    public static bool StepDelivered(this Cargo c) => c.Step == CargoStepContants.Delivered;
    #endregion

    #region Views
    public static bool ViewFloor(this Cargo c) => c.LastAction == "Departed";
    #endregion
}
