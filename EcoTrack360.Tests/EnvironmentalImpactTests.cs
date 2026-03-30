using Xunit;

public class EnvironmentalImpactTests
{
    [Fact]
    public void EnvironmentalImpact_Defaults()
    {
        var e = new EnvironmentalImpact();
        Assert.NotNull(e);
        Assert.Equal(0, e.CarbonFootprint);
        Assert.Equal(0, e.WaterUsage);
        Assert.Equal(0, e.WasteGenerated);
    }
}
