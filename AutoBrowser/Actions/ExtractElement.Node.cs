﻿using AutoBrowser.Enums;
using System;
using System.Collections.Generic;

namespace AutoBrowser.Actions
{
    public abstract class Node
    {
        public string Value { get; set; }

        public abstract void ReplaceVariables(KeyValuePair<string, object> savedValues);
        public abstract void ResetValues();
        public abstract void InitVariables();
    }

    public class SingleNode : Node
    {
        private string _originalValue;

        public SingleNodeType From { get; set; }

        public enum SingleNodeType
        {
            Id,
            Index
        }

        public SingleNode() { }

        public SingleNode(SingleNodeType from, string value)
        {
            From = from;
            Value = _originalValue = value;
        }

        public override void ReplaceVariables(KeyValuePair<string, object> variable)
        {
            if (Value.Contains($"[{variable.Key}]"))
            {
                Value = Value.Replace($"[{variable.Key}]", variable.Value.ToString());
            }
        }

        public override void ResetValues()
        {
            Value = _originalValue;
        }

        public override void InitVariables()
        {
            _originalValue = Value;
        }
    }

    public class MultiNode : Node
    {
        #region Global Variables
        private string _originalValue;
        private object _originalIndex;
        private object _originalClassName;
        #endregion

        #region Properties
        public MultiNodeType From { get; set; }
        public object Index { get; set; }
        public object ClassName { get; set; }
        #endregion

        #region Constructors
        public MultiNode() { }

        public MultiNode(HtmlTag tag)
        {
            From = MultiNodeType.Tag;
            Value = _originalValue = tag.Value;
            Index = _originalIndex = "";
            ClassName = _originalClassName = "";
        }

        public MultiNode(HtmlTag tag, object index)
        {
            From = MultiNodeType.Tag;
            Value = _originalValue = tag.Value;
            Index = _originalIndex = index;
            ClassName = _originalClassName = "";
        }

        public MultiNode(HtmlTag tag, object className, object index)
        {
            From = MultiNodeType.Class;
            Value = _originalValue = tag.Value;
            ClassName = _originalClassName = className;
            Index = _originalIndex = index;
        }
        #endregion

        #region Enums
        public enum MultiNodeType
        {
            Tag,
            Class
        }

        #endregion

        #region Functions
        public override void ReplaceVariables(KeyValuePair<string, object> variable)
        {
            if (Value.Contains($"[{variable.Key}]"))
            {
                Value = Value.Replace($"[{variable.Key}]", variable.Value.ToString());
            }

            if (Index.ToString().Contains($"[{variable.Key}]"))
            {
                Index = Index.ToString().Replace($"[{variable.Key}]", variable.Value.ToString());
            }

            if (ClassName.ToString().Contains($"[{variable.Key}]"))
            {
                ClassName = ClassName.ToString().Replace($"[{variable.Key}]", variable.Value.ToString());
            }
        }

        public override void ResetValues()
        {
            Value = _originalValue;
            Index = _originalIndex;
            ClassName = _originalClassName;
        }

        public override void InitVariables()
        {
            _originalValue = Value;
            _originalClassName = ClassName;
            _originalIndex = Index;
        }

        internal void CalculateIndex()
        {
            if (string.IsNullOrEmpty(Index?.ToString()))
            {
                return;
            }

            Index = Library.Math.Calculate(Index.ToString());
        }
        #endregion
    }
}