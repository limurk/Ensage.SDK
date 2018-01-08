﻿// <copyright file="SliderView.cs" company="Ensage">
//    Copyright (c) 2018 Ensage.
// </copyright>

namespace Ensage.SDK.Menu.Views
{
    using System;

    using Ensage.SDK.Input;
    using Ensage.SDK.Menu.Attributes;
    using Ensage.SDK.Menu.Entries;
    using Ensage.SDK.Menu.Items;

    using SharpDX;
    using SharpDX.Direct3D9;
    using SharpDX.DirectWrite;

    [ExportView(typeof(Slider))]
    public class SliderView : View
    {
        public override void Draw(MenuBase context)
        {
            var item = (MenuItemEntry)context;

            var pos = context.Position;
            var size = context.RenderSize;

            var activeStyle = context.MenuConfig.GeneralConfig.ActiveStyle.Value;
            var styleConfig = activeStyle.StyleConfig;
            var border = styleConfig.Border;

            context.Renderer.DrawTexture(activeStyle.Item, new RectangleF(pos.X, pos.Y, size.X, size.Y));

            pos += new Vector2(border.Thickness[0], border.Thickness[1]);
            var font = styleConfig.Font;
            context.Renderer.DrawText(pos, context.Name, styleConfig.Font.Color, font.Size, font.Family);

            var propValue = item.ValueBinding.GetValue<Slider>();
           

            var rightSide = context.Position.X + size.X - border.Thickness[2];
            pos.X = rightSide - item.ValueSize.X - styleConfig.TextSpacing;

            var rect = new RectangleF();
            rect.X = pos.X;
            rect.Y = pos.Y;
            rect.Right = rightSide;
            rect.Bottom = context.Position.Y + size.Y;
            context.Renderer.DrawText(rect, propValue.ToString(), styleConfig.Font.Color, FontDrawFlags.Right, font.Size, font.Family);
            pos = context.Position;
            pos.X += ((float)(propValue.Value - propValue.MinValue) / (propValue.MaxValue - propValue.MinValue)) * size.X;
            context.Renderer.DrawLine(pos, pos + new Vector2(0, size.Y), styleConfig.Slider.LineColor, styleConfig.Slider.LineWidth);
        }

        public override Vector2 GetSize(MenuBase context)
        {
            var totalSize = base.GetSize(context);
            var styleConfig = context.MenuConfig.GeneralConfig.ActiveStyle.Value.StyleConfig;

            var item = (MenuItemEntry)context;
            var propValue = item.ValueBinding.GetValue<Slider>();

            var font = styleConfig.Font;
            item.ValueSize = context.Renderer.MessureText(propValue.MaxValue.ToString(), font.Size, font.Family);
            totalSize.X += styleConfig.TextSpacing + item.ValueSize.X;

            return totalSize;
        }

        public override bool OnClick(MenuBase context, MouseButtons buttons, Vector2 clickPosition)
        {
            if ((buttons & MouseButtons.Left) == MouseButtons.Left)
            {
                var pos = context.Position;
                var size = context.RenderSize;

                var item = (MenuItemEntry)context;
                var propValue = item.ValueBinding.GetValue<Slider>();

                var percentage = (clickPosition.X - pos.X) / size.X;
                if (percentage >= 0 && percentage <= 1f)
                {
                    propValue.Value = (int)Math.Round(percentage * (propValue.MaxValue - propValue.MinValue)) + propValue.MinValue;
                    return true;
                }
            }

            return false;
        }
    }
}