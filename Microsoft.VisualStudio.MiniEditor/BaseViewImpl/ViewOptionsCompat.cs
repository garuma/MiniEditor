//
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  Licensed under the MIT License. See License.txt in the project root for license information.
//
using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;


#pragma warning disable CS0436 // Type conflicts with imported type

namespace Microsoft.VisualStudio.Text.Editor.OptionsExtensionMethods
{
	/// <summary>
	/// Provides methods for <see cref="ITextView"/>-related options.
	/// </summary>
	public static class TextViewOptionExtensions
	{
		public static bool IsVisibleWhitespaceOnlyWhenSelectedEnabled (this IEditorOptions options)
		{
			if (options == null)
				throw new ArgumentNullException (nameof (options));

			return options.GetOptionValue<bool> (DefaultTextViewOptions.UseVisibleWhitespaceOnlyWhenSelectedId);
		}

		public static DefaultTextViewOptions.IncludeWhitespaces VisibleWhitespaceEnabledTypes (this IEditorOptions options)
		{
			if (options == null)
				throw new ArgumentNullException (nameof (options));

			return options.GetOptionValue<DefaultTextViewOptions.IncludeWhitespaces> (DefaultTextViewOptions.UseVisibleWhitespaceIncludeId);
		}

		/// <summary>
		/// Determines if the caret should be moved to the end of the selection after performing the "select all" operation.
		/// </summary>
		public static bool ShouldMoveCaretOnSelectAll (this IEditorOptions options)
		{
			if (options == null)
				throw new ArgumentNullException (nameof (options));

			return options.GetOptionValue (DefaultTextViewOptions.ShouldMoveCaretOnSelectAllId);
		}
	}
}

namespace Microsoft.VisualStudio.Text.Editor
{
	/// <summary>
	/// Defines common <see cref="ITextView"/> options.
	/// </summary>
	public static class DefaultTextViewOptions
	{
		#region Option identifiers

		/// <summary>
		/// Determines whether cut and copy causes a blank line to be cut or copied when the selection is empty.
		/// </summary>
		public static readonly EditorOptionKey<bool> CutOrCopyBlankLineIfNoSelectionId = new EditorOptionKey<bool> (CutOrCopyBlankLineIfNoSelectionName);
		public const string CutOrCopyBlankLineIfNoSelectionName = "TextView/CutOrCopyBlankLineIfNoSelection";

		/// <summary>
		/// Determines whether to prohibit user input. The text in the view's
		/// buffer can still be modified, and other views on the same buffer may allow user input.
		/// </summary>
		public static readonly EditorOptionKey<bool> ViewProhibitUserInputId = new EditorOptionKey<bool> (ViewProhibitUserInputName);
		public const string ViewProhibitUserInputName = "TextView/ProhibitUserInput";

		/// <summary>
		/// Gets the word wrap style for the underlying view.
		/// </summary>
		/// <remarks>Turning word wrap on will always hide the host's horizontal scroll bar. Turning word wrap off
		/// will always expose the host's horizontal scroll bar.</remarks>
		public static readonly EditorOptionKey<WordWrapStyles> WordWrapStyleId = new EditorOptionKey<WordWrapStyles> (WordWrapStyleName);
		public const string WordWrapStyleName = "TextView/WordWrapStyle";

		/// <summary>
		/// Determines whether to enable virtual space in the view.
		/// </summary>
		public static readonly EditorOptionKey<bool> UseVirtualSpaceId = new EditorOptionKey<bool> (UseVirtualSpaceName);
		public const string UseVirtualSpaceName = "TextView/UseVirtualSpace";

		/// <summary>
		/// Determines whether the view's ViewportLeft property is clipped to the text width.
		/// </summary>
		public static readonly EditorOptionKey<bool> IsViewportLeftClippedId = new EditorOptionKey<bool> (IsViewportLeftClippedName);
		public const string IsViewportLeftClippedName = "TextView/IsViewportLeftClipped";

		/// <summary>
		/// Determines whether overwrite mode is enabled.
		/// </summary>
		public static readonly EditorOptionKey<bool> OverwriteModeId = new EditorOptionKey<bool> (OverwriteModeName);
		public const string OverwriteModeName = "TextView/OverwriteMode";

		/// <summary>
		/// Determines whether the view should auto-scroll on text changes.
		/// </summary>
		/// <remarks>
		/// If this option is enabled, whenever a text change occurs and the caret is on the last line,
		/// the view will be scrolled to make the caret visible.
		/// </remarks>
		public static readonly EditorOptionKey<bool> AutoScrollId = new EditorOptionKey<bool> (AutoScrollName);
		public const string AutoScrollName = "TextView/AutoScroll";

		/// <summary>
		/// Determines whether to show spaces and tabs as visible glyphs.
		/// </summary>
		public static readonly EditorOptionKey<bool> UseVisibleWhitespaceId = new EditorOptionKey<bool> (UseVisibleWhitespaceName);
		public const string UseVisibleWhitespaceName = "TextView/UseVisibleWhitespace";

		/// <summary>
		/// Determines whether to show spaces, tabs and EndOfLine as visible glyphs only under selection.
		/// </summary>
		public static readonly EditorOptionKey<bool> UseVisibleWhitespaceOnlyWhenSelectedId = new EditorOptionKey<bool> (UseVisibleWhitespaceOnlyWhenSelectedName);
		public const string UseVisibleWhitespaceOnlyWhenSelectedName = "TextView/UseVisibleWhitespace/OnlyWhenSelected";

		/// <summary>
		/// Determines whether to show spaces, tabs and EndOfLine as visible glyphs only for specific type of whitespace.
		/// </summary>
		public static readonly EditorOptionKey<IncludeWhitespaces> UseVisibleWhitespaceIncludeId = new EditorOptionKey<IncludeWhitespaces> (UseVisibleWhitespaceIncludeName);
		public const string UseVisibleWhitespaceIncludeName = "TextView/UseVisibleWhitespace/Include";

		[Flags]
		public enum IncludeWhitespaces
		{
			None = 0x0,
			Spaces = 0x1,
			Tabs = 0x2,
			LineEndings = 0x4,
			Ideographics = 0x8,
			All = 0xf,
		}

		/// <summary>
		/// Enables or disables the code block structure visualizer text adornment feature.
		/// </summary>
		public static readonly EditorOptionKey<bool> ShowBlockStructureId = new EditorOptionKey<bool> (ShowBlockStructureName);
		public const string ShowBlockStructureName = "TextView/ShowBlockStructure";

		/// <summary>
		/// Should the carets be rendered.
		/// </summary>
		public static readonly EditorOptionKey<bool> ShouldCaretsBeRenderedId = new EditorOptionKey<bool> (ShouldCaretsBeRenderedName);
		public const string ShouldCaretsBeRenderedName = "TextView/ShouldCaretsBeRendered";

		/// <summary>
		/// Should the selections be rendered.
		/// </summary>
		public static readonly EditorOptionKey<bool> ShouldSelectionsBeRenderedId = new EditorOptionKey<bool> (ShouldSelectionsBeRenderedName);
		public const string ShouldSelectionsBeRenderedName = "TextView/ShouldSelectionsBeRendered";

		/// <summary>
		/// Whether or not to replace the coding characters and special symbols (such as (,),{,},etc.) with their textual representation
		/// for automated objects to produce friendly text for screen readers.
		/// </summary>
		public static readonly EditorOptionKey<bool> ProduceScreenReaderFriendlyTextId = new EditorOptionKey<bool> (ProduceScreenReaderFriendlyTextName);
		public const string ProduceScreenReaderFriendlyTextName = "TextView/ProduceScreenReaderFriendlyText";

		/// <summary>
		/// The default option that determines whether outlining is undoable.
		/// </summary>
		public static readonly EditorOptionKey<bool> OutliningUndoOptionId = new EditorOptionKey<bool> (OutliningUndoOptionName);
		public const string OutliningUndoOptionName = "TextView/OutliningUndo";

		/// <summary>
		/// Determines whether URLs should be displayed as hyperlinks.
		/// </summary>
		public static readonly EditorOptionKey<bool> DisplayUrlsAsHyperlinksId = new EditorOptionKey<bool> (DisplayUrlsAsHyperlinksName);
		public const string DisplayUrlsAsHyperlinksName = "TextView/DisplayUrlsAsHyperlinks";

		/// <summary>
		/// The default option that determines whether drag/drop editing is enabled.
		/// </summary>
		public static readonly EditorOptionKey<bool> DragDropEditingId = new EditorOptionKey<bool> (DragDropEditingName);
		public const string DragDropEditingName = "TextView/DragDrop";

		/// <summary>
		/// Determines if automatic brace completion is enabled.
		/// </summary>
		public const string BraceCompletionEnabledOptionName = "BraceCompletion/Enabled";
		public readonly static EditorOptionKey<bool> BraceCompletionEnabledOptionId = new EditorOptionKey<bool> (BraceCompletionEnabledOptionName);

		/// <summary>
		/// Defines how wide the caret should be rendered. This is typically used to support accessibility requirements.
		/// </summary>
		public const string CaretWidthOptionName = "TextView/CaretWidth";
		public readonly static EditorOptionKey<double> CaretWidthId = new EditorOptionKey<double> (CaretWidthOptionName);

		/// <summary>
		/// Determines whether to enable the highlight current line adornment.
		/// </summary>
		public static readonly EditorOptionKey<bool> EnableHighlightCurrentLineId = new EditorOptionKey<bool> (EnableHighlightCurrentLineName);
		public const string EnableHighlightCurrentLineName = "Adornments/HighlightCurrentLine/Enable";

		/// <summary>
		/// Determines whether to enable the highlight current line adornment.
		/// </summary>
		public static readonly EditorOptionKey<bool> EnableSimpleGraphicsId = new EditorOptionKey<bool> (EnableSimpleGraphicsName);
		public const string EnableSimpleGraphicsName = "Graphics/Simple/Enable";

		/// <summary>
		/// Determines whether the opacity of text markers and selection is reduced in high contrast mode.
		/// </summary>
		public static readonly EditorOptionKey<bool> UseReducedOpacityForHighContrastOptionId = new EditorOptionKey<bool> (UseReducedOpacityForHighContrastOptionName);
		public const string UseReducedOpacityForHighContrastOptionName = "UseReducedOpacityForHighContrast";

		/// <summary>
		/// Determines whether to enable mouse wheel zooming
		/// </summary>
		public static readonly EditorOptionKey<bool> EnableMouseWheelZoomId = new EditorOptionKey<bool> (EnableMouseWheelZoomName);
		public const string EnableMouseWheelZoomName = "TextView/MouseWheelZoom";

		/// <summary>
		/// Determines the appearance category of a view, which selects a ClassificationFormatMap and EditorFormatMap.
		/// </summary>
		public static readonly EditorOptionKey<string> AppearanceCategory = new EditorOptionKey<string> (AppearanceCategoryName);
		public const string AppearanceCategoryName = "Appearance/Category";

		/// <summary>
		/// Determines the view zoom level.
		/// </summary>
		public static readonly EditorOptionKey<double> ZoomLevelId = new EditorOptionKey<double> (ZoomLevelName);
		public const string ZoomLevelName = "TextView/ZoomLevel";

		/// <summary>
		/// Determines the minimum view zoom level.
		/// </summary>
		public static readonly EditorOptionKey<double> MinZoomLevelId = new EditorOptionKey<double> (MinZoomLevelName);
		public const string MinZoomLevelName = "TextView/MinZoomLevel";

		/// <summary>
		/// Determines the maximum view zoom level.
		/// </summary>
		public static readonly EditorOptionKey<double> MaxZoomLevelId = new EditorOptionKey<double> (MaxZoomLevelName);
		public const string MaxZoomLevelName = "TextView/MaxZoomLevel";

		/// <summary>
		/// Determines whether to enable mouse click + modifier keypress for go to definition.
		/// </summary>
		public const string ClickGoToDefEnabledName = "TextView/ClickGoToDefEnabled";
		public static readonly EditorOptionKey<bool> ClickGoToDefEnabledId = new EditorOptionKey<bool> (ClickGoToDefEnabledName);

		/// <summary>
		/// Determines whether to open definition target in Peek view for mouse click + modifier keypress.
		/// </summary>
		public const string ClickGoToDefOpensPeekName = "TextView/ClickGoToDefOpensPeek";
		public static readonly EditorOptionKey<bool> ClickGoToDefOpensPeekId = new EditorOptionKey<bool> (ClickGoToDefOpensPeekName);

		/// <summary>
		/// The default option that determines whether to move the caret when performing the "select all" operation.
		/// </summary>
		public static readonly EditorOptionKey<bool> ShouldMoveCaretOnSelectAllId = new EditorOptionKey<bool> (ShouldMoveCaretOnSelectAllName);
		public const string ShouldMoveCaretOnSelectAllName = "TextView/ShouldMoveCaretOnSelectAll";
		#endregion
	}
	
	/// <summary>
	/// Names of common <see cref="ITextView"/> host-related options.
	/// </summary>
	public static class DefaultTextViewHostOptions
	{
		#region Option identifiers

		/// <summary>
		/// Determines whether to have a vertical scroll bar.
		/// </summary>
		public static readonly EditorOptionKey<bool> VerticalScrollBarId = new EditorOptionKey<bool> (VerticalScrollBarName);
		public const string VerticalScrollBarName = "TextViewHost/VerticalScrollBar";

		/// <summary>
		/// Determines whether to have a horizontal scroll bar.
		/// </summary>
		public static readonly EditorOptionKey<bool> HorizontalScrollBarId = new EditorOptionKey<bool> (HorizontalScrollBarName);
		public const string HorizontalScrollBarName = "TextViewHost/HorizontalScrollBar";

		/// <summary>
		/// Determines whether to have a glyph margin.
		/// </summary>
		public static readonly EditorOptionKey<bool> GlyphMarginId = new EditorOptionKey<bool> (GlyphMarginName);
		public const string GlyphMarginName = "TextViewHost/GlyphMargin";

		/// <summary>
		/// Determines whether to have a suggestion margin.
		/// </summary>
		public static readonly EditorOptionKey<bool> SuggestionMarginId = new EditorOptionKey<bool> (SuggestionMarginName);
		public const string SuggestionMarginName = "TextViewHost/SuggestionMargin";

		/// <summary>
		/// Determines whether to have a selection margin.
		/// </summary>
		public static readonly EditorOptionKey<bool> SelectionMarginId = new EditorOptionKey<bool> (SelectionMarginName);
		public const string SelectionMarginName = "TextViewHost/SelectionMargin";

		/// <summary>
		/// Determines whether to have a line number margin.
		/// </summary>
		public static readonly EditorOptionKey<bool> LineNumberMarginId = new EditorOptionKey<bool> (LineNumberMarginName);
		public const string LineNumberMarginName = "TextViewHost/LineNumberMargin";

		/// <summary>
		/// Determines whether to have the change tracking margin.
		/// </summary>
		/// <remarks>The change tracking margins will "reset" (lose the change history) when this option is turned off.
		/// If it is turned back on, it will track changes from the time the margin is turned on.</remarks>
		public static readonly EditorOptionKey<bool> ChangeTrackingId = new EditorOptionKey<bool> (ChangeTrackingName);
		public const string ChangeTrackingName = "TextViewHost/ChangeTracking";

		/// <summary>
		/// Determines whether to have an outlining margin.
		/// </summary>
		public static readonly EditorOptionKey<bool> OutliningMarginId = new EditorOptionKey<bool> (OutliningMarginName);
		public const string OutliningMarginName = "TextViewHost/OutliningMargin";

		/// <summary>
		/// Determines whether to have a zoom control.
		/// </summary>
		public static readonly EditorOptionKey<bool> ZoomControlId = new EditorOptionKey<bool> (ZoomControlName);
		public const string ZoomControlName = "TextViewHost/ZoomControl";

		/// <summary>
		/// Determines whether the editor is in either "Extra Contrast" or "High Contrast" modes.
		/// </summary>
		public static readonly EditorOptionKey<bool> IsInContrastModeId = new EditorOptionKey<bool> (IsInContrastModeName);
		public const string IsInContrastModeName = "TextViewHost/IsInContrastMode";

		/// <summary>
		/// Determines whether any annotations are shown over the vertical scroll bar.
		/// </summary>
		public const string ShowScrollBarAnnotationsOptionName = "OverviewMargin/ShowScrollBarAnnotationsOption";
		public readonly static EditorOptionKey<bool> ShowScrollBarAnnotationsOptionId = new EditorOptionKey<bool> (ShowScrollBarAnnotationsOptionName);

		/// <summary>
		/// Determines whether the vertical scroll bar is shown as a standard WPF scroll bar or the new enhanced scroll bar.
		/// </summary>
		public const string ShowEnhancedScrollBarOptionName = "OverviewMargin/ShowEnhancedScrollBar";
		public readonly static EditorOptionKey<bool> ShowEnhancedScrollBarOptionId = new EditorOptionKey<bool> (ShowEnhancedScrollBarOptionName);

		/// <summary>
		/// Determines whether changes are shown over the vertical scroll bar.
		/// </summary>
		public const string ShowChangeTrackingMarginOptionName = "OverviewMargin/ShowChangeTracking";
		public readonly static EditorOptionKey<bool> ShowChangeTrackingMarginOptionId = new EditorOptionKey<bool> (ShowChangeTrackingMarginOptionName);

		/// <summary>
		/// Determines the width of the change tracking margin.
		/// </summary>
		public const string ChangeTrackingMarginWidthOptionName = "OverviewMargin/ChangeTrackingWidth";
		public readonly static EditorOptionKey<double> ChangeTrackingMarginWidthOptionId = new EditorOptionKey<double> (ChangeTrackingMarginWidthOptionName);

		/// <summary>
		/// Determines whether a preview tip is shown when the mouse moves over the vertical scroll bar.
		/// </summary>
		public const string ShowPreviewOptionName = "OverviewMargin/ShowPreview";
		public readonly static EditorOptionKey<bool> ShowPreviewOptionId = new EditorOptionKey<bool> (ShowPreviewOptionName);

		/// <summary>
		/// Determines the size (in lines of text) of the default tip.
		/// </summary>
		public const string PreviewSizeOptionName = "OverviewMargin/PreviewSize";
		public readonly static EditorOptionKey<int> PreviewSizeOptionId = new EditorOptionKey<int> (PreviewSizeOptionName);

		/// <summary>
		/// Determines whether the vertical margin shows the location of the caret.
		/// </summary>
		public const string ShowCaretPositionOptionName = "OverviewMargin/ShowCaretPosition";
		public readonly static EditorOptionKey<bool> ShowCaretPositionOptionId = new EditorOptionKey<bool> (ShowCaretPositionOptionName);

		/// <summary>
		/// Determines whether the source image margin is displayed.
		/// </summary>
		/// <remarks>
		/// This margin is only shown if this option and the ShowEnhancedScrollBarOption, and the SourceImageMarginWidth is >= 25.0.
		/// </remarks>
		public const string SourceImageMarginEnabledOptionName = "OverviewMargin/ShowSourceImageMargin";
		public readonly static EditorOptionKey<bool> SourceImageMarginEnabledOptionId = new EditorOptionKey<bool> (SourceImageMarginEnabledOptionName);

		/// <summary>
		/// Determines the width of the source image margin.
		/// </summary>
		public const string SourceImageMarginWidthOptionName = "OverviewMargin/SourceImageMarginWidth";
		public readonly static EditorOptionKey<double> SourceImageMarginWidthOptionId = new EditorOptionKey<double> (SourceImageMarginWidthOptionName);

		/// <summary>
		/// Determines whether marks (bookmarks, breakpoints, etc.) are shown over the vertical scroll bar.
		/// </summary>
		public const string ShowMarksOptionName = "OverviewMargin/ShowMarks";
		public readonly static EditorOptionKey<bool> ShowMarksOptionId = new EditorOptionKey<bool> (ShowMarksOptionName);

		/// <summary>
		/// Determines whether errors are shown over the vertical scroll bar.
		/// </summary>
		public const string ShowErrorsOptionName = "OverviewMargin/ShowErrors";
		public readonly static EditorOptionKey<bool> ShowErrorsOptionId = new EditorOptionKey<bool> (ShowErrorsOptionName);

		/// <summary>
		/// Determines the width of the marks margin.
		/// </summary>
		public const string MarkMarginWidthOptionName = "OverviewMargin/MarkMarginWidth";
		public readonly static EditorOptionKey<double> MarkMarginWidthOptionId = new EditorOptionKey<double> (MarkMarginWidthOptionName);

		/// <summary>
		/// Determines the width of the error margin.
		/// </summary>
		public const string ErrorMarginWidthOptionName = "OverviewMargin/ErrorMarginWidth";
		public readonly static EditorOptionKey<double> ErrorMarginWidthOptionId = new EditorOptionKey<double> (ErrorMarginWidthOptionName);

		/// <summary>
		/// Determines whether to have a file health indicator.
		/// </summary>
		public static readonly EditorOptionKey<bool> EnableFileHealthIndicatorOptionId = new EditorOptionKey<bool> (EnableFileHealthIndicatorOptionName);
		public const string EnableFileHealthIndicatorOptionName = "TextViewHost/FileHealthIndicator";

		#endregion
	}

	/// <summary>
	/// Defines the Use Visible Whitespace option.
	/// </summary>
	[Export (typeof (EditorOptionDefinition))]
	[Name (DefaultTextViewOptions.UseVisibleWhitespaceOnlyWhenSelectedName)]
	public sealed class UseVisibleWhitespaceOnlyWhenSelected : ViewOptionDefinition<bool>
	{
		/// <summary>
		/// Gets the default value, which is <c>false</c>.
		/// </summary>
		public override bool Default { get { return false; } }

		/// <summary>
		/// Gets the default text view host value.
		/// </summary>
		public override EditorOptionKey<bool> Key { get { return DefaultTextViewOptions.UseVisibleWhitespaceOnlyWhenSelectedId; } }
	}

	/// <summary>
	/// Defines the Use Visible Whitespace option.
	/// </summary>
	[Export (typeof (EditorOptionDefinition))]
	[Name (DefaultTextViewOptions.UseVisibleWhitespaceIncludeName)]
	public sealed class UseVisibleWhitespaceEnabledTypes : ViewOptionDefinition<DefaultTextViewOptions.IncludeWhitespaces>
	{
		/// <summary>
		/// Gets the default value, which is <c>false</c>.
		/// </summary>
		public override DefaultTextViewOptions.IncludeWhitespaces Default { get { return DefaultTextViewOptions.IncludeWhitespaces.All; } }

		/// <summary>
		/// Gets the default text view host value.
		/// </summary>
		public override EditorOptionKey<DefaultTextViewOptions.IncludeWhitespaces> Key { get { return DefaultTextViewOptions.UseVisibleWhitespaceIncludeId; } }
	}

	/// <summary>
	/// Represents the option to highlight the current line.
	/// </summary>
	[Export (typeof (EditorOptionDefinition))]
	[Name (DefaultTextViewOptions.EnableHighlightCurrentLineName)]
	public sealed class HighlightCurrentLineOption : EditorOptionDefinition<bool>
	{
		/// <summary>
		/// Gets the default value.
		/// </summary>
		public override bool Default { get { return true; } }

		/// <summary>
		/// Gets the key for the highlight current line option.
		/// </summary>
		public override EditorOptionKey<bool> Key { get { return DefaultTextViewOptions.EnableHighlightCurrentLineId; } }
	}

	/// <summary>
	/// Represents the option to draw a selection gradient as opposed to a solid color selection.
	/// </summary>
	[Export (typeof (EditorOptionDefinition))]
	[Name (DefaultTextViewOptions.EnableSimpleGraphicsName)]
	public sealed class SimpleGraphicsOption : EditorOptionDefinition<bool>
	{
		/// <summary>
		/// Gets the default value.
		/// </summary>
		public override bool Default { get { return false; } }

		/// <summary>
		/// Gets the key for the simple graphics option.
		/// </summary>
		public override EditorOptionKey<bool> Key { get { return DefaultTextViewOptions.EnableSimpleGraphicsId; } }
	}

	[Export (typeof (EditorOptionDefinition))]
	[Name (DefaultTextViewOptions.UseReducedOpacityForHighContrastOptionName)]
	public sealed class UseReducedOpacityForHighContrastOption : EditorOptionDefinition<bool>
	{
		/// <summary>
		/// Gets the default value.
		/// </summary>
		public override bool Default { get { return false; } }

		/// <summary>
		/// Gets the key for the use reduced opacity option.
		/// </summary>
		public override EditorOptionKey<bool> Key { get { return DefaultTextViewOptions.UseReducedOpacityForHighContrastOptionId; } }
	}

	/// <summary>
	/// Defines the option to enable the mouse wheel zoom
	/// </summary>
	[Export (typeof (EditorOptionDefinition))]
	[Name (DefaultTextViewOptions.EnableMouseWheelZoomName)]
	public sealed class MouseWheelZoomEnabled : EditorOptionDefinition<bool>
	{
		/// <summary>
		/// Gets the default value, which is <c>true</c>.
		/// </summary>
		public override bool Default { get { return true; } }

		/// <summary>
		/// Gets the wpf text view  value.
		/// </summary>
		public override EditorOptionKey<bool> Key { get { return DefaultTextViewOptions.EnableMouseWheelZoomId; } }
	}

	/// <summary>
	/// Defines the appearance category.
	/// </summary>
	[Export (typeof (EditorOptionDefinition))]
	[Name (DefaultTextViewOptions.AppearanceCategoryName)]
	public sealed class AppearanceCategoryOption : EditorOptionDefinition<string>
	{
		/// <summary>
		/// Gets the default value.
		/// </summary>
		public override string Default { get { return "text"; } }

		/// <summary>
		/// Gets the key for the appearance category option.
		/// </summary>
		public override EditorOptionKey<string> Key { get { return DefaultTextViewOptions.AppearanceCategory; } }
	}

	/// <summary>
	/// Defines the zoomlevel.
	/// </summary>
	[Export (typeof (EditorOptionDefinition))]
	[Name (DefaultTextViewOptions.ZoomLevelName)]
	public sealed class ZoomLevel : EditorOptionDefinition<double>
	{
		/// <summary>
		/// Gets the default value.
		/// </summary>
		public override double Default { get { return (int)ZoomConstants.DefaultZoom; } }

		/// <summary>
		/// Gets the key for the text view zoom level.
		/// </summary>
		public override EditorOptionKey<double> Key { get { return DefaultTextViewOptions.ZoomLevelId; } }
	}

	/// <summary>
	/// Defines the minimum zoomlevel.
	/// </summary>
	[Export (typeof (EditorOptionDefinition))]
	[Name (DefaultTextViewOptions.MinZoomLevelName)]
	public sealed class MinZoomLevel : EditorOptionDefinition<double>
	{
		/// <summary>
		/// Gets the default value.
		/// </summary>
		public override double Default => ZoomConstants.MinZoom;

		/// <summary>
		/// Gets the key for the text view zoom level.
		/// </summary>
		public override EditorOptionKey<double> Key => DefaultTextViewOptions.MinZoomLevelId;
	}

	/// <summary>
	/// Defines the maximum zoomlevel.
	/// </summary>
	[Export (typeof (EditorOptionDefinition))]
	[Name (DefaultTextViewOptions.MaxZoomLevelName)]
	public sealed class MaxZoomLevel : EditorOptionDefinition<double>
	{
		/// <summary>
		/// Gets the default value.
		/// </summary>
		public override double Default => ZoomConstants.MaxZoom;

		/// <summary>
		/// Gets the key for the text view zoom level.
		/// </summary>
		public override EditorOptionKey<double> Key => DefaultTextViewOptions.MaxZoomLevelId;
	}

	/// <summary>
	/// Determines whether to enable mouse click + modifier keypress for go to definition.
	/// </summary>
	[Export (typeof (EditorOptionDefinition))]
	[Name (DefaultTextViewOptions.ClickGoToDefEnabledName)]
	public sealed class ClickGotoDefEnabledOption : EditorOptionDefinition<bool>
	{
		/// <summary>
		/// Gets the default value.
		/// </summary>
		public override bool Default => true;

		/// <summary>
		/// Gets the key for the option.
		/// </summary>
		public override EditorOptionKey<bool> Key => DefaultTextViewOptions.ClickGoToDefEnabledId;
	}

	/// <summary>
	/// Determines whether to open definition target in Peek view for mouse click + modifier keypress.
	/// </summary>
	[Export (typeof (EditorOptionDefinition))]
	[Name (DefaultTextViewOptions.ClickGoToDefOpensPeekName)]
	public sealed class ClickGotoDefOpensPeekOption : EditorOptionDefinition<bool>
	{
		/// <summary>
		/// Gets the default value.
		/// </summary>
		public override bool Default => false;

		/// <summary>
		/// Gets the key for the option.
		/// </summary>
		public override EditorOptionKey<bool> Key => DefaultTextViewOptions.ClickGoToDefOpensPeekId;
	}

	/// <summary>
	/// The option definition that determines if the caret should be moved to the end of the selection after performing the "select all" operation.
	/// </summary>
	[Export (typeof (EditorOptionDefinition))]
	[Name (DefaultTextViewOptions.ShouldMoveCaretOnSelectAllName)]
	internal sealed class ShouldMoveCaretOnSelectAll : EditorOptionDefinition<bool>
	{
		/// <summary>
		/// Gets the default value (true).
		/// </summary>
		public override bool Default { get => true; }

		/// <summary>
		/// Gets the editor option key.
		/// </summary>
		public override EditorOptionKey<bool> Key => DefaultTextViewOptions.ShouldMoveCaretOnSelectAllId;
	}
}

#pragma warning restore CS0436 // Type conflicts with imported type