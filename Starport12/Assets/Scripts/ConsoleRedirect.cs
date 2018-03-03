﻿using System;
using System.IO;
using System.Text;

using UnityEngine;

/// <summary>
/// Redirects writes to System.Console to Unity3D's Debug.Log.
/// </summary>
/// <author>
/// Jackson Dunstan, http://jacksondunstan.com/articles/2986
/// </author>
public static class UnitySystemConsoleRedirector
{
    private class UnityTextWriter : TextWriter
    {
        private StringBuilder buffer = new StringBuilder();

        private Action<object> _flush;

        public UnityTextWriter(Action<object> flush)
        {
            _flush = flush;
        }

        public override void Flush()
        {
            _flush(buffer.ToString());
            //Debug.Log(buffer.ToString());
            buffer.Length = 0;
        }

        public override void Write(string value)
        {
            buffer.Append(value);
            if (value != null)
            {
                var len = value.Length;
                if (len > 0)
                {
                    var lastChar = value[len - 1];
                    if (lastChar == '\n')
                    {
                        Flush();
                    }
                }
            }
        }

        public override void Write(char value)
        {
            buffer.Append(value);
            if (value == '\n')
            {
                Flush();
            }
        }

        public override void Write(char[] value, int index, int count)
        {
            Write(new string(value, index, count));
        }

        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }
    }

    public static void Redirect()
    {
        Console.SetError(new UnityTextWriter(Debug.LogError));
        Console.SetOut(new UnityTextWriter(Debug.Log));
    }
}