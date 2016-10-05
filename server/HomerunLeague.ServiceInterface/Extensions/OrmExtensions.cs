using HomerunLeague.ServiceModel.Operations;
using ServiceStack.OrmLite;

namespace HomerunLeague.ServiceInterface.Extensions
{
    public static class OrmExtensions
    {
        public static SqlExpression<T> PageTo<T>(this SqlExpression<T> expression, int page)
        {
            expression.Offset = (page - 1)*Meta.PageSize;
            expression.Rows = Meta.PageSize;

            return expression;
        }

        public static bool Search(this string target, string search)
        {
            if (search.StartsWith("*") && search.EndsWith("*"))
                return target.Contains(search.Trim('*'));
            if(search.EndsWith("*"))
                return target.StartsWith(search.TrimEnd('*'));
            if (search.StartsWith("*"))
                return target.EndsWith(search.TrimStart('*'));

            return target.Equals(search);
        }
    }
}

