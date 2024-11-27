namespace TestNamespace;

public struct True;
public struct False;
public struct Done;

[AterraEngine.Unions.UnionAliases(aliasT2: "Alias")]
public readonly partial struct TrueFalseOrAlias() : AterraEngine.Unions.IUnion<True, False, Done> { }
