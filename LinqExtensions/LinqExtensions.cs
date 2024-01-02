using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Herghys.Extensions.LinqUtility
{
	public static class LinqExtensions
	{
		/// <summary>
		/// Shuffle IEnumerable
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <returns></returns>
		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
		{
			return source.OrderBy(x => Guid.NewGuid());
		}

		/// <summary>
		/// Pick Random from IEnumerable
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
		{
			return source.Shuffle().Take(count);
		}

		/// <summary>
		/// Pick Random IEnumerable (Single)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <returns></returns>
		public static T PickRandom<T>(this IEnumerable<T> source)
		{
			return source.Shuffle().Take(1).SingleOrDefault();
		}

		/// <summary>
		/// Concat from IEnumerable
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumerables"></param>
		/// <returns></returns>
		public static IEnumerable<T> Concat<T>(params IEnumerable[] enumerables)
		{
			foreach (var enumerable in enumerables.NotNull())
			{
				foreach (var item in enumerable.OfType<T>())
				{
					yield return item;
				}
			}
		}

		/// <summary>
		/// Distinct IEnumerable
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="items"></param>
		/// <param name="property"></param>
		/// <returns></returns>
		public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
		{
			return items.GroupBy(property).Select(x => x.First());
		}

		public static IEnumerable<T> NotNull<T>(this IEnumerable<T> enumerable)
		{
			return enumerable.Where(i => i != null);
		}

		public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumerable)
		{
			return new HashSet<T>(enumerable);
		}

		/// <summary>
		/// Add range in IEnumerable
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <param name="items"></param>
		public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
		{
			foreach (var item in items)
			{
				collection.Add(item);
			}
		}

		/// <summary>
		/// Add range in IEnumerable
		/// </summary>
		/// <param name="list"></param>
		/// <param name="items"></param>
		public static void AddRange(this IList list, IEnumerable items)
		{
			foreach (var item in items)
			{
				list.Add(item);
			}
		}

		public static ICollection<T> AsReadOnlyCollection<T>(this IEnumerable<T> enumerable)
		{
			if (enumerable is ICollection<T>)
			{
				return (ICollection<T>)enumerable;
			}
			else
			{
				return enumerable.ToList().AsReadOnly();
			}
		}

		public static IList<T> AsReadOnlyList<T>(this IEnumerable<T> enumerable)
		{
			if (enumerable is IList<T>)
			{
				return (IList<T>)enumerable;
			}
			else
			{
				return enumerable.ToList().AsReadOnly();
			}
		}

		public static IEnumerable<T> IntersectAll<T>(this IEnumerable<IEnumerable<T>> groups)
		{
			HashSet<T> hashSet = null;

			foreach (var group in groups)
			{
				if (hashSet == null)
				{
					hashSet = new HashSet<T>(group);
				}
				else
				{
					hashSet.IntersectWith(group);
				}
			}

			return hashSet == null ? Enumerable.Empty<T>() : hashSet.AsEnumerable();
		}
	}
}
