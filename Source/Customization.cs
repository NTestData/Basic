namespace NTestData.Basic
{
    /// <summary>
    /// Encapsulates a method that customizes object
    /// of specified type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    /// Type of object to be customized.
    /// </typeparam>
    /// <param name="obj">
    /// Object to be customized.
    /// </param>
    public delegate void Customization<in T>(T obj);
}
