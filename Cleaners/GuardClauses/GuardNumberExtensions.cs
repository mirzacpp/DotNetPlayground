using System;
using System.Collections.Generic;

/// <summary>
/// Extension methods for <see cref="Ardalis.GuardClauses.IGuardClause"/>
/// </summary>
/// <remarks>
/// Namespace is named Ardalis.GuardClauses so we can use extended methods without referencing Cleaners.GuardClauses namespace
/// <seealso cref="https://github.com/ardalis/GuardClauses"/>
/// </remarks>
namespace Ardalis.GuardClauses
{
    /// <summary>
    /// Guard extension methods for numbers
    /// </summary>
    /// <remarks>
    /// </remarks>
    public static class GuardExtensions
    {
        public static void NegativeNumber(this IGuardClause guardClause, int value, string parameterName)
             => NegativeNumber<int>(guardClause, value, parameterName);

        public static void NegativeNumber(this IGuardClause guardClause, double value, string parameterName)
             => NegativeNumber<double>(guardClause, value, parameterName);

        public static void NegativeNumber(this IGuardClause guardClause, float value, string parameterName)
             => NegativeNumber<float>(guardClause, value, parameterName);

        public static void NegativeNumber(this IGuardClause guardClause, decimal value, string parameterName)
             => NegativeNumber<decimal>(guardClause, value, parameterName);

        /// <summary>
        /// Generic method for comparing negative numbers
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="guardClause"></param>
        /// <param name="value"></param>
        /// <param name="parameterName"></param>
        private static void NegativeNumber<T>(this IGuardClause guardClause, T value, string parameterName)
            where T : IComparable
        {
            if (guardClause is null)
            {
                throw new ArgumentNullException(nameof(guardClause));
            }

            if (Comparer<T>.Default.Compare(value, default) < 0)
            {
                throw new ArgumentException("Number must be greater than zero.", parameterName);
            }
        }

        public static void PositiveNumber(this IGuardClause guardClause, int value, string parameterName)
             => PositiveNumber<int>(guardClause, value, parameterName);

        public static void PositiveNumber(this IGuardClause guardClause, double value, string parameterName)
             => PositiveNumber<double>(guardClause, value, parameterName);

        public static void PositiveNumber(this IGuardClause guardClause, float value, string parameterName)
             => PositiveNumber<float>(guardClause, value, parameterName);

        public static void PositiveNumber(this IGuardClause guardClause, decimal value, string parameterName)
             => PositiveNumber<decimal>(guardClause, value, parameterName);

        /// <summary>
        /// Generic method for comparing positive numbers
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="guardClause"></param>
        /// <param name="value"></param>
        /// <param name="parameterName"></param>
        private static void PositiveNumber<T>(this IGuardClause guardClause, T value, string parameterName)
            where T : IComparable
        {
            if (guardClause is null)
            {
                throw new ArgumentNullException(nameof(guardClause));
            }

            if (Comparer<T>.Default.Compare(value, default) > 0)
            {
                throw new ArgumentException("Number must be less than zero.", parameterName);
            }
        }
    }
}