namespace RefactorMe;

public enum QuestionAnswerType
{
    /// <summary>
    ///     Question type when answer must be between two options (yes/no). Right value for this type always
    ///     <see langword="true" />
    /// </summary>
    Boolean,

    /// <summary>
    ///     Question type when answer numeric and must be more than minimal value
    /// </summary>
    Number,

    /// <summary>
    ///     Question type when there are multiple choices and only one correct answer. Answer value equal to value of chosen
    ///     answer variant.
    /// </summary>
    Select
}