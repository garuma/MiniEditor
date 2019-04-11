# MiniEditor

Composable subset of the [Visual Studio Editor platform](https://docs.microsoft.com/en-us/visualstudio/extensibility/inside-the-editor) that's UI-agnostic to allow cross-platform unit-testing scenarios

## Motivation

If you are creating editor extensions for both Visual Studio and Visual Studio for Mac that are cross-platform, IDE-agnostic and UI-agnostic then this library should allow you to instantiate and thus unit-test them.

The library doesn't require a specific test framework and uses [VS-mef](http://github.com/Microsoft/vs-mef) to compose itself at runtime.

The main testing scenarios supported are currently:

- Low-level usage of interfaces such as `ITextDocument`, `ITextBuffer`, `ITextSnapshot` and so on.
- [Async completion](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.language.intellisense.asynccompletion?view=visualstudiosdk-2017) Intellisense providers.

## Setup

For our scenario we assume your editor extensions are in one .NET library (.NET standard or .NET framework) and your test project (using whatever testing library) references it so that it's copied in its output.

Depending on your testing needs, you might need to supply a few MEF parts of your own for things to work:

- If you are trying to test async completion extensions, you should export a `JoinableTaskContext` (see [vs-threading repository](https://github.com/Microsoft/vs-threading) for more information):
``` csharp
using System.ComponentModel.Composition;

/*
 Set the field to an instance of the class with whatever option you
 want *before* initializing the composition, usually in your
 unit test framework setup fixture
*/
[Export]
public static Microsoft.VisualStudio.Threading.JoinableTaskContext MefJoinableTaskContext = null;
```
- You might be referencing MEF parts that are only available in the full IDE, a classic example is some Content-Type which you may need to export yourself manually:
``` csharp
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;

[Export]
[Name("xml")]
[BaseDefinition("text")]
public static readonly ContentTypeDefinition XmlContentTypeDefinition = null;
```

MiniEditor also provides a very basic file-system abstraction so that you can instantiate, load and reload `ITextDocument` instances from in-memory content instead of from disk:

```csharp
using System.IO;
using System.Text;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.MiniEditor;

// Register implementation statically
MiniEditorSetup.FileSystem = new DummyFileSystem ();

class DummyFileSystem : IFileSystemAbstraction
{
    public Stream OpenFile (string filePath, out DateTime lastModifiedTimeUtc, out long fileSize)
    {
        lastModifiedTimeUtc = DateTime.UtcNow;
        var bytes = Encoding.UTF8.GetBytes ("Hello World");
        fileSize = bytes.Length;
		return new MemoryStream (bytes);
    }

    public void PerformSave (
        ITextSnapshot textSnapshot,
        FileMode fileMode,
        string filePath,
        Encoding encoding,
        bool createFolder)
    {
        Console.WriteLine (textSnapshot.GetText ());
    }
}
```

## Usage

With setup out of the way, the first thing you need to do is create a MEF composition container to host the editor subset from the library and (if needed) the extensions that you are providing.

Usually you would end up doing this in your unit-test framework equivalent of an initialization method and it would look a bit like this:

```csharp
using Microsoft.VisualStudio.MiniEditor;

static EditorEnvironment EditorEnvironment { get; private set; }

public static void InitializeMiniEditor ()
{
    // Remember to initialize that JoinableTaskContext if you need it
    MefJoinableTaskContext = new JoinableTaskContext ();

    // Create the MEF composition
    // can be awaited instead if your framework supports it
    EditorEnvironment = EditorEnvironment.InitializeAsync (
        "Your.Assembly.With.Extensions.dll",
        "This.Assembly.With.The.Tests.dll"
    ).Result;
    if (EditorEnvironment.CompositionErrors.Length > 0) {
        Console.WriteLine ("Composition Errors:");
        foreach (var error in EditorEnvironment.CompositionErrors)
            Console.WriteLine ("\t" + error);
    }

    // Register your own logging mechanism to print eventual errors
    // in your extensions
    var errorHandler = EditorEnvironment
        .GetEditorHost ()
        .GetService<EditorHostExports.CustomErrorHandler> ();
    errorHandler.ExceptionHandled += (s, e) => Console.WriteLine (e.Exception);
}
```

You can then get all the pieces you need by retrieving an `EditorEnvironment.Host` instance and querying it using its `GetService<T>` method. For instance, here is an helper class that references a few well-known editor services:

``` csharp
class EditorCatalog
{
    public EditorCatalog (EditorEnvironment env) => Host = env.GetEditorHost ();

    EditorEnvironment.Host Host { get; }

    public ITextViewFactoryService TextViewFactory
        => Host.GetService<ITextViewFactoryService> ();

    public ITextDocumentFactoryService TextDocumentFactoryService
        => Host.GetService<ITextDocumentFactoryService> ();

    public IFileToContentTypeService FileToContentTypeService
        => Host.GetService<IFileToContentTypeService> ();

    public ITextBufferFactoryService BufferFactoryService
        => Host.GetService<ITextBufferFactoryService> ();

    public IContentTypeRegistryService ContentTypeRegistryService
        => Host.GetService<IContentTypeRegistryService> ();

    public IAsyncCompletionBroker AsyncCompletionBroker
        => Host.GetService<IAsyncCompletionBroker> ();

    public IClassifierAggregatorService ClassifierAggregatorService
        => Host.GetService<IClassifierAggregatorService> ();

    public IClassificationTypeRegistryService ClassificationTypeRegistryService
        => Host.GetService<IClassificationTypeRegistryService> ();

    public IBufferTagAggregatorFactoryService BufferTagAggregatorFactoryService
        => Host.GetService<IBufferTagAggregatorFactoryService> ();
}
```

## Examples

### Creating a `ITextBuffer` and `ITextView`

``` csharp
IContentType contentType = EditorCatalog.ContentTypeRegistryService.GetContentType ("MyContentType");
ITextBuffer buffer = EditorCatalog.BufferFactoryService.CreateTextBuffer (content, contentType);
ITextView textView = EditorCatalog.TextViewFactory.CreateTextView (buffer);
```

### Creating a text document

``` csharp
// Mock content associated to `filePath` via `MiniEditorSetup.FileSystem`
IContentType contentType = EditorCatalog.FileToContentTypeService.GetContentTypeForFilePath (filePath);
ITextDocument document = EditorCatalog.TextDocumentFactoryService.CreateAndLoadTextDocument (filePath, contentType);
```

### Instantiating an async completion broker

```csharp
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion.Data;

// Use previous examples to get those objects
ITextBuffer buffer = /* ... */;
ITextView view = /* ... */;
int caretPosition = /* where the cursor would be in your text view */;

IAsyncCompletionBroker broker = EditorCatalog.AsyncCompletionBroker;
ITextSnapshot snapshot = buffer.Snapshot;
var trigger = new CompletionTrigger (CompletionTriggerReason.Invoke, snapshot);

var context = await broker.GetAggregatedCompletionContextAsync (
    textView,
    trigger,
    new SnapshotPoint (snapshot, caretPosition),
    CancellationToken.None
);

// Your completion source should have hopefully filled this up with stuff
var completionItems = context.CompletionContext.Items;
```