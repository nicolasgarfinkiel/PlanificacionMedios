using System;
using System.Collections.Generic;
using System.Linq;

namespace Irsa.PDM.Infrastructure
{
    /// <summary>
    /// Generic Enum helper.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    public static class Enum<T> where T : struct
    {
        /// <summary>
        /// Determines whether the specified <typeparamref name="T"/> instances are considered equal.
        /// </summary>
        /// <param name="x">The first instance to compare.</param>
        /// <param name="y">The second instance to compare.</param>
        public static bool Equals(T x, T y)
        {
            return Enum.Equals(x, y);
        }

        /// <summary>
        /// Converts the specified value of a specified enumerated type to its equivalent 
        /// string representation according to the specified format.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="format">The output format to use.</param>
        public static string Format(T value, string format)
        {
            return Enum.Format(typeof(T), value, format);
        }

        /// <summary>
        /// Returns the name of the constant in the specified enumeration that has
        /// the specified value.
        /// </summary>
        /// <param name="value">The value of a particular enumerated constant in terms of 
        /// its underlying type.</param>
        public static string GetName(object value)
        {
            return Enum.GetName(typeof(T), value);
        }

        /// <summary>
        /// Returns an array of the names of the constants in a specified enumeration.
        /// </summary>
        public static string[] GetNames()
        {
            return Enum.GetNames(typeof(T));
        }

        /// <summary>
        /// Returns the underlying type of the specified enumeration.
        /// </summary>
        public static Type GetUnderlyingType()
        {
            return Enum.GetUnderlyingType(typeof(T));
        }

        /// <summary>
        /// Returns an array of the values of the constants in a specified enumeration.
        /// </summary>
        public static Array GetValues()
        {
            return Enum.GetValues(typeof(T));
        }

        /// <summary>
        /// Returns an indication whether a constant with the specified value or name exists
        /// in a specified enumeration.
        /// </summary>
        /// <param name="value">The value or name of a constant.</param>
        public static bool IsDefined(object value)
        {
            return Enum.IsDefined(typeof(T), value);
        }

        /// <summary>
        /// Returns an indication whether a constant with the specified name exists
        /// in a specified enumeration.
        /// </summary>
        /// <param name="name">The name of a constant.</param>
        /// <param name="ignoreCase">If true, ignore case; otherwise, regard case.</param>
        public static bool IsDefined(string name, bool ignoreCase)
        {
            if (!ignoreCase)
            {
                return Enum<T>.IsDefined(name);
            }

            var names = from item in Enum<T>.GetNames()
                        select item.ToLower();

            return names.Contains(name.ToLower());
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or
        /// more enumerated constants to an equivalent enumerated object.
        /// </summary>
        /// <param name="value">A string containing the name or value to convert.</param>
        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or
        /// more enumerated constants to an equivalent enumerated object. A parameter
        /// specifies whether the operation is case-sensitive.
        /// </summary>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <param name="ignoreCase">If true, ignore case; otherwise, regard case.</param>
        public static T Parse(string value, bool ignoreCase)
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        /// <summary>
        /// Returns an instance of the enumeration set to the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public static T ToObject(object value)
        {
            return (T)Enum.ToObject(typeof(T), value);
        }

        public static IList<KeyValuePair<int, string>> ToKeyValue()
        {
            // Ensure T is an enumerator
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerator type.");
            }

            // Return Enumertator as a Dictionary
            return Enum.GetValues(typeof(T)).Cast<T>()
                   .ToDictionary(i => (int)Convert.ChangeType(i, i.GetType()), t => t.ToString())
                   .ToList()
                   .OrderBy(d => d.Value)
                   .ToList();
        }
    }
}
