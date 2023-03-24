namespace DistantEdu.Types
{
    /// <summary>
    /// Describes condition on which lesson considered as passed
    /// </summary>
    public enum PassCondition
    {
        ReadOnly = 0,
        SingleTest = 1,
        KeyTests = 2,
        AllTests = 3
    }
}
