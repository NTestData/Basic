using System.Collections.Generic;

namespace NTestData.Basic
{
    public static class TestData
    {
        /// <summary>
        /// Creates object of specified type <typeparamref name="T"/>
        /// using default constructor and applies all supplied customizations.
        /// </summary>
        /// <typeparam name="T">
        /// Type of object to be created.
        /// </typeparam>
        /// <param name="customizations">
        /// Customizations to be applied to resulting object.
        /// </param>
        /// <returns>
        /// Customized object of type <typeparamref name="T"/>.
        /// </returns>
        public static T Create<T>(
            params Customization<T>[] customizations)
            where T : class, new()
        {
            return Create(() => new T(), customizations);
        }

        /// <summary>
        /// Creates object of specified type <typeparamref name="T"/>
        /// using provided instantiation method
        /// and applies all supplied customizations.
        /// </summary>
        /// <typeparam name="T">
        /// Type of object to be created.
        /// </typeparam>
        /// <param name="instantiator">
        /// Instantiation method used to create object of required type.
        /// </param>
        /// <param name="customizations">
        /// Customizations to be applied to resulting object.
        /// </param>
        /// <returns>
        /// Customized object of type <typeparamref name="T"/>.
        /// </returns>
        public static T Create<T>(
            Instantiation<T> instantiator,
            params Customization<T>[] customizations)
            where T : class
        {
            var obj = instantiator.Invoke();
            obj.Customize(customizations);
            return obj;
        }

        /// <summary>
        /// Creates list of objects of specified type <typeparamref name="T"/>
        /// using default constructor and applies all supplied customizations
        /// to each object in the list.
        /// </summary>
        /// <typeparam name="T">
        /// Type of objects to be created.
        /// </typeparam>
        /// <param name="size">
        /// Number of objects in resulting list.
        /// </param>
        /// <param name="customizations">
        /// Customizations to be applied to each object in the list.
        /// </param>
        /// <returns>
        /// List of customized objects of specified type <typeparamref name="T"/>.
        /// </returns>
        public static List<T> CreateListOf<T>(
            ushort size,
            params Customization<T>[] customizations)
            where T : class, new()
        {
            return CreateListOf(size, () => new T(), customizations);
        }

        /// <summary>
        /// Creates list of objects of specified type <typeparamref name="T"/>
        /// using provided instantiation method and applies all supplied
        /// customizations to each object in the list.
        /// </summary>
        /// <typeparam name="T">
        /// Type of objects to be created.
        /// </typeparam>
        /// <param name="size">
        /// Number of objects in resulting list.
        /// </param>
        /// <param name="instantiator">
        /// Instantiation method used to create object of required type.
        /// </param>
        /// <param name="customizations">
        /// Customizations to be applied to each object in the list.
        /// </param>
        /// <returns>
        /// List of customized objects of specified type <typeparamref name="T"/>.
        /// </returns>
        public static List<T> CreateListOf<T>(
            ushort size,
            Instantiation<T> instantiator,
            params Customization<T>[] customizations)
                 where T : class
        {
            var result = new List<T>(size);
            for (ushort i = 0; i < size; i++)
            {
                var item = Create(instantiator, customizations);
                result.Add(item);
            }
            return result;
        }

        /// <summary>
        /// Customizes specified object by applying all supplied customizations.
        /// </summary>
        /// <typeparam name="T">
        /// Type of object being customized.
        /// </typeparam>
        /// <param name="obj">
        /// Object being customized.
        /// </param>
        /// <param name="customizations">
        /// Customizations that need to be applied to object.
        /// </param>
        /// <returns>
        /// Customized object.
        /// </returns>
        public static T Customize<T>(
            this T obj,
            params Customization<T>[] customizations)
            where T : class
        {
            foreach (var action in customizations)
            {
                action(obj);
            }
            return obj;
        }
    }
}
