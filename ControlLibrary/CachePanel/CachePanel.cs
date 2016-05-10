using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.IO;
using System.Web.Caching;

namespace ControlLibrary.CachePanel
{
    public partial class CachePanel : Control
    {
        /// <summary>
        /// 过期时间
        /// </summary>
        public TimeSpan Duration { get; set; }
        
        public int copyCount { get; set; }
        /// <summary>
        /// 缓存名
        /// </summary>
        public string CacheKey { get; set; }
        /// <summary>
        /// 动态改变缓存内容事件
        /// </summary>
        public EventHandler ResolveCacheKey { get; set; }
        private static Random s_random = new Random(DateTime.Now.Millisecond);
        public bool CacheHit { get; private set; }
        private string m_cacheKey;
        private string m_cachedContent;
        

        protected override void OnInit(EventArgs e)
        {
            var resolveCacheKey = this.ResolveCacheKey;
            if (resolveCacheKey != null) {
                resolveCacheKey(this, EventArgs.Empty);
            }
            int copyIndex = s_random.Next(this.copyCount);
            this.m_cacheKey = this.GetCacheKey(copyIndex);
            this.m_cachedContent = this.Context.Cache.Get(this.m_cacheKey) as string;
            this.CacheHit = (this.m_cachedContent != null);    //缓存存在，命中
            //清空所有子控件
            if (this.CacheHit) {
                this.Controls.Clear();
            }
            base.OnInit(e);
        }

        private string GetCacheKey(int copyIndex) {
            var cacheKeyBase = this.CacheKey ?? this.GetDefaultCacheKeyBase();
            return "$CachePanel$" + cacheKeyBase + "_" + copyIndex;
        }

        private string GetDefaultCacheKeyBase()
        {
            return this.Context.Request.AppRelativeCurrentExecutionFilePath + "_" + this.UniqueID;
        }

        protected override void RenderChildren(HtmlTextWriter writer)
        {
            if (this.m_cachedContent == null) {
                StringBuilder sb = new StringBuilder();
                HtmlTextWriter innerwrite = new HtmlTextWriter(new StringWriter(sb));
                base.RenderChildren(writer);

                this.m_cachedContent = sb.ToString();
                this.Context.Cache.Insert(this.m_cacheKey, this.m_cachedContent, null, DateTime.Now.Add(this.Duration), Cache.NoSlidingExpiration);
            }
            writer.Write(this.m_cachedContent);
        }
    }
}
