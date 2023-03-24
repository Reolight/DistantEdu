namespace DistantEdu.Types
{
    public enum QuizType
    {
        // Usually is not mandatory
        Common = 0,

        // As Common but with one attempt
        Hardcore = 1,

        // Must be completed if pass condition is not ReadOnly
        Key = 2, 

        // As Key but with one attempt
        KeyHardcore = 4
    }
}
