/// <summary>
/// Extension methods for <see cref="IGuardClause"/>
/// </summary>
namespace Ardalis.GuardClauses
{
    /// <summary>
    /// Guard extension methods for numbers
    /// </summary>
    public static class GuardExtensions
    {
        public static void NegativeNumber(this IGuardClause guardClause, int value, string parameterName)
             => guardClause.NegativeNumber<int>(value, parameterName);

        public static void NegativeNumber(this IGuardClause guardClause, double value, string parameterName)
             => guardClause.NegativeNumber<double>(value, parameterName);

        public static void NegativeNumber(this IGuardClause guardClause, float value, string parameterName)
             => guardClause.NegativeNumber<float>(value, parameterName);

        public static void NegativeNumber(this IGuardClause guardClause, decimal value, string parameterName)
             => guardClause.NegativeNumber<decimal>(value, parameterName);

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
             => guardClause.PositiveNumber<int>(value, parameterName);

        public static void PositiveNumber(this IGuardClause guardClause, double value, string parameterName)
             => guardClause.PositiveNumber<double>(value, parameterName);

        public static void PositiveNumber(this IGuardClause guardClause, float value, string parameterName)
             => guardClause.PositiveNumber<float>(value, parameterName);

        public static void PositiveNumber(this IGuardClause guardClause, decimal value, string parameterName)
             => guardClause.PositiveNumber<decimal>(value, parameterName);

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