using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test._360doo.com
{
    internal class GetInterfaceCityName
    {
        private string m_result;
        /// <summary>
        /// 结果
        /// </summary>
        public string result { get { return m_result; } set { m_result = value; } }
        /// <summary>
        /// 城市列表
        /// </summary>
        public List<CityNameList> mapCityList { get; set; }
        /// <summary>
        /// 门店监测点信息列表
        /// </summary>
        public List<Store> StoreList { get; set; }
        /// <summary>
        /// 获取二手车预估值
        /// </summary>
        public EstimatesUsedCarPrice Data { get; set; }
        /// <summary>
        /// 错误码
        /// </summary>
        public string message_code { get; set; }
        /// <summary>
        /// 错误描述
        /// </summary>
        public string message { get; set; }
    }
}
