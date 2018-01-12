using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExpresstionTreeDemo.CNBlog
{
    public class PostHelper
    {
        static internal string BuildUrl(SearchCriteria criteria, string url = null)
        {
            if (criteria == null)
                throw new ArgumentNullException("criteria");

            var sbUrl = new StringBuilder(url ?? "http://linqtocnblogs.cloudapp.net/");
            var sbParameter = new StringBuilder();

            if (!String.IsNullOrEmpty(criteria.Title))
                AppendParameter(sbParameter, "Title", criteria.Title);

            if (!String.IsNullOrEmpty(criteria.Author))
                AppendParameter(sbParameter, "Author", criteria.Author);

            if (criteria.Start.HasValue)
                AppendParameter(sbParameter, "Start", criteria.Start.Value.ToString());

            if (criteria.End.HasValue)
                AppendParameter(sbParameter, "End", criteria.End.Value.ToString());

            if (criteria.MinDiggs > 0)
                AppendParameter(sbParameter, "MinDiggs", criteria.MinDiggs.ToString());

            if (criteria.MinViews > 0)
                AppendParameter(sbParameter, "MinViews", criteria.MinViews.ToString());

            if (criteria.MinComments > 0)
                AppendParameter(sbParameter, "MinComments",
                    criteria.MinComments.ToString());

            if (criteria.MaxDiggs > 0)
                AppendParameter(sbParameter, "MaxDiggs", criteria.MaxDiggs.ToString());

            if (criteria.MaxViews > 0)
                AppendParameter(sbParameter, "MaxViews", criteria.MaxViews.ToString());

            if (criteria.MaxComments > 0)
                AppendParameter(sbParameter, "MaxComments",
                    criteria.MaxComments.ToString());

            if (sbParameter.Length > 0)
                sbUrl.AppendFormat("?{0}", sbParameter.ToString());

            return sbUrl.ToString();
        }

        static internal void AppendParameter(StringBuilder sb, string name, string value)
        {
            if (sb.Length > 0)
                sb.Append("&");

            sb.AppendFormat("{0}={1}", name, value);
        }

        static internal IEnumerable<Post> PerformWebQuery(string url)
        {
            var request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;

            var response = (HttpWebResponse)request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var body = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<List<Post>>(body);
            }
        }
    }
}
