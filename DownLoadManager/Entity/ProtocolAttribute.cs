﻿/**
 * 功能: Entity反射模板类 
 * Input: 属性名称，位置，长度
 * Output: nONE
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownLoadManager.Entity
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public class ProtocolAttribute : Attribute
    {
        private int index;
        private int length;
        private string propertyName;

        public ProtocolAttribute(string propertyName, int index, int length)
        {
            this.propertyName = propertyName;
            this.length = length;
            this.index = index;
        }
        //得到的是反射
        public int Index { get { return index; } }
        public int Length { get { return length; } }
        public string PropertyName { get { return propertyName; } }
    }
}
