namespace MinimalApis.Entities.DTO;

public record GetRolesById
{
    public string UserId { get; set; } = string.Empty;
}

public readonly record struct GetRolesByGroup
{
    public int RoleDetailId { get; init; }
    public string EntityCode { get; init; }
    public bool Allow { get; init; }
    public bool New { get; init; }
    public bool Edit { get; init; }
    public string Path { get; init; }
    public int? OrderNum { get; init; }
    public string Icon { get; init; }
}

//public readonly record struct GetRolesByGroup(
//    int RoleDetailId,
//    string EntityCode,
//    bool Allow,
//    bool New,
//    bool Edit,
//    string Path,
//    int? OrderNum = null,
//    string? Icon = null
//);
