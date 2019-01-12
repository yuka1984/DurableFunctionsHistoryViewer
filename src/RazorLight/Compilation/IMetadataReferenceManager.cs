﻿using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Reflection;

namespace RazorLightCustom.Compilation
{
    public interface IMetadataReferenceManager
    {
        IReadOnlyList<MetadataReference> Resolve(Assembly assembly);

        HashSet<MetadataReference> AdditionalMetadataReferences { get; }
    }
}
