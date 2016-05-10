using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonEntity;

namespace LinqProgramDemo
{
    class LinqOfAsync
    {
        public void SortByCommentCount(List<Article> articleList,bool ascending) {
            AriticleComparison comparison = new AriticleComparison(ascending);
            articleList.Sort(new Comparison<Article>(comparison.Compare));
        }
        //匿名类型版本  不需要在对m_ascending参数的一个封装  匿名方法的实现原理正是由编译器自动生成了一个封装类
        public void SortByCommandCountUseByAnonymous(List<Article> articleList, bool ascending) {
            articleList.Sort(delegate(Article a, Article b)
            {
                return (a.Comment - b.Comment) * (ascending ? 1 : -1);
            });
        }
        public void SortByCommandCountUseByAnonymousandLambdaExpression(List<Article> articleList, bool ascending)
        {
            articleList.Sort((a, b) => (a.Comment - b.Comment) * (ascending ? 1 : -1));
        }

    }

    class AriticleComparison {
        private bool m_ascending;

        public AriticleComparison(bool ascending) {
            this.m_ascending = ascending;
        }

        public int Compare(Article a, Article b) {
            return (a.Comment - b.Comment) * (this.m_ascending ? 1 : -1);
        }
    }
}
