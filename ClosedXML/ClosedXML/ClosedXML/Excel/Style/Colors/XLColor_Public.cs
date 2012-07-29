﻿using System;
using System.Drawing;

namespace ClosedXML.Excel
{
    public enum XLColorType { Color, Theme, Indexed }
    public enum XLThemeColor
    {
        Background1,
        Text1,
        Background2,
        Text2,
        Accent1,
        Accent2,
        Accent3,
        Accent4,
        Accent5,
        Accent6,
        Hyperlink,
        FollowedHyperlink
    }
    public partial class XLColor : IEquatable<XLColor>
    {
        public Boolean HasValue { get; private set; }

        private readonly XLColorType _colorType;
        public XLColorType ColorType {
            get
            {
                return _colorType;
            }
        }
        private Color color;
        public Color Color 
        {
            get
            {
                if (_colorType == XLColorType.Theme)
                    throw new Exception("Cannot convert theme color to Color.");

                if (_colorType == XLColorType.Indexed)
                    return IndexedColors[_indexed].Color;

                return color;
            }
        }

        private readonly Int32 _indexed;
        public Int32 Indexed
        {
            get
            {
                if (ColorType == XLColorType.Theme)
                    throw new Exception("Cannot convert theme color to indexed color.");

                if (ColorType == XLColorType.Indexed)
                    return _indexed;

                throw new Exception("Cannot convert Color to indexed color.");
            }
        }

        private readonly XLThemeColor _themeColor;
        public XLThemeColor ThemeColor
        {
            get
            {
                if (ColorType == XLColorType.Theme)
                    return _themeColor;

                if (ColorType == XLColorType.Indexed)
                    throw new Exception("Cannot convert indexed color to theme color.");
                
                throw new Exception("Cannot convert Color to theme color.");
            }

        }

        private readonly Double _themeTint;
        public Double ThemeTint
        {
            get
            {
                if (ColorType == XLColorType.Theme)
                   return _themeTint;

                if (ColorType == XLColorType.Indexed)
                    throw new Exception("Cannot extract theme tint from an indexed color.");

                return color.A / 255.0;
            }
        }

        public bool Equals(XLColor other)
        {
            var otherC = other as XLColor;
            if (_colorType == otherC._colorType)
            {
                if (_colorType == XLColorType.Color)
                {
                    return color.ToArgb() == otherC.color.ToArgb();
                }
                if (_colorType == XLColorType.Theme)
                {
                    return _themeColor == otherC._themeColor
                        && Math.Abs(_themeTint - otherC._themeTint) < XLHelper.Epsilon;
                }
                return _indexed == otherC._indexed;
            }

            return false;
        }
        public override bool Equals(object obj)
        {
            return Equals((XLColor)obj);
        }

        private readonly int _hashCode;
        public override int GetHashCode()
        {
            return _hashCode;
        }

        public override string ToString()
        {
            if (_colorType == XLColorType.Color)
                return Color.ToHex();

            if (_colorType == XLColorType.Theme)
                return String.Format("Color Theme: {0}, Tint: {1}", _themeColor.ToString(), _themeTint.ToString());

            return "Color Index: " + _indexed;
        }

        public static Boolean operator ==(XLColor left, XLColor right)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(left, right)) return true;

            // If one is null, but not both, return false.
            if ((left as object) == null || (right as Object)== null) return false;

            return left.Equals(right);
        }

        public static Boolean operator !=(XLColor left, XLColor right)
        {
            return !(left == right);
        }
    }
}
