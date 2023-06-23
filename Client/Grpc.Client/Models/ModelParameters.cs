namespace Grpc.Client.Models
{
    public record ModelParameters(FillType FillType, CutType CutType, int TimeSeriesLength);

    public enum FillType
    {
        FILL_ZEROES_BOTH,
        FILL_ZEROES_RIGHT,
        FILL_ZEROES_LEFT,
    }

    public enum CutType
    {
        CUT_BOTH,
        CUT_RIGHT,
        CUT_LEFT,
    }
}
