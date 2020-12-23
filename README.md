# More

*by Tomaz Stih*

A growing collection of (MIT licensed) Windows Forms Controls for .NET Core.


# Controls (Alphabetically)

 * [Frame](#frame) Structure and draw on panel without affecting the content.
 * [Hierarchy](#hierarchy) Drawing and manipulating trees.
 * [Line](#line) Line control used a separator or a decorator.
 * [SpriteGrid](#spritegrid) Sprite grid control, base for a sprite editor.



# Frame

The `Frame` control is an example a `PanelEx` derivate. It is a panel with 
a customisable header, inner and outer border.

![](Images/frame-1.jpg)

## Background

`Frame` is derived from `PanelEx` and demostrates how to use it. The `PanelEx`
is a panel that allows creating and dawing on "non-client area" of arbitrary size 
around it. 

You create a new panel control by deriving from `PanelEx`. You set the `Margin` 
property to desired client margin. This creates a non client area around your client
area. Finally, you overriding the `Decorate()` function to draw in the new non-client area.

All layouting (docking, etc.) is done by the control and is allowed only inside the 
client area.

~~~cs
public class MyPanel : PanelEx
{
    public class MyPanel() {
        // Create a 5 pixel unified non client 
        // area around the panel.
        Margin=new Padding(5,5,5,5);
    }

    protected override void Decorate(
        Graphics g, 
        Rectangle lt, // Left top rectangle of NC area.
        Rectangle rt, // Right top rectangle of NC area.
        Rectangle lb, // Left bottom rectangle of NC area.
        Rectangle rb, // Right bottom rectangle of NC area.
        Rectangle l, // Left rectangle of NC area.
        Rectangle t, // Top rectangle of NC area.  
        Rectangle r, // Right rectangle of NC area.
        Rectangle b) // Left top rectangle of NC area.
    {
        // Here you draw in nonclient area.
    }
}
~~~

## Usage

Frame consists of three areas. 
 * First area is the title on the top of the frame. You can set it via
   the `Title` propety. It uses the `Font` property. You can set 
   values of the `TitleBackColor` and `TitleFrontColor`. The title
   is adjusted by `TitleAlignment`. If aligned left or right, the 
   `TitleOffset` is identation from the edge. Last but not least, you
   can increase or reduce title size by changing the value of `TitleHeight`.
 * Second area is the outer border. This border has `OuterBorderThickness`,
   `OuterBorderDarkColor` and `OuterBorderLightColor`. By convention the
   dark color is used for top and left edge, and the light color for the bottom
   and right edge. For the inset effect, simply replace these colors.
 * Third area is the inner border. This border has `InnerBorderThickness`,
   `InnerBorderDarkColor` and `InnerBorderLightColor`. By default the
   thickness is zero so no inner border is shown.
 * Fourth area is the `BorderThickness` (in pixels). This is simply the space
   between outer border and inner border. It can be zero but then it is very hard
   to differentiate between inner and outer border.
 

## Examples

~~~cs
// First the title.
_frame.Title = "Hello!";
_frame.TitleAlignment = StringAlignment.Center;
_frame.TitleHeight = 16;
_frame.TitleBackColor = BackColor;
_frame.TitleForeColor = ForeColor;

// Outer border.
_frame.OuterBorderDarkColor = Color.FromKnownColor(KnownColor.ControlDark);
_frame.OuterBorderLightColor = Color.FromKnownColor(KnownColor.ControlLight);
_frame.OuterBorderThickness = 1;

// Inner border (replace dark and light).
_frame.InnerBorderDarkColor = Color.FromKnownColor(KnownColor.ControlLight);
_frame.InnerBorderLightColor = Color.FromKnownColor(KnownColor.ControlDark);
_frame.InnerBorderThickness = 1;

// Pixels between inner and outer color.
_frame.BorderThickness = 2;
~~~

The code above creates this frame.

![](Images/frame-2.jpg)

# Hierarchy

Draw custom trees. The control does the layouting and you do the
drawing in events.

![](Images/hierarchy-1.jpg)

## Usage


You set the layout direction by manipulating `Direction` property,
the control can do left to right, right to let, top to bottom, and bottom
to top trees. Basic node properties are: `NodeWidth` and `NodeHeight`.
And minimal space in pixels between two nodes is determined by the 
`NodeHorzSpacing` and `NodeVertSpacing` properties.

You feed the data into the control by implemening a simple `IHierarchyFeed`
interface, and then passing this object by calling the `SetFeed()` method.

~~~cs
public interface IHierarchyFeed
{
    IEnumerable<string> Query(string key=null);
}
~~~

This interface returns node keys (node identifiers). Since youa are responsible
for drawing nodes and edges, the control really does not need any additional data
about the node. When it needs to draw it it passes the node key and expects
you to know how to draw it. The `Query()` function accepts a parent parameter.
If null is passed, it returns root node keys, otherwise it returns children nodes
of provided parent node.

You can capture all standard mouse events, and you can translate mouse coordinates
to node key by calling `NodeAt()` function.

## Examples

### File system feed

Here's a simple feed implementation for the file system. 

~~~cs
public class FileSystemHierarchyFeed : IHierarchyFeed
{
    private string _rootDir;

    public FileSystemHierarchyFeed(string rootDir) { _rootDir = rootDir; }

    public IEnumerable<string> Query(string key = null)
    {
        if (key == null) return new string[] { _rootDir };
        else return Directory.EnumerateDirectories(key + @"\");
    }
}
~~~

In the example above full path is used as a node key. If you wanted to draw
organigram, you'd probably use database identifier of a person.

  > Dislaimer: Letting the above file feed scan your `c:` drive is a very bad idea. 

### Drawing functions

There are two events that you can subscribe to: the `DrawEdge` to an edge i.e. a line 
connecting two nodes. And the `DrawNode` to draw a node. Both events will pass you
node key, node rectangle, and an instance of the `Graphics` to use for drawing.

Here are sample  implementations for both events.

~~~cs
private void _hierarchy_DrawEdge(object sender, DrawEdgeEventArgs e)
{
    // Calculate node centers.
    Point
        start = new Point(
            e.ParentRectangle.Left + e.ParentRectangle.Width / 2,
            e.ParentRectangle.Top + e.ParentRectangle.Height / 2),
        end = new Point(
            e.ChildRectangle.Left + e.ChildRectangle.Width / 2,
            e.ChildRectangle.Top + e.ChildRectangle.Height / 2);
    // And draw the line.
    using (Pen p = new Pen(ForeColor)) 
        e.Graphics.DrawLine(p,start,end);
}

private void _hierarchy_DrawNode(object sender, DrawNodeEventArgs e)
{
    // Extract directory name from the path.
    string dir= Path.GetFileName(Path.GetDirectoryName(e.Key+@"\"));

    // Draw the node.
    Graphics g = e.Graphics;
    using (Pen forePen = new Pen(ForeColor))
    using (Brush backBrush = new SolidBrush(BackColor),
        foreBrush = new SolidBrush(ForeColor))
    using(StringFormat sf=new StringFormat() { 
        LineAlignment=StringAlignment.Center, 
        Alignment=StringAlignment.Center})
    {
        g.FillRectangle(backBrush, e.Rectangle); // Border.
        g.DrawRectangle(forePen, e.Rectangle); // Rectangle.
        g.DrawString(dir, Font, foreBrush, e.Rectangle, sf); // Text.
    }
}
~~~

### Mouse input

You can subscribe to standard mouse events (clicks, moves, etc.) and use NodeAt 
function to find out which node was clicked. For example, if you'd like to 
highlight node on click, subscribe to the `MouseUp` event, find out which node was
clicked, and call `Refresh()` to repaint the control.

~~~cs
private string _highlightedNodeKey;
private void _hierarchy_MouseUp(object sender, MouseEventArgs e)
{
    _highlightedNodeKey = _hierarchy.NodeAt(e.Location);
    _hierarchy.Refresh();
}
~~~

Then, in your draw function you will check node key against `_highlightedNodeKey` and
paint it accordingly.

### Styling edges

Since the `DrawEdge` event gives you parent and child node, you can decide to draw them
differently. For example, you may want to start your edge at end of node and draw it to
start of the other node.

~~~cs
private void _hierarchy_DrawEdge(object sender, DrawEdgeEventArgs e)
{
    // Change start and end location of an edge.
    Point
        start = new Point(
            e.ParentRectangle.Right,
            e.ParentRectangle.Top + e.ParentRectangle.Height / 2),
        end = new Point(
            e.ChildRectangle.Left,
            e.ChildRectangle.Top + e.ChildRectangle.Height / 2);
    using (Pen p = new Pen(ForeColor))
        e.Graphics.DrawLine(p, start, end);
}
~~~

![](Images/hierarchy-2.jpg)



# Line

Vertical or horizontal line control, used as a separator or a decorator. 

![](Images/line-1.jpg)

## Usage

Set the `Orientation` property to `Horizontal` or `Vertical`. Use line 
`Thickness` to set the pen thickness. Set line `Text`,`Font`, and `ForeColor` 
properties to control appearance of title. If empty, no title is shown.
`TextAlignment` tells where the title is shown (at beginning, end or in the
middle of line). If at beginning or end then `TextOffset` (in pixels) is
used to move title away from begin/end point. `BackColor` controls line control 
background, and `LineColor` is used for line color. `DashValues` is an array
of floats that tells size of pixels and spaces. Default value of `{1,0}` means
solid line (i.e. one pixel, followed by zero spaces). A value of `{1,1}` is
interpreted as a pixel followed by a space. The pattern can be of arbitrary
length i.e. a value of `{3,1,1,1}` would be interpreted three pixels, 
followed by one space, followed by one pixel, followed by one space.

## Examples

~~~cs
_line.Orientation = Orientation.Horizontal;
_line.Text = string.Empty; // Remove text.
_line.LineColor = Color.Khaki;
_line.BackColor = Color.DarkSeaGreen;
_line.Thickness = 6;
_line.DashValues = new float[] { 3,1,1,1 };
~~~

![](Images/line-2.jpg)



# SpriteGrid

Raster image viewer (with mouse events and zoom), a basis for a sprite editor.

![](Images/spritegrid-1.jpg)

## Usage

Place the `SpriteGrid` control on your window. Set its `SourceImage` property to
the `Bitmap` you'd like to view or edit and you're done. 

You can show or hide rulers using the `ShowHorzRuler` and `ShowVertRuler` properties.
You can customize rulers by manipulating properties: `RulerHeight`, `RulerWidth`, 
`RulesBackgroundColor`, `MinorTickSize`, `MajorTickSize`, and 'MinorTicksPerMajorTick'.

You can customize grid appearance by manipulating properties `GridEdgeLineColor`,`GridEdgeLineDashPattern`,
`GridTickLineColor`, `GridTickLineDashPattern`.

`BackColor` is used to draw empty grid, and `ForeColor` is used for all text (currently just
the ruler content).

## Examples

### Passing image

You pass image to SpriteGrid by assigning the image to the `SourceImage` property.

~~~cs
_spriteGrid.SourceImage = Image.FromFile("pacman.png");
~~~

### Responding to events

SpriteGrid exposes basic mouse events and translates physical coordinates to logical
coordinates inside the sprite (i.e. row and column). To manipulate the sprite, 
simply manipulate the underlying image and call the `Refresh()` function on
SpriteGrid.

~~~cs
public partial class MainWnd : Form
{
    // ... code omitted ...
    private Bitmap _sprite;

    private void MainWnd_Load(object sender, System.EventArgs e)
    {
        // Create new 16x16 sprite.
        _spriteGrid.SourceImage = _sprite = new Bitmap(16, 16);
    }

    private void _spriteGrid_CellMouseDown(object sender, CellMouseButtonArgs e)
    {
        // Set image pixel.
        _sprite.SetPixel(e.Column, e.Row, Color.Black);
        _spriteGrid.Refresh();
    }
    // ... code omitted ...
}
~~~

### Implementing zoom

SpriteGrid uses mouse wheel for two purposes. If wheel is used without any control
key then sprite is scrolled up and down. If, while using the mouse wheel, you hold
down Ctlr key then the `ZoomIn` and `ZoomOut` events are triggered. You can use
this to implement zoom.

The simplest implementation (which would not consider the current mouse position),
would be increasing and decreasing values or properties `CellWidth` and `CellHeight`.

~~~cs
private void _spriteGrid_ZoomIn(object sender, ZoomInArgs e)
{
    if (_spriteGrid.CellWidth < 32) _spriteGrid.CellWidth++;
    if (_spriteGrid.CellHeight < 32) _spriteGrid.CellHeight++;
}

private void _spriteGrid_ZoomOut(object sender, ZoomOutArgs e)
{
    if (_spriteGrid.CellWidth > 1) _spriteGrid.CellWidth--;
    if (_spriteGrid.CellHeight > 1) _spriteGrid.CellHeight--;
}
~~~

And the result.

![](Images/spritegrid-2.gif)

### Visible margins

Sometimes it makes sense to set a visible sprite margin (for example, to show where
image will be cropped). Six properties controling sprite margin: `LeftMargin`,
`RightMargin`, `TopMargin`, `BottomMargin`, `MarginLineThickness`, and
`MarginColor`.

 > Sprite margin is not the same as control margin. Sprite margin is an area of sprite
 > that is visibly marked.

~~~cs
_spriteGrid.SourceImage = Image.FromFile("art.png");
_spriteGrid.LeftMargin = 40;
_spriteGrid.RightMargin = 40;
_spriteGrid.TopMargin = 30;
_spriteGrid.BottomMargin = 40;
_spriteGrid.MarginLineThickness = 4;
_spriteGrid.MarginColor = Color.Red;
~~~

![](Images/spritegrid-3.jpg)

### Selections

By responding to mouse events `CellMouseDown`, `CellMouseUp, `CellMouseMove` you 
can detect select operation.

 > To differentiate between a select and a pixel click you need to compare mouse
 > coordinates at `CellMouseDown` event with that of `CellMouseUp`. If 
 > coordinates are the same and there were no `CellMouseMove` events out o the
 > cell then it is a click. Otherwise it is a select.

To make SpriteEdit draw a selection you set the property GridSelection. 
GridSelection is a polygon and can be of any shape.

~~~cs
_spriteGrid.SourceImage = Image.FromFile("jetpac.png");
_spriteGrid.SetGridSelection(new GridSelection()
{
    LineColor = Color.White,
    LineWidth = 3,
    Poly = new Point[] { 
        new Point(200,200), 
        new Point(400, 200), 
        new Point(400,400), 
        new Point(200,400)}
});
~~~

![](Images/spritegrid-4.jpg)

