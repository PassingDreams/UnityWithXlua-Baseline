using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using Ueels;

namespace Ueels
{
    public class StringMaster 
    {
        
        public class StringChannel
        {
            private StringBuilder sb;

            private void BindStringBuilder()
            {
                sb = PoolManager.Depool<StringBuilder>();
                sb.Clear();
            }

            public StringChannel()
            {
                BindStringBuilder();
            }
            
            public StringChannel(string s)
            {
                BindStringBuilder();
                sb.Append(s);
            }

            public StringChannel Append(object s)
            {
                sb.Append(s);
                return this;
            }
            public StringChannel Squeeze(string left, string right)
            {
                sb.Insert(0, left);
                sb.Append(right);
                return this;
            }

            public StringChannel SqueezeColorTagOf(string color="red")
            {
                return Squeeze($"<color={color}>", "</color>");
            }

            public StringChannel AppendJoin(string seperator, params object[] infos)
            {
                sb.AppendJoin(seperator, infos);
                return this;
            }
            public StringChannel AppendJoin<T>(string seperator, IEnumerable<T> collection)
            {
                sb.AppendJoin(seperator, collection);
                return this;
            }

            /// <summary>
            /// 输出字符串并回收内存
            /// </summary>
            /// <returns></returns>
            public string BurstOut()
            {
                string s = sb.ToString();
                PoolManager.Enpool(sb);
                return s;
            }
        }

        
        public static string Params(params object[] infos)
        {
            
            var stringBuilder = new StringChannel();
            stringBuilder.Append("[ ").AppendJoin(" ", infos).Append("]");
            return stringBuilder.BurstOut();
        }

        public static string Collection<T>(IEnumerable<T> collection)
        {
            var stringBuilder = new StringChannel();
            stringBuilder.Append("[[").AppendJoin("][", collection).Append("]]");
            return stringBuilder.BurstOut();
        }

        public static string Colorfy(object info,string color="red")
        {
            var stringBuilder = new StringChannel();
            return stringBuilder.Append(info).SqueezeColorTagOf(color).BurstOut();
        }

        public static string DataClass<T>(T dataObj) where T : new()
        {
            return Reflection.DataClassToString(dataObj);
            
        }


    }

}
