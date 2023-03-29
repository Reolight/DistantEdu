namespace DistantEdu.Types
{
    public enum QuizType
    {
        // Usually is not mandatory
        Common = 0,

        // As Common but with one attempt, partially correct is considered as uncorrect
        Hardcore = 1,

        // Must be completed if pass condition is not ReadOnly
        Key = 2, 

        // As Key and hardcore
        KeyHardcore = 4
    }
}
