//
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  Licensed under the MIT License. See License.txt in the project root for license information.
//
namespace Microsoft.VisualStudio.MiniEditor
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.Composition;
    using Microsoft.VisualStudio.Text.Editor;

    /// <summary>
    /// A reusable environment that stores the composition graph. Created using <see cref="InitializeAsync()"/>
    /// The project that will host the editor must have assemblies specified in <see cref="DefaultAssemblyNames"/> in its output directory. />
    /// Any additional MEF parts can be added by using an overload of <see cref="InitializeAsync()"/>
    /// </summary>
    public class EditorEnvironment
    {
        /// <summary>
        /// Isolated, light weight composition environment
        /// </summary>
        public class Host
        {
            private ExportProvider _exportProvider;

            /// <summary>
            /// Creates an export provider which represents a unique container of values.
            /// You can create as many of these as you want,
            /// typicall each test has its own Host, and an application has a single Host.
            /// </summary>
            internal Host(IExportProviderFactory epf)
            {
                _exportProvider = epf.CreateExportProvider();
            }

            /// <summary>
            /// Gets a service of specified type from the composition graph
            /// </summary>
            /// <typeparam name="T">Service type</typeparam>
            /// <returns>Object of requested type</returns>
            public T GetService<T>() where T : class
            {
                return _exportProvider.GetExportedValue<T>();
            }

            /// <summary>
            /// Gets services of specified type from the composition graph
            /// </summary>
            /// <typeparam name="T">Service type</typeparam>
            /// <returns>Enumeration of objects of requested type</returns>
            public IEnumerable<T> GetServices<T>() where T : class
            {
                return _exportProvider.GetExportedValues<T>();
            }
        }

        /// <summary>
        /// Contains the composition graph, builds independent composition environments
        /// </summary>
        private IExportProviderFactory _epf;

        /// <summary>
        /// Provides a list of assembly names that contain parts needed to bootstrap editor with its standard features.
        /// Also provides names of packages that contain these assemblies
        /// </summary>
        /// <remarks>
        /// Any additional assemblies to the specific integration test should be added using an overload of <see cref="InitializeAsync()"/>.
        /// </remarks>
        /// <returns>An array of assembly names</returns>
        public static readonly ImmutableArray<string> DefaultAssemblyNames = new []
        {
            // Consolidated implementation. Most exports come from here.
            "Microsoft.VisualStudio.MiniEditor.dll",
            // Exports from this assembly. The file name must match!
            //"Microsoft.VisualStudio.Text.StandaloneEditor.dll",
            // DefaultWpfViewOptions has necessary EditorOptionDefinition
            //"Microsoft.VisualStudio.Text.UI.Wpf.dll",
            // DefaultTextViewHostOptions has necessary EditorOptionDefinition
            //"Microsoft.VisualStudio.Text.UI.dll",
            // DefaultOptions has necessary EditorOptionDefinition
            "Microsoft.VisualStudio.Text.Logic.dll",
            // WpfTextEditorFactoryService requires an implementation of undo.
            //"Microsoft.VisualStudio.Text.Implementation.StandaloneUndo.dll"
        }.ToImmutableArray();

        /// <summary>
        /// Acommodates modification of the list of assembly names used to boostrap editor with its standard features.
        /// This property defaults to <see cref="DefaultAssemblyNames"/> and may be changed before <see cref="InitializeAsync()"/> is invoked.
        /// This is made available so that VS4mac may experiment with WPF-free implementation. Good luck!
        /// </summary>
        public static ImmutableArray<string> DefaultAssemblies { get; set; } = DefaultAssemblyNames;

        /// <summary>
        /// Gets isolated, light weight composition environment for a specific test.
        /// Ideally, each test has its own <see cref="Host"/> obtained by calling this method in the test initialization method.
        /// </summary>
        /// <returns>Isolated instance of composition environment that provides MEF parts.</returns>
        public Host GetEditorHost()
        {
            return new Host(_epf);
        }

        /// <summary>
        /// Contains composition errors. Array is empty if there were no composition errors.
        /// </summary>
        public ImmutableArray<string> CompositionErrors { get; }

        // --- private constructor and public static initialization methods: ---

        /// <summary>
        /// Creates a reusable environment with a specified composition graph.
        /// </summary>
        private EditorEnvironment(IExportProviderFactory epf, ImmutableArray<string> compositionErrors)
        {
            _epf = epf;
            CompositionErrors = compositionErrors;
        }

        /// <summary>
        /// Creates a reusable environment that consists of default parts needed to bootstrap the editor.
        /// </summary>
        /// <returns>Composed environment that can be reused across tests</returns>
        public static async Task<EditorEnvironment> InitializeAsync()
        {
            return await InitializeAsync(assembliesToLoad: null, typesToLoad: null, typesThatOverride: null);
        }

        /// <summary>
        /// Creates a reusable environment that consists of default parts needed to bootstrap the editor,
        /// as well as parts found in the provided assemblies.
        /// </summary>
        /// <param name="assembliesToLoad">Filenames of assemblies that will be searched for MEF parts.</param>
        /// <returns>Composed environment that can be reused across tests</returns>
        public static async Task<EditorEnvironment> InitializeAsync(params string[] assembliesToLoad)
        {
            return await InitializeAsync(assembliesToLoad, null, null);
        }

        /// <summary>
        /// Creates a reusable environment that consists of default parts needed to bootstrap the editor,
        /// as well as parts found in the provided types.
        /// </summary>
        /// <param name="typesToLoad">Types to include in the MEF catalog.</param>
        /// <returns>Composed environment that can be reused across tests</returns>
        public static async Task<EditorEnvironment> InitializeAsync(params Type[] typesToLoad)
        {
            return await InitializeAsync(null, typesToLoad, null);
        }

        /// <summary>
        /// Creates a reusable environment that consists of parts found in the provided <paramref name="typesToLoad"></paramref>
        /// as well as default parts needed to bootstrap the editor, except for parts whose metadata matches these from <paramref name="typesThatOverride"></paramref>.
        /// The overriding of default parts is useful when providing mocks.
        /// </summary>
        /// <param name="typesToLoad">Types to include in the MEF catalog.</param>
        /// <param name="typesThatOverride">Types whose export signatures will be used to filter out the default catalog.</param>
        /// <returns>Composed environment that can be reused across tests</returns>
        public static async Task<EditorEnvironment> InitializeAsync(IEnumerable<Type> typesToLoad, IEnumerable<Type> typesThatOverride)
        {
            return await InitializeAsync(null, typesToLoad, typesThatOverride);
        }

        /// <summary>
        /// Creates a reusable environment that consists of
        /// 1) default parts and parts needed to bootstrap the editor, except for parts whose metadata matches these from <paramref name="typesThatOverride"></paramref>
        /// 2) parts found in the provided assemblies
        /// 3) parts found in the provided types.
        /// </summary>
        /// <param name="assembliesToLoad">Filenames of assemblies that will be searched for MEF parts.</param>
        /// <param name="typesToLoad">Types to include in the MEF catalog.</param>
        /// <param name="typesThatOverride">Types whose export signatures will be used to filter out the default catalog.</param>
        /// <returns>Composed environment that can be reused across tests</returns>
        public static async Task<EditorEnvironment> InitializeAsync(IEnumerable<string> assembliesToLoad, IEnumerable<Type> typesToLoad, IEnumerable<Type> typesThatOverride)
        {
            // Prepare part discovery to support both flavors of MEF attributes.
            var discovery = PartDiscovery.Combine(
                new AttributedPartDiscovery(Resolver.DefaultInstance, isNonPublicSupported: true), // "NuGet MEF" attributes (Microsoft.Composition)
                new AttributedPartDiscoveryV1(Resolver.DefaultInstance)); // ".NET MEF" attributes (System.ComponentModel.Composition)

            var assemblyCatalog = ComposableCatalog.Create(Resolver.DefaultInstance);

            // Create a list of parts that should override the defaults. Used for mocking when Import expects only one part.
            IEnumerable<ComposablePartDefinition> partsThatOverride = null;
            if (typesThatOverride != null)
            {
                partsThatOverride = (await discovery.CreatePartsAsync(typesThatOverride)).Parts;
            }

            // Load default parts
            foreach (var assemblyName in DefaultAssemblies)
            {
                try
                {
                    var parts = (await discovery.CreatePartsAsync(Assembly.LoadFrom(assemblyName))).Parts;
                    if (partsThatOverride != null)
                    {
                        // Exclude parts whose contract matches the contract of any of the overriding parts
                        var overriddenParts = parts.Where(defaultPart => defaultPart.ExportedTypes.Any(defaultType =>
                            partsThatOverride.Any(overridingPart => overridingPart.ExportedTypes.Any(overridingType => overridingType.ContractName == defaultType.ContractName))));
                        parts = parts.Except(overriddenParts);
                    }
                    assemblyCatalog = assemblyCatalog.AddParts(parts);
                }
                catch (System.IO.FileNotFoundException)
                {
                    // Note, if we provide inner exception, the MSTest runner will display it instead of informativeException. For this reason, we don't provide inner exception.
                    var informativeException = new InvalidOperationException($"Required assembly `{assemblyName}` not found. Please place it in this process' working directory. You can do it by adding reference to the NuGet package that contains this assembly.");
                    informativeException.Data["Missing file"] = assemblyName;
                    throw informativeException;
                }
            }

            // Load parts from all types in the specified assemblies
            if (assembliesToLoad != null)
            {
                foreach (string assembly in assembliesToLoad)
                {
                    var parts = await discovery.CreatePartsAsync(Assembly.LoadFrom(assembly));
                    assemblyCatalog = assemblyCatalog.AddParts(parts);
                }
            }

            // Load parts from the specified types
            if (typesToLoad != null)
            {
                var parts = await discovery.CreatePartsAsync(typesToLoad);
                assemblyCatalog = assemblyCatalog.AddParts(parts);
            }

            // TODO: see if this will be useful
            assemblyCatalog = assemblyCatalog.WithCompositionService(); // Makes an ICompositionService export available to MEF parts to import.

            // Assemble the parts into a valid graph.
            var compositionConfiguration = CompositionConfiguration.Create(assemblyCatalog);

            ImmutableArray<string> compositionErrors = ImmutableArray<string>.Empty;
            if (compositionConfiguration.CompositionErrors.Any())
            {
                compositionErrors = compositionConfiguration.CompositionErrors.SelectMany(collection => collection.Select(diagnostic => diagnostic.Message)).ToImmutableArray();
                // Uncomment to investigate errors and continue execution
                // Debugger.Break();

                // Uncomment to fail immediately:
                // var exception = new InvalidOperationException("There were composition errors");
                // exception.Data["Errors"] = errors;
                // throw exception;
            }

            // Prepare an ExportProvider factory based on this graph.
            // This factory can be now re-used across tests
            return new EditorEnvironment(compositionConfiguration.CreateExportProviderFactory(), compositionErrors);
        }
    }
}
