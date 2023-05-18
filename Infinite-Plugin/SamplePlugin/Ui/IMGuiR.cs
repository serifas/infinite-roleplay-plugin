using System;
using System.Collections.Generic;
using System.Numerics;
using ImGuiNET;

namespace InfiniteRoleplay
{
    public sealed class ImGuiR : IDisposable
    {
        private int _colorStack;
        private int _fontStack;
        private int _styleStack;
        private float _indentation;

        private Stack<Action>? _onDispose;

        public static ImGuiR NewGroup()
            => new ImGuiR().Group();

        public ImGuiR Group()
            => Begin(ImGui.BeginGroup, ImGui.EndGroup);

        public static ImGuiR NewTooltip()
            => new ImGuiR().Tooltip();

        public ImGuiR Tooltip()
            => Begin(ImGui.BeginTooltip, ImGui.EndTooltip);

        public ImGuiR PushColor(ImGuiCol which, uint color)
        {
            ImGui.PushStyleColor(which, color);
            ++_colorStack;
            return this;
        }

        public ImGuiR PushColor(ImGuiCol which, Vector4 color)
        {
            ImGui.PushStyleColor(which, color);
            ++_colorStack;
            return this;
        }

        public ImGuiR PopColors(int n = 1)
        {
            var actualN = Math.Min(n, _colorStack);
            if (actualN > 0)
            {
                ImGui.PopStyleColor(actualN);
                _colorStack -= actualN;
            }

            return this;
        }

        public ImGuiR PushStyle(ImGuiStyleVar style, Vector2 value)
        {
            ImGui.PushStyleVar(style, value);
            ++_styleStack;
            return this;
        }

        public ImGuiR PushStyle(ImGuiStyleVar style, float value)
        {
            ImGui.PushStyleVar(style, value);
            ++_styleStack;
            return this;
        }

        public ImGuiR PopStyles(int n = 1)
        {
            var actualN = Math.Min(n, _styleStack);
            if (actualN > 0)
            {
                ImGui.PopStyleVar(actualN);
                _styleStack -= actualN;
            }

            return this;
        }

        public ImGuiR PushFont(ImFontPtr font)
        {
            ImGui.PushFont(font);
            ++_fontStack;
            return this;
        }

        public ImGuiR PopFonts(int n = 1)
        {
            var actualN = Math.Min(n, _fontStack);

            while (actualN-- > 0)
            {
                ImGui.PopFont();
                --_fontStack;
            }

            return this;
        }

        public ImGuiR Indent(float width)
        {
            if (width != 0)
            {
                ImGui.Indent(width);
                _indentation += width;
            }

            return this;
        }

        public ImGuiR Unindent(float width)
            => Indent(-width);

        public bool Begin(Func<bool> begin, Action end)
        {
            if (begin())
            {
                _onDispose ??= new Stack<Action>();
                _onDispose.Push(end);
                return true;
            }

            return false;
        }

        public ImGuiR Begin(Action begin, Action end)
        {
            begin();
            _onDispose ??= new Stack<Action>();
            _onDispose.Push(end);
            return this;
        }

        public void End(int n = 1)
        {
            var actualN = Math.Min(n, _onDispose?.Count ?? 0);
            while (actualN-- > 0)
                _onDispose!.Pop()();
        }

        public void Dispose()
        {
            Unindent(_indentation);
            PopColors(_colorStack);
            PopStyles(_styleStack);
            PopFonts(_fontStack);
            if (_onDispose != null)
            {
                End(_onDispose.Count);
                _onDispose = null;
            }
        }
    }
}
