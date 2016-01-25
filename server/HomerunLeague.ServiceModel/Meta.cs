using System;
using System.Text.RegularExpressions;
using ServiceStack.DataAnnotations;

namespace HomerunLeague.ServiceModel
{
    public abstract class PageableRequest
    {
        public int? Page { get; set; }
    }

    public interface IMeta
    {
        Meta Meta { get; set; }
    }

    public class Meta
    {
        private readonly string _currentUrl;

        public Meta() { _currentUrl = string.Empty; }
        public Meta(string currentUrl) { _currentUrl = currentUrl ?? string.Empty; }

        public static int PageSize = 2;

        [Description("Current page")]
        public int Page { get; set; }

        [Description("Total number of pagable items")]
        public long TotalCount { get; set; }

        [Description("Total pages")]
        public long TotalPages { get { return TotalCount == 0 ? 1 : (TotalCount + PageSize - 1) / PageSize; } }

        [Description("Link to next page of items [if available]")]
        public string NextPage 
        {
            get
            {
                if (Page >= TotalPages)
                    return null;

                return AddQueryParam(_currentUrl, "page", Page + 1); 
            }
        }

        [Description("Link to previous page of items [if available]")]
        public string PreviousPage
        {
            get
            {
                if (Page <= 1)
                    return null;

                return AddQueryParam(_currentUrl, "page", Page - 1);
            }
        }


        [Description("Link to last page of items")]
        public string LastPage
        {
            get
            {
                return AddQueryParam(_currentUrl, "page", TotalPages);
            }
        }

        private string AddQueryParam(string url, string key, object val)
        {
            if (string.IsNullOrEmpty(url)) return null;

            if(url.IndexOf(key+"=", StringComparison.OrdinalIgnoreCase) > -1)
                return Regex.Replace(url, @"(?<=[ \?|&]" + key + "=)[^&?]*", val.ToString(), RegexOptions.IgnoreCase);

            var prefix = url.IndexOf('?') == -1 ? "?" : "&";
            return url + prefix + key + "=" + val;
        }
    }}

