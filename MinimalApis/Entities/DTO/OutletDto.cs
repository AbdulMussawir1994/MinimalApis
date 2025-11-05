namespace MinimalApis.Entities.DTO;

public record OutletDto(
    string Name,
    string Address,
    bool Status,
    bool Delivery,
    bool Pickup,
    bool Dine
);
