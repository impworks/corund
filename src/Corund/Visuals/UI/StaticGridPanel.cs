using System;
using System.Globalization;
using Corund.Geometry;
using Corund.Tools.Helpers;
using Corund.Tools.UI;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

namespace Corund.Visuals.UI;

/// <summary>
/// The grid with predefined columns and rows.
/// </summary>
public class StaticGridPanel: ObjectGroupBase, IGeometryObject
{
    #region Constructors

    /// <summary>
    /// Creates a new instance of the StaticGridPanel.
    /// The sizes of columns and rows are precalculated and cannot be updated.
    /// </summary>
    /// <param name="width">Maximum width of the grid in pixels.</param>
    /// <param name="height">Maximum height of the grid in pixels.</param>
    /// <param name="cols">Column definitions.</param>
    /// <param name="rows">Row definitions.</param>
    /// <param name="padding">Padding size in pixels.</param>
    /// <remarks>
    /// Column and row definitions must be an integer/float number with an optional "*" for relative columns.
    /// 
    /// Definition examples:
    ///    "10": 10 pixels
    ///    "10.5": 10 pixels and a half!
    ///    "*": 1 relative unit
    ///    "2*": 2 relative units (twice as wide/high as "*")
    /// 
    /// Layout example: 
    ///     Max Width = 100px
    ///     Padding = 5
    ///     Columns = ["*", "9", "2*"]
    ///
    ///     Gives: 
    ///     [ 27 ] -5- [ 9 ] -5- [ 54 ]
    /// </remarks>
    public StaticGridPanel(float width, float height, string[] cols, string[] rows, float padding = 0)
    {
        Padding = padding;
        Width = width;
        Height = height;
        Geometry = new GeometryRect(0, 0, width, height);

        _columns = ParseDefs(cols);
        _rows = ParseDefs(rows);

        UpdateLayoutForDefs(_columns, width, padding);
        UpdateLayoutForDefs(_rows, height, padding);
    }

    #endregion

    #region Private fields

    private readonly SizeDef[] _columns;
    private readonly SizeDef[] _rows;

    #endregion

    #region Fields

    /// <summary>
    /// Padding between rows and columns.
    /// </summary>
    public readonly float Padding;

    /// <summary>
    /// Full width of the grid.
    /// </summary>
    public readonly float Width;

    /// <summary>
    /// Full height of the grid.
    /// </summary>
    public readonly float Height;

    /// <summary>
    /// Geometry for this grid.
    /// </summary>
    public IGeometry Geometry { get; }

    #endregion

    #region Methods

    /// <summary>
    /// Adds an object to the cell defined by the column and row.
    /// </summary>
    public T Add<T>(T obj, int col, int row, HorizontalAlignment halign = HorizontalAlignment.Left, VerticalAlignment valign = VerticalAlignment.Top)
        where T: ObjectBase
    {
        if (obj == null || Children.Contains(obj))
            return obj;

        Attach(obj);
        Children.Add(obj);

        obj.Position = GetCellPosition(col, row, halign, valign);

        return obj;
    }

    #endregion

    #region Private helpers

    /// <summary>
    /// Calculates the object's position based on cell and alignment.
    /// </summary>
    private Vector2 GetCellPosition(int col, int row, HorizontalAlignment halign, VerticalAlignment valign)
    {
        if (col < 0 || col >= _columns.Length)
            throw new ArgumentOutOfRangeException(nameof(col), $"Column must be between 0 and {_columns.Length - 1}.");

        if (row < 0 || row >= _rows.Length)
            throw new ArgumentOutOfRangeException(nameof(row), $"Row must be between 0 and {_rows.Length - 1}.");

        var colDef = _columns[col];
        var rowDef = _rows[row];

        var pos = new Vector2(colDef.Offset, rowDef.Offset);

        if (halign == HorizontalAlignment.Center)
            pos.X += colDef.ActualSize / 2;
        else if (halign == HorizontalAlignment.Right)
            pos.X += colDef.ActualSize;

        if (valign == VerticalAlignment.Center)
            pos.Y += colDef.ActualSize / 2;
        else if (valign == VerticalAlignment.Bottom)
            pos.Y += colDef.ActualSize;

        return pos;
    }

    #endregion

    #region Size definitions

    /// <summary>
    /// Parses column/rows definitions from string representations.
    /// </summary>
    private SizeDef[] ParseDefs(string[] sources)
    {
        var results = new SizeDef[sources.Length];

        for (var idx = 0; idx < sources.Length; idx++)
        {
            var source = sources[idx];
            if (source == "*")
            {
                results[idx] = new SizeDef {Size = 1f};
                continue;
            }

            var isFixed = source[^1] != '*';
            var sizeStr = isFixed ? source : source.TrimEnd('*');
            var size = float.Parse(sizeStr, CultureInfo.InvariantCulture);

            results[idx] = new SizeDef
            {
                Size = size,
                IsFixed = isFixed
            };
        }

        return results;
    }

    /// <summary>
    /// Calculates the actual sizes of column/row definitions.
    /// </summary>
    private void UpdateLayoutForDefs(SizeDef[] defs, float max, float padding)
    {
        var totalFixed = defs.Length * padding;
        var totalStars = 0.0f;

        foreach (var def in defs)
        {
            if (def.IsFixed)
                totalFixed += def.Size;
            else
                totalStars += def.Size;
        }

        var starSize = totalStars.IsAlmostZero() ? 0.0f : (max - totalFixed) / totalStars;
        var offset = 0.0f;

        for(var i = 0; i < defs.Length; i++)
        {
            var def = defs[i];
            var size = def.IsFixed ? def.Size : def.Size * starSize;
            defs[i].Offset = offset;
            defs[i].ActualSize = size;

            offset += size + padding;
        }
    }

    private struct SizeDef
    {
        public float Size;
        public bool IsFixed;

        public float Offset;
        public float ActualSize;
    }

    #endregion
}