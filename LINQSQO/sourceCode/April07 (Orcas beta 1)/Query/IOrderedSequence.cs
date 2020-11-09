using System;
using System.Collections;
using System.Collections.Generic;

namespace BdsSoft.Linq
{
    /// <summary>
    /// Represents and ordered sequence.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public interface IOrderedSequence<TElement> : IEnumerable<TElement>, IEnumerable
    {
        /// <summary>
        /// Creates an ordered sequence based on the key selector and comparer.
        /// </summary>
        /// <typeparam name="TKey">Type of the key to sort on.</typeparam>
        /// <param name="keySelector">Selector to select the key to sort on for each element.</param>
        /// <param name="comparer">Comparer used for key comparison.</param>
        /// <param name="descending">Indicates whether the sort should be descending or not.</param>
        /// <returns>Ordered sequence.</returns>
        IOrderedSequence<TElement> CreateOrderedSequence<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending);
    }
}