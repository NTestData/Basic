namespace NTestData.Basic
{
    /// <summary>
    /// Encapsulates a method that has no parameters and returns a value
    /// of the type specified by the <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="TResult">
    /// Type of object to be created.
    /// </typeparam>
    /// <returns>
    /// Object of type <typeparamref name="TResult"/>.
    /// </returns>
    public delegate TResult Instantiation<out TResult>();
}
