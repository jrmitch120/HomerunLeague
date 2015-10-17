using System;

using ServiceStack;
using ServiceStack.OrmLite;

using HomerunLeague.ServiceModel;

namespace HomerunLeague.ServiceInterface
{
    public static class OrmExtensions
    {
        public static SqlExpression<T> PageTo<T>(this SqlExpression<T> expression, int page)
        {
            expression.Offset = (page - 1)*Paging.PageSize;
            expression.Rows = Paging.PageSize;

            return expression;
        }

        public static string ToPagedSql<TNewPoco, TBasePoco>(
            this JoinSqlBuilder<TNewPoco, TBasePoco> builder, int page)
        {
            // Hackaroo.
            return (builder.ToSql() + " LIMIT {0} OFFSET {1}").Fmt(Paging.PageSize, (page - 1) * Paging.PageSize);
        }

        public static bool Search(this string target, string search)
        {
            if (search.StartsWith("*") && search.EndsWith("*"))
                return (target.Contains(search.Trim(new[] {'*'})));
            if(search.EndsWith("*"))
                return (target.StartsWith(search.TrimEnd(new[] { '*' })));
            if (search.StartsWith("*"))
                return (target.EndsWith(search.TrimStart(new[] { '*' })));

            return (target.Equals(search));
        }
    }
}

