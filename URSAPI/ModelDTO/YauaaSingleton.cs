using OrbintSoft.Yauaa.Analyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URSAPI.ModelDTO
{
    public class YauaaSingleton
    {
        private static UserAgentAnalyzer.UserAgentAnalyzerBuilder Builder { get; }

        private static UserAgentAnalyzer analyzer = null;

        public static UserAgentAnalyzer Analyzer
        {
            get
            {
                if (analyzer == null)
                {
                    analyzer = Builder.Build();
                }
                return analyzer;
            }
        }

        static YauaaSingleton()
        {
            Builder = UserAgentAnalyzer.NewBuilder();
            Builder.DropTests();
            Builder.DelayInitialization();
            Builder.WithCache(100);
            Builder.HideMatcherLoadStats();
            Builder.WithAllFields();
        }
    }
}
