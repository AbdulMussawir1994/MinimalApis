namespace MinimalApis.Entities.DTO;

public readonly record struct AddOutletDto
{
    public string Name { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public string Address { get; init; }
    public bool Active { get; init; }
    public int CountryId { get; init; }
    public int CompanyId { get; init; }
    public int CurrencyId { get; init; }
}
