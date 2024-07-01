using AccessRightsValidation.Internal.Configuration;

namespace AccessRightsValidation.Internal;

internal abstract record ResourceConfiguration;

internal record ResourceConfiguration<TDescriptor, TAction, TUser>(
    Dictionary<TAction, ActionConfiguration<TDescriptor, TAction, TUser>> ConfiguredActions) : ResourceConfiguration
    where TDescriptor : IResourceDescriptor<TAction, TUser>
    where TAction : Enum;