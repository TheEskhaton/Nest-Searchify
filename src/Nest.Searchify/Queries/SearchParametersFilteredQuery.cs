﻿using System.Collections.Specialized;
using Nest.Searchify.Abstractions;
using Nest.Searchify.SearchResults;

namespace Nest.Searchify.Queries
{
	public class SearchParametersFilteredQuery<TDocument, TSearchResult> :
		SearchParametersFilteredQuery<ISearchParameters, TDocument, TSearchResult>
		where TDocument : class
        where TSearchResult : SearchResult<TDocument, ISearchParameters>
	{
        public SearchParametersFilteredQuery(ISearchParameters parameters) : base(parameters)
        {
        }
    }

    public class SearchParametersFilteredQuery<TSearchParameters, TDocument, TSearchResult> : SearchParametersFilteredQuery<TSearchParameters, TDocument, TDocument, TSearchResult>
		where TSearchParameters : ISearchParameters
	where TDocument : class
        where TSearchResult : SearchResult<TDocument, TSearchParameters>
	{
        public SearchParametersFilteredQuery(TSearchParameters parameters) : base(parameters)
        {
        }
    }

    public class SearchParametersFilteredQuery<TSearchParameters, TDocument, TReturnAs, TSearchResult> : CommonParametersQuery<TSearchParameters, TDocument, TReturnAs, TSearchResult>
		where TSearchParameters : ISearchParameters
		where TDocument : class
		where TReturnAs : class
        where TSearchResult : SearchResult<TDocument, TReturnAs, TSearchParameters>
	{
        public SearchParametersFilteredQuery(TSearchParameters parameters) : base(parameters)
		{
        }

        protected virtual QueryContainer WithQuery(IQueryContainer query, string queryTerm)
		{
			return Query<TDocument>.QueryString(q => q.Query(queryTerm));
		}

		protected QueryContainer WithQueryCore(IQueryContainer query, TSearchParameters parameters)
		{
			return !string.IsNullOrWhiteSpace(parameters.Query) ? WithQuery(query, parameters.Query.ToLowerInvariant()) : Query<TDocument>.MatchAll();
		}

		protected FilterContainer WithFilterCore(IFilterContainer filter, TSearchParameters parameters)
		{
			return WithFilter(filter, parameters);
		}

		protected virtual FilterContainer WithFilter(IFilterContainer filter, TSearchParameters parameters)
		{
			return null;
		}

		protected sealed override QueryContainer BuildQueryCore(QueryContainer query, TSearchParameters parameters)
		{
			return Query<TDocument>
				.Filtered(fq => fq
					.Filter(f => WithFilterCore(f, parameters))
					.Query(q => WithQueryCore(q, parameters))
				);
		}
	}
}
