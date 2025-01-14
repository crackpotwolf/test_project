﻿using System;
using System.Collections.Generic;
using Lucene.Net.Analysis;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Util;

namespace Search_Data.Search
{
    /// <summary>
    /// Парсер многомерных запросов с поддержкой псевдонимов
    /// </summary>
    public class AliasMultiFieldQueryParser : MultiFieldQueryParser
	{
		protected IDictionary<string, string> m_aliases;

		#region Конструктор

		public AliasMultiFieldQueryParser(LuceneVersion matchVersion, string[] fields, Analyzer analyzer, IDictionary<string, string> aliases, IDictionary<string, float> boosts)
		  : base(matchVersion, fields, analyzer, boosts)
		{
			this.m_aliases = aliases;
		}
		public AliasMultiFieldQueryParser(LuceneVersion matchVersion, string[] fields, Analyzer analyzer, IDictionary<string, string> aliases)
		 : base(matchVersion, fields, analyzer)
		{
			this.m_aliases = aliases;
		}

		#endregion

		#region Переопределения полей

		protected override Query GetFieldQuery(string field, string queryText, int slop)
		{
			return base.GetFieldQuery(GetAlias(field), queryText, slop);
		}
		protected override Query GetFieldQuery(string field, string queryText, bool quoted)
		{
			return base.GetFieldQuery(GetAlias(field), queryText, quoted);
		}
		protected override Query GetFuzzyQuery(string field, string termStr, float minSimilarity)
		{
			return base.GetFuzzyQuery(GetAlias(field), termStr, minSimilarity);
		}
		protected override Query GetPrefixQuery(string field, string termStr)
		{
			return base.GetPrefixQuery(GetAlias(field), termStr);
		}
		protected override Query GetWildcardQuery(string field, string termStr)
		{
			return base.GetWildcardQuery(GetAlias(field), termStr);
		}
		protected override Query GetRangeQuery(string field, string part1, string part2, bool startInclusive, bool endInclusive)
		{
			return base.GetRangeQuery(GetAlias(field), part1, part2, startInclusive, endInclusive);
		}
		protected override Query GetRegexpQuery(string field, string termStr)
		{
			return base.GetRegexpQuery(GetAlias(field), termStr);
		}

		#endregion

		/// <summary>
		/// Получить псевдоним для поля
		/// </summary>
		protected string GetAlias(string field)
		{
			if (string.IsNullOrEmpty(field))
				return field;

			if (m_aliases != null && m_aliases.ContainsKey(field))
			{
				return m_aliases[field];
			}
			else
			{
				return field;
			}
		}
	}
}